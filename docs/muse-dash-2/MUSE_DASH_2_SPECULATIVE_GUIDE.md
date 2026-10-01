# 🔮 뮤즈 대시 2 모딩 선행 가이드 (Muse Dash 2 Speculative Modding Guide)

트레일러에서 확인된 바와 같이, 뮤즈 대시 2는 1편의 직관적인 **2레인(지상/공중) 스크롤 전투 리듬 게임** 메커니즘을 그대로 유지하고 계승하는 연장선상에 있습니다. 

따라서 1편 모드에서 완성된 고유의 **추상화 아키텍처(래퍼 패턴)**와 **인터셉트(가로채기) 파이프라인**은 2편 모딩에서도 핵심 바이블로 기능할 것입니다. 본 가이드는 2편 출시 시 빠르게 커스텀 차트를 구동하기 위해 필요한 분석 철학과 ILSpy 검색 전략을 정리한 필수 핸드북입니다.

> **태그/곡/앨범 부분의 "채울 칸" 표와 1편 훅 전체 목록**은 [MD2_TAG_RETARGET_MAP.md](MD2_TAG_RETARGET_MAP.md)에 있습니다. 이 가이드는 "무엇을, 어떤 순서로"를, 지도는 "정확히 어느 멤버를"을 맡습니다.

---

## 🧰 먼저 알아둘 것 — 1편에서 실측으로 확인된 사실 (2026-09-28, 게임 6.7.0)

이 가이드의 나머지는 추측이 섞여 있지만, 아래는 1편 코드·덤프·로그로 확인한 사실입니다. 2편 계획은 이 전제 위에 세웁니다.

- **1편은 IL2CPP 게임입니다.** ILSpy로 `Assembly-CSharp.dll`(MelonLoader가 만든 `Il2CppAssemblies`)을 열면 **시그니처만 보이고 메서드 본문은 없습니다.** 이 저장소는 [SignatureDumper](../../SignatureDumper)로 `Decompiled/`에 덤프해서 검색합니다. 이 가이드에서 "ILSpy 검색"은 **덤프 검색**으로 읽으십시오. 2편이 Mono라면 `ilspycmd`로 본문까지 볼 수 있어 분석이 훨씬 쉬워집니다. **2편을 받으면 가장 먼저 백엔드부터 확인합니다**(`GameAssembly.dll`이 있으면 IL2CPP).
- **본문이 없으니 "무엇을 하는 메서드인지"는 로그로 확인합니다.** 1편의 [inferred_game_code](../inferred_game_code/README.md) 문서들은 Postfix 로그와 호출 스택으로 동작을 역추적한 결과입니다. 2편에서도 같은 방식이 필요합니다.
- **IL2CPP에서 Harmony 패치는 네이티브 디투어입니다.** MelonLoader의 `Il2CppInterop.HarmonySupport`(`Il2CppDetourMethodPatcher`)는 게임 함수의 네이티브 포인터(`MethodPointer`)에 디투어를 걸고, 그 앞에 관리 코드 트램펄린을 둡니다. 로그의 `During invoking native->managed trampoline`과 스택의 `DMD<...>`가 그 흔적입니다. 즉 **"관리형 래퍼를 쓰니까 네이티브 훅이 아니다"는 성립하지 않습니다**(Phase 1 경고 참고).
- **게임 멤버 이름이 컴파일러 검사를 거치지 않는 곳이 따로 있습니다.** `typeof`/`nameof`로 쓴 것은 빌드가 잡지만, [ModReflection](../../muse%20dash%20test/Patches/Common/ModReflection.cs)·래퍼·`[HarmonyPatch(..., "문자열")]`로 쓴 이름은 게임에서 실행해 보기 전까지 모릅니다. 6.7.0 대조 때 이런 이름이 약 50개였습니다.
- **덤프로 절대 확인할 수 없는 것**: GameObject 이름(`"BGM"`, `"TxtArtist"` 등), 씬 계층, 에셋·프리팹 이름, 애니메이션 이름. 이런 것은 로그로만 확인할 수 있습니다. 2편에서 가장 늦게까지 불확실하게 남을 부분입니다.

---

## 🎯 2편 모딩의 3대 핵심 질문 (Core Questions)

2편 모딩을 시작할 때, 단순히 "어떤 객체를 찾을까"라는 추상적인 생각보다 다음의 **3가지의 직접적이고 실무적인 핵심 질문**에 대한 답을 찾는 데 집중해야 합니다.

1. **"곡 목록 데이터베이스(DB)의 진짜 주인은 누구고, 어디에 상주하는가?"**
   - **의미**: 게임이 켜지거나 로비 화면을 그릴 때 전체 곡 정보를 보관하는 **실제 메모리 리스트(List/Dictionary)의 위치와 변수명**을 찾는 것입니다. (1편: `GlobalDataBase.dbMusicTag.m_AllMusicInfo` — UID → `MusicInfo` 딕셔너리. 조회는 `dbMusicTag.GetMusicInfoFromAll(uid)`)
2. **"BMS 차트를 받아 노트를 뿌려주는 '컨베이어 벨트'의 시작점은 어디인가?"**
   - **의미**: 플레이 버튼을 눌렀을 때, 차트 데이터를 화면에 배치할 런타임 노트 데이터로 조립하는 **핵심 메서드(함수)**를 찾는 것입니다. 이 함수의 길목을 지키고 있어야 차트를 우리 마음대로 갈아 끼울 수 있습니다. (1편: `DBStageInfo.SetRuntimeMusicData`의 **Postfix** — 게임이 순정 노트 리스트를 다 만든 뒤 그 리스트를 제자리에서 비우고 다시 채웁니다. Phase 3 참고)
3. **"카테고리/태그 탭 UI가 생성될 때, 각 탭 셀(Toggle)이 특정 앨범 데이터 및 아이콘을 로드하고 바인딩하는 '규칙과 시점'은 언제인가?"**
   - **의미**: 로비 화면에서 카테고리/태그 탭(기본 곡, 컬래버, 커스텀 태그 등)이 스폰될 때 각 셀이 어떤 컴포넌트(예: `AlbumTagToggle`)로 초기화되며, 클릭 시 스크롤 뷰와 앨범 데이터를 연결하는 **UI 이벤트 리스너의 길목**을 잡는 것입니다. 이 메커니즘을 정확히 알아야 우리가 주입한 커스텀 태그 아이콘 이미지를 깨끗하게 오버라이드하고 곡 스크롤을 무사히 통과시킬 수 있습니다. BGM/BGA 주입은 단순히 인게임 진입 타이밍만 맞춰 카메라와 오디오 소스에 얹어주면 되는 간단한 작업이므로, **이 UI 바인딩의 구조적 흐름을 뚫어내는 것이 최우선 순위**가 됩니다.


---

## 🔍 ILSpy 검색 대상 계열 분류 (ILSpy Search Categories)

ILSpy를 켜고 `Assembly-CSharp.dll`을 분석할 때, 무작위 검색 대신 메서드의 성격에 따라 다음 **3대 계열**로 묶어 정밀 타격 검색을 수행합니다.

### ① 등록/추가 (Registration / Injection) 계열
- **목적**: 커스텀 태그나 가상의 곡/앨범을 게임의 내부 정렬 리스트에 강제 삽입하는 타겟을 찾는 데 쓰입니다.
- **주요 키워드**: `Add...`, `Register...`, `Insert...`, `Init...`
- **예시**: 1편의 `AddAlbumTagData()`, `AddCustomAlbumTagsSort()`

### ② 조회/반환 (Lookup / Getter) 계열 (★가장 중요)
- **목적**: 게임 엔진이 가상 UID(`1999-0` 등)를 조회할 때 NullReference 예외로 폭사하지 않고, 우리가 준비한 복제본 데이터를 정상적으로 찾아가도록 우회 반환로를 가로채는 데 쓰입니다.
- **주요 키워드**: `Get...By...`, `Find...`, `Search...`
- **예시**: 1편의 `GetMusicInfoFromAll(string uid)`(호출만 함), `GetAlbumInfoByMusicInfo()`, `GetAlbumIndexByUid()`(Prefix로 가로챔)
- **1편에서 뒤늦게 발견한 함정 — UID가 아니라 "행 번호"로 묻는 조회**: 현지화 DB(`DBConfigLocalALBUM.GetLocalAlbumInfoByIndex(index)`, `DBConfigLocalAlbums.GetLocalTitleByIndex(index)`)는 곡을 UID가 아니라 인덱스 하나로 묻습니다. 가상 곡은 숙주 곡의 얇은 복제본이라 행 번호까지 물려받으므로, "누구를 위한 질문인지"를 인자로 구분할 수 없습니다. 1편은 곡 선택 문맥(`SelectedSongLocalizationScope`)과 `musicIndex` 대조로 풀었습니다. 2편에서도 `...ByIndex` 계열은 따로 의심하십시오.

### ③ 필터/제외 (Filter / Sanitization) 계열
- **목적**: 가상 곡을 깨고 점수를 저장할 때 가짜 UID 기록이 순정 세이브 파일에 포함되어 세이브가 깨지는 현상을 방지하는 타겟을 찾는 데 쓰입니다.
- **주요 키워드**: `Save...`, `Write...`, `Serialize...`, `Clean...`
- **예시**: 1편의 `SaveDataManager.Save()` 시점의 플레이 데이터 딕셔너리 정화 로직

---

## 🧯 Phase 0. 첫 빌드를 받았을 때의 순서 (1편 6.7.0 업데이트 대응에서 실제로 쓴 절차)

2편 첫날은 1편의 "게임 업데이트 대응"을 크게 한 번 하는 것과 같습니다. 2026-09-25 업데이트(6.7.0) 때 실제로 이 순서로 했고, 각 단계가 무엇을 잡아냈는지 함께 적습니다.

1. **덤프부터 뜹니다.** MelonLoader가 `Il2CppAssemblies`를 새로 만들면 SignatureDumper로 `Decompiled/`를 다시 뜹니다. **이전 덤프를 지우기 전에 복사해 두십시오.** 6.7.0 BGM 사고(아래 Phase 4)는 "업데이트 전 덤프에는 없던 `AudioManager.m_BgmOneShotSource`"를 비교로 찾았습니다. 2편에서는 1편 덤프가 그 "이전 덤프" 역할을 합니다.
2. **빌드가 되는지 봅니다.** `typeof`/`nameof`로 쓴 게임 멤버는 여기서 전부 걸립니다. 컴파일 오류 목록이 곧 "이름이 바뀐 멤버" 목록입니다.
3. **게임에서 한 번 켜고 `[PatchInstaller]`, `[PatchHealth]` 줄을 봅니다.** 1편은 패치 메서드가 현재 게임으로 JIT 되지 않으면 그 클래스를 **걸지 않고** 이름을 남깁니다([PatchInstaller.cs](../../muse%20dash%20test/Patches/Diagnostics/PatchInstaller.cs)). 깨진 패치가 걸리면 기능 하나로 끝나지 않고 게임 원본까지 멈추기 때문입니다(JIT에 실패한 Prefix는 호출마다 예외를 내고 원본 메서드도 실행되지 않음). 2편 모드에도 이 구조를 먼저 옮기십시오.
4. **컴파일러가 못 보는 이름을 대조합니다.** Harmony 대상 문자열, 래퍼·리플렉션 멤버명을 덤프와 맞춰 봅니다. 주의할 점 두 가지:
   - Harmony는 대상을 **그 클래스에 직접 선언된 멤버에서만** 찾습니다(`AccessTools.DeclaredMethod`). 부모 클래스로 옮겨진 메서드는 "있는데도" 못 겁니다. 1편 `PatchHealthCheck`는 부모까지 찾기 때문에 이 경우 초록불이 켜질 수 있습니다.
   - `ModReflection`은 `m_` 접두사·대소문자 무시까지 허용해서 **틀린 멤버를 조용히 잡을 수 있고**, 쓰기 전용 대상이 읽기 전용 프로퍼티면 **조용히 실패**합니다(1편 `MusicInfo.albumIndex` 등이 그렇습니다). "찾았는가"와 "쓸 수 있는가"를 따로 보십시오.
5. **덤프로 못 보는 것은 로그로 확인합니다.** GameObject 이름, 캔버스·마스크 순서, 애니메이션 이름. 1편 체크리스트([CHECKLIST.md](../guides/CHECKLIST.md))의 "이름 부분 일치로 고르지 않았는가", "마스크와 캔버스 순서를 확인했는가" 항목이 둘 다 이 단계에서 난 사고입니다.
6. **폴백 경로까지 실제로 밟습니다.** 커스텀 곡(BMS 있음)뿐 아니라 BMS·앨범이 없어 `0-0`으로 폴백하는 곡도 배틀까지 들어가 봅니다.

---

## 🛠️ Phase 1. 리플렉션 계층 설정 및 래퍼 준비

2편 빌드는 1편과 다른 변수명, 새로운 컴포넌트 네이밍, 그리고 강력한 난독화가 적용될 것입니다. 정적 종속성이 깨져 컴파일 조차 되지 않는 참사를 피하기 위해 **[Il2CppWrapperBase.cs](../../muse%20dash%20test/Patches/Common/Il2CppWrapperBase.cs)**와 **[ModReflection.cs](../../muse%20dash%20test/Patches/Common/ModReflection.cs)**를 그대로 복사해 이식합니다.

### 1. ILSpy 스캔 타겟
- **게임 에셋 로더 계층**: `AlbumsInfo`, `MusicInfo`에 대응되는 핵심 데이터 클래스명을 탐색합니다.
- **매핑 변경 예시**:
  ```csharp
  // 1편
  public string uid => Get<string>("uid");
  
  // 2편 추측 (예: uid 변수명이 uniqueSongId로 난독화/변경 시)
  public string uid => Get<string>("uniqueSongId");
  ```
  *우리는 래퍼 프로퍼티의 문자열 키만 교체해주면 모드 소스 전체를 유지할 수 있습니다.*

> [!CAUTION]
> **멀티플레이어/안티치트 대비 — "관리형이니 안전하다"는 착각 주의**:
> 만약 뮤즈 대시 2에 멀티플레이어(협동/경쟁 모드)나 안티치트가 탑재된다면, 게임 메모리를 변조하는 훅은 탐지와 제재(계정 정지)로 이어질 위험이 큽니다.
> 여기서 흔히 하는 착각이 "MelonLoader의 managed IL2CPP 래퍼와 Harmony만 쓰면 네이티브 훅이 아니다"입니다. **사실이 아닙니다.** IL2CPP 게임에서 Harmony 패치는 `Il2CppInterop.HarmonySupport`가 게임 함수의 네이티브 포인터에 디투어를 거는 방식으로 적용됩니다(위 "먼저 알아둘 것" 참고). 모드가 기계어를 직접 쓰지 않을 뿐, 안티치트 입장에서는 코드 패치와 구분되지 않을 수 있고, MelonLoader 주입 자체도 탐지 대상이 될 수 있습니다.
> 래퍼(`Il2CppWrapperBase`)와 리플렉션이 주는 것은 **게임 업데이트 내성**(필드명이 바뀌어도 크래시 대신 조용히 실패)이지 안티치트 회피가 아닙니다. 2편에 온라인 요소가 있다면 **오프라인 전용으로만 쓰고, 기록 업로드 경로를 막았는지부터 확인**하는 편이 현실적인 방어입니다. 1편은 오프라인 샌드박스가 켜졌을 때만 `peropero.net` 요청(`StandardNetworkRequest.SendRequest`)을 막고, 꺼져 있을 때 커스텀 곡 결과가 게임의 점수 업로드(`GameAccountSystem.UploadScore`, `SceneUploadResultData.musicUid`)로 가는 길을 따로 막는 코드는 없습니다(2026-09-28 확인, 업로드가 실제로 일어나는지는 미확인).


---

## 🏷️ Phase 2. 로비 UI 및 커스텀 카테고리(태그) 주입

2편 역시 정렬 및 다국어 탭 관리를 수행할 것이므로, 진입 탭 UI가 스폰되는 지점을 찾아 낚싯바늘을 던집니다.

### 1. ILSpy 분석 및 훅 타겟 찾기
- **검색 키워드**: `MusicTagManager`, `TagController`, `CategoryPanel`, `InitTags`
- **목표**: 탭 버튼들이 리스트 형태로 스폰되고 초기화가 마무리되는 시점의 **Postfix 후크**를 설치합니다.

### 2. 가상 곡 DB 등록 시퀀스
- 2편에도 존재할 `GlobalDataBase` 혹은 `SongDatabase` 형태의 싱크 객체를 확보합니다.
- 새로운 인스턴스를 무에서 `new`로 만들면 유니티 네이티브 메모리가 크래시될 수 있습니다. 1편에서 성공했던 **얇은 복제(Thin Clone, `MemberwiseClone()`)** 방식을 적용해 기존 순정 곡 정보를 복제한 뒤, 가상 곡 식별자(`1999-1`, `1999-2`, …)를 주입합니다.
- 가상 곡들이 등록된 앨범(`1999-0`) 목록 또한 글로벌 리스트에 삽입합니다.

### 2.1 복제 원본의 DLC 식별자와 커스텀 객체 분리
순정 곡이나 앨범을 얇게 복제하면 원본의 `needPurchase`, `pay_ids` 같은 상품 메타데이터도 복제본에 남을 수 있습니다. (1편 6.7.0 실측: 이런 멤버는 앨범 `AlbumsInfo`에만 있고 곡 `MusicInfo`/`MusicExInfo`에는 없습니다. 코드가 함께 찾는 `dlc`는 어디에도 없습니다. 2편에서는 상품 정보가 어느 타입에 붙는지부터 확인하십시오.) 이 경우 커스텀 객체가 원본 DLC 상품으로 잘못 분류될 수 있으므로, **가상 복제본에 한해서만** 상속된 상품 식별자를 제거해야 합니다.

이 정리는 원본 데이터, 실제 구매 상태, DLC 소유권 또는 정식 콘텐츠 잠금을 변경하는 절차가 아닙니다. 2편 이식 시에도 정리 함수가 반드시 새로 생성한 커스텀 객체만 받도록 호출 경계를 검증해야 합니다.

또한 `MemberwiseClone()`은 하위 객체 참조를 공유할 수 있습니다. `MusicExInfo`, `AlbumExInfo`에 대응하는 확장 객체를 정리할 때는 원본과 참조가 분리되었는지 먼저 확인하고, 공유 중이면 해당 하위 객체도 복제해야 합니다.

> [!IMPORTANT]
> **Live2D 앨범 커버 경고 - 얇은 객체 복사 필수**:
> 뮤즈 대시 2는 트레일러 기준 **앨범 커버가 Live2D(실시간 물리 연출)**로 정교하게 구성되어 있습니다. 이 Live2D 컴포넌트는 유니티 네이티브 메모리와의 포인터 결합도가 매우 높아, 임의로 에셋을 갈아끼우거나 수동 인스턴스화(`new`)하려고 하면 100% 런타임 오류가 납니다.
> 따라서 앨범 정보(`AlbumsInfo` 계열) 주입 시, 기존 순정 앨범 중 Live2D가 완벽히 구성된 대상을 선택하여 **반드시 얇은 객체 복사(`MemberwiseClone()`)를 수행**해야 합니다. 복사 완료 후 타이틀 텍스트나 인덱스 등 메타데이터만 기입하여 Live2D 포인터 오염을 사전에 차단하는 것이 모딩의 성패를 가릅니다.


> [!WARNING]
> **트러블슈팅 - 태그 이름이 빈 문자열("")로 나오는 현상**:
> 커스텀 태그 명칭을 추가 및 수정했는데 UI 탭 라벨이 빈 칸(빈 문자열)으로 표시되는 문제가 발생한다면, **가장 먼저 다국어 로컬 네임 사전(Languages Dictionary)에 등록된 언어 키값을 의심**해야 합니다.
> 게임이 현재 기동 중인 로컬 언어 환경(예: `Korean`, `English`, `Japanese`, `ChineseSimplified` 등)의 시스템 언어 코드 키가 주입하려는 딕셔너리에 정확히 포함되어 있는지, 문자열 스펠링에 오타가 없는지 ILSpy 번역 관리 매니저를 통해 반드시 교차 확인하십시오.

### 3. 주입 성공 시의 직관적인 디버그 로그 예시
가상 곡과 가상 앨범 등록 작업을 완료한 뒤, 모드가 올바르게 주입에 성공했는지 검증하기 위해 다음과 같이 **각 단계별 성공여부를 추적하는 디버그 로그**를 콘솔 및 파일에 남기도록 코딩하는 것이 좋습니다.

```text
// 디버그 로그 분석 예시
[성공] m_AllMusicInfo 맵에 '1999-1' 신규 주입 완료!
 ➡️ 게임의 전체 곡 메타데이터 DB 테이블에 가상 곡 UID를 충돌 없이 등록 완료했다는 뜻.

[대성공] GetMusicInfoFromAll('1999-1') 검증 성공! 반환된 곡 이름: '테스트'
 ➡️ 주입 직후, 게임 조회 API를 가상 UID로 다시 역호출했을 때 Null 오류 없이 우리가 설정한 실제 가상 곡 인스턴스가 완벽히 반환됨을 런타임 검증했다는 뜻.

[성공] 커스텀 태그 노출 목록에 '1999-1' 추가 완료!
 ➡️ 새로 등록된 커스텀 카테고리 태그 탭 아래에 가상 곡 번호가 안정적으로 소속 및 배치되었다는 뜻.

가상 곡 생성 완료: count=1
 ➡️ hwa/ 디렉터리 내 곡 폴더들을 스캔하여 가상 곡을 의도한 개수만큼 완성했다는 뜻.

[성공] 얇은 복제 방식으로 DBConfigAlbums.m_Items에 가상 앨범(1998-0) 주입 완료!
 ➡️ Live2D 앨범 커버의 메모리 꼬임을 방지하기 위해 순정 데이터를 복제하여 가상 앨범 목록에 추가 완료했다는 뜻.
```

> **1편의 실제 식별자** ([CustomContentIds.cs](../../muse%20dash%20test/Core/CustomContentIds.cs)): 가상 앨범 UID는 `1999-0`, 가상 곡은 `1999-1`부터(앨범과 겹치지 않게 1부터 셈), 태그 인덱스는 `1999`, 태그 UID 문자열은 `tag-muse-dash-test`입니다. 위 마지막 로그의 `(1998-0)`은 코드에 남은 옛 문구이고 실제 주입되는 앨범은 `1999-0`입니다(`jsonName`도 옛 이름 `custom_album_1998_0`을 그대로 씁니다). 세이브 정화도 `1999-` 접두사 하나만 봅니다.
>
> **가상 곡 UID는 `hwa/` 폴더를 이름순 정렬한 순번입니다.** 곡 폴더를 추가·삭제·개명하면 UID가 통째로 밀립니다. 1편은 이 때문에 기록 파일 키를 UID가 아니라 곡 폴더 이름으로 바꿨습니다([CHECKLIST.md](../guides/CHECKLIST.md)). 2편에서 곡별 데이터를 만들 때는 처음부터 순번이 아닌 키를 쓰십시오.

### 💡 1편의 커스텀 태그 주입 성공 공식 10줄 요약


1. **타이밍 후킹**: 게임이 순정 태그를 메모리에 올리는 시점(`MusicTagManager.InitAlbumTagInfo`)의 Postfix 훅을 잡는다.
2. **태그 인스턴스화**: 가상 태그의 고유 식별 명칭(`tag-muse-dash-test`, 태그 인덱스 `1999`)을 가진 `AlbumTagInfo` 객체를 새로 생성한다.
3. **다국어 매핑**: `Korean`, `English` 등 시스템 번역 사전(Dictionary)에 매치할 태그 제목 명칭을 등록하여 주입한다.
4. **순정 곡 얇은 복제**: 메모리 포인터 꼬임 방지를 위해 기존 `MusicInfo`를 `MemberwiseClone()`하여 가상 곡으로 복제한다. 하위 객체 `m_MusicExInfo`는 참조가 공유되므로 따로 복제한다.
5. **가상 곡 주입**: 복제된 곡에 가상 UID(`1999-1`부터)를 할당하고 글로벌 곡 DB(`GlobalDataBase.dbMusicTag.m_AllMusicInfo`)에 추가한다.
6. **가상 앨범 주입**: 기존 `AlbumsInfo`를 복제해 가상 앨범(`1999-0`)을 생성하고 앨범 리스트(`DBConfigAlbums.m_Items`)에 등록한다.
7. **태그-곡 바인딩**: 가상 곡의 UID를 주입 대상인 가상 태그의 곡 리스트(`music_list`) 컬렉션에 문자열로 등록한다.
8. **글로벌 태그 등록**: 가상 태그 정보를 최종 글로벌 태그 DB에 얹고 정렬 함수(`AddCustomAlbumTagsSort`)를 실행해 빌드한다.
9. **조회 인터셉터 우회**: 곡 스크롤 시 가상 UID에 대한 `AlbumsInfo` 반환 훅을 설치해 Null 예외를 방지하고 가상 앨범 정보를 뱉게 한다. `MusicInfo.albumUidName`/`albumIndex`는 게임 쪽이 읽기 전용 계산 프로퍼티라 값을 써 넣을 수 없어서, getter 자체를 Prefix로 가로챈다. 곡명·아티스트는 현지화 DB의 "행 번호" 조회(`GetLocalAlbumInfoByIndex`)와 `MusicInfo.GetLocal`까지 가로채야 화면에 뜬다.
10. **내장 이미지 오버라이드**: 탭 컴포넌트(`AlbumTagToggle.Init`) 후크로 진입해 DLL 리소스에서 이미지 바이너리를 추출하고 태그 아이콘을 강제 오버라이딩한다.



---

## 🎮 Phase 3. 인게임 차트 변조 파이프라인 개방

플레이어가 가상 곡을 클릭하고 배틀 씬으로 진입할 때, 원본 차트 대신 우리가 준비한 BMS 데이터를 밀어 넣는 파이프라인입니다.

### 1. ILSpy 분석 및 훅 타겟 찾기
- **검색 키워드**: `SetRuntimeMusicData`, `LoadChart`, `InitializeStage`, `MusicData`
- **목표**: 게임이 순정 차트를 런타임 노트 리스트로 **다 조립한 직후**, 노트 생성기(`NoteSpawner`)가 그 리스트를 읽기 전 지점을 찾습니다. 1편은 `DBStageInfo.SetRuntimeMusicData`의 **Postfix**입니다(Prefix가 아님). 게임이 만든 리스트를 템플릿 삼아 쓰기 때문에 "만들어진 뒤"여야 합니다.

### 2. 차트 변조 메커니즘 (1편 실제 흐름, [DBStageInfoExperimentChart.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.cs))
- 순정 리스트의 `[0]`(앵커)과 `[1]`(노트 템플릿) 두 개만 복제해 둡니다. 새 노트는 전부 이 템플릿을 복제한 뒤 UID·타입·시각만 바꿔 만듭니다. **그래서 노트 연출 일부(des, key_audio, scene 등)는 "숙주로 쓴 순정 곡"을 따라갑니다.**
- 리스트를 `Clear()`하고 앵커를 되돌려 넣은 뒤, `BmsParser`가 해석한 노트를 `Add`합니다. 이어서 더블 노트 상태를 맞추고 `showTick` 순으로 정렬합니다.
- **복제 함수는 필드를 하나씩 손으로 복사합니다.** 게임 업데이트로 노트 데이터에 필드가 생기면 그 필드는 조용히 빠집니다. 1편 6.7.0 대조에서는 `MusicData` 15개, `NoteConfigData` 27개, `MusicConfigData` 6개 멤버를 빠짐없이 복사하고 있었습니다. 2편에서는 이 대조를 가장 먼저 다시 하십시오.
- 노트끼리는 `objId`(1편은 `short`)로 서로를 가리킵니다. 롱노트는 잘게 전개되어 노트 수가 빨리 불어나므로 상한을 감시합니다.
- 노트 타입 값은 게임 enum과 같아야 합니다. 1편 6.7.0의 `NoteType`: 0 None(보스 동작 지시용), 1 Monster(일반), 2 Block(톱니), 3 Press(롱), 4 Hide(고스트), 5 Boss, 6 Hp(하트), 7 Music(음표), 8 Mul(샌드백), 9 SceneChange. 모드 쪽 상수 이름(`NoteTypes.Boss = 0`)은 게임의 `None`에 대응하니 이름만 보고 옮기지 마십시오.
- 이때, 2편에 새롭게 추가된 기믹 노트(예: 3선 노트, 특수 연출 노트 등)가 있다면 6자리 UID 파싱 규칙([BMS_PARSING.md](../guides/BMS_PARSING.md))에 해당 노트 유형 코드를 분기 처리해 얹어주면 끝납니다.

---

## 🎬 Phase 4. 커스텀 오디오(BGM) 및 비디오(BGA) 탈취

2편은 그래픽과 연출이 발전했을 것이므로 배경 영상(BGA) 지원 여부가 모딩의 퀄리티를 좌우할 것입니다. 특히 **소팅 속성(Sorting Layer/Order 및 Z축 거리)과 재생/싱크 타이밍을 정밀하게 정렬하고 맞춰야** 안정적인 연출을 쉽게 완성할 수 있습니다.

### 1. AudioSource 감지 및 교체
- 게임 플레이 중 배경 음악을 재생하는 전용 오디오 소스를 검색합니다.
- **ILSpy 키워드**: `MusicAudioSource`, `BgmPlayer`, `BattleBgm`, `AudioManager`
- **동작**: 유니티 배틀 씬 진입 시점에 해당 오디오 소스를 낚아채 정지시킨 뒤, `UnityWebRequestMultimedia.GetAudioClip`으로 디스크의 `.ogg`를 가져와 오버라이딩합니다.

> [!WARNING]
> **오디오 소스를 이름으로 고르지 마십시오 (1편 6.7.0 실제 사고)**:
> 1편은 이름에 "bgm"이 든 첫 `AudioSource`를 골랐는데, 그런 소스가 `"BGM"` 하나뿐이라 우연히 맞았습니다. 6.7.0이 `AudioManager.m_BgmOneShotSource`(GameObject `"BGM_OneShot"`)를 추가하자 그쪽이 먼저 걸려 원곡과 커스텀 곡이 겹쳐 들렸습니다. 지금은 게임이 들고 있는 참조(`Singleton<AudioManager>.instance.bgm`)를 1순위로 씁니다([GameBgmSource.cs](../../muse%20dash%20test/Core/GameBgmSource.cs)).
> **게임 참조를 쓰면 생기는 부작용도 챙기십시오.** 이제 커스텀 클립이 게임과 **공유하는** BGM 소스에 들어갑니다. 1편은 주입 때 `loop=true`, `playOnAwake=false`를 그 소스에 쓰고 배틀이 끝나도 되돌리지 않으며, 메뉴 미리듣기 보호 훅(`AudioSource.clip` setter Prefix)도 같은 소스를 봅니다. 2편에서는 공유 소스에 쓴 설정을 배틀 종료 때 원래대로 돌리는 것까지 한 묶음으로 만드십시오.

### 2. 3D Quad 메쉬 기반 비디오 렌더링 이식
- 2편에서도 카메라 구조는 크게 변하지 않을 것입니다. `Camera.main` 하위에 가상의 Quad 메쉬(`VideoBackgroundQuad`)를 생성해 부모-자식 관계를 형성합니다.
- 카메라 해상도 종횡비(`aspect`)와 사이즈를 계산하여 꽉 채운 캔버스를 빌드하고, `VideoPlayer` 컴포넌트를 부착해 디스크의 `.mp4` 파일을 Material Override 모드로 출력합니다.

> [!IMPORTANT]
> **안정적인 BGA 출력을 위한 소팅 속성 및 Z축 배치 설정**:
> - **로컬 Z축 위치**: `transform.localPosition`을 적절한 깊이 값(예: `25f`)으로 설정하여 카메라 클리핑 플레인 내에 안정적으로 두어야 합니다.
> - **소팅 레이어 및 오더**: UI 및 노트, 캐릭터 등의 인게임 스프라이트를 가리지 않도록 `MeshRenderer`의 `sortingLayerName`은 `"Background"`, `sortingOrder`는 `-17` 이하의 충분히 낮은 값으로 지정하여 백그라운드 영역으로 완벽히 밀어내야 합니다.

> [!TIP]
> **오디오-비디오 싱크 및 재생 타이밍 보정**:
> - 프레임 드랍이나 로딩 지연으로 인해 BGM 오디오와 BGA 비디오의 싱크가 틀어지는 것을 방지해야 합니다.
> - 인게임 진행도 게이지(예: `sldProgress.value`)의 비율을 활용해 오디오/비디오의 목표 재생 시간(`expectedTime = progressRatio * duration`)을 계산합니다.
> - 실시간 오차가 일정 범위(예: `0.2초`) 이상 벌어질 때만 강제로 재생 시점(`time`)을 보정하도록 구현하되, 한 번 보정이 일어난 후에는 짧은 쿨다운(예: `0.5초`)을 설정하여 동기화 연산이 무한 충돌하는 현상을 차단해야 끊김 없는 부드러운 연출이 가능합니다.


---

## 🎯 Phase 4.5. 정확도 보정(Accuracy Override) 및 결과 화면(Victory) 연출 개조

1편 모드에서 발생했던 가장 정밀한 이슈 중 하나는 커스텀 차트 플레이 시 **정확도가 비정상적으로 계산(예: 올 퍼펙트인데 85.4%로 표시)되거나, 올 퍼펙트 전용 배너가 나오지 않는 문제**였습니다. 2편 모딩 시에도 동일한 원리로 정확도 공식 부정합이 발생할 것이므로, 이를 해결하기 위한 설계가 필요합니다.

### 1. ILSpy 분석 및 훅 타겟 찾기
- **정확도 함수 검색**: `GetAccuracy`, `GetTrueAccuracy`, `GetTrueAccuracyNew`
- **결과 화면 매니저 검색**: `PnlVictory`, `VictoryManager`, `OnShowVictory`
- **목표**: 
  - 판정 점수가 누계되는 인게임 통계 객체(`TaskStageTarget` 계열)의 정확도 산출 메서드에 **Postfix 후크**를 걸어 올바른 런타임 계산값으로 덮어씁니다.
  - 곡이 끝나는 연출 시점에 개입하여 판정 결과 이미지와 컴포넌트를 오버라이딩합니다.

### 2. 정밀 정확도 산출 공식 설계
2편에서도 판정 통계 변수들(`Perfect`, `Great`, `Miss`, `JumpOver`/톱니바퀴, `Energy`/하트, `BluePoint`/음표 등)은 각기 다른 필드에 보관될 것입니다.
- **분모 스캔**: 차트 로드 시점에 더미/이벤트 노트를 제외하고 실제 판정이 발생하는 노트를 종류별로 전수 조사해 **진짜 분모값**을 미리 집계합니다.
  - 일반 판정 노트(단타, 롱노트 머리, 샌드백 등) 수량: `TotalStandard`
  - 톱니바퀴 수량: `TotalGears`
  - 하트 수량: `TotalHearts`
  - 음표 수량: `TotalBlueNotes`
- **반환값 재보정 공식**:
  - **`GetTrueAccuracy()`** (일반 노트 대상):
    $$\text{Accuracy} = \frac{\text{Perfect} + \text{Great} \times 0.5}{\text{TotalStandard}}$$
  - **`GetTrueAccuracyNew()`** (모든 오브젝트 대상):
    $$\text{Accuracy} = \frac{\text{Perfect} + \text{Great} \times 0.5 + \text{JumpOver} + \text{EnergyCount} + \text{BluePoint}}{\text{TotalStandard} + \text{TotalGears} + \text{TotalHearts} + \text{TotalBlueNotes}}$$
  - **`GetAccuracy()`** (최종 노출용): UI에 표시할 소수점 3째 자리 반올림 값을 반환합니다.

### 3. 결과 화면 ALL PERFECT 골드 배너 동적 주입
- 플레이어의 판정 결과가 **Great 0, Miss 0, Full Combo(정확도 100%)**를 만족하는 경우(`isAllPerfect`), 기존 `"FULL COMBO"` 낱개 문자 이미지를 비활성화합니다.
- 새 텍스트 오브젝트를 생성하고, 인게임 HUD에서 추출한 외곽선 폰트(예: `LuckiestGuy-Regular`)를 적용해 그라데이션 색상의 **"ALL PERFECT !"** 배너를 생성·배치합니다.

---

## 🔒 Phase 5. 안전판 가동 (세이브 파일 오염 방지)

가상 곡(`1999-0`)을 클리어했을 때 기록되는 점수와 결과 정보가 정식 세이브 파일 구조에 포함되면, 추후 모드를 껐을 때 세이브 데이터 손상(데이터 파싱 실패) 에러를 일으킬 수 있습니다.

### 1. ILSpy 분석 및 훅 타겟 찾기
- **검색 키워드**: `Save`, `Serialize`, `SaveDataManager`, `LocalSave`, `CloudSave`
- **목표**: 게임 저장 명령이 실행되는 시점의 **Prefix 후크**를 뚫어줍니다. 1편은 `Il2CppAssets.Scripts.PeroTools.Nice.Datas.DataManager.Save()`입니다(6.7.0 기준 같은 클래스에 `_Save()`, `FixSave()`도 있으니 2편에서는 **저장 경로가 하나뿐인지** 먼저 확인하십시오).

### 2. 세이브 정화(Sanitization) (1편 실제 동작, [SaveDataManagerPatch.cs](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs))
- 저장 직전 `1999-`로 시작하는 항목을 걸러냅니다(`CustomContentIds.IsVirtualContent`). 보는 곳: 각 데이터 묶음의 필드 키, `Achievement.highest`(최고 기록), `Achievement.recentPassLevelData`(최근 플레이), `easy_pass`/`hard_pass`/`master_pass`(난이도별 클리어 목록).
- **커스텀 곡 기록은 게임 세이브 대신 모드 전용 폴더(`record/`)에 따로 씁니다**([CustomRecordStore.cs](../../muse%20dash%20test/Core/CustomRecordStore.cs)). 2편에서도 "게임 세이브에서 지우기"와 "우리 쪽에 따로 남기기"를 짝으로 옮기십시오.
- **정화가 못 막는 경로**: 가상 UID가 아니라 **순정 UID로 커스텀 차트를 치는 경우**입니다. 1편은 매니페스트의 `uid:`로 지정한 숙주 곡을 실험 모드에서 고르면 커스텀 차트를 적용하는 경로(`ExperimentHost`)가 있는데, 이 판의 기록은 `1999-`가 아니라서 정화 대상이 아닙니다. 2편에서는 이런 경로를 두지 않거나, 둔다면 그 기록도 막아야 합니다.

---

## 🎨 Phase 6. Spine 커스텀 스킨 주입 (Custom Spine Skin Injection)

2편에서도 캐릭터 및 보스 렌더링에 Spine 2D 애니메이션이 계속 사용될 가능성이 90% 이상입니다. 커스텀 스킨 주입은 캐릭터 3D 메쉬를 바꾸지 않고도 개별 `.png` / `.atlas` / `.json` 세트를 런타임에 갈아끼우는 핵심 기술입니다.

### 1. Spine 에셋 주입 메커니즘 (`CustomSkinInjector`)
- **스킨 세트 폴더 스캔**: `skin test/<set_name>/` (예: `skin test/char_3_black/`) 폴더에서 개별 텍스처, 아틀라스, 스켈레톤 JSON 데이터를 감지합니다.
- **ILSpy 분석 및 훅 타겟**:
  - **검색 키워드**: `SpineActionController`, `SkeletonAnimation`, `SkeletonDataAsset`, `CreateRuntimeInstance`
  - **1편 실제 방식** ([Patch_Inject_BlackGirlBattle.cs](../../muse%20dash%20test/Spine/Patch_Inject_BlackGirlBattle.cs), [CustomSkinInjector.cs](../../muse%20dash%20test/Spine/CustomSkinInjector.cs)): Spine 초기화 함수를 건드리지 않습니다. 캐릭터 오브젝트가 준비되는 `SpineActionController.Init(int, int)`와 `OnControllerStart()`의 **Postfix**에서, **GameObject 이름**(`black_girl_battle(Clone)` 등)으로 대상을 고른 뒤 `SpineAtlasAsset.CreateRuntimeInstance` → `SkeletonDataAsset.CreateRuntimeInstance`로 만든 에셋을 `skeletonAnimation.skeletonDataAsset`에 넣고 `Initialize(true)`로 다시 초기화합니다. 6.7.0에서도 주입 성공 로그가 남아 있습니다.
  - 대상 이름은 덤프로 알 수 없어서, `SpineActionController.Awake` Postfix(`Patch_SkinNameProbe`)가 이름에 `battle`이 든 오브젝트를 로그로 찍어 줍니다. 2편에서도 이 탐침부터 옮기면 대상 목록을 빨리 채울 수 있습니다.
- **2편 주의사항**: Spine 런타임 버전(예: Spine 3.8 → Spine 4.x)이 업그레이드되면 아틀라스 헤더 및 메쉬 UV 좌표 포맷이 변경될 수 있으므로, Spine 버전 매칭을 사전에 점검해야 합니다. 1편 6.7.0은 아직 3.8 계열 타입 이름(`ColorTimeline`, `TwoColorTimeline`)을 씁니다(Phase 7이 이 구조에 기대고 있습니다).

---

## 👻 Phase 7. 고스트 노트 알파 유지 및 특수 연출 (Ghost Note Alpha Control)

커스텀 차트에서 투명 노트(Ghost Note)나 특수 연출 노트가 연타/홀드 도중 애니메이션 컨트롤러에 의해 원래의 불투명 알파값으로 강제 복원되는 문제가 생길 수 있습니다.

### 1. ILSpy 분석 및 훅 타겟
- **검색 키워드**: `SpineActionController`, `PlayByKey`, `PlayAnimation`
- **목표**: 노트 오브젝트가 애니메이션을 트는 `PlayByKey(string actionKey, bool isOverride)`의 **Postfix**를 잡습니다(1편 [GhostNoteAlphaHold.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/GhostNoteAlphaHold.cs)).

### 2. 1편에서 실제로 푼 방법 — 코드가 아니라 애니메이션 데이터
- 고스트 노트가 사라지는 원인은 C# 코드가 아니라 **Spine 애니메이션의 알파 키**였습니다. 고스트 노트가 날아오는 동안 도는 액션은 `in`(애니메이션 `in_nor_44`) 하나이고, 이 애니메이션의 컬러 타임라인이 알파를 깎습니다.
- 그래서 `actionKey == "in"`이고 고스트 노트(UID `zzxxyy`의 `xx=17`, 타입 4)일 때, 그 애니메이션의 컬러 타임라인에서 **알파 키만 1로 덮어씁니다.** 이동·스케일 타임라인은 건드리지 않습니다.
- `SkeletonData`는 프로세스 전체가 공유하므로, 덮기 전 알파를 기억해 두었다가 설정이 꺼진 곡에서는 되돌립니다. 곡별 설정은 커스텀 곡이면 `info.txt`, 공식곡이면 `config.txt`를 따릅니다.
- **확인된 막다른 길**(2편에서 다시 파지 않도록): `SetAlpha`, `OnNoteDisappear`, `NoteDisappearLogic`은 고스트 노트에 대해 불리지 않았고, 애니메이션을 통째로 바꾸면 비행 이동까지 사라져 노트가 멈췄습니다. 알파 필드는 노트 데이터(`NoteConfigData`/`MusicData`) 어디에도 없어서 차트 주입 단계에서는 손댈 수 없습니다.
- **아직 열린 문제**: "보이기 설정이 저절로 풀린다"는 증상이 있고, 원인으로 `SkeletonData`가 다시 만들어졌는데 모드 캐시는 "이미 덮었다"고 기억하는 경우를 의심하고 있습니다(임시 진단 코드가 들어가 있음). 2편에서는 캐시 키에 데이터 인스턴스(포인터)를 포함하는 쪽으로 시작하십시오.
- `SpineActionContract`는 알파를 제어하는 장치가 아니라 **애니메이션 목록을 파일로 떨어뜨리는 읽기 전용 탐침**입니다(개발자 스위치로만 켜짐). 위 `in_nor_44`를 찾은 것도 이 덤프였습니다. (씬 전환 때 부르던 빈 `ResetWindow()`는 지웠습니다.)

---

## 🔄 Phase 8. 실시간 캐릭터/외형 교체 (FavGirl & RealTime Swapper) — 🗑️ 제거됨

1편의 FavGirl 실시간 외형 교체(스킬 캐릭터와 외형 캐릭터 분리, `P`/`O` 핫키)는
**2026년 9월 25일 업데이트로 바닐라에 정식 도입되어 모드에서 뺐습니다.** 2편에서도 이 단계는 건너뜁니다.
2편 본편에 같은 기능이 없어서 다시 만들어야 한다면, 1편 구현은 커밋 `42440fb`의
`Integration/RealTimeSwapper.cs`, `Patches/Fav/FavManager.cs`, `Core/FavSave.cs`에서 볼 수 있습니다.

---

## 🎯 Phase 9. 판정 강제(Force Perfect), 오토 플레이 및 HUD 오버레이

모드 시연 영상 제작, 커스텀 차트 난이도 테스트, 봇 시뮬레이션을 위해 판정을 정밀 조작하거나 화면에 키 입력 시각화를 렌더링합니다.

### 1. All-Perfect Parameter Mod (`ForcePerfectPatch`)
- **ILSpy 타겟**: `GameTouchPlay.TouchResult` (터치/키 판정 처리 시점)
- **동작**: 판정이 발생한 직후 결과 코드(`resultCode`)를 Perfect 코드로 덮어써서 판정 파라미터를 조작하고 올퍼펙트를 유도합니다.
- **범위 주의**: 1편은 이 조작과 오토플레이·피버 차단을 **공식곡에도** 적용합니다(`config.txt`로 켜고 끔). 2편이 온라인 기록을 올린다면, 조작된 판정의 기록이 서버로 가지 않게 막는 장치를 함께 두어야 합니다(Phase 1 경고 참고).

### 2. 오토 플레이 스킬 패치 (`AutoPlayPatch`)
- **ILSpy 타겟**: `DBSkill.SetAutoPlay`
- **동작**: 게임의 오토플레이 설정 함수를 가로채 모드 설정 파일(`forceAutoPlay`)의 값으로 강제 덮어씁니다.

### 3. 키 입력 오버레이 & 판정바 UI (`InputOverlay` / `JudgmentBar`)
- 유니티 `OnGUI` 파이프라인에서 매 프레임 동적 텍스처 스타일(`airActiveStyle`, `groundActiveStyle`)을 그려 화면 하단 하드웨어 키 입력 상태와 판정바를 실시간 노출합니다.

---

## 💡 요약: 2편 모딩 전체 분석 타임라인
0. **첫날**: 백엔드(IL2CPP/Mono) 확인 → 덤프 → 1편 덤프와 비교(Phase 0). [MD2_TAG_RETARGET_MAP.md](MD2_TAG_RETARGET_MAP.md)의 "1편 훅 전체 목록"에서 대응 타입이 있는 줄부터 표시.
1. **일주일 이내**: `ModReflection`을 이식해 데이터 래퍼 클래스들의 변수명 정상 바인딩 확인.
2. **이주일 이내**: 로비 UI 태그 주입 훅을 성공시켜 "실험 모드" 카테고리를 리스트 스크롤에 띄우기 성공.
3. **삼주일 이내**: 인게임 진입 훅을 성공시켜 디스크의 커스텀 `.ogg` 재생 및 `.bms` 차트 노트들을 실제 배틀 레인에 렌더링 완료.
4. **한달 이내**: Spine 커스텀 스킨 주입, 고스트 노트 알파 보존, 오토플레이/판정 조작 2편 바인딩 완료.


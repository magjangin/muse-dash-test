**개요**
- 목적: 런타임에 커스텀 곡 UID(예: `999-0`)를 데이터베이스에 등록하고 UI에 선택되도록 처리하는 절차를 정리합니다.

**전제조건**
- `CustomTagPatch.cs` 같은 파일로 앨범/태그를 먼저 주입해야 합니다. (참조: [muse dash test/Patches/UI/CustomTagPatch.cs](muse dash test/Patches/UI/CustomTagPatch.cs))
- Il2Cpp 타입(예: `Il2CppAssets.Scripts.Database.MusicInfo`, `GlobalDataBase.dbMusic`, `GlobalDataBase.dbMusicTag`)에 접근 가능한 상태여야 합니다.

**핵심 개념 요약**
- DB 등록만으로는 UI의 현재 선택(`selectedUid`)이 자동으로 채워지지 않습니다.
- UI에 보여지려면 음악 데이터(`MusicInfo`)를 DB에 삽입한 뒤, 패널(`PnlStage` 또는 `PnlMusicTag`)의 선택 상태를 갱신하거나 해당 패널에 `MusicInfo` 인스턴스를 직접 할당해야 합니다.

**절차 (요약)**
1. `MusicInfo` 인스턴스 생성 및 필드 설정
   - `uid`, `name`, `author`, `music` 등 필요한 속성 채움.
2. 글로벌 음악 DB에 삽입
   - `GlobalDataBase.dbMusic` 또는 게임이 사용하는 음악 리스트(내부 컬렉션)에 추가.
3. 앨범/태그 연결
   - `GlobalDataBase.dbMusicTag`의 `m_MusicUids`/`m_DisplayMusicUids`/`m_AlbumsInfos`와 `stageShowMusicList` 등에 `"999-0"` 추가.
4. UI 선택 트리거
   - 가능한 방법 A: `PnlMusicTag`의 선택 API(또는 스크롤/클릭 시뮬레이션)를 호출해 뷰 아이템 선택을 유도.
   - 가능한 방법 B: `PnlStage` 인스턴스의 내부 `MusicInfo` 필드(또는 프로퍼티)를 리플렉션으로 직접 설정하고 `Refresh()` 호출.

**권장 구현(샘플, 간단)**
- 파일 위치 제안: `muse dash test/Patches/UI/CustomUidRegistrar.cs`

샘플 코드(개념적):
```csharp
// Il2Cpp 타입 사용 예시 (실제 네임스페이스/메서드는 프로젝트에 맞게 조정)
var music = new Il2CppAssets.Scripts.Database.MusicInfo();
music.uid = "999-0";
music.name = "테스트 곡";
music.author = "작성자";
// 필요한 다른 필드들 설정

// 1) DB 내부 리스트에 추가 (예: reflection으로 내부 List를 찾아 Add 호출)
var dbMusic = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusic;
// dbMusic 내부의 컬렉션 필드명을 확인한 뒤 Add/Insert 수행
// 예: dbMusic.m_MusicList.Add(music)  // (실제 필드명 확인 필요)

// 2) 태그/앨범 연결
var dbMusicTag = Il2CppAssets.Scripts.Database.GlobalDataBase.dbMusicTag;
// albumInfo.m_MusicUids 또는 info.m_DisplayMusicUids 등에 "999-0" 추가

// 3) UI 선택 강제 (예: PnlStage에 직접 할당)
var stage = UnityEngine.Object.FindObjectOfType<Il2CppAssets.Scripts.UI.Panels.PnlStage>();
if(stage != null) {
    // 리플렉션으로 내부 MusicInfo 필드/프로퍼티를 찾아 할당
    // 이후 stage.Refresh() 또는 관련 갱신 호출
}
```

**테스트 절차**
- 빌드 후 게임 실행, `Latest.log`에 다음 로그 라인들이 보이는지 확인:
  - `글로벌 데이터베이스에 커스텀 태그/앨범 데이터 등록 완료` (이미 존재)
  - `AddCustomAlbumTagsSort로 커스텀 태그 UID(...) 등록 완료`
  - `PnlMusicTag`/`PnlStage` 로그에서 `selectedUid=999-0` 또는 `uid=999-0` 관련 출력

**주의사항 및 팁**
- IL2CPP 환경에서는 직접 `new`로 Il2Cpp 타입을 생성하거나 내부 컬렉션에 추가하는 방식이 미묘하게 다를 수 있으므로, 리플렉션과 기존 인스턴스 복사를 조합해 안전하게 구현하세요.
- UI 갱신은 메인 스레드(유니티 스레드)에서 수행되어야 합니다. 코루틴 또는 `UnityEngine.Object` 관련 API를 사용하세요.
- 기존 `CustomTagPatch.cs`, `PnlMusicTagPatch.cs`, `PnlStagePatchHelper.cs`의 로그 출력을 참고해 올바른 시점(패널 초기화 직후 등)에 등록/선택 코드를 호출하세요.

**다음 단계 제안**
- 원하시면 제가 `CustomUidRegistrar.cs`의 구현 스켈레톤(실제 리플렉션 코드 포함)과, `Patches/UI/`에 적용할 Harmony 패치를 작성해 드리겠습니다.

**UID 관련 후보 메서드 (코드베이스에서 발견된 항목)**
- `Il2CppAssets.Scripts.Database.MusicInfo` 접근자: `get_uid()` / `set_uid()` — `MusicInfo`의 uid를 읽고 쓸 수 있습니다. (참조: reflect_output.txt)
- `GlobalDataBase.dbMusicTag.AddCustomAlbumTagsSort(int)` — 커스텀 태그 UID를 태그 정렬 목록에 등록합니다. (파일: muse dash test/Patches/UI/CustomTagPatch.cs)
- `GlobalDataBase.dbMusicTag.AddAlbumTagData(int, AlbumTagInfo)` — 앨범/태그 데이터를 글로벌 DB에 최종 등록합니다. (파일: muse dash test/Patches/UI/CustomTagPatch.cs)
- `PnlStagePatchHelper.GetCurrentSelectedMusicUid()` — `PnlStage` 인스턴스에서 현재 선택된 곡의 UID를 검색하는 헬퍼입니다. (파일: muse dash test/Patches/UI/PnlStagePatchHelper.cs)
- `PnlStage` / `LongSongNameController` 관련 패치들 — UI에서 `selectedUid`를 처리하거나 강제 변경 로그를 남기는 위치입니다. (파일: muse dash test/Patches/UI/PnlStagePatch.cs, muse dash test/Patches/UI/LongSongNameControllerPatch.cs)
- `PnlMusicTagPatchLogger.ApplyCustomCellTitle()` — `cell.musicInfo.uid`로 셀을 식별해 제목을 변경합니다; 셀 레벨에서 UID를 검사하는 좋은 예제입니다. (파일: muse dash test/Patches/UI/PnlMusicTagPatch.cs)
- `PnlMusicUtils.ExtractMusicInfo(...)` — 패널 객체에서 `MusicInfo`를 추출하는 유틸리티로, UI→데이터 연동 흐름을 이해하는 데 유용합니다. (파일: muse dash test/Patches/UI/PnlMusicUtils.cs)
- `DBStageInfoExperimentChart`의 UID 처리 로직 — 노트/스테이지 도메인에서 UID를 생성/변환/할당하는 예시가 존재합니다. (파일: muse dash test/Patches/Database/DBStageInfoExperimentChart.cs)

위 후보들은 `999-0`을 생성·등록·선택 상태로 만드는 구현에서 직접 참고하거나 호출할 수 있는 지점들입니다. 문서 상단의 절차와 조합해 실제 구현을 진행하면 됩니다.

# 디컴파일·모드 대조 리뷰 (2026-09-11)

검토 기준: 원격과 동기화된 main. 코드 변경 없이 차트 주입, 값 타입 쓰기,
배틀 생명주기, 판정/피버 패치, 가상 곡 폴백, Discord 갱신을 정적으로 검토했다.
전체 모드의 동작 보증은 아니다. Decompiled_AssemblyCSharp의 메서드는 네이티브 호출
래퍼이므로 시그니처·필드·박싱 방식은 확인할 수 있지만 원본 네이티브 본체의
분기와 실행 순서를 모두 증명하지는 못한다. inferred_game_code 문서는 추론 자료다.

## 발견 사항

1. **[P2] BMS 판정 시간을 배치 정밀도로 반올림한다.**
   `DBStageInfoExperimentChart.Bms.cs:126-128`의 일반 노트 StartTick은
   `NormalizeChartValue`(소수 둘째 자리)를 거친다. `MoveNote`에서 셋째 자리로
   다시 반올림해도 이미 잃은 정밀도는 돌아오지 않는다. 예를 들어 서로 다른
   레인의 1.231초와 1.234초 노트가 모두 1.23초가 되어
   `ApplyBmsDoubleState`에서 동시 더블로 묶일 수 있다. 타격 시각 자체에도
   최대 약 5ms 오차가 생긴다. 같은 파일 35-36행은 홀드 시작과 길이를 각각
   반올림하여 끝 시각의 오차도 누적시킨다. 판정 시각과 길이는 timing 정밀도를
   유지하고 showTick 등 배치값에만 chart 정밀도를 적용하는 것이 적절하다.
   원본 `MusicData.tick`은 Decimal 필드다(디컴파일 MusicData.cs:87).

2. **[P2] Discord 기능 설정을 우회하는 갱신 경로가 있다.**
   `CustomPlaySession.cs:45-55`는 `UpdateForSelection` 뒤에 게임의
   `DiscordManager.SetUpdateActivity`를 직접 호출하며 EnableDiscordRPC를
   확인하지 않는다. `DiscordManagerDebugPatch.cs:25`의 Prefix도 설정 검사 없이
   levelInfo/isPlaying을 바꾼다. 따라서 EnableDiscordRPC=false여도 모드가
   곡 선택 상태를 갱신하고 원본 요청을 변조할 수 있다. 초기화/Update만 막는
   현재 게이트로는 부족하다. 디컴파일 DiscordManager.cs에서 해당 메서드가
   실제 네이티브 호출을 수행하는 것을 확인했다. 외부 Discord 수신은 미검증이다.

## 런타임 확인이 필요한 위험

- `ChangeFeverValuePatch.cs:8`은 정확히 `AddFever(int)`를 지정한다.
  디컴파일 AbstractFeverManager.cs:494에서 이 오버로드는 virtual이며
  `il2cpp_object_get_virtual_method`를 사용한다. 비가상 오버로드는
  `AddFever(int, bool)`이다. 저장소 체크리스트의 virtual 훅 위험에 해당한다.
  시그니처가 맞는 것과 안전하게 실행되는 것은 다르므로, 실제 크래시나
  차단 실패를 확정하지 않았다. 호출 경로 계측 후 개입 지점을 정해야 한다.

## 확인 결과와 한계

- git fetch origin 성공, 시작 시 HEAD...origin/main = 0/0, 작업 트리 깨끗함.
- Release 빌드: 경고 0, 오류 0. ModsDir를 저장소 artifacts/review-build로
  지정하여 리뷰 빌드가 게임 설치본을 교체하지 않도록 했다.
- 로직 테스트: `dotnet run --project "muse dash test.LogicTests/muse dash test.LogicTests.csproj" -c Release --no-restore`, 80개 성공.
  이 테스트들은 IL2CPP 차트 주입/Harmony 실행을 검증하지 않는다.
- MusicData.noteData/configData의 getter는 il2cpp_value_box로 사본을 만든다.
  검토한 MoveNote/정렬/ApplyNoteSpec은 지역 변수 수정 후 setter로 되쓰고 있다.
- TouchResult, StageBattleComponent.Pause/Resume, SetRuntimeMusicData의 검토한
  패치 인자들은 디컴파일 시그니처와 일치했다.
- 가상 곡 생성은 원본 조회 실패 시 0-0을 조회하며 music 에셋 키를 유지한다.
  정상 및 BMS/앨범 없는 폴백의 실제 배틀 진입은 이번 리뷰에서 실행하지 않았다.
- 게임 Mods에는 모드 DLL 1개(502272 bytes)가 있다. SHA256은
  E48F52E08DF9AA4628507F69C2EFB64B1A3AE218202655464DF1D38B5E9A4CBE이며
  저장소의 기존 Debug DLL과 일치한다. 이번 Release 빌드의 SHA256은
  8031C5566757659BF4C530DC0E832150CAFD24E7DA9BDB9937C28B3CEFADA1EE다.
  이 차이만으로 게임 설치본이 오래되었다고 판단할 수는 없다.
- Latest.log 수정 시각은 2026-09-06이다. 이번 빌드의 실행 증거로 사용하지 않았다.
- docs/guides/CHECKLIST.md를 검토했다. 실행 코드 수정 및 배포는 수행하지 않았다.

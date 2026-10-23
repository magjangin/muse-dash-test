# Project Instructions

- If a source file grows beyond 600 lines, automatically refactor it into smaller, focused units before adding more substantial logic.
- Keep refactors behavior-preserving unless the user explicitly asks for behavior changes.
- After changes, run `dotnet build` when practical.
- Review `docs/guides/CHECKLIST.md` before finishing a code change and before publishing a release. Every item there records an actual past incident in this repo, not general advice — in particular, verifying the fallback path (no BMS/album → `0-0` fallback → actually enter battle) and checking that the build really landed in the game's `Mods` folder. When a new incident is worth preventing, add an item; when its rationale no longer holds, delete it. (단, 단순 문서 수정이나 게임과 무관한 독립 툴(`SignatureDumper`) 수정 시에는 게임 런타임 관련 항목을 생략하고 해당 범위의 빌드/테스트만 확인합니다.)
- After completing work, always create a git commit.
- Before starting any work, run `git fetch origin` and check `git status -sb` for `ahead/behind` against `origin/main`. This repo is worked on from multiple clones/sessions, and unsynced starts have caused real divergence before (duplicate reimplementation of the same feature, docs reorganized into two different folder schemes on the same day). If behind, pull/rebase before making changes. If diverged (both ahead and behind), stop and surface it instead of guessing which side is authoritative — that call is the user's.
- **Branching & Working Directory**: 게임 모드 코드 변경은 기능 브랜치(feature branch)를 만들어 작업합니다. 단, 단순 문서 수정이나 사용자의 명시적인 요청이 있을 때는 `main` 브랜치에 직접 커밋할 수 있습니다.
- **Pushing**: 원격 `origin`으로의 `push`는 사용자가 요청하거나 명시적으로 승인했을 때만 실행합니다. Never `git push --force` to `main`, and never delete a branch (local or remote) without explicit confirmation, even if it looks merged or stale.

# 인수인계 (Handover)

마지막 갱신: 2026-10-23 (`feature/all-perfect-table-manipulation`, 인게임 확인 완료)

1. **게임 버전·고정**: Muse Dash 6.7.0, Unity 2019.4.41f1, MelonLoader v0.7.3. 작업용 사본은 `H:\muse dash hwa`(Steam 밖 독립 사본, `steam_appid.txt`만 있고 `.acf`가 없어 Steam Build ID는 기록되어 있지 않음). 자동 업데이트를 받지 않는 사본이라 버전이 고정되어 있습니다.
2. **런타임·디컴파일**: Il2Cpp. 시그니처는 `Decompiled/`(SignatureDumper 출력)와 `H:\muse dash hwa\MelonLoader\Dependencies\Il2CppAssemblyGenerator\Cpp2IL\cpp2il_out`(RVA·필드 오프셋은 `ilspycmd -t <타입>`). 메서드 본문은 같은 폴더의 `Cpp2IL.exe --game-path "H:/muse dash hwa" --output-as isil --output-to <폴더>`로 ISIL을 뽑아 읽습니다(약 30초, 380MB).
3. **설계 의도**
   - 가상 곡은 공식 `MusicInfo`의 얇은 복제본입니다. 제목은 게임이 `m_MusicExInfo.m_AlbumIndex`로 앨범 현지화 표를 고른 뒤 `BaseDataObject.listIndex` 행을 묻는 두 겹 조회로 정해지므로, `DBConfigLocalALBUM.GetLocalAlbumInfoByIndex`에서 `MusicInfoWrapper.listIndex`가 같고 선택 곡 스코프 안일 때만 커스텀 제목으로 답합니다.
   - 커스텀 음표·하트는 `NoteDataMananger.GetNoteByUid`의 공식 행을 바탕으로 만듭니다. 일반 노트를 복제하면 타격 판정으로 집계됩니다.
   - 커스텀 곡 정확도는 `AccuracyCalculator`가 덮어씁니다: (PERFECT + GREAT/2 + 기어 통과 + 하트 `m_EnergyCount` + 음표 `m_MusicCount`) / 전체.
4. **핵심 훅**: `DBStageInfo.SetRuntimeMusicData`(BMS → `MusicData` 주입, `DBStageInfoExperimentChart*.cs`), `DBConfigLocalALBUM.GetLocalAlbumInfoByIndex` / `MusicInfo.GetLocal`(제목), `TaskStageTarget.GetAccuracy*`(정확도), `GameTouchPlay.TouchResult` / `StageBattleComponent.InitTimeNodeOrder`(강제 퍼펙트).
5. **막다른 길**
   - 현지화 행 번호를 `musicIndex`로 본 것: 값이 우연히 같았을 뿐, 실제는 `listIndex`입니다.
   - 하단 제목을 `LongSongNameController.Refresh`에서 바꾸는 것: 화면은 맞지만 원인보다 늦은 자리라 버렸습니다.
   - PERFECT 과다를 `ForcePerfect`의 `isPerfectNode` 탓으로 본 것: 강제퍼펙트를 끈 오토플레이에서도 재현되어 무관했습니다.
   - 노트 속도를 dt만 바꿔 내려는 것: 애니메이션은 uid `yy`가 정하므로 렌더가 깨집니다([NOTE_SPEED_DT.md](docs/experiments/NOTE_SPEED_DT.md)).
6. **빌드·배포**: `dotnet build`(또는 `build.bat`) → `H:\muse dash hwa\Mods\muse-dash-custom-chart.dll`로 자동 배포. `[AutoDeploy] Successfully deployed` 줄과 `ls -la "H:/muse dash hwa/Mods/"`로 확인합니다.

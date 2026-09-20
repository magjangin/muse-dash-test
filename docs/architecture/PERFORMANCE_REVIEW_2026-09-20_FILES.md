# 성능 검토 범위 — 2026-09-20

기준 커밋: `495597d321ddb3999a78990acf1f3d9f95612dfc`. 보고서 작성 전 Git 추적 파일 197개.

전체 텍스트 파일을 읽어 실행 진입점·반복·I/O·객체 검색·리플렉션 패턴을 검색했다. 표는 전수 검색 범위를 나타내며, 모든 파일을 같은 깊이로 수동 정독했다는 뜻은 아니다. 주요 실행 경로의 상세 판단은 [성능 검토](PERFORMANCE_REVIEW_2026-09-20.md)에 있다. PNG는 크기와 사용 경로를 확인했다.

| 파일 | 줄 수 / 크기 | 검토 분류 |
| --- | ---: | --- |
| [.claude/settings.local.json](../../.claude/settings.local.json) | 117 | 설정/빌드: 실행·배포 경로 검색 |
| [.gitignore](../../.gitignore) | 33 | 설정/빌드: 실행·배포 경로 검색 |
| [AGENTS.md](../../AGENTS.md) | 9 | 문서: 구조·기존 사고·성능 설명 검색 |
| [README.md](../../README.md) | 325 | 문서: 구조·기존 사고·성능 설명 검색 |
| [SignatureDumper/Program.cs](../../SignatureDumper/Program.cs) | 116 | 개발 도구: 런타임과 구분 |
| [SignatureDumper/SignatureDumper.cs](../../SignatureDumper/SignatureDumper.cs) | 406 | 개발 도구: 런타임과 구분 |
| [SignatureDumper/SignatureDumper.csproj](../../SignatureDumper/SignatureDumper.csproj) | 15 | 개발 도구: 런타임과 구분 |
| [build.bat](../../build.bat) | 165 | 설정/빌드: 실행·배포 경로 검색 |
| [docs/README.md](../../docs/README.md) | 57 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/architecture/ARCHITECTURE.md](../../docs/architecture/ARCHITECTURE.md) | 174 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/architecture/CAST_AND_CUSTOM_TAG_GUIDE.md](../../docs/architecture/CAST_AND_CUSTOM_TAG_GUIDE.md) | 107 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/architecture/CODE_REFERENCE.md](../../docs/architecture/CODE_REFERENCE.md) | 305 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/architecture/MOD_SYSTEM_BLUEPRINT.md](../../docs/architecture/MOD_SYSTEM_BLUEPRINT.md) | 168 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/BOSS_EXPERIMENTS.md](../../docs/experiments/BOSS_EXPERIMENTS.md) | 359 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/DIALOG_INJECTION.md](../../docs/experiments/DIALOG_INJECTION.md) | 235 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/GHOST_NOTE_ALPHA_HOLD.md](../../docs/experiments/GHOST_NOTE_ALPHA_HOLD.md) | 93 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/NOTE_COLOR_TINTING.md](../../docs/experiments/NOTE_COLOR_TINTING.md) | 251 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/NOTE_EXPERIMENTS.md](../../docs/experiments/NOTE_EXPERIMENTS.md) | 498 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/SCENE_BACKGROUND_SWAP.md](../../docs/experiments/SCENE_BACKGROUND_SWAP.md) | 188 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/experiments/UID_INJECTION.md](../../docs/experiments/UID_INJECTION.md) | 97 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/getting-started/ANALOGIES.md](../../docs/getting-started/ANALOGIES.md) | 104 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/getting-started/MODDING.md](../../docs/getting-started/MODDING.md) | 120 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/getting-started/MODDING_MINDSET.md](../../docs/getting-started/MODDING_MINDSET.md) | 32 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/BMS_PARSING.md](../../docs/guides/BMS_PARSING.md) | 153 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/CHECKLIST.md](../../docs/guides/CHECKLIST.md) | 168 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/CUSTOM_CHART_GUIDE.md](../../docs/guides/CUSTOM_CHART_GUIDE.md) | 176 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/DISCORD_RICH_PRESENCE.md](../../docs/guides/DISCORD_RICH_PRESENCE.md) | 70 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/LOGGING_AND_TROUBLESHOOTING.md](../../docs/guides/LOGGING_AND_TROUBLESHOOTING.md) | 354 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md](../../docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md) | 239 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/guides/OFFLINE_CUSTOM_SANDBOX_GUIDE.md](../../docs/guides/OFFLINE_CUSTOM_SANDBOX_GUIDE.md) | 37 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/CollabExpiration_Reconstruction.md](../../docs/inferred_game_code/CollabExpiration_Reconstruction.md) | 93 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/DBStageInfo_Reconstruction.md](../../docs/inferred_game_code/DBStageInfo_Reconstruction.md) | 96 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/GameMusicScene_Reconstruction.md](../../docs/inferred_game_code/GameMusicScene_Reconstruction.md) | 149 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/NoteController_Reconstruction.md](../../docs/inferred_game_code/NoteController_Reconstruction.md) | 119 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/README.md](../../docs/inferred_game_code/README.md) | 33 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/SaveDataManager_Reconstruction.md](../../docs/inferred_game_code/SaveDataManager_Reconstruction.md) | 105 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/inferred_game_code/StageBattleComponent_Reconstruction.md](../../docs/inferred_game_code/StageBattleComponent_Reconstruction.md) | 159 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/muse-dash-2/MD2_TAG_RETARGET_MAP.md](../../docs/muse-dash-2/MD2_TAG_RETARGET_MAP.md) | 265 | 문서: 구조·기존 사고·성능 설명 검색 |
| [docs/muse-dash-2/MUSE_DASH_2_SPECULATIVE_GUIDE.md](../../docs/muse-dash-2/MUSE_DASH_2_SPECULATIVE_GUIDE.md) | 285 | 문서: 구조·기존 사고·성능 설명 검색 |
| [muse dash test.LogicTests/Assert.cs](../../muse%20dash%20test.LogicTests/Assert.cs) | 97 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/BmsBossSwapPlannerTests.cs](../../muse%20dash%20test.LogicTests/BmsBossSwapPlannerTests.cs) | 92 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/BmsFileNamesTests.cs](../../muse%20dash%20test.LogicTests/BmsFileNamesTests.cs) | 62 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/BmsNoteMatcherTests.cs](../../muse%20dash%20test.LogicTests/BmsNoteMatcherTests.cs) | 112 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/BmsParserTests.cs](../../muse%20dash%20test.LogicTests/BmsParserTests.cs) | 209 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/BmsWavParserTests.cs](../../muse%20dash%20test.LogicTests/BmsWavParserTests.cs) | 196 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/ManifestAndOffsetTests.cs](../../muse%20dash%20test.LogicTests/ManifestAndOffsetTests.cs) | 96 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/MelonLoggerStub.cs](../../muse%20dash%20test.LogicTests/MelonLoggerStub.cs) | 26 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/MusicDecimalTextTests.cs](../../muse%20dash%20test.LogicTests/MusicDecimalTextTests.cs) | 69 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/OffsetHookStubs.cs](../../muse%20dash%20test.LogicTests/OffsetHookStubs.cs) | 60 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/PlayRecordMergeTests.cs](../../muse%20dash%20test.LogicTests/PlayRecordMergeTests.cs) | 144 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/Program.cs](../../muse%20dash%20test.LogicTests/Program.cs) | 92 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/RealChartTests.cs](../../muse%20dash%20test.LogicTests/RealChartTests.cs) | 177 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.LogicTests/muse dash test.LogicTests.csproj](../../muse%20dash%20test.LogicTests/muse%20dash%20test.LogicTests.csproj) | 26 | 로직 테스트: 구성·검증 경로 |
| [muse dash test.slnx](../../muse%20dash%20test.slnx) | 4 | 설정/빌드: 실행·배포 경로 검색 |
| [muse dash test/Bms/BmsBossSwapPlanner.cs](../../muse%20dash%20test/Bms/BmsBossSwapPlanner.cs) | 107 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsFileNames.cs](../../muse%20dash%20test/Bms/BmsFileNames.cs) | 65 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsModels.cs](../../muse%20dash%20test/Bms/BmsModels.cs) | 54 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsNoteMatcher.cs](../../muse%20dash%20test/Bms/BmsNoteMatcher.cs) | 155 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsParser.Lexer.cs](../../muse%20dash%20test/Bms/BmsParser.Lexer.cs) | 184 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsParser.cs](../../muse%20dash%20test/Bms/BmsParser.cs) | 324 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Bms/BmsWavParser.cs](../../muse%20dash%20test/Bms/BmsWavParser.cs) | 365 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/ChartFingerprint.cs](../../muse%20dash%20test/Core/ChartFingerprint.cs) | 113 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/CustomContentIds.cs](../../muse%20dash%20test/Core/CustomContentIds.cs) | 31 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/CustomPlaySession.cs](../../muse%20dash%20test/Core/CustomPlaySession.cs) | 87 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/CustomRecordStore.cs](../../muse%20dash%20test/Core/CustomRecordStore.cs) | 419 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/DeviceDetector.cs](../../muse%20dash%20test/Core/DeviceDetector.cs) | 169 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/EmbeddedResource.cs](../../muse%20dash%20test/Core/EmbeddedResource.cs) | 77 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/FavSave.cs](../../muse%20dash%20test/Core/FavSave.cs) | 100 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/FeatureGuard.cs](../../muse%20dash%20test/Core/FeatureGuard.cs) | 123 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/GameBindings.cs](../../muse%20dash%20test/Core/GameBindings.cs) | 86 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/HealthBarFinder.cs](../../muse%20dash%20test/Core/HealthBarFinder.cs) | 51 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/ModConfig.cs](../../muse%20dash%20test/Core/ModConfig.cs) | 206 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/ModLogger.cs](../../muse%20dash%20test/Core/ModLogger.cs) | 130 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/MusicDecimalText.cs](../../muse%20dash%20test/Core/MusicDecimalText.cs) | 68 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/PlayRecordMerge.cs](../../muse%20dash%20test/Core/PlayRecordMerge.cs) | 86 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/TouchInput.cs](../../muse%20dash%20test/Core/TouchInput.cs) | 199 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Core/UnlockAllMasterGuard.cs](../../muse%20dash%20test/Core/UnlockAllMasterGuard.cs) | 28 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Integration/DiscordPresenceManager.cs](../../muse%20dash%20test/Integration/DiscordPresenceManager.cs) | 177 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Integration/RealTimeSwapper.cs](../../muse%20dash%20test/Integration/RealTimeSwapper.cs) | 359 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/MainMod.cs](../../muse%20dash%20test/MainMod.cs) | 260 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/AutoPlayPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/AutoPlayPatch.cs) | 43 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/BossPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/BossPatch.cs) | 437 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/ChangeFeverValuePatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/ChangeFeverValuePatch.cs) | 57 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/ForcePerfectPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/ForcePerfectPatch.cs) | 140 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/GhostNoteAlphaHold.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/GhostNoteAlphaHold.cs) | 411 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/Mechanics/MouseTouchBridgePatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/MouseTouchBridgePatch.cs) | 373 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/APModPatch.Accuracy.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.Accuracy.cs) | 62 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/APModPatch.VictoryBanner.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.VictoryBanner.cs) | 280 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/APModPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.cs) | 192 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/AllPerfectSound.cs](../../muse%20dash%20test/Patches/Battle/UI/AllPerfectSound.cs) | 108 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/ExperimentHitPointInstaller.cs](../../muse%20dash%20test/Patches/Battle/UI/ExperimentHitPointInstaller.cs) | 447 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs) | 175 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/HwaBattleMediaController.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.cs) | 427 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/PnlBattleGameStartPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/PnlBattleGameStartPatch.cs) | 25 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/PnlVictoryLoggingPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/PnlVictoryLoggingPatch.cs) | 112 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/ProgressBarPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/ProgressBarPatch.cs) | 46 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Battle/UI/StageBattleComponentPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs) | 287 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Common/AlbumsInfoWrapper.cs](../../muse%20dash%20test/Patches/Common/AlbumsInfoWrapper.cs) | 90 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Common/Il2CppWrapperBase.cs](../../muse%20dash%20test/Patches/Common/Il2CppWrapperBase.cs) | 100 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Common/ModReflection.cs](../../muse%20dash%20test/Patches/Common/ModReflection.cs) | 390 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Common/MusicInfoWrapper.cs](../../muse%20dash%20test/Patches/Common/MusicInfoWrapper.cs) | 126 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Save/SaveDataManagerPatch.cs](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs) | 218 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Bms.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Bms.cs) | 390 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Diagnostics.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Diagnostics.cs) | 331 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Resolve.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Resolve.cs) | 220 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Schema.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Schema.cs) | 55 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Sorting.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Sorting.cs) | 270 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.cs) | 401 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Database/Stage/DBStageInfoPatch.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoPatch.cs) | 124 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Diagnostics/CollabEndTimeDumpPatch.cs](../../muse%20dash%20test/Patches/Diagnostics/CollabEndTimeDumpPatch.cs) | 118 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Diagnostics/DiscordManagerDebugPatch.cs](../../muse%20dash%20test/Patches/Diagnostics/DiscordManagerDebugPatch.cs) | 132 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Diagnostics/HwaChartDiagnostics.cs](../../muse%20dash%20test/Patches/Diagnostics/HwaChartDiagnostics.cs) | 112 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Diagnostics/OffsetHookPatches.cs](../../muse%20dash%20test/Patches/Diagnostics/OffsetHookPatches.cs) | 142 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Diagnostics/PatchHealthCheck.cs](../../muse%20dash%20test/Patches/Diagnostics/PatchHealthCheck.cs) | 324 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Fav/FavManager.cs](../../muse%20dash%20test/Patches/Fav/FavManager.cs) | 294 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaManifest.cs](../../muse%20dash%20test/Patches/Hwa/HwaManifest.cs) | 29 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaManifestLoader.cs](../../muse%20dash%20test/Patches/Hwa/HwaManifestLoader.cs) | 346 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaMenuBgmController.cs](../../muse%20dash%20test/Patches/Hwa/HwaMenuBgmController.cs) | 429 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaResourceManager.Bms.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Bms.cs) | 158 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaResourceManager.Watcher.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Watcher.cs) | 158 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaResourceManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.cs) | 380 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Hwa/HwaSyncManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaSyncManager.cs) | 121 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Sandbox/OfflineCustomSandbox.cs](../../muse%20dash%20test/Patches/Sandbox/OfflineCustomSandbox.cs) | 181 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Sandbox/OfflineNetworkSandboxPatch.cs](../../muse%20dash%20test/Patches/Sandbox/OfflineNetworkSandboxPatch.cs) | 94 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/GameMusicSceneInitPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicSceneInitPatch.cs) | 262 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/GameMusicScenePatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicScenePatch.cs) | 45 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs) | 99 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/GameMusicSceneRunPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicSceneRunPatch.cs) | 141 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/SceneDiagnosticLogger.cs](../../muse%20dash%20test/Patches/Scene/SceneDiagnosticLogger.cs) | 39 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/SceneFlowPatch.cs](../../muse%20dash%20test/Patches/Scene/SceneFlowPatch.cs) | 92 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/ScenePatchHelpers.cs](../../muse%20dash%20test/Patches/Scene/ScenePatchHelpers.cs) | 38 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.ReflectionCache.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.ReflectionCache.cs) | 72 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.Restore.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.Restore.cs) | 391 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.cs) | 302 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs) | 99 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/HpMod/HywStageManager.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/HywStageManager.cs) | 169 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/HpMod/HywTextStyler.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/HywTextStyler.cs) | 32 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Config.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Config.cs) | 454 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/InputOverlay.ConfigFile.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.ConfigFile.cs) | 274 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Patches.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Patches.cs) | 29 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Render.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Render.cs) | 184 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/InputOverlay.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.cs) | 278 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/JudgmentBar.cs](../../muse%20dash%20test/Patches/UI/Custom/JudgmentBar.cs) | 304 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/AlbumTagTogglePatch.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/AlbumTagTogglePatch.cs) | 212 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagPatch.AlbumPatches.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.AlbumPatches.cs) | 376 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagPatch.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.cs) | 45 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagRegistry.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagRegistry.cs) | 140 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/SelectedSongLocalizationScope.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/SelectedSongLocalizationScope.cs) | 64 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/Support/CustomTagRegistryDebug.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/Support/CustomTagRegistryDebug.cs) | 87 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Custom/Tags/Support/CustomTagRegistrySupport.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/Support/CustomTagRegistrySupport.cs) | 426 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Menu/PnlHomeDebugPatch.cs](../../muse%20dash%20test/Patches/UI/Menu/PnlHomeDebugPatch.cs) | 51 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Menu/PnlMenuBgmStopPatch.cs](../../muse%20dash%20test/Patches/UI/Menu/PnlMenuBgmStopPatch.cs) | 21 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/MusicButtonAreaTitlePatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicButtonAreaTitlePatch.cs) | 25 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/MusicButtonCellPatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicButtonCellPatch.cs) | 265 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/MusicStageCellPatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicStageCellPatch.cs) | 39 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.AudioClip.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.AudioClip.cs) | 160 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.Extraction.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.Extraction.cs) | 170 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.cs) | 219 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/PnlMusicOverride.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicOverride.cs) | 237 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Music/PnlMusicTagPatch.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicTagPatch.cs) | 100 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.Search.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.Search.cs) | 206 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.TextDebug.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.TextDebug.cs) | 377 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.cs) | 172 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Pnl/SetSelectedMusicNameTxtPatch.cs](../../muse%20dash%20test/Patches/UI/Pnl/SetSelectedMusicNameTxtPatch.cs) | 111 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Setting/PnlInputMobilePatch.cs](../../muse%20dash%20test/Patches/UI/Setting/PnlInputMobilePatch.cs) | 103 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/CustomRecordUiPatchHelper.cs](../../muse%20dash%20test/Patches/UI/Stage/CustomRecordUiPatchHelper.cs) | 178 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/PnlPreparationPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlPreparationPatch.cs) | 216 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/PnlRankHookPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlRankHookPatch.cs) | 110 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/PnlRecordPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlRecordPatch.cs) | 32 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/PnlReportCardPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlReportCardPatch.cs) | 267 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/PnlStagePatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlStagePatch.cs) | 375 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Stage/RankCellHookPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/RankCellHookPatch.cs) | 58 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Patches/UI/Welcome/DisableUpdateNoticePatch.cs](../../muse%20dash%20test/Patches/UI/Welcome/DisableUpdateNoticePatch.cs) | 213 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Properties/AssemblyInfo.cs](../../muse%20dash%20test/Properties/AssemblyInfo.cs) | 34 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Resources/tag_icon.png](../../muse%20dash%20test/Resources/tag_icon.png) | 19539 bytes | 리소스: 로드·캐시 경로 확인 |
| [muse dash test/Spine/CustomSkinInjector.cs](../../muse%20dash%20test/Spine/CustomSkinInjector.cs) | 175 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Spine/Patch_Inject_BlackGirlBattle.cs](../../muse%20dash%20test/Spine/Patch_Inject_BlackGirlBattle.cs) | 87 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Spine/Patch_SkinNameProbe.cs](../../muse%20dash%20test/Spine/Patch_SkinNameProbe.cs) | 28 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/Spine/Patch_SpineActionContract.cs](../../muse%20dash%20test/Spine/Patch_SpineActionContract.cs) | 240 | 모드 소스: 성능 패턴 전수 검색 |
| [muse dash test/muse dash test.csproj](../../muse%20dash%20test/muse%20dash%20test.csproj) | 59 | 설정/빌드: 실행·배포 경로 검색 |
| [run-logic-tests.bat](../../run-logic-tests.bat) | 26 | 설정/빌드: 실행·배포 경로 검색 |
| [scripts/_complexity_analyze.py](../../scripts/_complexity_analyze.py) | 159 | 개발 도구: 런타임과 구분 |
| [scripts/_refactor_apply.py](../../scripts/_refactor_apply.py) | 351 | 개발 도구: 런타임과 구분 |
| [scripts/extract_names_from_dll.ps1](../../scripts/extract_names_from_dll.ps1) | 27 | 개발 도구: 런타임과 구분 |
| [scripts/filter_dump.ps1](../../scripts/filter_dump.ps1) | 9 | 개발 도구: 런타임과 구분 |
| [scripts/find_account_save_types.ps1](../../scripts/find_account_save_types.ps1) | 59 | 개발 도구: 런타임과 구분 |
| [scripts/find_progress_types.ps1](../../scripts/find_progress_types.ps1) | 75 | 개발 도구: 런타임과 구분 |
| [scripts/find_record_types.ps1](../../scripts/find_record_types.ps1) | 47 | 개발 도구: 런타임과 구분 |
| [scripts/inspect_datamanager.ps1](../../scripts/inspect_datamanager.ps1) | 53 | 개발 도구: 런타임과 구분 |
| [scripts/inspect_ivariable.ps1](../../scripts/inspect_ivariable.ps1) | 61 | 개발 도구: 런타임과 구분 |
| [scripts/inspect_record_saving.ps1](../../scripts/inspect_record_saving.ps1) | 66 | 개발 도구: 런타임과 구분 |
| [scripts/list_all_types.ps1](../../scripts/list_all_types.ps1) | 6 | 개발 도구: 런타임과 구분 |
| [scripts/list_il2cpp_assemblies.ps1](../../scripts/list_il2cpp_assemblies.ps1) | 22 | 개발 도구: 런타임과 구분 |
| [scripts/list_matched_types.ps1](../../scripts/list_matched_types.ps1) | 1 | 개발 도구: 런타임과 구분 |
| [scripts/reset_progress.bat](../../scripts/reset_progress.bat) | 86 | 개발 도구: 런타임과 구분 |
| [scripts/search_members.ps1](../../scripts/search_members.ps1) | 34 | 개발 도구: 런타임과 구분 |
| [scripts/test_types.ps1](../../scripts/test_types.ps1) | 52 | 개발 도구: 런타임과 구분 |

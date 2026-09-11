# 검토 대상 파일 — 2026-09-11

기준 커밋 `dd7a8cc`의 Git 추적 파일 197개입니다. 이 목록은 범위 누락을 방지하기 위한 인벤토리이며, 각 파일의 모든 동작이 검증됐다는 의미는 아닙니다. 검토 방법과 한계는 [검토 보고서](2026-09-11-repository-review.md)에 기록했습니다.

| 파일 | 줄 수 |
| --- | --- |
| [.claude/settings.local.json](../../.claude/settings.local.json) | 117 |
| [.gitignore](../../.gitignore) | 33 |
| [AGENTS.md](../../AGENTS.md) | 9 |
| [README.md](../../README.md) | 326 |
| [SignatureDumper/Program.cs](../../SignatureDumper/Program.cs) | 116 |
| [SignatureDumper/SignatureDumper.cs](../../SignatureDumper/SignatureDumper.cs) | 406 |
| [SignatureDumper/SignatureDumper.csproj](../../SignatureDumper/SignatureDumper.csproj) | 15 |
| [build.bat](../../build.bat) | 165 |
| [docs/README.md](../../docs/README.md) | 57 |
| [docs/architecture/ARCHITECTURE.md](../../docs/architecture/ARCHITECTURE.md) | 174 |
| [docs/architecture/CAST_AND_CUSTOM_TAG_GUIDE.md](../../docs/architecture/CAST_AND_CUSTOM_TAG_GUIDE.md) | 107 |
| [docs/architecture/CODE_REFERENCE.md](../../docs/architecture/CODE_REFERENCE.md) | 308 |
| [docs/architecture/MOD_SYSTEM_BLUEPRINT.md](../../docs/architecture/MOD_SYSTEM_BLUEPRINT.md) | 168 |
| [docs/experiments/BOSS_EXPERIMENTS.md](../../docs/experiments/BOSS_EXPERIMENTS.md) | 359 |
| [docs/experiments/DIALOG_INJECTION.md](../../docs/experiments/DIALOG_INJECTION.md) | 235 |
| [docs/experiments/GHOST_NOTE_ALPHA_HOLD.md](../../docs/experiments/GHOST_NOTE_ALPHA_HOLD.md) | 93 |
| [docs/experiments/NOTE_COLOR_TINTING.md](../../docs/experiments/NOTE_COLOR_TINTING.md) | 251 |
| [docs/experiments/NOTE_EXPERIMENTS.md](../../docs/experiments/NOTE_EXPERIMENTS.md) | 498 |
| [docs/experiments/SCENE_BACKGROUND_SWAP.md](../../docs/experiments/SCENE_BACKGROUND_SWAP.md) | 188 |
| [docs/experiments/UID_INJECTION.md](../../docs/experiments/UID_INJECTION.md) | 97 |
| [docs/getting-started/ANALOGIES.md](../../docs/getting-started/ANALOGIES.md) | 104 |
| [docs/getting-started/MODDING.md](../../docs/getting-started/MODDING.md) | 120 |
| [docs/getting-started/MODDING_MINDSET.md](../../docs/getting-started/MODDING_MINDSET.md) | 32 |
| [docs/guides/BMS_PARSING.md](../../docs/guides/BMS_PARSING.md) | 153 |
| [docs/guides/CHECKLIST.md](../../docs/guides/CHECKLIST.md) | 168 |
| [docs/guides/CUSTOM_CHART_GUIDE.md](../../docs/guides/CUSTOM_CHART_GUIDE.md) | 176 |
| [docs/guides/DISCORD_RICH_PRESENCE.md](../../docs/guides/DISCORD_RICH_PRESENCE.md) | 70 |
| [docs/guides/LOGGING_AND_TROUBLESHOOTING.md](../../docs/guides/LOGGING_AND_TROUBLESHOOTING.md) | 354 |
| [docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md](../../docs/guides/MOBILE_TOUCH_AND_INPUT_GUIDE.md) | 239 |
| [docs/guides/OFFLINE_CUSTOM_SANDBOX_GUIDE.md](../../docs/guides/OFFLINE_CUSTOM_SANDBOX_GUIDE.md) | 37 |
| [docs/inferred_game_code/CollabExpiration_Reconstruction.md](../../docs/inferred_game_code/CollabExpiration_Reconstruction.md) | 93 |
| [docs/inferred_game_code/DBStageInfo_Reconstruction.md](../../docs/inferred_game_code/DBStageInfo_Reconstruction.md) | 96 |
| [docs/inferred_game_code/GameMusicScene_Reconstruction.md](../../docs/inferred_game_code/GameMusicScene_Reconstruction.md) | 149 |
| [docs/inferred_game_code/NoteController_Reconstruction.md](../../docs/inferred_game_code/NoteController_Reconstruction.md) | 119 |
| [docs/inferred_game_code/README.md](../../docs/inferred_game_code/README.md) | 33 |
| [docs/inferred_game_code/SaveDataManager_Reconstruction.md](../../docs/inferred_game_code/SaveDataManager_Reconstruction.md) | 105 |
| [docs/inferred_game_code/StageBattleComponent_Reconstruction.md](../../docs/inferred_game_code/StageBattleComponent_Reconstruction.md) | 159 |
| [docs/muse-dash-2/MD2_TAG_RETARGET_MAP.md](../../docs/muse-dash-2/MD2_TAG_RETARGET_MAP.md) | 265 |
| [docs/muse-dash-2/MUSE_DASH_2_SPECULATIVE_GUIDE.md](../../docs/muse-dash-2/MUSE_DASH_2_SPECULATIVE_GUIDE.md) | 285 |
| [muse dash test.LogicTests/Assert.cs](../../muse%20dash%20test.LogicTests/Assert.cs) | 97 |
| [muse dash test.LogicTests/BmsBossSwapPlannerTests.cs](../../muse%20dash%20test.LogicTests/BmsBossSwapPlannerTests.cs) | 92 |
| [muse dash test.LogicTests/BmsFileNamesTests.cs](../../muse%20dash%20test.LogicTests/BmsFileNamesTests.cs) | 62 |
| [muse dash test.LogicTests/BmsNoteMatcherTests.cs](../../muse%20dash%20test.LogicTests/BmsNoteMatcherTests.cs) | 112 |
| [muse dash test.LogicTests/BmsParserTests.cs](../../muse%20dash%20test.LogicTests/BmsParserTests.cs) | 194 |
| [muse dash test.LogicTests/BmsWavParserTests.cs](../../muse%20dash%20test.LogicTests/BmsWavParserTests.cs) | 196 |
| [muse dash test.LogicTests/MelonLoggerStub.cs](../../muse%20dash%20test.LogicTests/MelonLoggerStub.cs) | 26 |
| [muse dash test.LogicTests/MusicDecimalTextTests.cs](../../muse%20dash%20test.LogicTests/MusicDecimalTextTests.cs) | 69 |
| [muse dash test.LogicTests/PlayRecordMergeTests.cs](../../muse%20dash%20test.LogicTests/PlayRecordMergeTests.cs) | 144 |
| [muse dash test.LogicTests/Program.cs](../../muse%20dash%20test.LogicTests/Program.cs) | 92 |
| [muse dash test.LogicTests/RealChartTests.cs](../../muse%20dash%20test.LogicTests/RealChartTests.cs) | 177 |
| [muse dash test.LogicTests/muse dash test.LogicTests.csproj](../../muse%20dash%20test.LogicTests/muse%20dash%20test.LogicTests.csproj) | 23 |
| [muse dash test.slnx](../../muse%20dash%20test.slnx) | 4 |
| [muse dash test/Bms/BmsBossSwapPlanner.cs](../../muse%20dash%20test/Bms/BmsBossSwapPlanner.cs) | 107 |
| [muse dash test/Bms/BmsFileNames.cs](../../muse%20dash%20test/Bms/BmsFileNames.cs) | 65 |
| [muse dash test/Bms/BmsModels.cs](../../muse%20dash%20test/Bms/BmsModels.cs) | 54 |
| [muse dash test/Bms/BmsNoteMatcher.cs](../../muse%20dash%20test/Bms/BmsNoteMatcher.cs) | 155 |
| [muse dash test/Bms/BmsParser.Lexer.cs](../../muse%20dash%20test/Bms/BmsParser.Lexer.cs) | 184 |
| [muse dash test/Bms/BmsParser.cs](../../muse%20dash%20test/Bms/BmsParser.cs) | 302 |
| [muse dash test/Bms/BmsWavParser.cs](../../muse%20dash%20test/Bms/BmsWavParser.cs) | 365 |
| [muse dash test/Core/ChartFingerprint.cs](../../muse%20dash%20test/Core/ChartFingerprint.cs) | 113 |
| [muse dash test/Core/CustomContentIds.cs](../../muse%20dash%20test/Core/CustomContentIds.cs) | 31 |
| [muse dash test/Core/CustomPlaySession.cs](../../muse%20dash%20test/Core/CustomPlaySession.cs) | 87 |
| [muse dash test/Core/CustomRecordStore.cs](../../muse%20dash%20test/Core/CustomRecordStore.cs) | 419 |
| [muse dash test/Core/DeviceDetector.cs](../../muse%20dash%20test/Core/DeviceDetector.cs) | 169 |
| [muse dash test/Core/EmbeddedResource.cs](../../muse%20dash%20test/Core/EmbeddedResource.cs) | 77 |
| [muse dash test/Core/FavSave.cs](../../muse%20dash%20test/Core/FavSave.cs) | 100 |
| [muse dash test/Core/FeatureGuard.cs](../../muse%20dash%20test/Core/FeatureGuard.cs) | 123 |
| [muse dash test/Core/GameBindings.cs](../../muse%20dash%20test/Core/GameBindings.cs) | 86 |
| [muse dash test/Core/HealthBarFinder.cs](../../muse%20dash%20test/Core/HealthBarFinder.cs) | 51 |
| [muse dash test/Core/ModConfig.cs](../../muse%20dash%20test/Core/ModConfig.cs) | 206 |
| [muse dash test/Core/ModLogger.cs](../../muse%20dash%20test/Core/ModLogger.cs) | 130 |
| [muse dash test/Core/MusicDecimalText.cs](../../muse%20dash%20test/Core/MusicDecimalText.cs) | 68 |
| [muse dash test/Core/PlayRecordMerge.cs](../../muse%20dash%20test/Core/PlayRecordMerge.cs) | 86 |
| [muse dash test/Core/TouchInput.cs](../../muse%20dash%20test/Core/TouchInput.cs) | 199 |
| [muse dash test/Core/UnlockAllMasterGuard.cs](../../muse%20dash%20test/Core/UnlockAllMasterGuard.cs) | 28 |
| [muse dash test/Integration/DiscordPresenceManager.cs](../../muse%20dash%20test/Integration/DiscordPresenceManager.cs) | 177 |
| [muse dash test/Integration/RealTimeSwapper.cs](../../muse%20dash%20test/Integration/RealTimeSwapper.cs) | 359 |
| [muse dash test/MainMod.cs](../../muse%20dash%20test/MainMod.cs) | 259 |
| [muse dash test/Patches/Battle/Mechanics/AutoPlayPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/AutoPlayPatch.cs) | 43 |
| [muse dash test/Patches/Battle/Mechanics/BossPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/BossPatch.cs) | 437 |
| [muse dash test/Patches/Battle/Mechanics/ChangeFeverValuePatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/ChangeFeverValuePatch.cs) | 57 |
| [muse dash test/Patches/Battle/Mechanics/ForcePerfectPatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/ForcePerfectPatch.cs) | 140 |
| [muse dash test/Patches/Battle/Mechanics/GhostNoteAlphaHold.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/GhostNoteAlphaHold.cs) | 411 |
| [muse dash test/Patches/Battle/Mechanics/MouseTouchBridgePatch.cs](../../muse%20dash%20test/Patches/Battle/Mechanics/MouseTouchBridgePatch.cs) | 373 |
| [muse dash test/Patches/Battle/UI/APModPatch.Accuracy.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.Accuracy.cs) | 62 |
| [muse dash test/Patches/Battle/UI/APModPatch.VictoryBanner.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.VictoryBanner.cs) | 280 |
| [muse dash test/Patches/Battle/UI/APModPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/APModPatch.cs) | 192 |
| [muse dash test/Patches/Battle/UI/AllPerfectSound.cs](../../muse%20dash%20test/Patches/Battle/UI/AllPerfectSound.cs) | 108 |
| [muse dash test/Patches/Battle/UI/ExperimentHitPointInstaller.cs](../../muse%20dash%20test/Patches/Battle/UI/ExperimentHitPointInstaller.cs) | 447 |
| [muse dash test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.Lifecycle.cs) | 175 |
| [muse dash test/Patches/Battle/UI/HwaBattleMediaController.cs](../../muse%20dash%20test/Patches/Battle/UI/HwaBattleMediaController.cs) | 427 |
| [muse dash test/Patches/Battle/UI/PnlBattleGameStartPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/PnlBattleGameStartPatch.cs) | 25 |
| [muse dash test/Patches/Battle/UI/PnlVictoryLoggingPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/PnlVictoryLoggingPatch.cs) | 112 |
| [muse dash test/Patches/Battle/UI/ProgressBarPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/ProgressBarPatch.cs) | 46 |
| [muse dash test/Patches/Battle/UI/StageBattleComponentPatch.cs](../../muse%20dash%20test/Patches/Battle/UI/StageBattleComponentPatch.cs) | 287 |
| [muse dash test/Patches/Common/AlbumsInfoWrapper.cs](../../muse%20dash%20test/Patches/Common/AlbumsInfoWrapper.cs) | 90 |
| [muse dash test/Patches/Common/Il2CppWrapperBase.cs](../../muse%20dash%20test/Patches/Common/Il2CppWrapperBase.cs) | 100 |
| [muse dash test/Patches/Common/ModReflection.cs](../../muse%20dash%20test/Patches/Common/ModReflection.cs) | 390 |
| [muse dash test/Patches/Common/MusicInfoWrapper.cs](../../muse%20dash%20test/Patches/Common/MusicInfoWrapper.cs) | 126 |
| [muse dash test/Patches/Database/Save/SaveDataManagerPatch.cs](../../muse%20dash%20test/Patches/Database/Save/SaveDataManagerPatch.cs) | 218 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Bms.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Bms.cs) | 390 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Diagnostics.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Diagnostics.cs) | 331 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Resolve.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Resolve.cs) | 220 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Schema.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Schema.cs) | 55 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.Sorting.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.Sorting.cs) | 270 |
| [muse dash test/Patches/Database/Stage/DBStageInfoExperimentChart.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoExperimentChart.cs) | 401 |
| [muse dash test/Patches/Database/Stage/DBStageInfoPatch.cs](../../muse%20dash%20test/Patches/Database/Stage/DBStageInfoPatch.cs) | 124 |
| [muse dash test/Patches/Diagnostics/CollabEndTimeDumpPatch.cs](../../muse%20dash%20test/Patches/Diagnostics/CollabEndTimeDumpPatch.cs) | 118 |
| [muse dash test/Patches/Diagnostics/DiscordManagerDebugPatch.cs](../../muse%20dash%20test/Patches/Diagnostics/DiscordManagerDebugPatch.cs) | 132 |
| [muse dash test/Patches/Diagnostics/HwaChartDiagnostics.cs](../../muse%20dash%20test/Patches/Diagnostics/HwaChartDiagnostics.cs) | 112 |
| [muse dash test/Patches/Diagnostics/OffsetHookPatches.cs](../../muse%20dash%20test/Patches/Diagnostics/OffsetHookPatches.cs) | 142 |
| [muse dash test/Patches/Diagnostics/PatchHealthCheck.cs](../../muse%20dash%20test/Patches/Diagnostics/PatchHealthCheck.cs) | 324 |
| [muse dash test/Patches/Diagnostics/UidMethodTracePatches.cs](../../muse%20dash%20test/Patches/Diagnostics/UidMethodTracePatches.cs) | 330 |
| [muse dash test/Patches/Fav/FavManager.cs](../../muse%20dash%20test/Patches/Fav/FavManager.cs) | 294 |
| [muse dash test/Patches/Hwa/HwaManifest.cs](../../muse%20dash%20test/Patches/Hwa/HwaManifest.cs) | 29 |
| [muse dash test/Patches/Hwa/HwaManifestLoader.cs](../../muse%20dash%20test/Patches/Hwa/HwaManifestLoader.cs) | 354 |
| [muse dash test/Patches/Hwa/HwaMenuBgmController.cs](../../muse%20dash%20test/Patches/Hwa/HwaMenuBgmController.cs) | 429 |
| [muse dash test/Patches/Hwa/HwaResourceManager.Bms.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Bms.cs) | 158 |
| [muse dash test/Patches/Hwa/HwaResourceManager.Watcher.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.Watcher.cs) | 153 |
| [muse dash test/Patches/Hwa/HwaResourceManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaResourceManager.cs) | 380 |
| [muse dash test/Patches/Hwa/HwaSyncManager.cs](../../muse%20dash%20test/Patches/Hwa/HwaSyncManager.cs) | 121 |
| [muse dash test/Patches/Sandbox/OfflineCustomSandbox.cs](../../muse%20dash%20test/Patches/Sandbox/OfflineCustomSandbox.cs) | 181 |
| [muse dash test/Patches/Sandbox/OfflineNetworkSandboxPatch.cs](../../muse%20dash%20test/Patches/Sandbox/OfflineNetworkSandboxPatch.cs) | 94 |
| [muse dash test/Patches/Scene/GameMusicSceneInitPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicSceneInitPatch.cs) | 262 |
| [muse dash test/Patches/Scene/GameMusicScenePatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicScenePatch.cs) | 45 |
| [muse dash test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicScenePreLoadEnemyPatch.cs) | 99 |
| [muse dash test/Patches/Scene/GameMusicSceneRunPatch.cs](../../muse%20dash%20test/Patches/Scene/GameMusicSceneRunPatch.cs) | 141 |
| [muse dash test/Patches/Scene/SceneDiagnosticLogger.cs](../../muse%20dash%20test/Patches/Scene/SceneDiagnosticLogger.cs) | 39 |
| [muse dash test/Patches/Scene/SceneFlowPatch.cs](../../muse%20dash%20test/Patches/Scene/SceneFlowPatch.cs) | 92 |
| [muse dash test/Patches/Scene/ScenePatchHelpers.cs](../../muse%20dash%20test/Patches/Scene/ScenePatchHelpers.cs) | 38 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.ReflectionCache.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.ReflectionCache.cs) | 72 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.Restore.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.Restore.cs) | 391 |
| [muse dash test/Patches/Scene/SceneZzTransformTracker.cs](../../muse%20dash%20test/Patches/Scene/SceneZzTransformTracker.cs) | 302 |
| [muse dash test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs) | 99 |
| [muse dash test/Patches/UI/Custom/HpMod/HywStageManager.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/HywStageManager.cs) | 169 |
| [muse dash test/Patches/UI/Custom/HpMod/HywTextStyler.cs](../../muse%20dash%20test/Patches/UI/Custom/HpMod/HywTextStyler.cs) | 32 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Config.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Config.cs) | 454 |
| [muse dash test/Patches/UI/Custom/InputOverlay.ConfigFile.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.ConfigFile.cs) | 274 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Patches.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Patches.cs) | 29 |
| [muse dash test/Patches/UI/Custom/InputOverlay.Render.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.Render.cs) | 184 |
| [muse dash test/Patches/UI/Custom/InputOverlay.cs](../../muse%20dash%20test/Patches/UI/Custom/InputOverlay.cs) | 278 |
| [muse dash test/Patches/UI/Custom/JudgmentBar.cs](../../muse%20dash%20test/Patches/UI/Custom/JudgmentBar.cs) | 306 |
| [muse dash test/Patches/UI/Custom/Tags/AlbumTagTogglePatch.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/AlbumTagTogglePatch.cs) | 212 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagPatch.AlbumPatches.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.AlbumPatches.cs) | 376 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagPatch.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagPatch.cs) | 45 |
| [muse dash test/Patches/UI/Custom/Tags/CustomTagRegistry.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/CustomTagRegistry.cs) | 140 |
| [muse dash test/Patches/UI/Custom/Tags/SelectedSongLocalizationScope.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/SelectedSongLocalizationScope.cs) | 64 |
| [muse dash test/Patches/UI/Custom/Tags/Support/CustomTagRegistryDebug.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/Support/CustomTagRegistryDebug.cs) | 87 |
| [muse dash test/Patches/UI/Custom/Tags/Support/CustomTagRegistrySupport.cs](../../muse%20dash%20test/Patches/UI/Custom/Tags/Support/CustomTagRegistrySupport.cs) | 426 |
| [muse dash test/Patches/UI/Menu/PnlHomeDebugPatch.cs](../../muse%20dash%20test/Patches/UI/Menu/PnlHomeDebugPatch.cs) | 51 |
| [muse dash test/Patches/UI/Menu/PnlMenuBgmStopPatch.cs](../../muse%20dash%20test/Patches/UI/Menu/PnlMenuBgmStopPatch.cs) | 21 |
| [muse dash test/Patches/UI/Music/MusicButtonAreaTitlePatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicButtonAreaTitlePatch.cs) | 25 |
| [muse dash test/Patches/UI/Music/MusicButtonCellPatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicButtonCellPatch.cs) | 265 |
| [muse dash test/Patches/UI/Music/MusicStageCellPatch.cs](../../muse%20dash%20test/Patches/UI/Music/MusicStageCellPatch.cs) | 39 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.AudioClip.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.AudioClip.cs) | 160 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.Extraction.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.Extraction.cs) | 170 |
| [muse dash test/Patches/UI/Music/PnlMusicDiagnostics.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDiagnostics.cs) | 219 |
| [muse dash test/Patches/UI/Music/PnlMusicDumper.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicDumper.cs) | 326 |
| [muse dash test/Patches/UI/Music/PnlMusicOverride.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicOverride.cs) | 237 |
| [muse dash test/Patches/UI/Music/PnlMusicTagPatch.cs](../../muse%20dash%20test/Patches/UI/Music/PnlMusicTagPatch.cs) | 100 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.Search.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.Search.cs) | 206 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.TextDebug.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.TextDebug.cs) | 377 |
| [muse dash test/Patches/UI/Pnl/PnlStagePatchHelper.cs](../../muse%20dash%20test/Patches/UI/Pnl/PnlStagePatchHelper.cs) | 172 |
| [muse dash test/Patches/UI/Pnl/SetSelectedMusicNameTxtPatch.cs](../../muse%20dash%20test/Patches/UI/Pnl/SetSelectedMusicNameTxtPatch.cs) | 111 |
| [muse dash test/Patches/UI/Setting/PnlInputMobilePatch.cs](../../muse%20dash%20test/Patches/UI/Setting/PnlInputMobilePatch.cs) | 103 |
| [muse dash test/Patches/UI/Stage/CustomRecordUiPatchHelper.cs](../../muse%20dash%20test/Patches/UI/Stage/CustomRecordUiPatchHelper.cs) | 178 |
| [muse dash test/Patches/UI/Stage/PnlPreparationPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlPreparationPatch.cs) | 216 |
| [muse dash test/Patches/UI/Stage/PnlRankHookPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlRankHookPatch.cs) | 110 |
| [muse dash test/Patches/UI/Stage/PnlRecordPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlRecordPatch.cs) | 32 |
| [muse dash test/Patches/UI/Stage/PnlReportCardPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlReportCardPatch.cs) | 267 |
| [muse dash test/Patches/UI/Stage/PnlStagePatch.cs](../../muse%20dash%20test/Patches/UI/Stage/PnlStagePatch.cs) | 375 |
| [muse dash test/Patches/UI/Stage/RankCellHookPatch.cs](../../muse%20dash%20test/Patches/UI/Stage/RankCellHookPatch.cs) | 58 |
| [muse dash test/Patches/UI/Welcome/DisableUpdateNoticePatch.cs](../../muse%20dash%20test/Patches/UI/Welcome/DisableUpdateNoticePatch.cs) | 213 |
| [muse dash test/Properties/AssemblyInfo.cs](../../muse%20dash%20test/Properties/AssemblyInfo.cs) | 34 |
| [muse dash test/Resources/tag_icon.png](../../muse%20dash%20test/Resources/tag_icon.png) | 이미지 |
| [muse dash test/Spine/CustomSkinInjector.cs](../../muse%20dash%20test/Spine/CustomSkinInjector.cs) | 170 |
| [muse dash test/Spine/Patch_Inject_BlackGirlBattle.cs](../../muse%20dash%20test/Spine/Patch_Inject_BlackGirlBattle.cs) | 87 |
| [muse dash test/Spine/Patch_SkinNameProbe.cs](../../muse%20dash%20test/Spine/Patch_SkinNameProbe.cs) | 28 |
| [muse dash test/Spine/Patch_SpineActionContract.cs](../../muse%20dash%20test/Spine/Patch_SpineActionContract.cs) | 240 |
| [muse dash test/muse dash test.csproj](../../muse%20dash%20test/muse%20dash%20test.csproj) | 56 |
| [run-logic-tests.bat](../../run-logic-tests.bat) | 26 |
| [scripts/_complexity_analyze.py](../../scripts/_complexity_analyze.py) | 159 |
| [scripts/_refactor_apply.py](../../scripts/_refactor_apply.py) | 351 |
| [scripts/extract_names_from_dll.ps1](../../scripts/extract_names_from_dll.ps1) | 27 |
| [scripts/filter_dump.ps1](../../scripts/filter_dump.ps1) | 9 |
| [scripts/find_account_save_types.ps1](../../scripts/find_account_save_types.ps1) | 59 |
| [scripts/find_progress_types.ps1](../../scripts/find_progress_types.ps1) | 75 |
| [scripts/find_record_types.ps1](../../scripts/find_record_types.ps1) | 47 |
| [scripts/inspect_datamanager.ps1](../../scripts/inspect_datamanager.ps1) | 53 |
| [scripts/inspect_ivariable.ps1](../../scripts/inspect_ivariable.ps1) | 61 |
| [scripts/inspect_record_saving.ps1](../../scripts/inspect_record_saving.ps1) | 66 |
| [scripts/list_all_types.ps1](../../scripts/list_all_types.ps1) | 6 |
| [scripts/list_il2cpp_assemblies.ps1](../../scripts/list_il2cpp_assemblies.ps1) | 22 |
| [scripts/list_matched_types.ps1](../../scripts/list_matched_types.ps1) | 1 |
| [scripts/reset_progress.bat](../../scripts/reset_progress.bat) | 86 |
| [scripts/search_members.ps1](../../scripts/search_members.ps1) | 34 |
| [scripts/test_types.ps1](../../scripts/test_types.ps1) | 52 |

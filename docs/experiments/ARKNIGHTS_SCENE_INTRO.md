# 명일방주 배경의 "OPERATION" 인트로 (조사만)

명일방주 곡에서 곡 시작 때 뜨는 `OPERATION 0-2 / ManiFesto:` 타이틀 카드가 어떻게 정해지는지 조사한 기록입니다. **모드 코드는 바꾸지 않았습니다.**

> **고친 기록**: 처음에는 이 카드가 `씬번호: 12`에 딸린다고 적었는데 틀렸습니다. 12번은 비취 랜드이고(실기 확인), 명일방주 곡은 설정상 1·2·4번 같은 일반 씬을 씁니다. 명일방주 연출은 씬이 아니라 **앨범**으로 켜집니다(2-1절).

- 조사일: 2026-10-25, 게임 6.7.0(IL2CPP)
- 방법: Cpp2IL ISIL로 `ArkNightGameSceneStartControl`의 `.ctor`/`Start`/`Update` 본문을 읽고, 필드 위치는 `cpp2il_out`을 `ilspycmd -t ArkNightGameSceneStartControl`로 확인했습니다.

---

## 1. 담당 클래스

`ArkNightGameSceneStartControl` (`Assets/Scripts/GameCore/GameObjectLogics/ExtraControl/`)

| 필드 | 위치 | 용도 |
| --- | --- | --- |
| `gaussionDuration` (float) | 0x18 | 배경 흐림 정도 |
| `m_LevelTMP` (TextMeshProUGUI) | 0x20 | 가운데 큰 글자 `0-2` |
| `m_SongNameTMP` (TextMeshProUGUI) | 0x28 | 아래 곡 이름 `ManiFesto:` |
| `m_SongInfo` (RectTransform) | 0x30 | 글자 묶음 레이아웃 |
| `m_StartBg` (Image) | 0x38 | 흐린 배경 이미지 |
| `m_BgMat` (Material) | 0x40 | 흐림 셰이더 재질 |
| `m_SongUidDic` (Dictionary<string, string>) | 0x48 | 곡 uid → 작전 번호 |

## 2. 동작 (ISIL 확인)

**생성자**가 `m_SongUidDic`을 채웁니다. 명일방주 콜라보 곡 두 묶음입니다.

| 곡 uid | 작전 번호 |
| --- | --- |
| `59-0` ~ `59-7` | `0-1` ~ `0-8` |
| `82-0` ~ `82-8` | `1-1` ~ `1-9` |

**`Start()`**

```
music = GlobalDataBase.dbMusicTag.CurMusicInfo();
if (m_SongUidDic.TryGetValue(music.uid, out level)) m_LevelTMP.text = level;   // 예: 59-1 → "0-2"
else                                                 m_LevelTMP.text = music.uid; // 표에 없으면 uid를 그대로 씀
m_SongNameTMP.text = music.GetLocal(...).name;       // 곡 이름
LayoutRebuilder.ForceRebuildLayoutImmediate(m_SongInfo);
```

**`Update()`**: 매 프레임 `m_BgMat.SetFloat("_Size", gaussionDuration)`로 배경 흐림을 갱신합니다. `gaussionDuration` 값은 애니메이션이 바꾸는 것으로 보입니다(정적으로 미확인).

`OPERATION` 글자와 붉은 띠, 뒤의 `MUSE DASH` 글자는 코드에 없어서 프리팹에 박힌 그림으로 보입니다.

## 2-1. 명일방주 곡인지 정하는 곳 (ISIL 확인)

- `StageBattleComponent.EnterGameBattle()`이 배틀에 들어갈 때 `GlobalDataBase.dbOther.CheckIsArkNightSong()`을 부릅니다.
- `DBOther.CheckIsArkNightSong()`은 지금 곡의 앨범(`DBConfigAlbums.GetAlbumInfoByMusicInfo(CurMusicInfo())`)의 uid가 `music_package_59` 또는 `music_package_82`면 `IsArkNightSong = true`로 둡니다.
- 이 값을 읽는 곳으로 `PnlVictory2dManager.Init`(결과 화면)과 `ChangeScoreValue.InitUi`(점수 UI)를 찾았습니다. 카드(`ArkNightGameSceneStartControl`)가 어디서 켜지는지는 아직 따라가지 않았습니다.
- 로딩 화면 그림(`loadinginterlude_assets_interlude_arknights_1~4` 번들)도 명일방주 전용으로 있습니다.

## 3. 커스텀 곡에서 생길 일 (추정, 실기 미확인)

- 커스텀 곡은 모드가 만든 가상 앨범(`1999-0`)에 속하므로 `IsArkNightSong`이 거짓이라, **씬 번호와 상관없이 이 카드와 명일방주 결과 화면은 나오지 않을 것**으로 봅니다.
- 만약 카드를 켠다면 커스텀 곡 uid(`1999-N`)는 작전 번호 표에 없으므로 가운데에 `1999-N`이 그대로 뜨고, 곡 이름은 모드가 가로챈 `MusicInfo.GetLocal` 덕분에 커스텀 제목이 뜰 것입니다.

## 4. 고친다면 (구상만)

- 작전 번호: `ArkNightGameSceneStartControl.Start` Postfix에서 커스텀 곡일 때 `m_LevelTMP.text`를 `info.txt`의 새 값(예: `작전번호: 2-1`)으로 바꾸거나, Prefix에서 `m_SongUidDic`에 `1999-N → 값`을 넣어 두면 게임 코드가 그대로 씁니다. 사전에 넣는 쪽이 게임 흐름을 덜 건드립니다.
- 같은 배경에 딸린 명일방주 요소로 아미야 전용 `ArkNightsAmiyaFeverManager`, `ArkNightsAmiyaBattleSubControl`이 있습니다(캐릭터 `Character_22`). 캐릭터 쪽이라 손대지 않습니다([CHARACTER_SKILL_LOGIC.md](CHARACTER_SKILL_LOGIC.md)).

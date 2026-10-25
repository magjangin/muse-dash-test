# 명일방주 배경의 "OPERATION" 인트로 (조사만)

명일방주 배경(`씬번호: 12`)에서 곡 시작 때 뜨는 `OPERATION 0-2 / ManiFesto:` 타이틀 카드가 어떻게 정해지는지 조사한 기록입니다. **모드 코드는 바꾸지 않았습니다.**

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

## 3. 커스텀 곡에서 생길 일 (추정, 실기 미확인)

- `씬번호: 12`로 명일방주 배경을 쓰면 이 카드도 같은 배경에 딸려 나올 것으로 보입니다.
- 커스텀 곡 uid(`1999-N`)는 표에 없으므로 가운데에 **`1999-N`이 그대로** 뜰 것입니다.
- 곡 이름은 `MusicInfo.GetLocal`에서 오는데, 모드가 이 함수를 가로채 커스텀 제목을 돌려주므로(`CustomTagPatch.AlbumPatches.cs`) **커스텀 제목**이 뜰 것입니다.

## 4. 고친다면 (구상만)

- 작전 번호: `ArkNightGameSceneStartControl.Start` Postfix에서 커스텀 곡일 때 `m_LevelTMP.text`를 `info.txt`의 새 값(예: `작전번호: 2-1`)으로 바꾸거나, Prefix에서 `m_SongUidDic`에 `1999-N → 값`을 넣어 두면 게임 코드가 그대로 씁니다. 사전에 넣는 쪽이 게임 흐름을 덜 건드립니다.
- 같은 배경에 딸린 명일방주 요소로 아미야 전용 `ArkNightsAmiyaFeverManager`, `ArkNightsAmiyaBattleSubControl`이 있습니다(캐릭터 `Character_22`). 캐릭터 쪽이라 손대지 않습니다([CHARACTER_SKILL_LOGIC.md](CHARACTER_SKILL_LOGIC.md)).

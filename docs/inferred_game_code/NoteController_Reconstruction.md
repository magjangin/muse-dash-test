# NoteController & Hit Judgment Inferred Method Body Reconstruction

`Il2CppGameLogic.NoteController` 및 `Il2CppFormulaBase.TaskStageTarget` 클래스는 *Muse Dash*의 노트 피격 판정(Perfect, Great, Pass, Miss), 피격 시 키 음 오디오 재생, 점수/콤보 갱신 및 플레이어 HP 증감(`ChangeHealthValue`) 연산을 조율하는 판정 엔진입니다.

---

## 🏗 추측된 클래스 구조 (Inferred Field Layout)

```csharp
namespace Il2CppGameLogic
{
    public class NoteController : UnityEngine.MonoBehaviour
    {
        public MusicData noteData;
        public int noteType; // 1: Small, 2: Saw, 3: Long, 6: Heart, 7: Note, 8: Sandbag
        public int pathway;  // 0: Air, 1: Ground
        public float speed;
        public float hitTime;
        private bool m_IsHitted;
    }
}
```

---

## 🔍 핵심 메서드 바디 복원 (Reconstructed Method Bodies)

### 1. `OnHit(int resultResult)`
**역추적 설명**: 입력 판정(0: Perfect, 1: Great, 2: Early/Late Miss)에 맞춰 점수 추가, 콤보 증가, 캐릭터 피버 게이지 충전 및 판정 이펙트를 생성합니다.

```csharp
public void OnHit(int judgeResult)
{
    // [추측된 메서드 바디]
    if (this.m_IsHitted) return;
    this.m_IsHitted = true;

    // 1. 키음 오디오 파일 재생
    if (this.noteData != null && !string.IsNullOrEmpty(this.noteData.noteData.key_audio))
    {
        AudioManager.instance.PlayKeyAudio(this.noteData.noteData.key_audio);
    }

    // 2. 판정 결과에 따른 점수 및 콤보 계산
    int addedScore = 0;
    if (judgeResult == 0) // Perfect
    {
        addedScore = 300;
        TaskStageTarget.instance.AddCombo(1);
    }
    else if (judgeResult == 1) // Great
    {
        addedScore = 150;
        TaskStageTarget.instance.AddCombo(1);
    }
    else // Miss / Pass
    {
        TaskStageTarget.instance.BreakCombo();
    }

    TaskStageTarget.instance.AddScore(addedScore);

    // 3. 노트 파괴 또는 오브젝트 풀 반환
    ObjectPoolManager.instance.RecycleNote(this.gameObject);
}
```

---

### 2. `OnMiss()`
**역추적 설명**: 노트를 놓치거나 장애물/톱니에 충돌했을 때 플레이어 체력을 차감하고 피격 애니메이션을 재생시킵니다.

```csharp
public void OnMiss()
{
    // [추측된 메서드 바디]
    if (this.m_IsHitted) return;
    this.m_IsHitted = true;

    // 콤보 리셋
    TaskStageTarget.instance.BreakCombo();

    // 노트 종류별 체력 차감량 계산
    int hpDamage = 20;
    if (this.noteType == 2) hpDamage = 40; // 톱니 몬스터

    // 체력 차감 이벤트 트리거 (ChangeHealthValue 패치 훅 지점)
    Il2Cpp.ChangeHealthValue.OnHpDeduct(hpDamage);

    // 노트 풀 반환
    ObjectPoolManager.instance.RecycleNote(this.gameObject);
}
```

---

### 3. `ChangeHealthValue.OnHpDeduct(int damage)`
**연관 패치**: [ChangeHealthValue_OnHpDeduct_Patch](file:///h:/source/repos/muse%20dash%20test/muse%20dash%20test/Patches/UI/Custom/HpMod/ChangeHealthValuePatch.cs#L78-L98)
**역추적 설명**: 체력 차감 발생 시 플레이어 HP 게이지 슬라이더와 체력 텍스트 UI를 업데이트합니다.

```csharp
public static void OnHpDeduct(int damage)
{
    // [추측된 메서드 바디]
    int currentHp = PlayerSessionData.instance.hp - damage;
    if (currentHp < 0) currentHp = 0;
    PlayerSessionData.instance.hp = currentHp;

    // 체력 비율(0.0 ~ 1.0) 계산 및 UI 바인딩
    float hpRate = (float)currentHp / (float)PlayerSessionData.instance.maxHp;
    UIManager.instance.UpdateHpBar(hpRate);

    // 체력 잔여량이 0 이하일 경우 스테이지 실패 처리
    if (currentHp <= 0)
    {
        StageBattleComponent.instance.OnPlayerDeath();
    }
}
```

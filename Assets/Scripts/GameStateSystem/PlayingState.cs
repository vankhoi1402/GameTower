using UnityEngine;

public class PlayingState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 1f;
        

        // KÍCH HOẠT TRỌNG TÀI TẠI ĐÂY
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.StartCombatPhase();
        }
        GameEvents.TriggerStateChanged(GameState.Playing); // Phát loa cho UI mở HUD, cho phép lính di chuyển
        BGMManager.Instance.PlayBGM(SoundType.SO_BGM_Battle);
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) {
        

    }
}
using UnityEngine;

public class PrepareState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 1f;
        GameEvents.TriggerStateChanged(GameState.Prepare); // Phát loa thông báo cho UI mở PrepareMenu
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) { }
}
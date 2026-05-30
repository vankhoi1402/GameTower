// PauseState.cs
using UnityEngine;

public class PauseState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 0f; // Đóng băng mọi chuyển động vật lý, update của lính
        GameEvents.TriggerStateChanged(GameState.Paused); // Phát loa cho UI mở PauseMenu
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) { }
}
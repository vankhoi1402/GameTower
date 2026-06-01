// GameOverState.cs
using UnityEngine;

public class GameOverState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 0f;
        GameEvents.TriggerStateChanged(GameState.EndGame); // Phát loa cho UI mở màn hình kết thúc
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) { }
}
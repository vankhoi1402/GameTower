// GameOverState.cs
using UnityEngine;

public class GameOverState : GameBaseState
{
    public override void EnterState(GameManager game)
    {
        Time.timeScale = 0f;
        // Phát loa cho UI mở màn hình kết thúc
        GameEvents.TriggerStateChanged(GameState.EndGame);
       
    }
    public override void UpdateState(GameManager game) { }
    public override void ExitState(GameManager game) {
       // Debug.Log(" ra overstate");
        //  BGMManager.Instance.PlayBGM(SoundType.SO_BGM_Menu);
    }
}
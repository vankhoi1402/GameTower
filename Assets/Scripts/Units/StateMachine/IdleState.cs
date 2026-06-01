using UnityEngine;

public class IdleState :  IState
{
    private BaseUnit _unit;
    public IdleState(BaseUnit unit) => _unit = unit;

    public void OnEnter() {
        // Debug.Log("IDLE: Bắt đầu đứng nghỉ"); 
       // _unit.Anim.Play(UnitAnimState.Idle);
    }

    public void OnUpdate()
    {
        // Kiểm tra điều kiện để chuyển sang Move
        //if (_unit.Targeting.HasTarget)
        //    _unit.StateMachine.ChangeState(new MoveState(_unit));
    }

    public void OnExit() {
        //Debug.Log("IDLE: Thôi đứng nghỉ, đi làm việc đây")
          }
}
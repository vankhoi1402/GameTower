using UnityEngine;

public class MoveState : IState
{
    private BaseUnit _unit;
    public MoveState(BaseUnit unit) => _unit = unit;

    public void OnEnter() => Debug.Log("MOVE: B?t ??u ch?y");

    public void OnUpdate()
    {
        if (!_unit.Targeting.HasTarget)
        {
            _unit.StateMachine.ChangeState(new IdleState(_unit));
            return;
        }

        // Logic di chuy?n
        _unit.MoveSystem.MoveTo(_unit.Targeting.CurrentTarget.transform.position, _unit.stats.currentMoveSpeed);

        // Ki?m tra n?u ?? t?m ?ánh thì chuy?n sang Attack
       // if (_unit.Targeting.IsTargetInAttackRange())
          //  _unit.StateMachine.ChangeState(new AttackState(_unit));
    }

    public void OnExit() => Debug.Log("MOVE: D?ng l?i");
}
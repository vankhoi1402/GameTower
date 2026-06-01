using UnityEngine;

public class MoveState : IState
{
    private BaseUnit _unit;
    public MoveState(BaseUnit unit) => _unit = unit;

    public void OnEnter() {
      //  _unit.Anim.Play(UnitAnimState.Move);
    } 

    public void OnUpdate()
    {
        if (!_unit.Targeting.HasTarget)
        {
            _unit.StateMachine.ChangeState(new IdleState(_unit));
            return;
        }

        // Logic di chuy?n
        _unit.MoveSystem.MoveTo(_unit.Targeting.CurrentTarget.transform.position, _unit.stats.currentMoveSpeed);
        _unit.Anim.Play(UnitAnimState.Move);

        // Ki?m tra n?u ?? t?m ?ánh thì chuy?n sang Attack
        // if (_unit.Targeting.IsTargetInAttackRange())
        //  _unit.StateMachine.ChangeState(new AttackState(_unit));
    }

    public void OnExit() { }
}
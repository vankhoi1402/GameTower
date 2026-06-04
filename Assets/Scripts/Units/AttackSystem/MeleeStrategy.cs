using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MeleeStrategy : IAttackStrategy
{
    public void ExecuteAttack(BaseUnit owner, BaseUnit target, float damage)
    {
        // Gây sát th??ng tr?c ti?p lên Health c?a m?c tiêu
        target.Health.TakeDamage(damage);
        //Debug.Log($"[Melee] {owner.name} chém {target.name}");
        BattleEvents.RaisePlaySound3D(SoundType.Battle_SwordSlash, owner.transform.position);
    }
}
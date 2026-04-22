using UnityEngine;

public interface ITargetSearch
{
    BaseUnit Find(BaseUnit owner, float range, LayerMask enemyLayer);
}
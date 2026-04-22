using UnityEngine;

public interface ITargetSearchStrategy
{
    // Trả về mục tiêu tốt nhất dựa trên vị trí, tầm quét và lớp địch
    BaseUnit SelectTarget(BaseUnit owner, float detectRange, LayerMask enemyLayer);
}
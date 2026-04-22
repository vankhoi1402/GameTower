public interface IAttackStrategy
{
    // Cần biết: Ai đánh, Mục tiêu chính là ai, Chỉ số sát thương bao nhiêu
    void ExecuteAttack(BaseUnit owner, BaseUnit target, float damage);
}
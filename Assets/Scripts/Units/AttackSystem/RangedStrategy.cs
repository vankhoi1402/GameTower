using UnityEngine;

public class RangedStrategy : IAttackStrategy
{
    private GameObject _projectilePrefab;
    private Transform _firePoint;

    public RangedStrategy(GameObject prefab, Transform firePoint)
    {
        _projectilePrefab = prefab;
        _firePoint = firePoint;
    }

    public void ExecuteAttack(BaseUnit owner, BaseUnit target, float damage)
    {
        // Sinh ra mũi tên và truyền thông số cho nó
       // GameObject projectileGO = GameObject.Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
       // Projectile projectile = projectileGO.GetComponent<Projectile>();

        //if (projectile != null)
        //    projectile.Setup(target, damage);

        Debug.Log($"[Ranged] {owner.name} bắn tên vào {target.name}");
    }
}
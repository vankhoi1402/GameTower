using UnityEngine;

// Tạo menu để bạn có thể chuột phải trong Unity tạo file dữ liệu
[CreateAssetMenu(fileName = "NewUnitData", menuName = "Game/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Base Stats")]
    public string unitName;
    public float maxHP;
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public float detectRange;
    [Header("Army")]
    public int capacityCost = 1;
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    [Header("UI")]
    public Sprite icon;
}
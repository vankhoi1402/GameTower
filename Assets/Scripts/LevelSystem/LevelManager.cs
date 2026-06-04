using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private FormationGrid2D grid;

    [Header("Current Level")]
    [SerializeField] private LevelData currentLevelData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // LẤY DỮ LIỆU TỪ NGOÀI VÀO TẠI ĐÂY:
        if (GameMenuManager.Instance != null && GameMenuManager.Instance.SelectedLevelData != null)
        {
            currentLevelData = GameMenuManager.Instance.SelectedLevelData;
        }

        // Thực hiện load trận đấu bình thường dựa trên dữ liệu nhận được
        if (currentLevelData != null)
        {
            LoadLevel(currentLevelData);
        }
        else
        {
            Debug.LogError("Không tìm thấy dữ liệu LevelData để load trận đấu!");
        }
    }

    public void LoadLevel(LevelData levelData)
    {
        currentLevelData = levelData;

        // Reset dữ liệu Manager cũ
        if (BattleManager.Instance != null) BattleManager.Instance.ResetBattleData();
        if (ArmyManager.Instance != null) ArmyManager.Instance.SetupCapacity(levelData.maxPlacementCapacity);

        // Đặt quân địch lên Grid
        SetupEnemyFormation(levelData.enemyTroopsSetup);
    }

    private void SetupEnemyFormation(List<EnemyGridConfig> enemySetup)
    {
        if (grid == null) return;

        foreach (var config in enemySetup)
        {
            if (config.enemyData == null)
                continue;

            FormationCell targetCell = grid.GetCell(
                config.gridCoordinate.x,
                config.gridCoordinate.y);

            if (targetCell == null || targetCell.Occupied)
                continue;

            SpawnManager.Instance.SpawnUnit(
                config.enemyData,
                targetCell.WorldPosition,
                TeamType.Enemy);

            targetCell.Occupied = true;
        }
    }
}
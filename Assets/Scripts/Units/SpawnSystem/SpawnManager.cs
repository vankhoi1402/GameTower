using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Nơi duy nhất trong game thực hiện Instantiate lính.
    /// </summary>
    public BaseUnit SpawnUnit(UnitData unitData, Vector3 spawnPosition, TeamType team)
    {
        if (unitData == null || unitData.prefab == null)
        {
            Debug.LogError("[SpawnManager] Dữ liệu lính không hợp lệ!");
            return null;
        }

        // 1. Sinh ra GameObject
        GameObject unitObj = Instantiate(unitData.prefab, spawnPosition, Quaternion.identity);

        // 2. Thiết lập các thông số cơ bản
        BaseUnit unitScript = unitObj.GetComponent<BaseUnit>();
        if (unitScript != null)
        {
            unitScript.Team = team;

            // Bạn có thể inject thêm các dependencies khác vào đây sau này nếu cần
            // Ví dụ: unitScript.Initialize();
        }
        else
        {
            Debug.LogError($"[SpawnManager] Prefab {unitData.unitName} thiếu script BaseUnit!");
        }

        return unitScript;
    }
}
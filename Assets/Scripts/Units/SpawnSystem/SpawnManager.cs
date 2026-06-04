using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public BaseUnit SpawnUnit(UnitData unitData, Vector3 spawnPosition, TeamType team)
    {
        if (unitData == null)
        {
            Debug.LogError("[SpawnManager] UnitData không hợp lệ!");
            return null;
        }

        GameObject prefabToSpawn = team == TeamType.Player
            ? unitData.playerPrefab
            : unitData.enemyPrefab;

        if (prefabToSpawn == null)
        {
            Debug.LogError($"[SpawnManager] {unitData.unitName} chưa gán prefab cho team {team}");
            return null;
        }

        GameObject unitObj = Instantiate(
            prefabToSpawn,
            spawnPosition,
            Quaternion.identity);
        

        BaseUnit unitScript = unitObj.GetComponent<BaseUnit>();

        if (unitScript != null)
        {
            unitScript.Team = team;

            SpriteRenderer sr = unitObj.GetComponentInChildren<SpriteRenderer>();

            if (sr != null)
            {
                sr.flipX = (team == TeamType.Enemy);
            }
        }

        unitScript.Team = team;

        return unitScript;
    }
}
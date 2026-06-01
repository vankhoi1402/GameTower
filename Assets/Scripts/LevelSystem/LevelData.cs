using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyGridConfig
{
    public UnitData enemyData;
    public Vector2Int gridCoordinate; // Tọa độ (X,Y) xếp sẵn trên FormationGrid2D
}

[CreateAssetMenu(fileName = "New Level", menuName = "RTS/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneToLoad;
    public int maxPlacementCapacity = 10;
    public List<EnemyGridConfig> enemyTroopsSetup;
}
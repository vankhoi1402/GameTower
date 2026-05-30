using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;

    public void Spawn(UnitData data, Transform point)
    {
        Instantiate(data.prefab, point.position, Quaternion.identity);
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level System/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Map Prefab")]
    public GameObject mapPrefab;

    [Header("Player Spawn Position")]
    public Vector3 playerSpawnPosition;
}

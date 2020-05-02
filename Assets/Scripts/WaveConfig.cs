using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
   [SerializeField] GameObject enemyPrefab;
   [SerializeField] GameObject pathPrefab;
   [SerializeField] float timeBetweenSpawns = 0.5f;
   [SerializeField] float spawnRandomFactor = 0.3f;
   [SerializeField] int numberOfEnemies = 5;
   [SerializeField] float moveSpeed = 2f;

   public GameObject GetEnemyPrefab() => enemyPrefab;
   public List<Transform> GetWaypoints() => 
    pathPrefab.transform.Cast<Transform>().Select((Transform child) => child).ToList();
   public float GetTimeBetweenSpawns() => timeBetweenSpawns;
   public float GetSpawnRandomFactor() => spawnRandomFactor;
   public float GetNumberOfEnemies() => numberOfEnemies;
   public float GetMoveSpeed() => moveSpeed;
}

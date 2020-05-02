using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;
    
    private Vector2 GetCurrentPosition() => waypoints[waypointIndex].transform.position;
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = GetCurrentPosition();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(waypointIndex <= waypoints.Count - 1)
        {
            Vector2 targetPosition = GetCurrentPosition();
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position =
                Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            
            if(new Vector2(transform.position.x, transform.position.y) == targetPosition)
                waypointIndex++;
        }
        else{
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObstaclePositionExample : MonoBehaviour
{
    public ObstacleController obstacleController;

    void Start()
    {
        // print obstacles positions
        foreach (GameObject obstacle in obstacleController.Obstacles)
        {
            Debug.Log(obstacle.transform.position);
        }
    }
}

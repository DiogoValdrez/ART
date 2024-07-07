using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSwarm : MonoBehaviour
{
    public GameObject dronePrefab;
    public int numberOfDrones = 5;
    public Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 targetPosition = new Vector3(10, 0, 10); // Set a target position

    public float spacing_x = 1.0f;
    public float spacing_y = 1.0f;
    public float spacing_z = 1.0f;
    public float max_spacing_multiplier = 1.0f;

    ObstacleController obstacleController; 
    AStarPathfinding aStarPathfinding;
    void Start()
    {
        obstacleController = GetComponent<ObstacleController>();
        aStarPathfinding = GetComponent<AStarPathfinding>();

        aStarPathfinding.obstacleController = obstacleController;
        aStarPathfinding.startPosition = startPosition;
        aStarPathfinding.targetPosition = targetPosition;

        CreateDroneLine();
    }

    void CreateDroneLine()
    {
        Vector3 Position = startPosition;
        List<Vector3> path = aStarPathfinding.GetPath();
        for (int i = 0; i < numberOfDrones; i++)
        {
            float aux = Random.Range(0.0f, max_spacing_multiplier);
            Vector3 position = Position + new Vector3(spacing_x * aux, spacing_y * aux, spacing_z * aux);

            GameObject drone = Instantiate(dronePrefab, position, Quaternion.identity);
            DroneController droneController = drone.GetComponent<DroneController>();
            droneController.set_path(path);
        }
    }
}

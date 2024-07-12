using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class to retrieve and manage the positions and sizes of objects tagged with a specified tag.
/// </summary>
public class ObstacleController : MonoBehaviour
{
    /// <summary>
    /// Tag used to identify obstacle objects.
    /// </summary>
    public string tag = "obstacle";

    /// <summary>
    /// List of positions of the obstacles.
    /// </summary>
    public List<Vector3> obstaclePositions = new List<Vector3>();

    /// <summary>
    /// List of sizes of the obstacles.
    /// </summary>
    public List<Vector3> obstacleSizes = new List<Vector3>();

    /// <summary>
    /// Initializes the script by resetting and updating the lists.
    /// </summary>
    void Start()
    {
        reset_list();
        update_list();
    }

    /// <summary>
    /// Resets the lists of obstacle positions and sizes.
    /// </summary>
    public void reset_list()
    {
        obstaclePositions.Clear();
        obstacleSizes.Clear();
    }

    /// <summary>
    /// Updates the lists of obstacle positions and sizes by finding all objects with the specified tag.
    /// </summary>
    public void update_list()
    {
        // Find all game objects with the specified tag
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obstacle in obstacles)
        {
            // Get the Renderer component of the obstacle
            Renderer renderer = obstacle.GetComponent<Renderer>();

            if (renderer != null)
            {
                // Get the size of the obstacle
                Vector3 size = renderer.bounds.size;

                // Get the position of the obstacle
                Vector3 position = obstacle.transform.position;

                // Add the position and size to the lists
                obstaclePositions.Add(position);
                obstacleSizes.Add(size);
            }
        }
    }

    /// <summary>
    /// Retrieves the list of obstacle positions.
    /// </summary>
    /// <returns>List of positions of the obstacles.</returns>
    public List<Vector3> GetObstaclePositions()
    {
        return obstaclePositions;
    }

    /// <summary>
    /// Retrieves the list of obstacle sizes.
    /// </summary>
    /// <returns>List of sizes of the obstacles.</returns>
    public List<Vector3> GetObstacleSizes()
    {
        return obstacleSizes;
    }

    /// <summary>
    /// Sets the tag used to identify obstacle objects.
    /// </summary>
    public void set_tag(string new_tag)
    {
        tag = new_tag;
    }

    public void log_obstacle_positions()
    {
        foreach (Vector3 position in obstaclePositions)
        {
            Debug.Log("Obstacle position: " + position);
        }
    }

    public int get_obstacle_min_size() {
        int min_size = int.MaxValue;
         
        foreach (Vector3 size in obstacleSizes) {
            int size_x = (int) size.x;
            int size_y = (int) size.y;
            int size_z = (int) size.z;
            int min = Mathf.Min(size_x, size_y, size_z);
            if (min < min_size) {
                min_size = min;
            }
        }

        return min_size;
    }

    public int get_number_of_obstacles() {
        return obstaclePositions.Count;
    }
    
}

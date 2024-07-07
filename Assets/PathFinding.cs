using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public ObstacleController obstacleController;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float gridSize = 1.0f;
    
    private List<Vector3> path = new List<Vector3>();
    private HashSet<Vector3> obstacles = new HashSet<Vector3>();

    void Start()
    {
        obstacleController.update_list();
        foreach (var pos in obstacleController.GetObstaclePositions())
        {
            obstacles.Add(pos);
        }
        path = FindPath(startPosition, targetPosition);
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        List<Vector3> openList = new List<Vector3>();
        HashSet<Vector3> closedList = new HashSet<Vector3>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
        Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float>();
        Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float>();

        openList.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, target);

        while (openList.Count > 0)
        {
            Vector3 current = GetLowestFScore(openList, fScore);

            if (current == target)
            {
                return ReconstructPath(cameFrom, current);
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (Vector3 neighbor in GetNeighbors(current))
            {
                if (closedList.Contains(neighbor) || obstacles.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector3.Distance(current, neighbor);

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
                else if (tentativeGScore >= gScore[neighbor])
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
            }
        }

        return new List<Vector3>(); // Return an empty path if no path is found
    }

    private float Heuristic(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b);
    }

    private Vector3 GetLowestFScore(List<Vector3> openList, Dictionary<Vector3, float> fScore)
    {
        Vector3 lowest = openList[0];
        float lowestScore = fScore[lowest];

        foreach (Vector3 node in openList)
        {
            if (fScore[node] < lowestScore)
            {
                lowest = node;
                lowestScore = fScore[node];
            }
        }

        return lowest;
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        List<Vector3> totalPath = new List<Vector3> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }

    private List<Vector3> GetNeighbors(Vector3 node)
    {
        List<Vector3> neighbors = new List<Vector3>();

        neighbors.Add(new Vector3(node.x + gridSize, node.y, node.z));
        neighbors.Add(new Vector3(node.x - gridSize, node.y, node.z));
        neighbors.Add(new Vector3(node.x, node.y + gridSize, node.z));
        neighbors.Add(new Vector3(node.x, node.y - gridSize, node.z));
        neighbors.Add(new Vector3(node.x, node.y, node.z + gridSize));
        neighbors.Add(new Vector3(node.x, node.y, node.z - gridSize));

        return neighbors;
    }

    public List<Vector3> GetPath()
    {
        return path;
    }
}

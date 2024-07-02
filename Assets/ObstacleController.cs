using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject[] Obstacles { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        // foreach (GameObject obstacle in Obstacles)
        // {
        //     Debug.Log(obstacle.transform.position);
        // }
    }
}

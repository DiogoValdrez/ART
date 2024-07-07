using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    List<Vector3> path = new List<Vector3>();
    Vector3 CurrentPosition;

    void Update()
    {
        MoveDrone();
    }

    public void MoveDrone()
    {
        if (path.Count > 0)
        {
            Vector3 target = path[0];
            Vector3 direction = target - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.Translate(0, 0, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                path.RemoveAt(0);
            }
        }
    }

    public void set_path(List<Vector3> path)
    {
        this.path = path;
    }
}

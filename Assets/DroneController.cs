using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


/**
    DroneController class

    This class is responsible for interating with the drone object

    This will be the class responsible for changing the drone position and orientation
    based on either a path or simply an open loop

*/
public class DroneController : MonoBehaviour
{
    private float obstacle_avoidance_coeficient = 1.5f;
    private List<Vector3> path = new List<Vector3>();
    private Vector3 CurrentPosition;
    private bool follow_path = false ;
    private float speed = 5.0f;
    private float rotationSpeed = 1.0f;

    /**
        Start function for the drone
         
        Put drone in the starting orientation
    */
    void start() {
        
        transform.Rotate(0, 0, 0);
    }


    public void followPath() {
        return;
    }

    public void openLoop(List<Vector3> obstacles_positions, List<Vector3> obstacles_sizes) {
        Vector3 current_position = transform.position;
        Vector3 current_orientation = transform.rotation.eulerAngles;

        if (obstacles_positions.Count != obstacles_sizes.Count) {
            Debug.Log("Error: obstacles_positions and obstacles_sizes must have the same size");
            return;
        }

        Vector3 avoidance_force = Vector3.zero;

        for (int i = 0; i < obstacles_positions.Count; i++) {
            Vector3 obstacle_position = obstacles_positions[i];
            Vector3 obstacle_size = obstacles_sizes[i];

            Vector3 dist_vector = current_position - obstacle_position;
            Vector2 dist_vector_2d = new Vector2(dist_vector.x, dist_vector.z); 

            float ang = Mathf.Atan2(dist_vector.z, dist_vector.x);


            if (dist_vector_2d.magnitude < obstacle_size.x * 1.5f + 1) {
                avoidance_force += new Vector3(Mathf.Cos(ang), 0, Mathf.Sin(ang)) * speed;
            }
        }

        // Normalize the avoidance force to avoid excessive speeds
        if (avoidance_force != Vector3.zero) {
            avoidance_force.Normalize();
        }

        float norm = avoidance_force.magnitude;
        if (norm == 0) {
            norm = 1;
        }

        Vector3 base_speed_vector = new Vector3(0, 0, speed);

        Vector3 speed_vector = (base_speed_vector + avoidance_force * norm) * Time.deltaTime;
        
        // Combine the base speed and avoidance force
        // Move the drone
        transform.Translate(speed_vector.x, speed_vector.y, speed_vector.z);
    }

    public void MoveDrone(float foward = 0, float side = 0, float updown = 0, float yaw = 0)
    {
        float translation_x = side * speed;
        float translation_y = updown * speed;
        float translation_z =  foward * speed;

        float rotation_pitch = 0;
        float rotation_roll = 0;

        float rotation_yaw = yaw * rotationSpeed;

        translation_x *= Time.deltaTime;
        translation_y *= Time.deltaTime;
        translation_z *= Time.deltaTime;


        rotation_pitch *= Time.deltaTime;
        rotation_roll *= Time.deltaTime;
        rotation_yaw *= Time.deltaTime;

        transform.Translate(translation_x, translation_y, translation_z);
        transform.Rotate(rotation_pitch, rotation_yaw, rotation_roll);
    }

    private void get_next_position()
    {
        if ( this.follow_path  ) {
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
        } else {
            // Current positioon =  
            Vector3 current_position = transform.position;
            Vector3 current_orientation = transform.rotation.eulerAngles;


        }
    }

    public void set_path(List<Vector3> path)
    {
        this.path = path;
    }

    public void set_follow_path(bool follow_path)
    {
        this.follow_path = follow_path;
    }

    public bool get_follow_path()
    {
        return this.follow_path;
    }

    public void set_speed(float speed)
    {
        this.speed = speed;
    }

    public float get_speed()
    {
        return this.speed;
    }

    public void set_rotation_speed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }

    public float get_rotation_speed()
    {
        return this.rotationSpeed;
    }

    public void set_obstacle_avoidance_coeficient(float obstacle_avoidance_coeficient)
    {
        this.obstacle_avoidance_coeficient = obstacle_avoidance_coeficient;

    }

    public float get_obstacle_avoidance_coeficient()
    {
        return this.obstacle_avoidance_coeficient;
    }
}

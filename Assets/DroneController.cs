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
    private List <Vector3> path = new List <Vector3>();

    private Vector3 current_position = new Vector3(0, 0, 0);
    private Vector3 target_position = new Vector3(0, 0, 0);
    private bool follow_path = false ;
    private float speed = 5.0f;
    private float rotationSpeed = 1.0f;
    private int num_step = 100;

    /**
        Start function for the drone
         
        Put drone in the starting orientation
    */
    void start() {
        transform.Rotate(0, 0, 0);
        this.current_position = this.transform.position;
    }


    public void followPath() {
        if (path.Count == 0) {
            Debug.Log("No more map");
            return;
        } 

        Vector3 target_position = path[0];
        Vector3 current_position = transform.position;
        Vector3 diff = (target_position - current_position);

        if (diff.magnitude < 0.1) {
            this.current_position = target_position;;
            path.RemoveAt(0);
            Debug.Log("Target reached");
            return;
        }
        
        Vector3 movement = (target_position - this.current_position) / this.num_step;
        Debug.Log("Movement : " +  movement + " Current position: " + this.current_position + " Target position: " + transform.position + " current_positioon " + current_position);
        
        transform.Translate(movement.x, movement.y, movement.z);


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

    public void set_path(List<Pair<int, int>> path)
    {
        float y = transform.position.y;

        foreach (Pair<int, int> pos in path) {
            

            this.path.Add(new Vector3(pos.First, y, pos.Second));
        }
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

    public Pair<int, int> get_current_positon_in_map()
    {
        return new Pair<int, int>((int)transform.position.x, (int)transform.position.z);
    }

    public float get_obstacle_avoidance_coeficient()
    {
        return this.obstacle_avoidance_coeficient;
    }
}

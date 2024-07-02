using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public float inclination = 10.0f;

    public void MoveDrone(float foward = 0, float side = 0, float updown = 0, float yaw = 0)
    {
        float translation_x = side * speed;
        float translation_y = updown * speed;
        float translation_z =  foward * speed;

        // float rotation_pitch;
        // float rotation_roll;

        // if(foward>0)
        // {
        //     rotation_pitch = inclination;
        // }
        // else if(foward<0)
        // {
        //     rotation_pitch = -inclination;
        // }
        // else
        // {
        //     rotation_pitch = 0;
        // }

        // if(side>0)
        // {
        //     rotation_roll = inclination;
        // }
        // else if(side<0)
        // {
        //     rotation_roll = -inclination;
        // }
        // else
        // {
        //     rotation_roll = 0;
        // }



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
}

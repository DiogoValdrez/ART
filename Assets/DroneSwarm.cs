using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DroneSwarm : MonoBehaviour
{
    public GameObject dronePrefab;
    public int numberOfDrones = 5;
    public Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 targetPosition = new Vector3(10, 0, 10); // Set a target position

   public bool find_path = false;

    public float spacing_x = 1.0f;
    public float spacing_y = 1.0f;
    public float spacing_z = 1.0f;
    public float max_spacing_multiplier = 1.0f;

    private ObstacleController obstacleController;
    private PathFinding pf;

    List<DroneController> drones = new List<DroneController>();

    void Start()
    {
        // Add and configure the ObstacleController
        obstacleController = gameObject.AddComponent<ObstacleController>();
        obstacleController.tag = "obstacle";
        obstacleController.reset_list();
        obstacleController.update_list();
        //obstacleController.log_obstacle_positions();


        for(int i = 0; i < this.numberOfDrones; i++) {
            Vector3 position = startPosition + new Vector3(spacing_x, spacing_y, spacing_z) * i;
            GameObject drone = Instantiate(dronePrefab, position, Quaternion.identity);

            DroneController droneController = drone.GetComponent<DroneController>();
            droneController.set_speed(5.0f);
            droneController.set_obstacle_avoidance_coeficient(1.5f);

            drones.Add(droneController);
        }

        if (this.find_path) {
            this.pf = gameObject.AddComponent<PathFinding>();

            this.pf.init_map();
            this.pf.create_map(obstacleController);
            

            for (int i = 0; i < this.numberOfDrones; i++) {
                Pair<int, int> start = this.drones[i].get_current_positon_in_map();
                Pair<int, int> end = this.drones [i].get_current_positon_in_map();
                end.Second += 100; 

                List<Pair<int, int>> path = this.pf.solve_path(start, end);
                this.pf.reset_map();
                drones[i].set_path(path);

                Debug.Log("New drone") ;
            }
        } else {
            // No need too path finding, the drone will simply try to walk in a straight line
        }


    }

    void Update() {
        foreach (DroneController drone in drones) {
            if (this.find_path) {
                drone.followPath();
            } else {
                drone.openLoop(obstacleController.GetObstaclePositions(), obstacleController.GetObstacleSizes());
            }
        }
    }

    // This return a 2d map that is used used in the path finding algorithm, since we assume that the 
    // drone will only care about the X and Z  axis and will ignore the Y axis (the changes in height)
    private List<List <int>>  create_map (Pair<int, int> start, Pair<int, int> end, int grid_size) {
        int x_map_size = end.First - start.First + 1;
        int z_map_size = end.Second - start.Second + 1;

        List<List<int>> map = new List<List<int>>(); 


        // create and empty map
        for (int i = 0; i < x_map_size; i++) {
            List<int> row = new List<int>();
            for (int j = 0; j < z_map_size; j++) {
                row.Add(0);
            }
            map.Add(row);
        }

        int number_obstacles = obstacleController.get_number_of_obstacles(); 

        // Add obstacles to the map  
        for  (int i = 0; i < number_obstacles; i++)
        {
            Vector3 position = obstacleController.GetObstaclePositions()[i];
            Vector3 size = obstacleController.GetObstacleSizes()[i];
            int x = (int) position.x;
            int z = (int) position.z;
            int size_x = (int) size.x;
            int size_z = (int) size.z;


            for (int j = -size_x; j < size_x; j++) {
                for (int k = -size_z; k < size_z; k++) {
                    if (x + j - start.First >= 0 && x + j - start.First < x_map_size && z + k - start.Second >= 0 && z + k - start.Second < z_map_size){
                        Debug.Log("Obstacle at: " + (x + j) + " " + (z + k) + " size " +  x_map_size + " " + z_map_size);
                        map[x + j - start.First][z + k - start.Second] = 1;
                    }
                }
            }
        }

        return map;
    }

    
}

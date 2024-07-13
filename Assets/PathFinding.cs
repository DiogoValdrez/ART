using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* Path finding class for and Array
*/ 
public class PathFinding : MonoBehaviour
{
    public int grid_size_x = 1000;
    public int grid_size_z = 1000;
    private List <List <Node>> grid = new List <List <Node>>(); 

    public void init_map() { 
        for (int i = 0; i < this.grid_size_x; i++) {
            List <Node> row = new List <Node>();
            for (int j = 0; j < this.grid_size_z; j++) {
                row.Add(new Node(false, true, new Pair<int, int>(-1, -1), new Pair<int, int>(i, j)));
            }
            this.grid.Add(row);
        }
    }
    

    public void create_map (ObstacleController oc) {
        List<Vector3> obstacles_positions = oc.GetObstaclePositions();
        List<Vector3> obstacles_size = oc.GetObstacleSizes();

        if (obstacles_positions.Count != obstacles_size.Count) {
            Debug.Log("Error: obstacles_positions.Count != obstacles_size.Count");
            return;
        }
        
        for (int i = 0; i < obstacles_positions.Count; i++) {
            Vector3 position = obstacles_positions[i];
            Vector3 size = obstacles_size[i];
            int x = (int) position.x;
            int z = (int) position.z;
            int size_x = (int) size.x;
            int size_z = (int) size.z;

            for (int j = -size_x; j < size_x; j++) {
                for (int k = -size_z; k < size_z; k++) {
                    if (x + j >= 0 && x + j < this.grid_size_x && z + k >= 0 && z + k < this.grid_size_z) {
                        this.grid[x + j][z + k].set_walkable(false);
                    }
                }
            }
        }
    }

    public List <Pair<int, int> > solve_path(Pair<int, int> start_pos, Pair<int, int> end_pos) {
        if (start_pos.First < 0 || start_pos.First >= this.grid_size_x || start_pos.Second < 0 || start_pos.Second >= this.grid_size_z) {
            Debug.Log("Error: start_pos out of bounds");
            return new List <Pair<int, int>>();
        }

        Queue < Pair <int, int> > q = new Queue < Pair <int, int> >(); 
        
        q.Enqueue(start_pos);

        List < Pair <int, int>> neighbours = new List < Pair <int, int>> {
            new Pair<int, int>(0, 1),
            new Pair<int, int>(0, -1),
            new Pair<int, int>(1, 0),
            new Pair<int, int>(-1, 0)
        };

        Debug.Log("Solving path");

        // Solve Path
        while (q.Count != 0) {
            Pair <int, int>  current = q.Dequeue();

            if (current.Equals(end_pos)) {
                Debug.Log(current.First + " " + current.Second + " " + end_pos.First + " " + end_pos.Second);
                break;
            }

            foreach (Pair <int, int> neighbour in neighbours) {
                Pair <int, int> new_pos = new Pair<int, int>(current.First + neighbour.First, current.Second + neighbour.Second);
                if ( this.should_enqueue(new_pos) ) {
                    this.grid[new_pos.First][new_pos.Second].set_visited(true);
                    this.grid[new_pos.First][new_pos.Second].set_origin(current);
                    q.Enqueue(new_pos);
                }
            }
        }

        Debug.Log("Path found");
         
        List <Pair<int, int>> path = new List <Pair<int, int>>();
        Pair <int, int> c = end_pos;
        path.Add (c); 

        while (c != start_pos) {
            c = this.grid[c.First][c.Second].get_origin();
            path.Add (c);
        }

        path.Add (start_pos);
        path.Reverse();
        Debug.Log("Path length: " + path.Count);
        
        return path;
    }
    // Reset map after applying one round of path finding
    public void reset_map () {
        for (int i = 0; i < this.grid_size_x; i++) {
            for (int j = 0; j < this.grid_size_z; j++) {
                this.grid[i][j].set_visited(false);
                this.grid[i][j].set_origin(new Pair<int, int>(-1, -1));
            }
        }
    }

    private bool should_enqueue(Pair <int, int> pos) {
        return this.check_bounds(pos) && this.is_walkable(pos) && !this.is_visited(pos);
    }

    private bool check_bounds(Pair<int, int> pos) {
        return pos.First >= 0 && pos.First < this.grid_size_x && pos.Second >= 0 && pos.Second < this.grid_size_z;
    }

    private bool is_walkable(Pair<int, int> pos) {
        return this.grid[pos.First][pos.Second].get_walkable();
    }

    private bool is_visited(Pair<int, int> pos) {
        return this.grid[pos.First][pos.Second].get_visited();
    }

    /**
    * This class hooklds all the atributes that a singles node on the grid need to have to be able to used in a path finding algorithm
    */
    class Node 
    {
        private bool visited; 

        private bool walkable;

        private Pair<int, int> pos; 
        
        private Pair<int, int> origin;

        public Node (bool visited, bool walkable, Pair<int, int> origin, Pair<int, int> pos) {
            this.visited = visited;
            this.walkable = walkable;
            this.origin = origin;
            this.pos = pos;
        }

        public void set_visited(bool visited) {
            this.visited = visited;
        }

        public bool get_visited() {
            return this.visited;
        }

        public void set_walkable(bool walkable) {
            this.walkable = walkable;
        }

        public bool get_walkable() {
            return this.walkable;
        }   

        public void set_origin(Pair<int, int> origin) {
            this.origin = origin;
        }

        public Pair<int, int> get_origin() {
            return this.origin;
        }   
        
        public Pair<int, int> get_pos () {
            return this.pos;
        }
    }
}
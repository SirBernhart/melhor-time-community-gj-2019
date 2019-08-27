using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCrontroller : MonoBehaviour
{
    public enum TileState {Clear, Obstacle, Danger};

    [SerializeField] private int gridSize = 7;
    private Tile[][] grid; // 1st [] = line / 2nd [] = column

    // Initializes the grid
    void Start()
    {
        grid = new Tile[gridSize][];

        for(int i = 0 ; i < gridSize ; i++)
        {
            Transform currentLine = transform.GetChild(i);
            grid[i] = new Tile[7];
            for(int j = 0 ; j < gridSize ; j++)
            {
                grid[i][j] = currentLine.GetChild(j).GetComponent<Tile>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

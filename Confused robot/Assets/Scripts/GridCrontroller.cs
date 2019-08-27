using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCrontroller : MonoBehaviour
{
    public enum TileState {Clear, Obstacle, Danger, Enemy, Player};

    [SerializeField] private int gridSize = 7;
    private Tile[][] grid; // 1st [] = line / 2nd [] = column

    // Initializes the grid
    void Start()
    {
        grid = new Tile[gridSize][];

        for(int i = 0 ; i < gridSize ; i++) // Lines
        {
            Transform currentLine = transform.GetChild(i);
            grid[i] = new Tile[7];
            for(int j = 0 ; j < gridSize ; j++) // Columns
            {
                grid[i][j] = currentLine.GetChild(j).GetComponent<Tile>();
                grid[i][j].position = new Vector2(i, j);
            }
        }
    }

    // Moves the entity, in the passed entityPos, tilesToMoveInX and tilesToMoveInY tiles
    // Returns the entity's new position
    public Vector2 MoveEntity (Vector2 entityPos, int tilesToMoveInX, int tilesToMoveInY)
    {
        // Entity's pos in int
        int ePosX = (int) entityPos.x, 
            ePosY = (int)entityPos.y;

        // Getting the entity in the (ePosX, ePosY) grid position
        Tile entity = grid[ePosX][ePosY];

        // If the passed entity isn't moveable, don't do anything 
        if(entity.GetState() != TileState.Player && entity.GetState() != TileState.Enemy)
        {
            return entityPos;
        }

        // If the passed entity's move isn't possible, don't do anything
        if (CheckIfCanMove(entity, tilesToMoveInX, tilesToMoveInY))
        {
            return entityPos;
        }
        return Vector2.zero;
    }

    private bool CheckIfCanMove(Tile tile, int movX, int movY)
    {
        int newXPos = (int) tile.position.x + movX,
            newYPos = (int) tile.position.y + movY;

        if (newXPos >= gridSize ||
            newXPos + movX < 0 ||
            newYPos + movY >= gridSize ||
            newYPos < 0 ||
            grid[newXPos][newYPos].GetState() == TileState.Obstacle)
        {
            return false;
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
        Debug.Log(grid[0][0].GetState());
        Debug.Log(CheckIfCanMove(grid[0][0].position, -1f, false));

    }

    // Moves the entity found in the grid's passed entityPos position "tilesToMove" in the x (if moveInX == true), else in the y axis  
    // Returns the entity's new position
    // ===> If the new position is negative, it means the player has died
    public Vector2 MoveEntity (Vector2 entityPos, float tilesToMove, bool moveInX)
    {
        Tile currentTile = grid[(int)entityPos.x][(int)entityPos.y];
        

        if(CheckIfCanMove(entityPos, tilesToMove, moveInX))
        {
            Tile newEntityTile = GetTileInNewPosition(entityPos, tilesToMove, moveInX);
            if (newEntityTile == null)
            {
                return entityPos;
            }
            // Instances where the player dies
            if (currentTile.GetState() == TileState.Enemy)
            {
                if(newEntityTile.GetState() == TileState.Player)
                {
                    return new Vector2(-1, -1);
                }
            }
            else if(currentTile.GetState() == TileState.Player)
            {
                if(newEntityTile.GetState() == TileState.Danger || newEntityTile.GetState() == TileState.Enemy) 
                {
                    return new Vector2(-1, -1);
                }
            }

            // If the player doesn't die with the move
            newEntityTile.SetState(currentTile.GetState());
            currentTile.SetState(TileState.Clear);

            return newEntityTile.position;

        }
        return entityPos;
    }

    // Checks if the entity is capable of moving in the passed direction
    private bool CheckIfCanMove(Vector2 entityPos, float tilesToMove, bool moveInX)
    {
        Tile newEntityTile = GetTileInNewPosition(entityPos, tilesToMove, moveInX);

        if(newEntityTile == null)
        {
            Debug.Log("Reached end of grid!");
            return false;
        }

        if (grid[(int)newEntityTile.position.x][(int)newEntityTile.position.y].GetState() == TileState.Obstacle)
        {
            Debug.Log("There is an obstacle on the way");
            return false;
        }

        return true;
    }

    // Gets the Tile in the position arrived when moving "tilesToMove" tiles from "currentTilePos" in the passed axis
    private Tile GetTileInNewPosition (Vector2 currentTilePos, float tilesToMove, bool moveInX)
    {
        int newXPos = (int)currentTilePos.x + (int)tilesToMove,
            newYPos = (int)currentTilePos.y + (int)tilesToMove;
        if (moveInX)
        {
            newXPos = (int)currentTilePos.x + (int)tilesToMove;
            newYPos = (int)currentTilePos.y;
        }
        else
        {
            newXPos = (int)currentTilePos.x;
            newYPos = (int)currentTilePos.y + (int)tilesToMove;
        }

        // If the new position surpasses the grid's borders, returns null
        if(newXPos >= gridSize ||
            newXPos < 0 ||
            newYPos >= gridSize ||
            newYPos < 0)
        {
            return null;
        }

        // Returning the tile in the (newXPos, newYPos) grid position
        return grid[newXPos][newYPos];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

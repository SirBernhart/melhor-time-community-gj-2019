using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GridCrontroller : MonoBehaviour
{
    
    public enum TileState {Clear, Obstacle, Danger, Enemy, Player, Button, Door, Puddle};

    [SerializeField] private SceneTransitionController transitionController;
    [SerializeField] private int gridSize = 7;
    private Tile[][] grid; // 1st [] = line / 2nd [] = column

    private int pressedButtonsCount;
    private int buttonsCount;
    HashSet<Tile> pressedButtons;

    private Door _door;
    public AudioSource doorUnlock;
    public AudioSource buttonPress;
    private PlayerMovement _pm;

    // Initializes the grid
    void Start()
    {
        grid = new Tile[gridSize][];

        buttonsCount = 0;

        for (int i = 0; i < gridSize; i++) // Lines
        {
            Transform currentLine = transform.GetChild(i);
            grid[i] = new Tile[gridSize];
            for (int j = 0; j < gridSize; j++) // Columns
            {
                grid[i][j] = currentLine.GetChild(j).GetComponent<Tile>();
                grid[i][j].position = new Vector2(i, j);
                if(grid[i][j].GetState() == TileState.Button)
                {
                    buttonsCount++;
                }
            }
        }

        pressedButtonsCount = 0; //0 button are pressed on scene start
        pressedButtons = new HashSet<Tile>();
        
        _door = GameObject.FindObjectOfType<Door>();
        SetDoorLights();

        _pm = GameObject.FindObjectOfType<PlayerMovement>();

    }

    // Moves the entity found in the grid's passed entityPos position "tilesToMove" in the x (if moveInX == true), else in the y axis  
    // Returns the entity's new position
    // ===> If the new position is negative, it means the player has died
    public Tile MoveEntity (Tile entityTile, float tilesToMove, bool moveInX)
    {
        if(CheckIfCanMove(entityTile.position, tilesToMove, moveInX))
        {
            Tile newEntityTile = GetTileInNewPosition(entityTile.position, tilesToMove, moveInX);
            if (newEntityTile == null)
            {
                return entityTile;
            }
            // Instances where the player dies
            if (entityTile.GetState() == TileState.Enemy)
            {
                if(newEntityTile.GetState() == TileState.Player)
                {
                    return null;
                }
            }
            else if(entityTile.GetState() == TileState.Player)
            {
                if(newEntityTile.GetState() == TileState.Danger || newEntityTile.GetState() == TileState.Enemy) 
                {
                    _pm.EndGame();
                    return newEntityTile;
                }
            }


            //the player reached a button
            if(newEntityTile.GetState() == TileState.Button)
            {
                //has the button *not* already been pressed?
                if(!pressedButtons.Contains(newEntityTile))
                {
                    //we press a new button
                    pressedButtonsCount++;
                    pressedButtons.Add(newEntityTile);
                    buttonPress.Play();
                    
                }
                SetDoorLights();

            }
            // The player reached the door
            //if the player has already pressed all the needed buttons,
            if(newEntityTile.GetState() == TileState.Door && pressedButtonsCount >= buttonsCount)
            {
                SetDoorLights();
                transitionController.FadeOutOfScene();
            }
            else if(newEntityTile.GetState() == TileState.Door)
            {
                //if i entered the door, but am not allowed to access it, i still set the lights
                SetDoorLights();
            }

            // If the player doesn't die with the move
            newEntityTile.SetState(entityTile.GetState());
            entityTile.SetState(TileState.Clear);

            return newEntityTile;
        }
        return entityTile;
    }

    private void SetDoorLights()
    {
        if(_door == null) return;

        
        if(pressedButtonsCount >= buttonsCount)
        {
            _door.SetLight(DoorAccess.Allowed);
            doorUnlock.Play();
        }
        else if(pressedButtonsCount == 0)
        {
            _door.SetLight(DoorAccess.Denied);
        }
        else if(pressedButtonsCount < buttonsCount)
        {
            _door.SetLight(DoorAccess.Intermediate);
        }
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

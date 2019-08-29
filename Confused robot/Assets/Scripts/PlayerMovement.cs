using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GridCrontroller gridControllerScript;
    [Tooltip("Ponha aqui o tile inicial")]
    [SerializeField] private Tile currentTile;
    [SerializeField] private float movementCooldown = 1.5f;
    private bool canMove = true;
    
    void Update()
    {
        float moveValue;
        if((moveValue = Input.GetAxisRaw("Horizontal")) != 0f)
        {
            HandleMovement(moveValue, false);
        }
        else if((moveValue = Input.GetAxisRaw("Vertical")) != 0f)
        {
            HandleMovement(moveValue, true);
        }
    }

    private void HandleMovement(float moveValue, bool moveInX)
    {
        if (canMove)
        {
            currentTile = gridControllerScript.MoveEntity(currentTile, moveValue, moveInX);
            transform.SetParent(currentTile.transform);
            canMove = false;
            StartCoroutine(Timer(movementCooldown));
        }
    }

    // Used to count the cooldown time
    IEnumerator Timer(float maxTime)
    {
        float currentTime = 0;
        while(currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        canMove = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GridCrontroller gridControllerScript;
    [SerializeField] private Animator animator;
    [Tooltip("Ponha aqui o tile inicial")]
    [SerializeField] private Tile currentTile;
    [SerializeField] private float movementCooldown = 1.5f;
    [SerializeField] private float moveSpeed = 1000;
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
            Tile newTile = gridControllerScript.MoveEntity(currentTile, moveValue, moveInX);
            
            if (!newTile.gameObject.Equals(currentTile.gameObject))
            {
                animator.SetTrigger("Moved");
                transform.LookAt(newTile.transform.position);
            
                canMove = false;
                StartCoroutine(MakeTransition(movementCooldown, newTile));
            }
        }
    }


    // Used to count the cooldown time
    IEnumerator MakeTransition(float maxTime, Tile newTile)
    {
        float currentTime = 0;
        while(currentTime < maxTime)
        {
            if(Vector3.Distance(currentTile.transform.position, transform.position) <= 1)
            {
                transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
            }
            
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.SetParent(newTile.transform);
        currentTile = newTile;
        transform.position.Set(0f, 0f, 0f);
        canMove = true;
        Debug.Log(currentTile.position);
    }
}

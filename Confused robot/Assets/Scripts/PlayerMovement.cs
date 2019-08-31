using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GridCrontroller gridControllerScript;
    [SerializeField] private Animator animator;
    [Tooltip("Ponha aqui o tile inicial")]
    [SerializeField] private Tile currentTile;
    [SerializeField] private float movementCooldown = 1.5f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float timeToStartMove;
    private bool canMove = true;

    private List<int> shuffledDirections = new List<int>{0,1,2,3}; // s,d,a,w ???

    private float[] moveValues = {-1.0f, 1.0f, -1.0f, 1.0f}; //s,d,a,w

    private bool[] boolValues = {true, false, false, true}; //s,d,a,w

    public bool shuffledMovement  = false;

    void Update()
    {
        float moveValue;
        if((moveValue = Input.GetAxisRaw("Horizontal")) != 0f)
        {
            ButtonToMovement(moveValue, false);
        }
        else if((moveValue = Input.GetAxisRaw("Vertical")) != 0f)
        {
            ButtonToMovement(moveValue, true);
        }
        
    }

    //transforma o input do teclado para o movimento "embaralhado"
    private void ButtonToMovement(float moveValue, bool moveInX)
    {
        if(!shuffledMovement)
        {
            //normal movement
            HandleMovement(moveValue, moveInX);
            return;
        }
        
        int index = 0;
        if(moveValue < 0.0f && moveInX)
        {
            index = 0;    
        }
        else if(moveValue > 0.0f && !moveInX)
        {
            index = 1;
        }
        else if(moveValue < 0.0f && !moveInX)
        {
            index = 2;
        }
        else if(moveValue > 0.0f && moveInX)
        {
            index = 3;
        }
        else
        {
            Debug.LogWarning("IF não entrou em nenhum caso");
        }

        Debug.Log(index);

        ShuffleList();
        HandleMovement(moveValues[shuffledDirections[index]], boolValues[shuffledDirections[index]]);
        

    }

    private void ShuffleList()
    {
        List<int> newList = new List<int>();
        int count = shuffledDirections.Count;
        for(int i = 0; i < count; i++)
        {
            int index = Random.Range(0, shuffledDirections.Count);
            newList.Add(shuffledDirections[index]);
            shuffledDirections.RemoveAt(index);
        }
        shuffledDirections = newList;
    }

    private void HandleMovement(float moveValue, bool moveInX)
    {
        if (canMove)
        {
            Tile newTile = gridControllerScript.MoveEntity(currentTile, moveValue, moveInX);
            
            // The player has died
            if(newTile == null)
            {
                sceneController.ReloadScene();
                return;
            }

            if (!newTile.gameObject.Equals(currentTile.gameObject))
            {
                transform.LookAt(newTile.transform.position);
                animator.SetTrigger("Moved");
            
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
            if(currentTime >= timeToStartMove && Vector3.Distance(currentTile.transform.position, transform.position) <= 1)
            {
                transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
            }
            
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.SetParent(newTile.transform);
        currentTile = newTile;
        transform.position.Set(0f, 0f, 0f);
        Debug.Log(transform.position);
        canMove = true;
        Debug.Log(currentTile.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GridCrontroller gridControllerScript;
    [SerializeField] private Animator animator;
    [Tooltip("Ponha aqui o tile inicial")]
    private Tile currentTile;
    [SerializeField] private float movementCooldown = 1.5f;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float timeToStartMove;
    private bool canMove = true;
    private int movesToJumbleKeys;
    [SerializeField] private JumbleKeysDifficulty difficultySO;


    private List<int> shuffledDirections = new List<int>{0,1,2,3}; // s,d,a,w ???

    private float[] moveValues = {-1.0f, 1.0f, -1.0f, 1.0f}; //s,d,a,w

    private bool[] boolValues = {true, false, false, true}; //s,d,a,w

    public bool shuffledMovement  = false;

    private Countdown _countdown;

    private bool gameHasEnded;

    public void EndGame()
    {
        gameHasEnded = true;
    }

    // Checks if it's necessary to reduce the number of moves to jumble the keys
    private void Start()
    {
        difficultySO.ResetToBaseValue(); // Reseta o valor do currentMovesToJumble se estiver na primeira cena
        difficultySO.DecreaseMovesToJumble();
        movesToJumbleKeys = difficultySO.currentMovesToJumble;

        currentTile = transform.parent.GetComponent<Tile>();
        shuffledMovement = true;

        _countdown = GameObject.FindObjectOfType<Countdown>();

    }

    void Update()
    {
        float moveValue;
        if(canMove && !gameHasEnded)
        {
            if((moveValue = Input.GetAxisRaw("Horizontal")) != 0f)
            {
                ButtonToMovement(moveValue, false);
                _countdown.StartCountdown(); //começa countdown, só se não tiver começado já
            }
            else if((moveValue = Input.GetAxisRaw("Vertical")) != 0f)
            {
                ButtonToMovement(moveValue, true);
                _countdown.StartCountdown(); //começa countdown, só se não tiver começado já
            }

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
            //s
            index = 0;    
        }
        else if(moveValue > 0.0f && !moveInX)
        {
            //d
            index = 1;
        }
        else if(moveValue < 0.0f && !moveInX)
        {
            //a
            index = 2;
        }
        else if(moveValue > 0.0f && moveInX)
        {
            //w
            index = 3;
        }
        else
        {
            Debug.LogWarning("IF não entrou em nenhum caso");
        }

        Debug.Log(index);
        Debug.Log(moveValues[shuffledDirections[index]].ToString() + boolValues[shuffledDirections[index]].ToString());

        HandleMovement(moveValues[shuffledDirections[index]], boolValues[shuffledDirections[index]]);

        if (movesToJumbleKeys <= 1)
        {
            movesToJumbleKeys = difficultySO.currentMovesToJumble;
            ShuffleList();
            ControlsInfo.Instance.UpdateDisplay(shuffledDirections);
            // Play som curto circuito
            animator.SetTrigger("Short Circuit");
        }

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

            if (!newTile.gameObject.Equals(currentTile.gameObject))
            {
                movesToJumbleKeys--;
                Debug.Log("Moves to jumble: " + movesToJumbleKeys);
                transform.LookAt(newTile.transform.position);
                animator.SetTrigger("Moved");
            
                StartCoroutine(MakeTransition(movementCooldown, newTile));
            }
            else
            {
                movesToJumbleKeys--;
                Debug.Log("Moves to jumble: " + movesToJumbleKeys);
                StartCoroutine(TimerCooldownCantMove(movementCooldown));
            }
        }
    }

    // Used when the player can't move
    IEnumerator TimerCooldownCantMove(float maxTime)
    {
        float currentTime = 0;

        canMove = false;
        while (currentTime < maxTime - 0.5)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        canMove = true;
    }

    // Used to count the cooldown time
    IEnumerator MakeTransition(float maxTime, Tile newTile)
    {
        float currentTime = 0;

        canMove = false;
        while (currentTime < maxTime)
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
        canMove = true;
    }
}

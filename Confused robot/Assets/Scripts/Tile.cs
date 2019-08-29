using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GridCrontroller.TileState state;
    [HideInInspector] public Vector2 position = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GridCrontroller.TileState GetState()
    {
        return state;
    }

    public void SetState(GridCrontroller.TileState state)
    {
        this.state = state;
    }
}

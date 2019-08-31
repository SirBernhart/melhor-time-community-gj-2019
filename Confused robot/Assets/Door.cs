using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum DoorAccess{Allowed, Denied, Intermediate}
public class Door : MonoBehaviour
{
    
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLight(DoorAccess da)
    {
        switch(da)
        {
            case DoorAccess.Allowed:
                sprite1.color = Color.green;
                sprite2.color = Color.green;
                break;
            case DoorAccess.Denied:
                sprite1.color = Color.red;
                sprite2.color = Color.red;
                break;
            case DoorAccess.Intermediate:
                sprite1.color = Color.yellow;
                sprite2.color = Color.yellow;
                break;
        }
    }
}

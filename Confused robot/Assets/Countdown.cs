using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    
    public float CountdownTime = 60.0f;

    private float timeStep = 1.0f;

    private Text _text;

    private bool hasStarted;

    public GameObject losePanel;

    public void StartCountdown()
    {
        if(!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(DoCountdown());
        }
    }

    IEnumerator DoCountdown()
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < CountdownTime)
        {
            yield return new WaitForSecondsRealtime(timeStep);
            elapsedTime += timeStep;
            _text.text = (CountdownTime - elapsedTime).ToString();
        }
        
        var pm = GameObject.FindObjectOfType<PlayerMovement>();
        if(pm != null)
        {
            pm.EndGame();
        }

    }
    
    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
        _text = GetComponent<Text>();
        _text.text = (CountdownTime).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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

    //o painel a ser aberto quando você perdeu o jogo
    public GameObject losePanel;

    public void StartCountdown()
    {
        if(!hasStarted)
        {
            hasStarted = true;
            StartCoroutine("DoCountdown");
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
        if(losePanel != null)
        {
            losePanel.SetActive(true);
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

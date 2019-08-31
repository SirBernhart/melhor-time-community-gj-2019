using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetSceneHardcoded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Reset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        GameObject.FindObjectOfType<SceneTransitionController>().ResetScene();
    }
}

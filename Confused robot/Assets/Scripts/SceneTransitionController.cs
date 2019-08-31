using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    // Fades fancily out of the scene
    public void FadeOutOfScene()
    {
        sceneController.LoadNextLevel();
    }

    // Fades fancily into the scene
    public void FadeIntoScene()
    {
        
    }
}

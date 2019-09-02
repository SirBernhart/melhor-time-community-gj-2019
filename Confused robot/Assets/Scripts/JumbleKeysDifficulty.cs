using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Jumble keys difficulty", menuName = "Game Configs")]
public class JumbleKeysDifficulty : ScriptableObject
{
    [Tooltip("Em quanto será reduzido o número de movimentos necessários pra cada cena na lista abaixo")]
    public int movesToJumbleDecrese;
    public List<string> sceneNamesToUpDifficulty;
    public int currentMovesToJumble;
    public int baseMovesToJumble;
    public string lastSceneName = SceneManager.GetActiveScene().name;

    public void ResetToBaseValue()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            currentMovesToJumble = baseMovesToJumble;
        }
    }

    public void DecreaseMovesToJumble()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        for (int i = 0 ; i < sceneNamesToUpDifficulty.Count ; i++)
        {
            if(currentSceneName == sceneNamesToUpDifficulty[i])
            {
                if(currentMovesToJumble - movesToJumbleDecrese > 0)
                {
                    currentMovesToJumble -= movesToJumbleDecrese;
                }
                return;
            }
        }
    }
}

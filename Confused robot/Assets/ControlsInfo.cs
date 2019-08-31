using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsInfo : MonoBehaviour
{
    [Header("s,d,a,w")]
    public List<Image> UIImages;

    public List<Sprite> sprites;

    public static ControlsInfo Instance {get; private set;}
    
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplay(List<int> shuffledDirections)
    {
        for(int i = 0; i < UIImages.Count; i++)
        {
            UIImages[i].sprite = sprites[shuffledDirections[i]];
        }
    }
}

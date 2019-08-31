using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsInfo : MonoBehaviour
{
    public List<Image> UIImages;

    public List<Sprite> sprites;
    
    // Start is called before the first frame update
    void Start()
    {
        
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

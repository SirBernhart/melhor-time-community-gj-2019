using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource pulo;
    public AudioSource shock;

    public void PlayPulo()
    {
        pulo.pitch = Random.Range(0.55f, 1.25f);
        pulo.Play();
    }
    public void PlayShock()
    {
        shock.Play();
    }
}

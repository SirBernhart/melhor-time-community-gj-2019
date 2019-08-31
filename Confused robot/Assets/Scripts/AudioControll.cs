using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControll : MonoBehaviour
{
    public bool stops;
    public float timeToStop, timeToReturn;
    public AudioSource som;
    public float fadeTimeMax;

    float timer = 0;
    // Update is called once per frame
    void Start()
    {
        som.Play();
        if (stops)
        {
            StartCoroutine(CountdownStopSound());
        }
    }

    IEnumerator CountdownStopSound()
    {
        StopCoroutine(CountdownReturnSound());
        while (timer < timeToStop)
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
            yield return null;
        }
        // Fade out
        float fadeTimer = fadeTimeMax;

        while (fadeTimer < fadeTimeMax)
        {
            som.volume = fadeTimeMax / fadeTimer;
            Debug.Log(som.volume);
            fadeTimer -= Time.deltaTime;
            yield return null;
        }
        som.Stop();
        timer = 0;
        StartCoroutine(CountdownReturnSound());
    }

    IEnumerator CountdownReturnSound()
    {
        StopCoroutine(CountdownStopSound());

        while (timer < timeToReturn)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        som.Play();

        // Fade in
        float fadeTimer = 0;

        while (fadeTimer < fadeTimeMax)
        {
            som.volume = fadeTimeMax / fadeTimer;
            fadeTimer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        StartCoroutine(CountdownStopSound());
    }
}

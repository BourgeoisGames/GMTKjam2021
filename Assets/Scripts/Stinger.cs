using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    public AudioSource stingerSource;

    public float minPitch;
    public float maxPitch;
    public float minTime;
    public float maxTime;
    private float timer;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (stingerSource.isPlaying)
            return;

        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            stingerSource.pitch = Random.Range(minPitch, maxPitch);
            stingerSource.Play();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minTime, maxTime);
    }
}

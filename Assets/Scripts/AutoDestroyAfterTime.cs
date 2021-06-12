using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
    public float time = 5.0f;

    private float timer;

    private void Start()
    {
        timer = 0.0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > time)
            Destroy(gameObject);
    }
}

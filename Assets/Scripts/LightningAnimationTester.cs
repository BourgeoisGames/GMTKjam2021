using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAnimationTester : MonoBehaviour
{
    public LightningEndpoint pt1, pt2;
    public LightningAnimation anim;

    private float timer = 0.2f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer += 0.2f;

            pt1.transform.position = Random.insideUnitSphere * 5.0f;
            pt2.transform.position = Random.insideUnitSphere * 5.0f;
            anim.set_points(pt1.transform.position, pt2.transform.position);
        }
    }
}

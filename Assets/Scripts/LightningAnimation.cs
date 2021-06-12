using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAnimation : MonoBehaviour
{
    public void set_points(Vector3 start, Vector3 end)
    {
        // Yes, this uses the transform.
        // If we modify the lightning animation to the point where this code is no longer valid,
        // we should scrap it and re-code it anyway since that's how big a change it would be.
        transform.position = (start + end) / 2.0f;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, end - start);
        transform.localScale = new Vector3(transform.localScale.x, (end - start).magnitude / 2.0f, transform.localScale.z);
    }

    public void despawn()
    {
        // Despawn the lightning, very simple for now
        Destroy(gameObject);
    }
}

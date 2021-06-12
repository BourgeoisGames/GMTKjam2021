using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEndpoint : MonoBehaviour
{
    public void despawn()
    {
        // Despawn the lightning ball, very simple for now
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEndpoint : MonoBehaviour
{

	public Transform endpoint_transform {
		get { return transform; }
	}

    public void despawn()
    {
        // Despawn the lightning ball, very simple for now
        Destroy(gameObject);
    }
}

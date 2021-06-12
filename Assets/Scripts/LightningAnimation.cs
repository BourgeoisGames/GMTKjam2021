using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAnimation : MonoBehaviour
{
    public LightningMeshGeneration[] meshes;

    public void set_points(Vector3 start, Vector3 end)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].set_points(start, end);
        }
    }

    public void despawn()
    {
        // Despawn the lightning, very simple for now
        Destroy(gameObject);
    }
}

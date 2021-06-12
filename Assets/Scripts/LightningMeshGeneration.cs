using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMeshGeneration : MonoBehaviour
{
    public MeshFilter lightning;

    // Radius of the lightning bolt
    public float boltRadius = 0.05f;
    // Number of triangular tube control points in the middle of the beam
    public int midsteps = 4;

    // Amplitude of noise
    public float noiseAmplitude = 0.4f;
    // Seconds before each "skip"
    public float skipSeconds = 1.0f;
    // Frequency of noise along beam (per midstep)
    public float spatialFreq = 1.0f;
    // Frequency of noise in time (per second)
    public float temporalFreq = 1.0f;

    private Mesh lightningMesh;

    private Vector3 startPos, endPos;

    private float noiseTimer;

    private void Start()
    {
        noiseTimer = Random.value * 10000.0f; // Initial "seed"

        lightning.transform.position = Vector3.zero;
        lightning.transform.rotation = Quaternion.identity;
        lightning.transform.localScale = Vector3.one;

        lightningMesh = new Mesh();
        lightning.mesh = lightningMesh;

        GenerateMeshVertices();

        // The triangles stay constant, so we can just do that part once here
        int[] meshTriangles = new int[18 * midsteps];

        // This is nasty ... just trust me, this is how the lightning bolt geometry works
        // Unfortunately I can't adequately explain how this code works in a comment ...

        meshTriangles[0] = 0;
        meshTriangles[1] = 1;
        meshTriangles[2] = 2;
        meshTriangles[3] = 0;
        meshTriangles[4] = 2;
        meshTriangles[5] = 3;
        meshTriangles[6] = 0;
        meshTriangles[7] = 3;
        meshTriangles[8] = 1;

        for (int i = 0; i < midsteps - 1; i++)
        {
            int triangleIndex = 9 + 18 * i;
            int vertexIndex = 1 + 3 * i;

            meshTriangles[triangleIndex] = vertexIndex;
            meshTriangles[triangleIndex + 1] = vertexIndex + 3;
            meshTriangles[triangleIndex + 2] = vertexIndex + 1;
            meshTriangles[triangleIndex + 3] = vertexIndex + 1;
            meshTriangles[triangleIndex + 4] = vertexIndex + 3;
            meshTriangles[triangleIndex + 5] = vertexIndex + 4;
            meshTriangles[triangleIndex + 6] = vertexIndex + 1;
            meshTriangles[triangleIndex + 7] = vertexIndex + 4;
            meshTriangles[triangleIndex + 8] = vertexIndex + 5;
            meshTriangles[triangleIndex + 9] = vertexIndex + 1;
            meshTriangles[triangleIndex + 10] = vertexIndex + 5;
            meshTriangles[triangleIndex + 11] = vertexIndex + 2;
            meshTriangles[triangleIndex + 12] = vertexIndex + 2;
            meshTriangles[triangleIndex + 13] = vertexIndex + 5;
            meshTriangles[triangleIndex + 14] = vertexIndex + 3;
            meshTriangles[triangleIndex + 15] = vertexIndex + 2;
            meshTriangles[triangleIndex + 16] = vertexIndex + 3;
            meshTriangles[triangleIndex + 17] = vertexIndex;
        }

        meshTriangles[meshTriangles.Length - 9] = 2 + 3 * midsteps - 1;
        meshTriangles[meshTriangles.Length - 8] = 2 + 3 * midsteps - 2;
        meshTriangles[meshTriangles.Length - 7] = 2 + 3 * midsteps - 3;
        meshTriangles[meshTriangles.Length - 6] = 2 + 3 * midsteps - 1;
        meshTriangles[meshTriangles.Length - 5] = 2 + 3 * midsteps - 3;
        meshTriangles[meshTriangles.Length - 4] = 2 + 3 * midsteps - 4;
        meshTriangles[meshTriangles.Length - 3] = 2 + 3 * midsteps - 1;
        meshTriangles[meshTriangles.Length - 2] = 2 + 3 * midsteps - 4;
        meshTriangles[meshTriangles.Length - 1] = 2 + 3 * midsteps - 2;

        lightningMesh.triangles = meshTriangles;
        lightningMesh.RecalculateBounds();
    }

    private void GenerateMeshVertices()
    {
        // Come up with a lightning bolt between the points!
        Vector3[] meshVertices = new Vector3[2 + 3 * midsteps];

        if (startPos == endPos)
        {
            for (int i = 0; i < meshVertices.Length; i++)
                meshVertices[i] = startPos;

            lightningMesh.vertices = meshVertices;
            return;
        }

        // Find offsets in the shape of a triangle ... using MATH! HAHAHAHA
        Vector3 y = Vector3.ProjectOnPlane(Vector3.up, endPos - startPos);
        if (y.magnitude < 0.01f)
            y = Vector3.ProjectOnPlane(Vector3.forward, endPos - startPos);
        y = y.normalized;
        Vector3 x = Vector3.Cross(endPos - startPos, y).normalized;

        Vector3 offset1 = boltRadius * y;
        Vector3 offset2 = -0.5f * boltRadius * y + 0.866f * boltRadius * x; // 0.866 is sqrt(3)/2, this is for triangles
        Vector3 offset3 = -0.5f * boltRadius * y - 0.866f * boltRadius * x;

        meshVertices[0] = startPos;
        meshVertices[meshVertices.Length - 1] = endPos;
        for (int i = 0; i < midsteps; i++)
        {
            int vertexIndex = 1 + 3 * i;
            Vector3 triangleBasePosition = Vector3.Lerp(startPos, endPos, (1 + i) / (float)(midsteps + 1));

            // Add noise
            float noiseY = noiseTimer * temporalFreq;
            float noiseX = i * spatialFreq;

            float addedX = (Mathf.PerlinNoise(noiseX, noiseY) * 2.0f - 1.0f) * noiseAmplitude;
            float addedY = (Mathf.PerlinNoise(noiseX + 10000.0f, noiseY) * 2.0f - 1.0f) * noiseAmplitude;
            Vector3 noiseAddition = addedX * x + addedY * y;

            meshVertices[vertexIndex] = triangleBasePosition + noiseAddition + offset1;
            meshVertices[vertexIndex + 1] = triangleBasePosition + noiseAddition + offset2;
            meshVertices[vertexIndex + 2] = triangleBasePosition + noiseAddition + offset3;
        }

        lightningMesh.vertices = meshVertices;
    }

    private void Update()
    {
        noiseTimer += Time.deltaTime;
        if (noiseTimer % (5.0f * skipSeconds) > skipSeconds)
            noiseTimer += 4.0f * skipSeconds;

        GenerateMeshVertices();
        lightningMesh.RecalculateBounds();
    }

    public void set_points(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;
    }
}

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Sway : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] newVertices;

    [SerializeField] private float swaySpeed = 1f;
    [SerializeField] private float swayAmount = 0.05f;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        newVertices = new Vector3[originalVertices.Length];
    }

    private void Update()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            if (originalVertices[i].z <= 0.1f) //rotated -90f deg is world y, to stop the vertices connecting ground from swaying
            {
                newVertices[i] = originalVertices[i];
                continue;
            }

            float offset = Mathf.Sin(Time.time * swaySpeed + originalVertices[i].x) * swayAmount;
            newVertices[i] = originalVertices[i] + Vector3.right * offset;
        }

        mesh.vertices = newVertices;
    }
}
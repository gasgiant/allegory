using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge {

    public Vector3 start;
    public Vector3 dir;
    public float length;

    public Edge(Vector3 a, Vector3 b)
    {
        start = a;
        dir = (b - a).normalized;
        length = (b - a).magnitude;
    }
}

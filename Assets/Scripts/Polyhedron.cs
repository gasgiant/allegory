using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New polyhedron", menuName = "Polyhedron")]
public class Polyhedron : ScriptableObject {

    public List<edgeIn> edges;
    public List<Face> faces;

}

[System.Serializable]
public struct Face
{
    public List<int> edgeIndices;
}

[System.Serializable]
public struct edgeIn
{
    public Vector3 a;
    public Vector3 b;
}

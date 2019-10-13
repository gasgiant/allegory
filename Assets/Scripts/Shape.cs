using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public Plane plane;
    public Polyhedron ph;
    public MeshFilter meshFilter;
    public LineRenderer lineRenderer;
    public Renderer rend;
    public bool createInEditor = true;
    public Color tint;
    public bool drawGizmos;

    List<Edge> edges;
    Dictionary<int, Node> nodes;
    List<Vector3> polygonVertices;
    List<Vector2> projectedVertices;

    Transform meshTr;

	void Start()
	{
        edges = new List<Edge>();
        nodes = new Dictionary<int, Node>();
        polygonVertices = new List<Vector3>();
        projectedVertices = new List<Vector2>();
        createInEditor = false;
        meshTr = meshFilter.transform;
        plane = Plane.Instance;
	}

	void Update()
	{
        CreateShape();
	}

	void CreateShape()
    {
        edges.Clear();
        foreach (var edge in ph.edges)
        {
            Vector3 a = edge.a;
            Vector3 b = edge.b;
            a = new Vector3(a.x * transform.localScale.x, a.y * transform.localScale.y, a.z * transform.localScale.z);
            b = new Vector3(b.x * transform.localScale.x, b.y * transform.localScale.y, b.z * transform.localScale.z);
            a = transform.rotation * a;
            b = transform.rotation * b;
            edges.Add(new Edge(transform.position + a, transform.position + b));
        }

        FindIntersections(plane.p0, plane.normal);
        int lastKey = FindNeighbors();
        polygonVertices.Clear();
        if (lastKey >= 0)
            ArrangePolygonVertices(lastKey);
        ProjectPolygonVertices();

        Triangulator tr = new Triangulator(projectedVertices.ToArray());
        int[] indices = tr.Triangulate();
        Vector3[] vertices = new Vector3[polygonVertices.Count];
        Vector2[] uvs = new Vector2[polygonVertices.Count];
        Vector3[] outline = new Vector3[polygonVertices.Count];
        for (int i = 0; i < polygonVertices.Count; i++)
        {
            vertices[i] = transform.InverseTransformPoint(polygonVertices[i]);
            Vector2 proj = projectedVertices[i];
            uvs[i] = proj;
            outline[i] = polygonVertices[i] - plane.normal * 0.1f;
        }

        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.uv = uvs;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        meshFilter.mesh = msh;

        lineRenderer.positionCount = polygonVertices.Count;
        lineRenderer.SetPositions(outline);
    }

    void FindIntersections(Vector3 p0, Vector3 normal)
    {
        nodes.Clear();
        float d;
        float dot;
        for (int i = 0; i < edges.Count; i++)
        {
            dot = Vector3.Dot(normal, edges[i].dir);
            if (Mathf.Abs(dot) > 0)
            {
                d = Vector3.Dot((p0 - edges[i].start), normal) / dot;
                if (d > 0 && d < edges[i].length)
                {
                    nodes.Add(i, new Node(edges[i].start + edges[i].dir * d));
                }
            }
        }
    }

    int FindNeighbors()
    {
        int ind1, ind2, last = -1;
        foreach (Face face in ph.faces)
        {
            ind1 = -1;
            ind2 = -1;
            foreach (int index in face.edgeIndices)
            {
                if (nodes.ContainsKey(index))
                {
                    if (ind1 >= 0)
                    {
                        ind2 = index;
                        nodes[ind1].neighbors.Add(nodes[ind2]);
                        nodes[ind2].neighbors.Add(nodes[ind1]);
                        last = ind2;
                        break;
                    }
                    ind1 = index;
                }
            }

        }
        return last;
    }

    void ArrangePolygonVertices(int startKey)
    {
        List<Node> remainedNodes = new List<Node>();
        foreach (var node in nodes)
        {
            remainedNodes.Add(node.Value);
        }

        Node currentNode = nodes[startKey];
        while (remainedNodes.Count > 1)
        {
            foreach (Node node in currentNode.neighbors)
            {
                if (remainedNodes.Contains(node))
                {
                    remainedNodes.Remove(currentNode);
                    polygonVertices.Add(currentNode.position);
                    currentNode = node;
                    break;
                }
            }
        }
        polygonVertices.Add(currentNode.position);
    }

    void ProjectPolygonVertices()
    {
        projectedVertices.Clear();
        foreach (Vector3 vert in polygonVertices)
        {
            projectedVertices.Add(plane.transform.worldToLocalMatrix * vert);
        }
    }

    void OnDrawGizmos()
    {
        if (createInEditor)
        {
            edges = new List<Edge>();
            nodes = new Dictionary<int, Node>();
            polygonVertices = new List<Vector3>();
            projectedVertices = new List<Vector2>();

            CreateShape();
        }

        if (drawGizmos)
        {
            Gizmos.color = Color.white;
            foreach (Edge edge in edges)
            {
                Gizmos.DrawLine(edge.start, edge.start + edge.dir * edge.length);
            }

            Gizmos.color = Color.blue;
            foreach (var node in nodes)
            {
                Gizmos.DrawSphere(node.Value.position, 0.05f);
            }
            Gizmos.color = Color.black;
            if (polygonVertices.Count > 0)
            {
                for (int i = 0; i < polygonVertices.Count - 1; i++)
                {
                    Gizmos.DrawLine(polygonVertices[i], polygonVertices[i + 1]);
                }
                Gizmos.DrawLine(polygonVertices[polygonVertices.Count - 1], polygonVertices[0]);
            }
        }
    }

    public class Node
    {
        public Vector3 position;
        public List<Node> neighbors;

        public Node(Vector3 _pos)
        {
            position = _pos;
            neighbors = new List<Node>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rend.material.SetColor("_TintColor", tint);
            lineRenderer.material.SetColor("_TintColor", tint);
        }
    }

}

using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomQuad : MonoBehaviour
{
    [Header("Quad Corners (local space)")]
    public Vector3 bottomLeft = new Vector3(-0.5f, -0.5f, 0f);
    public Vector3 bottomRight = new Vector3(0.5f, -0.5f, 0f);
    public Vector3 topLeft = new Vector3(-0.5f, 0.5f, 0f);
    public Vector3 topRight = new Vector3(0.5f, 0.5f, 0f);

    public Material material;

    void OnValidate() // auto rebuild when you change values in editor
    {
        BuildMesh();
    }
    private void OnEnable()
    {
        BuildMesh();
    }

    public void BuildMesh()
    {
        var mf = GetComponent<MeshFilter>();
        var mr = GetComponent<MeshRenderer>();
        if (material != null) mr.sharedMaterial = material;

        Mesh m = new Mesh();
        m.name = "EditableQuad";

        Vector3[] verts = new Vector3[]
        {
            bottomLeft,
            bottomRight,
            topLeft,
            topRight
        };

        // normal based on first triangle
        Vector3 normal = Vector3.Cross(verts[2] - verts[0], verts[1] - verts[0]).normalized;
        Vector3[] normals = { normal, normal, normal, normal };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1)
        };

        int[] tris = { 0, 2, 1, 2, 3, 1 };

        m.vertices = verts;
        m.normals = normals;
        m.uv = uvs;
        m.triangles = tris;

        mf.sharedMesh = m;
    }
}

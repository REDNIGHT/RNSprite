// Copyright (c) 2012 Xilin Chen (RN)
// Please direct any bugs/comments/suggestions to http://blog.sina.com.cn/u/2840185437

using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// SpriteUV
/// This SpriteUV script can edit mesh uv.
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SpriteUV : MonoBehaviour
{
    /// <summary>
    /// Gets the sprite mesh vertices.
    /// </summary>
    /// <returns></returns>
    Vector3[] getVertices()
    {
        return new Vector3[4]
        {
            new Vector3(-width * 0.5f, height * 0.5f, 0),new Vector3(width * 0.5f, height * 0.5f, 0),
            new Vector3(-width * 0.5f, -height * 0.5f, 0),new Vector3(width * 0.5f, -height * 0.5f, 0)
        };
    }
    /// <summary>
    /// Gets the sprite mesh uvs.
    /// </summary>
    Vector2[] getUVs()
    {
        return new Vector2[4]
        {
            new Vector2(uvX0, uvY1), new Vector3(uvX1, uvY1),
            new Vector2(uvX0, uvY0), new Vector3(uvX1, uvY0),
        };
    }


    /// <summary>
    /// Creates the sprite mesh.
    /// </summary>
    Mesh createSpriteMesh()
    {
        var mesh = new Mesh();
        mesh.vertices = getVertices();
        mesh.uv = getUVs();
        mesh.triangles = new int[6]
        {
            0,1,2,
            3,2,1
        };
        mesh.RecalculateNormals();
        mesh.name = "sprite_uv";
        mesh.hideFlags = HideFlags.HideAndDontSave;
        return mesh;
    }


    //
    void Awake()
    {
        GetComponent<MeshFilter>().sharedMesh = createSpriteMesh();
    }



    /// <summary>
    /// sprite width.
    /// </summary>
    public float width = 2.0f;
    /// <summary>
    /// sprite height.
    /// </summary>
    public float height = 1.0f;

    /// <summary>
    /// sprite mesh uv x0 position.
    /// </summary>
    public float uvX0 = 0.0f;
    /// <summary>
    /// sprite mesh uv x1 position.
    /// </summary>
    public float uvX1 = 1f;
    /// <summary>
    /// sprite mesh uv y0 position.
    /// </summary>
    public float uvY0 = 0.0f;
    /// <summary>
    /// sprite mesh uv y1 position.
    /// </summary>
    public float uvY1 = 1f;

    /// <summary>
    /// Refreshes the sprite mesh.
    /// </summary>
    public void refreshWH()
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.vertices = getVertices();
        mesh.RecalculateBounds();
    }

    /// <summary>
    /// Refreshes the sprite mesh uv.
    /// </summary>
    public void refreshUV()
    {
        //print("uvY0=" + uvY0 + "  uvY1=" + uvY1);
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.uv = getUVs();
    }
}

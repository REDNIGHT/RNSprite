// Copyright (c) 2012 Xilin Chi (RN)
// Please direct any bugs/comments/suggestions to http://blog.sina.com.cn/u/2840185437

using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteMeshBuilder : EditorWindow
{

    [MenuItem("RN/Tool/Sprite Mesh Builder")]
    static void Init()
    {
        var window = GetWindow<SpriteMeshBuilder>();
        window.Show();
    }

    void OnGUI()
    {
        _size = EditorGUILayout.FloatField(_size);
        if (GUILayout.Button("Build"))
        {
            var m = createSpriteXY(_size);
            AssetDatabase.CreateAsset(m, "Assets/Src/RNKit/Sprite/" + m.name + ".asset");

            m = createSpriteXZ(_size);
            AssetDatabase.CreateAsset(m, "Assets/Src/RNKit/Sprite/" + m.name + ".asset");

            Debug.Log("Created new Sprite meshes in Assets/Src/RNKit/Sprite/");
        }
    }

    /// <summary>
    /// The sprite mesh size
    /// </summary>
    float _size = 1.0f;


    /// <summary>
    /// Creates the plane.
    /// </summary>
    /// <param name="size">The size of the plane.</param>
    public static Mesh createSpriteXY(float size)
    {
        var halfSize = size * 0.5f;

        var newVertices = new Vector3[4]
        {
            new Vector3(-halfSize,-halfSize,0),new Vector3(halfSize,-halfSize,0),
            new Vector3(-halfSize,halfSize,0),new Vector3(halfSize,halfSize,0)
        };
        var newUV = new Vector2[4]
        {
            new Vector2(0,0),new Vector3(1.0f,0),
            new Vector2(0,1.0f),new Vector3(1.0f,1.0f)
        };
        var newTriangles = new int[6]
        {
            2,1,0,
            1,2,3
        };

        var mesh = new Mesh();
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.name = "spriteXY";
        //mesh.hideFlags = HideFlags.HideAndDontSave;
        return mesh;
    }

    /// <summary>
    /// Creates the plane.
    /// </summary>
    /// <param name="size">The size of the plane.</param>
    public static Mesh createSpriteXZ(float size)
    {
        var halfSize = size * 0.5f;

        var newVertices = new Vector3[4]
        {
            new Vector3(-halfSize,0,-halfSize),new Vector3(halfSize,0,-halfSize),
            new Vector3(-halfSize,0,halfSize),new Vector3(halfSize,0,halfSize)
        };
        var newUV = new Vector2[4]
        {
            new Vector2(0,0),new Vector3(1.0f,0),
            new Vector2(0,1.0f),new Vector3(1.0f,1.0f)
        };
        var newTriangles = new int[6]
        {
            2,1,0,
            1,2,3
        };

        var mesh = new Mesh();
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.name = "spriteXZ";
        //mesh.hideFlags = HideFlags.HideAndDontSave;

        return mesh;
    }

}

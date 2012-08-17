// Copyright (c) 2012 Xilin Chi (RN)
// Please direct any bugs/comments/suggestions to http://rnsprite.blogspot.com/

using UnityEngine;
using UnityEditor;
using System.Collections;



/// <summary>
/// Sprite UV Property edit.
/// </summary>
[CustomEditor(typeof(SpriteUV))]
public class SpriteUVEditor : Editor
{
    SpriteUV sprite
    {
        get { return target as SpriteUV; }
    }


    SerializedObject so;

    SerializedProperty width;
    SerializedProperty height;
    SerializedProperty uvX0;
    SerializedProperty uvX1;
    SerializedProperty uvY0;
    SerializedProperty uvY1;

    void OnEnable()
    {
        so = new SerializedObject(target);

        width = so.FindProperty("width");
        height = so.FindProperty("height");
        uvX0 = so.FindProperty("uvX0");
        uvX1 = so.FindProperty("uvX1");
        uvY0 = so.FindProperty("uvY0");
        uvY1 = so.FindProperty("uvY1");
    }


    //
    public override void OnInspectorGUI()
    {
        propertyField();

        if (GUILayout.Button("edit uv"))
        {
            var window = EditorWindow.GetWindow<SpriteUVEditorWindow>();
            window.Show();
        }

        //
        sprite.refreshWH();
        sprite.refreshUV();
    }



    //
    bool showPosition = false;
    void propertyField()
    {
        // Grab the latest data from the object
        so.Update();
        EditorGUILayout.PropertyField(width);
        EditorGUILayout.PropertyField(height);


        showPosition = EditorGUILayout.Foldout(showPosition, "uvs");
        if (showPosition)
        {
            EditorGUILayout.PropertyField(uvX0);
            EditorGUILayout.PropertyField(uvX1);
            EditorGUILayout.PropertyField(uvY0);
            EditorGUILayout.PropertyField(uvY1);
        }


        // Apply the property, handle undo
        so.ApplyModifiedProperties();
    }
}

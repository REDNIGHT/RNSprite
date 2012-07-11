// Copyright (c) 2012 Xilin Chi (RN)
// Please direct any bugs/comments/suggestions to http://blog.sina.com.cn/u/2840185437

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;



/// <summary>
/// Sprite Animation Editor
/// </summary>
[CustomEditor(typeof(SpriteAnimation))]
public class SpriteAnimationEditor : Editor
{

    /// <summary>
    /// Gets the sprite.
    /// </summary>
    SpriteAnimation sprite
    {
        get { return target as SpriteAnimation; }
    }


    /// <summary>
    /// Called when [inspector GUI].
    /// </summary>
    public override void OnInspectorGUI()
    {
        if (sprite.animations == null)
            return;

        using (new UndoHelp(sprite, "SpriteAnimationEditor"))
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("play on awake", GUILayout.Width(100));
            sprite.playOnAwake = EditorGUILayout.Toggle(sprite.playOnAwake);
            EditorGUILayout.LabelField("auto destroy", GUILayout.Width(100));
            sprite.autoDestroy = EditorGUILayout.Toggle(sprite.autoDestroy);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("face 2 camera", GUILayout.Width(100));
            sprite.face2Camera = EditorGUILayout.Toggle(sprite.face2Camera);
            EditorGUILayout.LabelField("random angles", GUILayout.Width(100));
            sprite.randomAngles = (SpriteAnimation.RandomAngles)EditorGUILayout.EnumPopup(sprite.randomAngles);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("x frames count", GUILayout.Width(100));
            sprite.xCount = EditorGUILayout.IntField(sprite.xCount);
            EditorGUILayout.LabelField("y frames count", GUILayout.Width(100));
            sprite.yCount = EditorGUILayout.IntField(sprite.yCount);
            sprite.updateUVScale();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("animation Index", GUILayout.Width(100));
            sprite.animationIndex = (int)EditorGUILayout.Slider(sprite.animationIndex, 0.0f, sprite.animations.Count - 1);
            EditorGUILayout.EndHorizontal();
            //sprite.step();
        }



        //
        EditorGUILayout.Space();
        if (GUILayout.Button("add animation", GUILayout.Height(25)))
        {
            Undo.RegisterUndo(sprite, "sprite_add");
            sprite.animations.Add(new SpriteAnimation.AnimationInfo());
        }
        EditorGUILayout.Space();



        var anim_index = 0;
        if (sprite.animations != null)
            foreach (var ainfo in sprite.animations)
            {
                animationUI(ainfo,anim_index++);
            }

        if (remove_index != -1)
        {
            sprite.animations.RemoveAt(remove_index);
            shows.RemoveAt(remove_index);

            remove_index = -1;
        }

        //update and redraw:
        if (GUI.changed)
        {
            EditorUtility.SetDirty(sprite);
        }
    }


    List<bool> shows = new List<bool>();
    int remove_index = -1;
    int step_index = 0;

    /// <summary>
    /// Animations info UI.
    /// </summary>
    /// <param name="info">Animation info.</param>
    /// <param name="anim_index">The animation index.</param>
    /// <remarks></remarks>
    void animationUI(SpriteAnimation.AnimationInfo info, int anim_index)
    {
        EditorGUILayout.Space();

        if (shows.Count < anim_index + 1)
        {
            shows.Add(true);
        }

        shows[anim_index] = EditorGUILayout.Foldout(shows[anim_index], " [ " + anim_index + " ]");
        if (shows[anim_index])
        {
            //GUILayout.Label(" [ " + anim_index + " ]", style);
            //EditorGUILayout.LabelField("index", anim_index.ToString());
            info.beginIndex = EditorGUILayout.IntField("begin", info.beginIndex);
            info.endIndex = EditorGUILayout.IntField("end", info.endIndex);
            info.frameTime = EditorGUILayout.FloatField("frame time", info.frameTime);
            info.isloop = EditorGUILayout.Toggle("loop", info.isloop);


            //
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("step"))
            {
                if (step_index < info.beginIndex || step_index > info.endIndex)
                    step_index = info.beginIndex;

                sprite.setSubImage(step_index);
                ++step_index;
            }
            if (GUILayout.Button("delete", GUILayout.Width(48)))
            {
                Undo.RegisterUndo(sprite, "sprite_del");
                remove_index = anim_index;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}

// Copyright (c) 2012 Xilin Chi (RN)
// Please direct any bugs/comments/suggestions to http://blog.sina.com.cn/u/2840185437

using UnityEngine;
using UnityEditor;
using System.Collections;


/// <summary>
/// Sprite uv editor window.
/// </summary>
public class SpriteUVEditorWindow : EditorWindow
{

    [MenuItem("RN/Tool/Sprite UV Editor")]
    static void Init()
    {
        var window = GetWindow<SpriteUVEditorWindow>();
        window.Show();
    }


    
    bool alpha = false;
    Color lineColor = Color.blue;
    void OnGUI()
    {
        if (Selection.activeGameObject == null
        || Selection.activeGameObject.renderer == null
        || Selection.activeGameObject.renderer.sharedMaterial == null
        || Selection.activeGameObject.renderer.sharedMaterial.mainTexture == null)
            return;

        var sprite = Selection.activeGameObject.GetComponent<SpriteUV>();
        if (sprite == null)
            return;


        //
        using (new UndoHelp(sprite, "SpriteUVEditorWindow"))
        {
            //
            var texture = Selection.activeGameObject.renderer.sharedMaterial.mainTexture;


            float x0 = sprite.uvX0 * texture.width;
            float x1 = sprite.uvX1 * texture.width;
            float y0 = sprite.uvY0 * texture.height;
            float y1 = sprite.uvY1 * texture.height;


            //
            EditorGUILayout.LabelField("texture : " + texture.name + "      width : " + texture.width + "      height : " + texture.height);



            //
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("x : ", GUILayout.Width(20));
            x0 = EditorGUILayout.IntField((int)x0, GUILayout.Width(45));
            x1 = EditorGUILayout.IntField((int)x1, GUILayout.Width(45));
            EditorGUILayout.MinMaxSlider(ref x0, ref x1, 0, texture.width);
            x0 = (int)x0;
            x1 = (int)x1;
            var w = x1 - x0;
            w = EditorGUILayout.Slider(w, 0, texture.width - x0);
            x1 = x0 + (int)w;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("y : ", GUILayout.Width(20));
            y0 = EditorGUILayout.IntField((int)y0, GUILayout.Width(45));
            y1 = EditorGUILayout.IntField((int)y1, GUILayout.Width(45));
            EditorGUILayout.MinMaxSlider(ref y0, ref y1, 0, texture.height);
            y0 = (int)y0;
            y1 = (int)y1;
            var h = y1 - y0;
            h = EditorGUILayout.Slider(h, 0, texture.height - y0);
            y1 = y0 + (int)h;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("...", GUILayout.Width(20));
            y1 = EditorGUILayout.IntField((int)(texture.height - y1), GUILayout.Width(45));
            y0 = EditorGUILayout.IntField((int)(texture.height - y0), GUILayout.Width(45));
            y0 = texture.height - y0;
            y1 = texture.height - y1;
            EditorGUILayout.EndHorizontal();


            //
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("    alpha", GUILayout.Width(60));
            alpha = EditorGUILayout.Toggle(alpha);
            lineColor = EditorGUILayout.ColorField(lineColor);
            EditorGUILayout.EndHorizontal();



            //
            var offsetX = (this.position.width - texture.width) * 0.5f;
            var offsetY = 150.0f;

            var rect = new Rect();
            rect.xMin = x0;
            rect.xMax = x1;
            rect.yMin = texture.height - y1;
            rect.yMax = texture.height - y0;
            rect.x += offsetX;
            rect.y += offsetY;



            //
            var textureRect = new Rect(offsetX, offsetY, texture.width, texture.height);



            if (alpha)
                EditorGUI.DrawTextureAlpha(textureRect, texture);
            else
                EditorGUI.DrawPreviewTexture(textureRect, texture);





            //
            var nr = RectDragButton.drag(rect);
            if (nr != rect)
            {
                isRepaint = true;

                nr.x -= offsetX;
                nr.y -= offsetY;

                if (nr.xMin >= 0
                 && nr.xMax <= texture.width)
                {
                    x0 = nr.xMin;
                    x1 = nr.xMax;
                }
                if(nr.yMin >= 0
                 && nr.yMax <= texture.height)
                {
                    y1 = texture.height - nr.yMin;
                    y0 = texture.height - nr.yMax;
                }
            }
            else
            {
                isRepaint = false;
            }


            Drawing.DrawRect(rect, lineColor, 2, false);



            //
            sprite.uvX0 = x0 / texture.width;
            sprite.uvX1 = x1 / texture.width;
            sprite.uvY0 = y0 / texture.height;
            sprite.uvY1 = y1 / texture.height;
            sprite.refreshUV();
            SceneView.RepaintAll();

            var gv = EditorUtils.mainGameView;
            if (gv != null)
                gv.Repaint();
        }
    }
    

    void OnSelectionChange()
    {
        Repaint();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
    bool isRepaint = false;
    void Update()
    {
        if (isRepaint)
            Repaint();
    }
}

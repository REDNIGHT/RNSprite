// Copyright (c) 2012 Xilin Chen (RN)
// Please direct any bugs/comments/suggestions to http://www.blogger.com/profile/04078856863398606871

using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script can draw line,draw bezier line and draw rect.
/// </summary>
public class Drawing
{
    /// <summary>
    /// Anti alias line texture.
    /// </summary>
    public static Texture2D aaLineTex = null;
    /// <summary>
    /// Line texture/
    /// </summary>
    public static Texture2D lineTex = null;

    /// <summary>
    /// Draws the line.
    /// </summary>
    /// <param name="pointA">The point A.</param>
    /// <param name="pointB">The point B.</param>
    /// <param name="color">The color.</param>
    /// <param name="width">The width.</param>
    /// <param name="antiAlias">if set to <c>true</c> [anti alias].</param>
    public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width, bool antiAlias)
    {
        Color savedColor = GUI.color;
        Matrix4x4 savedMatrix = GUI.matrix;

        if (!lineTex)
        {
            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            lineTex.SetPixel(0, 1, Color.white);
            lineTex.Apply();
            lineTex.hideFlags = HideFlags.HideAndDontSave;
        }
        if (!aaLineTex)
        {
            aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, true);
            aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
            aaLineTex.SetPixel(0, 1, Color.white);
            aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
            aaLineTex.Apply();
            aaLineTex.hideFlags = HideFlags.HideAndDontSave;
        }
        if (antiAlias) width *= 3;
        float angle = Vector3.Angle(pointB - pointA, Vector2.right) * (pointA.y <= pointB.y ? 1 : -1);
        float m = (pointB - pointA).magnitude;
        if (m > 0.01f)
        {
            Vector3 dz = new Vector3(pointA.x, pointA.y, 0);

            GUI.color = color;
            GUI.matrix = translationMatrix(dz) * GUI.matrix;
            GUIUtility.ScaleAroundPivot(new Vector2(m, width), new Vector3(-0.5f, 0, 0));
            GUI.matrix = translationMatrix(-dz) * GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, Vector2.zero);
            GUI.matrix = translationMatrix(dz + new Vector3(width / 2, -m / 2) * Mathf.Sin(angle * Mathf.Deg2Rad)) * GUI.matrix;

            if (!antiAlias)
                GUI.DrawTexture(new Rect(0, 0, 1, 1), lineTex);
            else
                GUI.DrawTexture(new Rect(0, 0, 1, 1), aaLineTex);
        }
        GUI.matrix = savedMatrix;
        GUI.color = savedColor;
    }

    /// <summary>
    /// Beziers the line.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <param name="startTangent">The start tangent.</param>
    /// <param name="end">The end.</param>
    /// <param name="endTangent">The end tangent.</param>
    /// <param name="color">The color.</param>
    /// <param name="width">The width.</param>
    /// <param name="antiAlias">if set to <c>true</c> [anti alias].</param>
    /// <param name="segments">The segments.</param>
    public static void bezierLine(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, bool antiAlias, int segments)
    {
        Vector2 lastV = cubeBezier(start, startTangent, end, endTangent, 0);
        for (int i = 1; i <= segments; ++i)
        {
            Vector2 v = cubeBezier(start, startTangent, end, endTangent, i / (float)segments);

            Drawing.DrawLine(
                lastV,
                v,
                color, width, antiAlias);
            lastV = v;
        }
    }


    /// <summary>
    /// Draws the rect.
    /// </summary>
    /// <param name="rect">The rect.</param>
    /// <param name="color">The color.</param>
    /// <param name="width">The width.</param>
    /// <param name="antiAlias">if set to <c>true</c> [anti alias].</param>
    public static void DrawRect(Rect rect, Color color, float width, bool antiAlias)
    {
        DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMax, rect.yMin), color, width, antiAlias);
        DrawLine(new Vector2(rect.xMin, rect.yMax), new Vector2(rect.xMax, rect.yMax), color, width, antiAlias);
        DrawLine(new Vector2(rect.xMin, rect.yMin), new Vector2(rect.xMin, rect.yMax), color, width, antiAlias);
        DrawLine(new Vector2(rect.xMax, rect.yMin), new Vector2(rect.xMax, rect.yMax), color, width, antiAlias);
    }

    //
    /// <summary>
    /// the bezier equation.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="st">The st.</param>
    /// <param name="e">The e.</param>
    /// <param name="et">The et.</param>
    /// <param name="t">The t.</param>
    /// <returns></returns>
    private static Vector2 cubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
    {
        float rt = 1 - t;
        float rtt = rt * t;
        return rt * rt * rt * s + 3 * rt * rtt * st + 3 * rtt * t * et + t * t * t * e;
    }

    /// <summary>
    /// Translations the matrix.
    /// </summary>
    /// <param name="v">The v.</param>
    /// <returns></returns>
    private static Matrix4x4 translationMatrix(Vector3 v)
    {
        return Matrix4x4.TRS(v, Quaternion.identity, Vector3.one);
    }
}

// Copyright (c) 2012 Xilin Chen (RN)
// Please direct any bugs/comments/suggestions to http://www.blogger.com/profile/04078856863398606871

using UnityEngine;
using UnityEditor;


/// <summary>
/// Rect drag button.
/// Use this like to use EditorGUILayout.FloatField().
/// This script can edit rect data.
/// 
///     //example
///     rect = RectDragButton.drag(rect)
/// 
/// </summary>
public static class RectDragButton
{

    /// <summary>
    /// draw this color if mouse is over the rect.
    /// </summary>
    public static Color overColor = Color.gray;
    /// <summary>
    /// draw this color if mouse is draging.
    /// </summary>
    public static Color dragColor = Color.red;
    /// <summary>
    /// rect line width.
    /// </summary>
    public static float width = 1;
    /// <summary>
    /// drag area.
    /// </summary>
    public static float scale = 0.2f;


    enum STATE
    {
        NONE,

        DragRectCenter,

        DragRectLeft,
        DragRectRight,
        DragRectTop,
        DragRectBottom,

        DragRectLeftTop,
        DragRectRightTop,
        DragRectLeftBottom,
        DragRectRightBottom,
    }

    static STATE state = STATE.NONE;
    static Vector2 mouseDownPos;

    public static Rect drag(Rect rect)
    {
        if (dragRectCenter(rect))
        {
            rect.center += Event.current.mousePosition - mouseDownPos;
            mouseDownPos = Event.current.mousePosition;
            return rect;
        }
        else if (dragRectLeft(rect))
        {
            rect.xMin += Event.current.mousePosition.x - mouseDownPos.x;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectRight(rect))
        {
            rect.xMax += Event.current.mousePosition.x - mouseDownPos.x;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectTop(rect))
        {
            rect.yMin += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectBottom(rect))
        {
            rect.yMax += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectLeftTop(rect))
        {
            rect.xMin += Event.current.mousePosition.x - mouseDownPos.x;
            rect.yMin += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectRightTop(rect))
        {
            rect.xMax += Event.current.mousePosition.x - mouseDownPos.x;
            rect.yMin += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectLeftBottom(rect))
        {
            rect.xMin += Event.current.mousePosition.x - mouseDownPos.x;
            rect.yMax += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }
        else if (dragRectRightBottom(rect))
        {
            rect.xMax += Event.current.mousePosition.x - mouseDownPos.x;
            rect.yMax += Event.current.mousePosition.y - mouseDownPos.y;
            mouseDownPos = Event.current.mousePosition;
        }

        return rect;
    }


    static void DrawRect(Rect r)
    {
        if (state == STATE.NONE)
            Drawing.DrawRect(r, overColor, width, false);
        else
            Drawing.DrawRect(r, dragColor, width, false);
    }
    static bool Contains(Rect rect,STATE nowState)
    {
        if (Event.current.type == EventType.mouseDrag && state == nowState)
        {
            DrawRect(rect);
            return true;
        }

        bool result = false;
        if (rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.mouseDown)
            {
                state = nowState;
                mouseDownPos = Event.current.mousePosition;
                result = true;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.mouseUp)
            {
                state = STATE.NONE;
                result = true;
                Event.current.Use();
            }

            DrawRect(rect);
        }

        return result;
    }

    public static bool dragRectCenter(Rect r)
    {
        var dr = r;

        dr.xMin += r.width * scale;
        dr.xMax -= r.width * scale;
        dr.yMin += r.height * scale;
        dr.yMax -= r.height * scale;

        return Contains(dr, STATE.DragRectCenter);
    }
    public static bool dragRectLeft(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMin - r.width * scale;
        dr.xMax = r.xMin + r.width * scale;
        dr.yMin += r.height * scale;
        dr.yMax -= r.height * scale;

        return Contains(dr, STATE.DragRectLeft);
    }
    public static bool dragRectRight(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMax - r.width * scale;
        dr.xMax = r.xMax + r.width * scale;
        dr.yMin += r.height * scale;
        dr.yMax -= r.height * scale;


        return Contains(dr, STATE.DragRectRight);
    }
    public static bool dragRectTop(Rect r)
    {
        var dr = r;

        dr.xMin += r.width * scale;
        dr.xMax -= r.width * scale;
        dr.yMin = r.yMin - r.height * scale;
        dr.yMax = r.yMin + r.height * scale;

        return Contains(dr, STATE.DragRectTop);
    }
    public static bool dragRectBottom(Rect r)
    {
        var dr = r;

        dr.xMin += r.width * scale;
        dr.xMax -= r.width * scale;
        dr.yMin = r.yMax - r.height * scale;
        dr.yMax = r.yMax + r.height * scale;
        return Contains(dr, STATE.DragRectBottom);
    }
    public static bool dragRectLeftTop(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMin - r.width * scale;
        dr.xMax = r.xMin + r.width * scale;
        dr.yMin = r.yMin - r.height * scale;
        dr.yMax = r.yMin + r.height * scale;
        return  Contains(dr, STATE.DragRectLeftTop);
    }
    public static bool dragRectRightTop(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMax - r.width * scale;
        dr.xMax = r.xMax + r.width * scale;
        dr.yMin = r.yMin - r.height * scale;
        dr.yMax = r.yMin + r.height * scale;
        return Contains(dr, STATE.DragRectRightTop);
    }
    public static bool dragRectLeftBottom(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMin - r.width * scale;
        dr.xMax = r.xMin + r.width * scale;
        dr.yMin = r.yMax - r.height * scale;
        dr.yMax = r.yMax + r.height * scale;
        return Contains(dr, STATE.DragRectLeftBottom);
    }
    public static bool dragRectRightBottom(Rect r)
    {
        var dr = r;

        dr.xMin = r.xMax - r.width * scale;
        dr.xMax = r.xMax + r.width * scale;
        dr.yMin = r.yMax - r.height * scale;
        dr.yMax = r.yMax + r.height * scale;
        return Contains(dr, STATE.DragRectRightBottom);
    }
}


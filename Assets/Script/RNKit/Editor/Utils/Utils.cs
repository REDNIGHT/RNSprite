// Copyright (c) 2012 Xilin Chen (RN)
// Please direct any bugs/comments/suggestions to http://www.blogger.com/profile/04078856863398606871

using UnityEngine;
using UnityEditor;

public static class EditorUtils
{

    public static EditorWindow mainGameView
    {
        get
        {
            System.Type GameView = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetMainGameView = GameView.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            return GetMainGameView.Invoke(null, null) as EditorWindow;
        }
    }


}

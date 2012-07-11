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

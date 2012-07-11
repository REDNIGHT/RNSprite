using UnityEngine;
using UnityEditor;



/// <summary>
/// UndoHelp
/// auto add undo data if mouse down.
/// 
///     //example
///     using (new UndoHelp(obj, "name"))
///     {
///         //...
///     }
/// 
/// </summary>
public class UndoHelp : System.IDisposable
{
    bool _isMouseDown = false;
    Object _obj;
    string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="UndoHelp"/> class.
    /// </summary>
    /// <param name="obj">The object you want to save undo info for.</param>
    /// <param name="name">The name of the action to undo.</param>
    public UndoHelp(Object obj, string name)
    {
        if (Event.current.type == EventType.mouseDown)
            _isMouseDown = true;

        _obj = obj;
        _name = name;
    }

    /// <summary>
    /// Register Undo if Event.current.type is used and mouse is down.
    /// </summary>
    void System.IDisposable.Dispose()
    {
        if (_isMouseDown && Event.current.type == EventType.used)
        {
            //Debug.Log("isMouseDown && Event.current.type == EventType.used");
            Undo.RegisterUndo(_obj, _name);
        }
    }
}


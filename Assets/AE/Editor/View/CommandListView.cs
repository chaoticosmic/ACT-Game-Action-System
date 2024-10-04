
using System;
using UnityEngine;
using XMLib;

public class CommandListView : IDataView
{
    public override string title => "命令列表";
    
    public override bool useAre => true;
    private Vector2 scrollPos;
    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentCommands == null)
        {
            return;
        }
        
        win.commandSelectIndex = EditorGUILayoutEx.DrawList(win.currentCommands, win.commandSelectIndex, ref scrollPos, NewState, ActionEditorUtility.CommandDrawer);

    }

    private void NewState(Action<ActionCommand> adder)
    {
        adder(new ActionCommand());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentCommands;
    }

    public override void PasteData(object data)
    {
        if (win.currentCommands != null && data is ActionCommand configs)
        {
            win.currentCommands.Add(configs);
        }
    }
}

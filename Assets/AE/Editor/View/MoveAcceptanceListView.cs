
using System;
using UnityEngine;
using XMLib;

public class MoveAcceptanceListView : IDataView
{
    public override string title => "移动接受度列表";
    public override bool useAre => true;

    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentMoveAcceptances == null)
        {
            return;
        }
        
        win.moveAcceptanceSelectIndex = EditorGUILayoutEx.DrawList(win.currentMoveAcceptances, win.moveAcceptanceSelectIndex, 
            ref scrollPos, NewState, ActionEditorUtility.MoveAcceptanceDrawer);

    }

    private void NewState(Action<MoveInputAcceptance> adder)
    {
        adder(new MoveInputAcceptance());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentMoveAcceptances;
    }

    public override void PasteData(object data)
    {
        if (win.currentMoveAcceptances != null && data is MoveInputAcceptance configs)
        {
            win.currentMoveAcceptances.Add(configs);
        }
    }
}

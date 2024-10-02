
using System;
using UnityEngine;
using XMLib;

public class BeCancelledTagListView : IDataView
{
    public override string title => "被取消列表";
    public override bool useAre => true;
    
    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentBeCancels == null)
        {
            return;
        }
        
        win.beCancelSelectIndex = EditorGUILayoutEx.DrawList(win.currentBeCancels, win.beCancelSelectIndex, ref scrollPos, NewState, ActionEditorUtility.BeCancelTagDrawer);

    }

    private void NewState(Action<BeCancelledTag> adder)
    {
        adder(new BeCancelledTag());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentBeCancels;
    }

    public override void PasteData(object data)
    {
        if (win.currentBeCancels != null && data is BeCancelledTag configs)
        {
            win.currentBeCancels.Add(configs);
        }
    }
}

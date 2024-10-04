
using System;
using UnityEngine;
using XMLib;

public class TempBeCancelTagListView : IDataView
{
    public override string title => "临时取消列表";
    public override bool useAre => true;

    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        win.tempBeCancelTagSelectIndex = EditorGUILayoutEx.DrawList(win.currentTempBeCancels, 
            win.tempBeCancelTagSelectIndex, ref scrollPos, NewState, ActionEditorUtility.TempBeCancelTagDrawer);
    }

    private void NewState(Action<TempBeCancelledTag> adder)
    {
        adder(new TempBeCancelledTag());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentTempBeCancels;
    }

    public override void PasteData(object data)
    {
        if (win.currentTempBeCancels != null && data is TempBeCancelledTag configs)
        {
            win.currentTempBeCancels.Add(configs);
        }
    }
}

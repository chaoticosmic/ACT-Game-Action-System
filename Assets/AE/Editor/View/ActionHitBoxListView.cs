
using System;
using UnityEngine;
using XMLib;

public class ActionHitBoxListView : IDataView
{
    public override string title => "动作受击盒列表";
    public override bool useAre => true;

    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentActionHitBoxs == null)
        {
            return;
        }
        
        win.actionHitBoxSelectIndex = EditorGUILayoutEx.DrawList(win.currentActionHitBoxs, win.actionHitBoxSelectIndex, 
            ref scrollPos, NewState, ActionEditorUtility.ActionHitBoxDrawer);

    }

    private void NewState(Action<BeHitBoxTurnOnInfo> adder)
    {
        adder(new BeHitBoxTurnOnInfo());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentActionHitBoxs;
    }

    public override void PasteData(object data)
    {
        if (win.currentActionHitBoxs != null && data is BeHitBoxTurnOnInfo configs)
        {
            win.currentActionHitBoxs.Add(configs);
        }
    }
}

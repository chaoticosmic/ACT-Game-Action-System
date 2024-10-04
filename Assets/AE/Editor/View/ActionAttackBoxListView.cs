
using System;
using UnityEngine;
using XMLib;

public class ActionAttackBoxListView : IDataView
{
    public override string title => "动作攻击盒列表";
    public override bool useAre => true;

    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentActionAttackBoxs == null)
        {
            return;
        }
        
        win.actionAttackBoxSelectIndex = EditorGUILayoutEx.DrawList(win.currentActionAttackBoxs, win.actionAttackBoxSelectIndex, 
            ref scrollPos, NewState, ActionEditorUtility.ActionAttackBoxDrawer);

    }

    private void NewState(Action<AttackBoxTurnOnInfo> adder)
    {
        adder(new AttackBoxTurnOnInfo());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentActionAttackBoxs;
    }

    public override void PasteData(object data)
    {
        if (win.currentActionAttackBoxs != null && data is AttackBoxTurnOnInfo configs)
        {
            win.currentActionAttackBoxs.Add(configs);
        }
    }
}

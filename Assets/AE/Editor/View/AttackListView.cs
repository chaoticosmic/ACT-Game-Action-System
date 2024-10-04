
using System;
using UnityEngine;
using XMLib;

public class AttackListView : IDataView
{
    public override string title => "攻击列表";
    public override bool useAre => true;
    
    private Vector2 scrollPos;
    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentAttackInfos == null)
        {
            return;
        }
        
        win.attackInfoSelectIndex = EditorGUILayoutEx.DrawList(win.currentAttackInfos, win.attackInfoSelectIndex, ref scrollPos, NewState, ActionEditorUtility.AttackInfoDrawer);

    }

    private void NewState(Action<AttackInfo> adder)
    {
        adder(new AttackInfo());
    }


    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentAttackInfos;
    }

    public override void PasteData(object data)
    {
        if (win.currentAttackInfos != null && data is AttackInfo configs)
        {
            win.currentAttackInfos.Add(configs);
        }
    }
}

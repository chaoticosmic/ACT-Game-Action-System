using System;
using UnityEngine;
using XMLib;


public class StateListView : IDataView
{
    public override string title => "动作列表";

    public override bool useAre => true;
    
    private Vector2 scrollPos;

    protected override void OnGUI(Rect rect)
    {
        GUILayout.Space(4);
        win.stateSelectIndex = EditorGUILayoutEx.DrawList(win.config.data, win.stateSelectIndex, ref scrollPos, NewState, ActionEditorUtility.StateDrawer);
    }
    
    private void NewState(Action<ActionInfo> adder)
    {
        adder(new ActionInfo());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentState;
    }

    public override void PasteData(object data)
    {
        if (win.currentStates != null && data is ActionInfo configs)
        {
            win.currentStates.Add(configs);
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;
using XMLib;


public class CancelTagView : IDataView
{
    public override string title => "取消列表";
    
    public override bool useAre => true;
    
    private Vector2 scrollPos;
    protected override void OnGUI(Rect contentRect)
    {
        GUILayout.Space(4);
        if (win.currentCancels == null)
        {
            return;
        }
        
        win.cancelSelectIndex = EditorGUILayoutEx.DrawList(win.currentCancels, win.cancelSelectIndex, ref scrollPos, NewState, ActionEditorUtility.CancelTagDrawer);
        

    }
    
    private void NewState(Action<CancelTag> adder)
    {
        adder(new CancelTag());
    }

    public override void OnUpdate()
    {
        
    }

    public override object CopyData()
    {
        return win.currentCancels;
    }

    public override void PasteData(object data)
    {
        if (win.currentCancels != null && data is CancelTag configs)
        {
            win.currentCancels.Add(configs);
        }
    }
}

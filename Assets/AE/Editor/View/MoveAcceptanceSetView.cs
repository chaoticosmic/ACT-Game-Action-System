
using UnityEditor;
using UnityEngine;
using XMLib;

public class MoveAcceptanceSetView : IView
{
    public override string title => "移动接受度设置";
    public override bool useAre => true;

    private Vector2 scrollView;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentMoveInputAcceptance;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        obj.range = EditorGUILayoutEx.DrawObject("时间段", obj.range);
        obj.rate = EditorGUILayoutEx.DrawObject("接受度", obj.rate);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

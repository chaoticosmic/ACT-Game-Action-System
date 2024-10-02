
using UnityEditor;
using UnityEngine;
using XMLib;

public class BeCancelledTagSetView : IView
{
    public override string title => "被取消设置";

    public override bool useAre => true;
    
    private Vector2 scrollPos;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentBeCancelTag;
        if (null == obj)
        {
            return;
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayoutEx.DrawObject("时间段", obj.percentageRange);
        EditorGUILayoutEx.DrawObject("取消列表", obj.cancelTag);
        EditorGUILayoutEx.DrawObject("融合出去时间", obj.fadeOutPercentage);
        EditorGUILayoutEx.DrawObject("优先级", obj.priority);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {

    }
}

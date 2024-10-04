
using UnityEditor;
using UnityEngine;
using XMLib;

public class TempBeCancelTagSetView : IView
{
    public override string title => "临时取消设置";
    public override bool useAre => true;

    private Vector2 scrollView;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentTempBeCancelTag;
        if (null == obj)
        {
            return;
        }
        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        obj.id = EditorGUILayoutEx.DrawObject("id", obj.id);
        obj.percentage = EditorGUILayoutEx.DrawObject("开始时间0-1", obj.percentage);
        obj.cancelTag = EditorGUILayoutEx.DrawObject("取消列表", obj.cancelTag);
        obj.fadeOutPercentage = EditorGUILayoutEx.DrawObject("融合时间", obj.fadeOutPercentage);
        obj.priority = EditorGUILayoutEx.DrawObject("优先级增量", obj.priority);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

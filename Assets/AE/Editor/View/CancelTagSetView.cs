
using UnityEditor;
using UnityEngine;
using XMLib;

public class CancelTagSetView : IView
{
    public override string title => "当前取消";

    public override bool useAre => true;
    private Vector2 scrollView;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentCancelTag;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        EditorGUILayoutEx.DrawObject("Tag", obj.tag);
        EditorGUILayoutEx.DrawObject("开始时间", obj.startFromPercentage);
        EditorGUILayoutEx.DrawObject("融合时间", obj.fadeInPercentage);
        EditorGUILayoutEx.DrawObject("优先级", obj.priority);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

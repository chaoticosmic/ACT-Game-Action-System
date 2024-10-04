
using UnityEditor;
using UnityEngine;
using XMLib;

public class ActionAttackBoxSetView : IView
{
    public override string title => "动作攻击盒设置";
    public override bool useAre => true;

    private Vector2 scrollView;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentActionAttackBox;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        obj.inPercentage = EditorGUILayoutEx.DrawObject("时间段列表", obj.inPercentage);
        obj.tag = EditorGUILayoutEx.DrawObject("tag列表", obj.tag);
        obj.attackPhase = EditorGUILayoutEx.DrawObject("索引", obj.attackPhase);
        obj.priority = EditorGUILayoutEx.DrawObject("优先级", obj.priority);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

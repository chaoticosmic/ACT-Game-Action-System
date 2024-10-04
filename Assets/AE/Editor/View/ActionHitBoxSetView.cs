
using UnityEditor;
using UnityEngine;
using XMLib;

public class ActionHitBoxSetView : IView
{
    public override string title => "动作受击盒设置";
    public override bool useAre => true;

    private Vector2 scrollView;

    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentActionHitBox;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        obj.inPercentage = EditorGUILayoutEx.DrawObject("时间段", obj.inPercentage);
        obj.tag = EditorGUILayoutEx.DrawObject("tag列表", obj.tag);
        obj.priority = EditorGUILayoutEx.DrawObject("优先级", obj.priority);
        obj.tempBeCancelledTagTurnOn = EditorGUILayoutEx.DrawObject("临时取消", obj.tempBeCancelledTagTurnOn);
        obj.attackerActionChange = EditorGUILayoutEx.DrawObject("攻击者变化", obj.attackerActionChange);
        obj.selfActionChange = EditorGUILayoutEx.DrawObject("自己变化", obj.selfActionChange);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

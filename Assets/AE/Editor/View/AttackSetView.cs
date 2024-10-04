
using UnityEditor;
using UnityEngine;
using XMLib;

public class AttackSetView : IView
{
    public override string title => "攻击设置";
    public override bool useAre => true;
    
    private Vector2 scrollView;
    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentAttackInfo;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        //EditorGUI.BeginChangeCheck();
        
        
        obj.phase = EditorGUILayoutEx.DrawObject("段", obj.phase);
        obj.attack = EditorGUILayoutEx.DrawObject("伤害", obj.attack);
        obj.forceDir = EditorGUILayoutEx.DrawObject("方向", obj.forceDir);
        obj.pushPower = EditorGUILayoutEx.DrawObject("推动", obj.pushPower);
        obj.hitStun = EditorGUILayoutEx.DrawObject("目标硬直", obj.hitStun);
        obj.freeze = EditorGUILayoutEx.DrawObject("自身卡帧", obj.freeze);
        obj.canHitSameTarget = EditorGUILayoutEx.DrawObject("命中次数", obj.canHitSameTarget);
        obj.hitSameTargetDelay = EditorGUILayoutEx.DrawObject("每次间隔", obj.hitSameTargetDelay);
        obj.selfActionChange = EditorGUILayoutEx.DrawObject("自身变化", obj.selfActionChange);
        obj.targetActionChange = EditorGUILayoutEx.DrawObject("目标变化", obj.targetActionChange);
        obj.tempBeCancelledTagTurnOn = EditorGUILayoutEx.DrawObject("临时开启", obj.tempBeCancelledTagTurnOn);
        
        // if (EditorGUI.EndChangeCheck())
        // {
        //     win.actionMachineDirty = true;
        // }
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

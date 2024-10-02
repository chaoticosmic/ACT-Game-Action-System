using UnityEditor;
using UnityEngine;
using XMLib;


public class StateSetView : IDataView
{
    public override string title => "1.状态设置";
    public override bool useAre => true;

    private Vector2 scrollView = Vector2.zero;
    protected override void OnGUI(Rect contentRect)
    {
        ActionInfo config = win.currentState;
        if (config == null)
        {
            return;
        }
        
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        EditorGUI.BeginChangeCheck();
        
        config.id = EditorGUILayoutEx.DrawObject("状态名", config.id);
        config.animKey = EditorGUILayoutEx.DrawObject("动画名", config.animKey);
        config.catalog = EditorGUILayoutEx.DrawObject("分类", config.catalog);
        config.autoNextActionId = EditorGUILayoutEx.DrawObject("自动下一个id", config.autoNextActionId);
        config.keepPlayingAnim = EditorGUILayoutEx.DrawObject("保持播放", config.keepPlayingAnim);
        config.autoTerminate = EditorGUILayoutEx.DrawObject("自动终结", config.autoTerminate);
        config.priority = EditorGUILayoutEx.DrawObject("优先级", config.priority);
        config.flip = EditorGUILayoutEx.DrawObject("翻转", config.flip);

        
        
        if (EditorGUI.EndChangeCheck())
        {
            win.actionMachineDirty = true;
        }

        EditorGUILayout.EndScrollView();
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
        if (win.currentState != null && data is ActionInfo state)
        {
            win.currentStates[win.stateSelectIndex] = state;
        }
    }
}

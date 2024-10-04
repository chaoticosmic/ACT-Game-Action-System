
using UnityEditor;
using UnityEngine;
using XMLib;

public class CommandSetView : IView
{
    public override string title => "命令设置";
    
    public override bool useAre => true;
    private Vector2 scrollView;
    protected override void OnGUI(Rect contentRect)
    {
        var obj = win.currentCommand;
        if (null == obj)
        {
            return;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        obj.keySequence = EditorGUILayoutEx.DrawObject("命令列表", obj.keySequence);
        obj.validInSec = EditorGUILayoutEx.DrawObject("合法时间", obj.validInSec);
        EditorGUILayout.EndScrollView();
    }

    public override void OnUpdate()
    {
        
    }
}

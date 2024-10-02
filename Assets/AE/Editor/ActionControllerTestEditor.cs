using UnityEditor;
using UnityEngine;

/// <summary>
/// TestActionMachineEditor
/// </summary>
[CustomEditor(typeof(ActionControllerTest))]
public class ActionMachineTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ActionControllerTest actionMachine = (ActionControllerTest)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("打开编辑器"))
        {
            ActionEditorWindow.ShowEditor(actionMachine.gameObject, actionMachine.config);
        }
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XMLib;
using XMLib.AM;

namespace AE
{
    public class ActionListView : IDataView
    {
        public override string title => "动作列表";

        public override bool useAre => true;
        
        private Vector2 scrollPos;

        protected override void OnGUI(Rect rect)
        {
            // List<object> configs = win.currentActions;
            // if (null == configs)
            // {
            //     return;
            // }
            //
            // EditorGUI.BeginChangeCheck();
            //
            // win.actionSelectIndex = EditorGUILayoutEx.DrawList(configs, win.actionSelectIndex, ref scrollPos, NewAction, ActionEditorUtility.ItemDrawer);
            // if (EditorGUI.EndChangeCheck())
            // {
            //     //win.configModification = true;
            // }
        }

        public override void OnUpdate()
        {
            
        }

        public override object CopyData()
        {
            return null;
        }

        public override void PasteData(object data)
        {
            
        }
    }
}
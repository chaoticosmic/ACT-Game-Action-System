    using System;
    using UnityEngine;
    using XMLib;

    /// <summary>
    /// ActionEditorUtility
    /// </summary>
    public static class ActionEditorUtility
    {
        public static bool HasOpenInstances(Type windowType)
        {
            UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(windowType);
            return array != null && array.Length != 0;
        }

        public static void ItemDrawer<T>(int index, ref bool selected, T obj)
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj?.GetType().GetSimpleName()}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }

        public static void StateDrawer(int index, ref bool selected, ActionInfo obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.id}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }

        // public static void RangeConfigDrawer(int index, ref bool selected, RangeConfig obj)
        // {
        //     if (GUILayout.Button($"{index}", selected ? XMLib.AM.AEStyles.item_head_select : XMLib.AM.AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
        //     {
        //         GUI.FocusControl(null);
        //         selected = !selected;
        //     }
        //
        //     EditorGUILayoutEx.DrawObject(GUIContent.none, obj);
        // }
        
        
        public static void CancelTagDrawer(int index, ref bool selected, CancelTag obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.tag}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void BeCancelTagDrawer(int index, ref bool selected, BeCancelledTag obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.cancelTag.Count}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void TempBeCancelTagDrawer(int index, ref bool selected, TempBeCancelledTag obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.id}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void CommandDrawer(int index, ref bool selected, ActionCommand obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.keySequence.Count}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void MoveAcceptanceDrawer(int index, ref bool selected, MoveInputAcceptance obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.range.min}-{obj.range.max}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void AttackInfoDrawer(int index, ref bool selected, AttackInfo obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.phase}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void ActionAttackBoxDrawer(int index, ref bool selected, AttackBoxTurnOnInfo obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.attackPhase}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
        
        public static void ActionHitBoxDrawer(int index, ref bool selected, BeHitBoxTurnOnInfo obj) 
        {
            if (GUILayout.Button($"{index}", selected ? AEStyles.item_head_select : AEStyles.item_head_normal, GUILayout.ExpandHeight(true), GUILayout.Width(15)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
            if (GUILayout.Button($"{obj.inPercentage.Count}", selected ? AEStyles.item_body_select : AEStyles.item_body_normal, GUILayout.Height(30f), GUILayout.ExpandWidth(true)))
            {
                GUI.FocusControl(null);
                selected = !selected;
            }
        }
    }
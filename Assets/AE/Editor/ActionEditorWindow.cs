using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XMLib;
using XMLib.Extensions;


[Flags]
public enum ViewType
{
    None = 0,

    GlobalAction = 1,
    State = 1 << 1,
    StateSet = 1 << 2,
    Action = 1 << 3,
    Tool = 1 << 4,
    Cancel = 1 << 5,
    BeCancel = 1 << 6,
    TempBeCancel = 1 << 7,
    Command = 1 << 8,
    MoveAcceptance = 1 << 9,
    ActionAttackBox = 1 << 10,
    ActionHitBox = 1 << 11,
    Attack = 1 << 12,
    Other = 1 << 13,
    Frame = 1 << 14,
}

/// <summary>
/// 编辑器配置
/// </summary>
[Serializable]
public partial class ActionEditorSetting
{
    public int stateSelectIndex = -1;
    public int cancelSelectIndex = -1;
    public int beCancelSelectIndex = -1;
    public int tempBeCancelSelectIndex = -1;
    public int commandSelectIndex = -1;
    public int moveAcceptanceSelectIndex = -1;
    public int actionAttackBoxSelectIndex = -1;
    public int actionHitBoxSelectIndex = -1;
    public int attackInfoSelectIndex = -1;
    public int attackRangeSelectIndex = -1;
    public int bodyRangeSelectIndex = -1;
    public int actionSelectIndex = -1;
    public int globalActionSelectIndex = -1;
    public int frameSelectIndex = -1;
    public bool enableAllControl = false;
    public bool enableQuickKey = false;

    public ViewType showView;

    public float frameRate => 0.033f;

    public Vector2 otherViewScrollPos = Vector2.zero;

    public float frameWidth = 40;
    public float frameListViewRectHeight = 200f;
}

    public class ActionEditorWindow : EditorWindow
    {
        [MenuItem("XMLib/AE编辑")]
        public static void ShowEditor()
        {
            EditorWindow.GetWindow<ActionEditorWindow>();
        }
    
    
        public static void ShowEditor(GameObject target, TextAsset config)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Debug.LogWarning("编辑器不能在运行时打开");
                return;
            }

            var win = EditorWindow.GetWindow<ActionEditorWindow>();
            if (win.configAsset != null)
            {
                if (win.configAsset == config)
                {// 如果当前已打开的窗口相同，则focus,并直接返回
                    win.Focus();
                    return;
                }
                /*else
                {
                    //如果不相同，则创建一个新的窗口
                    win = EditorWindow.CreateWindow<ACActionEditorWindow>();
                    win.Show();
                }*/
            }
            
            //更新参数
            win.UpdateTarget(target);
            win.UpdateConfig(config);
        }
        
        [NonSerialized] public readonly MenuView menuView;
        [NonSerialized] public readonly StateListView stateListView;
        [NonSerialized] public readonly StateSetView stateSetView;
        [NonSerialized] public readonly ToolView toolView;
        [NonSerialized] public readonly CancelTagListView cancelTagListView;
        [NonSerialized] public readonly CancelTagSetView cancelTagSetView;
        [NonSerialized] public readonly BeCancelledTagListView beCancelTagListView;
        [NonSerialized] public readonly BeCancelledTagSetView beCancelTagSetView;
        [NonSerialized] public readonly TempBeCancelTagListView tempBeCancelTagListView;
        [NonSerialized] public readonly TempBeCancelTagSetView tempBeCancelTagSetView;
        [NonSerialized] public readonly CommandListView commandListView;
        [NonSerialized] public readonly CommandSetView commandSetView;
        [NonSerialized] public readonly AttackListView attackListView;
        [NonSerialized] public readonly AttackSetView attackSetView;
        [NonSerialized] public readonly ActionAttackBoxListView actionAttackBoxListView;
        [NonSerialized] public readonly ActionAttackBoxSetView actionAttackBoxSetView;
        [NonSerialized] public readonly ActionHitBoxListView actionHitBoxListView;
        [NonSerialized] public readonly ActionHitBoxSetView actionHitBoxSetView;
        [NonSerialized] public readonly MoveAcceptanceListView moveAcceptanceListView;
        [NonSerialized] public readonly MoveAcceptanceSetView moveAcceptanceSetView;
        
        
        public List<IView> views { get; private set; }

        #region style

        private readonly float space = 3f;
        private readonly float scrollHeight = 13f;
        private readonly float menuViewRectHeight = 26f;
        private readonly float stateListViewRectWidth = 150f;
        private readonly float stateSetViewRectWidth = 200f;
        private readonly float bodyRangeListViewRectWidth = 200f;
        private readonly float attackRangeListViewRectWidth = 200f;
        private readonly float actionListViewRectWidth = 180f;
        private readonly float actionSetViewRectWidth = 300f;
        private readonly float globalActionListViewRectWidth = 180f;
        private readonly float globalActionSetViewRectWidth = 300f;
        private readonly float toolViewRectWidth = 200f;
        private readonly float cancelTagViewRectWidth = 200f;
        private readonly float cancelTagSetViewRectWidth = 200f;
        private readonly float beCancelTagListViewRectWidth = 200f;
        private readonly float beCancelTagSetViewRectWidth = 200f;
        private readonly float tempBeCancelTagListViewRectWidth = 200f;
        private readonly float tempBeCancelTagSetViewRectWidth = 200f;
        private readonly float commandListRectWidth = 200f;
        private readonly float commandSetViewRectWidth = 200f;
        private readonly float attackListViewRectWidth = 200f;
        private readonly float attackSetViewRectWidth = 250f;
        private readonly float actionAttackBoxListViewRectWidth = 200f;
        private readonly float actionAttackBoxSetViewRectWidth = 200f;
        private readonly float actionHitBoxListViewRectWidth = 200f;
        private readonly float actionHitBoxSetViewRectWidth = 250f;
        private readonly float moveAcceptanceListViewRectWidth = 200f;
        private readonly float moveAcceptanceSetViewRectWidth = 200f;
        
        #endregion style
        

        #region data




        
        #region raw data

        protected static string settingPath = "AE.ActionEditorWindow";

        public ActionEditorSetting setting = new ActionEditorSetting();

        public bool actionMachineDirty = false;

        public bool isRunning => EditorApplication.isPlaying;

        public string lastEditorTargetPath = null;
        public GameObject actionMachineObj = null;
        public ActionControllerTest actionMachine = null;
        public TextAsset configAsset = null;

        [NonSerialized] public ActionInfoContainer config;//SerializeReference 存在bug，先不使用，即无法使用回滚

        
        
        #endregion raw data
        
        
        
        private int CheckSelectIndex<T>(ref int index, IList<T> list)
        {
            return index = list == null ? -1 : Mathf.Clamp(index, -1, list.Count - 1);
        }

        private T GetSelectItem<T>(int index, IList<T> list) where T : class
        {
            if (index < 0 || null == list || list.Count == 0)
            {
                return null;
            }
            return list[index];
        }
        
        
        #endregion data





        public ActionEditorWindow()
        {
            views = new List<IView>();
            
            menuView = CreateView<MenuView>();
            stateListView = CreateView<StateListView>();
            stateSetView = CreateView<StateSetView>();
            toolView = CreateView<ToolView>();
            cancelTagListView = CreateView<CancelTagListView>();
            cancelTagSetView = CreateView<CancelTagSetView>();
            beCancelTagListView = CreateView<BeCancelledTagListView>();
            beCancelTagSetView = CreateView<BeCancelledTagSetView>();
            tempBeCancelTagListView = CreateView<TempBeCancelTagListView>();
            tempBeCancelTagSetView = CreateView<TempBeCancelTagSetView>();
            commandListView = CreateView<CommandListView>();
            commandSetView = CreateView<CommandSetView>();
            attackListView = CreateView<AttackListView>();
            attackSetView = CreateView<AttackSetView>();
            actionAttackBoxListView = CreateView<ActionAttackBoxListView>();
            actionAttackBoxSetView = CreateView<ActionAttackBoxSetView>();
            actionHitBoxListView = CreateView<ActionHitBoxListView>();
            actionHitBoxSetView = CreateView<ActionHitBoxSetView>();
            moveAcceptanceListView = CreateView<MoveAcceptanceListView>();
            moveAcceptanceSetView = CreateView<MoveAcceptanceSetView>();
        }
        
        private T CreateView<T>() where T : IView, new()
        {
            T obj = new T();
            obj.win = this;
            views.Add(obj);
            return obj;
        }
        
        private void OnDestroy()
        {
            foreach (var view in views)
            {
                view.OnDestroy();
            }
        }
        
        private void OnEnable()
        {
            this.titleContent = new GUIContent("动作编辑器");

            //加载配置
            string data = EditorUserSettings.GetConfigValue(settingPath);
            if (!string.IsNullOrEmpty(data))
            {
                EditorJsonUtility.FromJsonOverwrite(data, setting);
            }
            //

            autoRepaintOnSceneChange = true;

            SceneView.duringSceneGui += OnSceneGUI;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (config == null && configAsset != null)
            {
                UpdateConfig(configAsset);
            }
        }
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

            //保存配置
            string data = EditorJsonUtility.ToJson(setting, false);
            EditorUserSettings.SetConfigValue(settingPath, data);
            //
        }
        
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    actionMachineObj = null;
                    actionMachine = null;
                    configAsset = null;
                    config = default;
                    break;

                case PlayModeStateChange.EnteredEditMode:
                    //还原最后的选择
                    GameObject obj;
                    ActionController amTest;
                    if (!string.IsNullOrEmpty(lastEditorTargetPath)
                        && (obj = GameObject.Find(lastEditorTargetPath)) != null
                        && (amTest = obj.GetComponent<ActionController>()) != null)
                    {
                        //UpdateTarget(obj);
                        //UpdateConfig(amTest.AllActions);
                    }
                    lastEditorTargetPath = string.Empty;
                    break;

                case PlayModeStateChange.ExitingEditMode:
                    //记录最后的选择
                    lastEditorTargetPath = actionMachineObj?.GetScenePath();
                    break;
            }
        }
        
        protected virtual void OnGUI()
        {
            Check();
            //Undo.RecordObject(this, "ActionEditorWindow");
            Draw();
            //UpdateActionMachine();

            EventProcess();

            //quickButtonHandler.OnGUI();

            Repaint();
        }
        
        private void EventProcess()
        {
            Rect rect = position;

            Event evt = Event.current;
            if (!rect.Contains(Event.current.mousePosition))
            {
                return;
            }
        }

        private void Check()
        {
            if (!isActionMachineValid)
            {
                actionMachineObj = null;
                actionMachine = null;
            }

            if (!isConfigValid)
            {
                configAsset = null;
                config = default;
            }

            //更新标题
            //this.titleContent = new GUIContent(configAsset != null ? $"编辑 {configAsset.name} " : $"动作编辑器");
        }

        public void UpdateTarget(GameObject target)
        {
            if (target == null)
            {
                throw new RuntimeException($"未选择目标");
            }
            actionMachineObj = target;
            actionMachine = target.GetComponent<ActionControllerTest>();
            if (actionMachine == null)
            {
                actionMachineObj = null;
                throw new RuntimeException($"目标不存在{nameof(ActionControllerTest)}脚本");
            }
        }

        public void UpdateConfig(TextAsset config)
        {
            if (config == null)
            {
                throw new RuntimeException($"未选择配置资源");
            }

            this.configAsset = config;
            this.config = DataUtility.FromJson<ActionInfoContainer>(config.text);
            if (this.config.Equals(default(ActionInfoContainer)))
            {
                throw new RuntimeException($"配置资源解析失败");
            }
        }

        private void Update()
        {
            foreach (var view in views)
            {
                view.OnUpdate();
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!isConfigValid || actionMachine == null)
            {
                return;
            }

            //Undo.RecordObject(this, "ActionEditorWindow");

            //guiDrawer.OnSceneGUI(sceneView);
            //quickButtonHandler.OnSceneGUI(sceneView);

            sceneView.Repaint();
            Repaint();
        }
        
        
        public bool isConfigValid => config != null && (isRunning || configAsset != null);
        public bool isActionMachineValid => actionMachine != null;
        

        #region index

        public int cancelSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.cancelSelectIndex, currentCancels);
                return setting.cancelSelectIndex;
            }

            set
            {
                int oldIndex = setting.cancelSelectIndex;
                setting.cancelSelectIndex = value;
                CheckSelectIndex(ref setting.cancelSelectIndex, currentCancels);
                if (oldIndex != value && oldIndex != setting.cancelSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int beCancelSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.beCancelSelectIndex, currentBeCancels);
                return setting.beCancelSelectIndex;
            }

            set
            {
                int oldIndex = setting.beCancelSelectIndex;
                setting.beCancelSelectIndex = value;
                CheckSelectIndex(ref setting.beCancelSelectIndex, currentBeCancels);
                if (oldIndex != value && oldIndex != setting.beCancelSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int commandSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.commandSelectIndex, currentCommands);
                return setting.commandSelectIndex;
            }

            set
            {
                int oldIndex = setting.commandSelectIndex;
                setting.commandSelectIndex = value;
                CheckSelectIndex(ref setting.commandSelectIndex, currentCommands);
                if (oldIndex != value && oldIndex != setting.commandSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int moveAcceptanceSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.moveAcceptanceSelectIndex, currentMoveAcceptances);
                return setting.moveAcceptanceSelectIndex;
            }

            set
            {
                int oldIndex = setting.moveAcceptanceSelectIndex;
                setting.moveAcceptanceSelectIndex = value;
                CheckSelectIndex(ref setting.moveAcceptanceSelectIndex, currentMoveAcceptances);
                if (oldIndex != value && oldIndex != setting.moveAcceptanceSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int attackInfoSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.attackInfoSelectIndex, currentAttackInfos);
                return setting.attackInfoSelectIndex;
            }

            set
            {
                int oldIndex = setting.attackInfoSelectIndex;
                setting.attackInfoSelectIndex = value;
                CheckSelectIndex(ref setting.attackInfoSelectIndex, currentAttackInfos);
                if (oldIndex != value && oldIndex != setting.attackInfoSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int tempBeCancelTagSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.tempBeCancelSelectIndex, currentTempBeCancels);
                return setting.tempBeCancelSelectIndex;
            }

            set
            {
                int oldIndex = setting.tempBeCancelSelectIndex;
                setting.tempBeCancelSelectIndex = value;
                CheckSelectIndex(ref setting.tempBeCancelSelectIndex, currentTempBeCancels);
                if (oldIndex != value && oldIndex != setting.tempBeCancelSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int actionAttackBoxSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.actionAttackBoxSelectIndex, currentActionAttackBoxs);
                return setting.actionAttackBoxSelectIndex;
            }

            set
            {
                int oldIndex = setting.actionAttackBoxSelectIndex;
                setting.actionAttackBoxSelectIndex = value;
                CheckSelectIndex(ref setting.actionAttackBoxSelectIndex, currentActionAttackBoxs);
                if (oldIndex != value && oldIndex != setting.actionAttackBoxSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int actionHitBoxSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.actionHitBoxSelectIndex, currentActionHitBoxs);
                return setting.actionHitBoxSelectIndex;
            }

            set
            {
                int oldIndex = setting.actionHitBoxSelectIndex;
                setting.actionHitBoxSelectIndex = value;
                CheckSelectIndex(ref setting.actionHitBoxSelectIndex, currentActionHitBoxs);
                if (oldIndex != value && oldIndex != setting.actionHitBoxSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        
        public int stateSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.stateSelectIndex, currentStates);
                return setting.stateSelectIndex;
            }
            set
            {
                int oldIndex = setting.stateSelectIndex;
                setting.stateSelectIndex = value;
                CheckSelectIndex(ref setting.stateSelectIndex, currentStates);
                if (oldIndex != value && oldIndex != setting.stateSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }
        public int frameSelectIndex
        {
            get
            {
                CheckSelectIndex(ref setting.frameSelectIndex, currentFrames);
                return setting.frameSelectIndex;
            }
            set
            {
                int oldIndex = setting.frameSelectIndex;
                setting.frameSelectIndex = value;
                CheckSelectIndex(ref setting.frameSelectIndex, currentFrames);
                if (oldIndex != value && oldIndex != setting.frameSelectIndex)
                {//当前帧发生改变
                    actionMachineDirty = true;
                }
            }
        }



        public CancelTag currentCancelTag => GetSelectItem(cancelSelectIndex, currentCancels);
        public BeCancelledTag currentBeCancelTag => GetSelectItem(beCancelSelectIndex, currentBeCancels);
        public TempBeCancelledTag currentTempBeCancelTag => GetSelectItem(tempBeCancelTagSelectIndex, currentTempBeCancels);
        public ActionCommand currentCommand => GetSelectItem(commandSelectIndex, currentCommands);
        public MoveInputAcceptance currentMoveInputAcceptance =>
            GetSelectItem(moveAcceptanceSelectIndex, currentMoveAcceptances);
        public AttackInfo currentAttackInfo => GetSelectItem(attackInfoSelectIndex, currentAttackInfos);

        public AttackBoxTurnOnInfo currentActionAttackBox =>
            GetSelectItem(actionAttackBoxSelectIndex, currentActionAttackBoxs);

        public BeHitBoxTurnOnInfo currentActionHitBox => GetSelectItem(actionHitBoxSelectIndex, currentActionHitBoxs);
        public ActionInfo currentState => GetSelectItem(stateSelectIndex, currentStates);
        public FrameInfo currentFrame => GetSelectItem(frameSelectIndex, currentFrames);
        
        public List<FrameInfo> currentFrames = new();

        public List<CancelTag> currentCancels => currentState?.cancelTag;
        public List<BeCancelledTag> currentBeCancels => currentState?.beCancelledTag;
        public List<TempBeCancelledTag> currentTempBeCancels => currentState?.tempBeCancelledTag;
        public List<ActionCommand> currentCommands => currentState?.commands;
        public List<MoveInputAcceptance> currentMoveAcceptances => currentState?.inputAcceptance;
        public List<AttackInfo> currentAttackInfos => currentState?.attacks;
        public List<AttackBoxTurnOnInfo> currentActionAttackBoxs => currentState?.attackPhase;
        public List<BeHitBoxTurnOnInfo> currentActionHitBoxs => currentState?.defensePhase;
        public List<ActionInfo> currentStates => config?.data;
        
        #endregion index
        
        
        #region Anima

        private void UpdateActionMachine()
        {
            if (!actionMachineDirty || !isActionMachineValid)
            {
                return;
            }
            actionMachineDirty = false;

            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            AnimationClip clip = GetCurrentAnimationClip();
            int frameIndex = frameSelectIndex;
            if (clip == null || frameIndex < 0)
            {
                return;
            }

            float time = frameIndex * setting.frameRate;

            var state = currentState;

            Animator animator = GetAnimator();
            clip.SampleAnimation(animator.gameObject, time);
        }

        public Animator GetAnimator()
        {
            if (null == actionMachineObj)
            {
                return null;
            }

            Animator animator = actionMachineObj.GetComponent<Animator>();
            if (null != animator)
            {
                return animator;
            }

            animator = actionMachineObj.GetComponentInChildren<Animator>();

            return animator;
        }

        public AnimationClip GetCurrentAnimationClip()
        {
            Animator animator = GetAnimator();
            var state = currentState;

            if (animator == null || state.Equals(default(ActionInfo)))
            {
                return null;
            }

            return GetAnimationClipByStateName(animator, state.animKey);
        }

        public AnimationClip GetAnimationClipByName(Animator animator, string clipName)
        {
            return Array.Find(animator.runtimeAnimatorController.animationClips, t => string.Compare(clipName, t.name) == 0);

        }
        
        public AnimationClip GetAnimationClipByStateName(Animator animator, string stateName)
        {
            if (animator == null) return null;

            RuntimeAnimatorController runtimeController = animator.runtimeAnimatorController;

            if (runtimeController is AnimatorController animatorController)
            {
                foreach (var layer in animatorController.layers)
                {
                    AnimatorStateMachine stateMachine = layer.stateMachine;
                    foreach (var state in stateMachine.states)
                    {
                        if (state.state.name == stateName)
                        {
                            return state.state.motion as AnimationClip;
                        }
                    }
                }
            }

            return null;
        }

        #endregion Anima
        
        #region Draw view

        private static string copyData;
        private static Type copyDataType;

        public object copyBuffer
        {
            set
            {
                if (value == null)
                {
                    copyDataType = null;
                    copyData = null;
                    return;
                }
                copyDataType = value.GetType();
                copyData = DataUtility.ToJson(value, copyDataType);
            }
            get
            {
                if (copyDataType == null || copyData == null) { return null; }
                return DataUtility.FromJson(copyData, copyDataType);
            }
        }

        private void Draw()
        {
            #region calc size
            
            
            Rect rect = this.position;
            rect.position = Vector2.zero;

            float startPosX = rect.x;
            float startPosY = rect.y;
            float height = rect.height;
            float width = rect.width;

            Rect menuViewRect = Rect.zero;
            Rect toolViewRect = Rect.zero;
            Rect globalActionListViewRect = Rect.zero;
            Rect globalActionSetViewRect = Rect.zero;
            Rect stateListViewRect = Rect.zero;
            Rect frameListViewRect = Rect.zero;
            Rect bodyRangeListViewRect = Rect.zero;
            Rect attackRangeListViewRect = Rect.zero;
            Rect actionSetViewRect = Rect.zero;
            Rect actionListViewRect = Rect.zero;
            Rect stateSetViewRect = Rect.zero;
            Rect cancelTagViewRect = Rect.zero;
            Rect cancelTagSetViewRect = Rect.zero;
            Rect beCancelTagListViewRect = Rect.zero;
            Rect beCancelTagSetViewRect = Rect.zero;
            Rect tempBeCancelTagListViewRect = Rect.zero;
            Rect tempBeCancelTagSetViewRect = Rect.zero;
            Rect commandListViewRect = Rect.zero;
            Rect commandSetViewRect = Rect.zero;
            Rect moveAcceptanceListViewRect = Rect.zero;
            Rect moveAcceptanceSetViewRect = Rect.zero;
            Rect attackListViewRect = Rect.zero;
            Rect attackSetViewRect = Rect.zero;
            Rect actionAttackBoxListViewRect = Rect.zero;
            Rect actionAttackBoxSetViewRect = Rect.zero;
            Rect actionHitBoxListViewRect = Rect.zero;
            Rect actionHitBoxSetViewRect = Rect.zero;

            menuViewRect = new Rect(
                startPosX + space,
                startPosY + space,
                rect.width - space,
                menuViewRectHeight - space);
            startPosY += menuViewRectHeight;
            height -= menuViewRectHeight;
            
            
            if ((setting.showView & ViewType.State) != 0 && !stateListView.isPop)
            {
                stateListViewRect = new Rect(
                    startPosX + space,
                    startPosY + space,
                    stateListViewRectWidth - space,
                    height - space * 2);
                startPosX += stateListViewRectWidth;
                width -= stateListViewRectWidth;
            }
            
            
            float itemHeight = height - scrollHeight;
            float nextPosX = startPosX;
            float nextPosY = startPosY;
            bool hasNextView = false;
            
            if ((setting.showView & ViewType.Tool) != 0 && !toolView.isPop)
            {
                toolViewRect = new Rect(
                    nextPosX + space,
                    nextPosY + space,
                    toolViewRectWidth - space,
                    itemHeight - space * 2);
                nextPosX += toolViewRectWidth;
                hasNextView = true;
            }
            
            if ((setting.showView & ViewType.StateSet) != 0 && !stateSetView.isPop)
            {
                stateSetViewRect = new Rect(
                    nextPosX + space,
                    nextPosY + space,
                    stateSetViewRectWidth - space,
                    itemHeight - space * 2);
                nextPosX += stateSetViewRectWidth;
                hasNextView = true;
            }
            
            if ((setting.showView & ViewType.Cancel) != 0)
            {
                if (!cancelTagListView.isPop)
                {
                    cancelTagViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        cancelTagViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += cancelTagViewRectWidth;
                    hasNextView = true;
                }
                
                if (!cancelTagSetView.isPop)
                {
                    cancelTagSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        cancelTagSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += cancelTagSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            
            if ((setting.showView & ViewType.BeCancel) != 0)
            {
                if (!beCancelTagListView.isPop)
                {
                    beCancelTagListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        beCancelTagListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += beCancelTagListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!beCancelTagSetView.isPop)
                {
                    beCancelTagSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        beCancelTagSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += beCancelTagSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            if ((setting.showView & ViewType.TempBeCancel) != 0)
            {
                if (!tempBeCancelTagListView.isPop)
                {
                    tempBeCancelTagListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        tempBeCancelTagListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += tempBeCancelTagListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!tempBeCancelTagSetView.isPop)
                {
                    tempBeCancelTagSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        tempBeCancelTagListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += tempBeCancelTagListViewRectWidth;
                    hasNextView = true;
                }

            }
            
            if ((setting.showView & ViewType.Command) != 0)
            {
                if (!commandListView.isPop)
                {
                    commandListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        commandListRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += commandListRectWidth;
                    hasNextView = true;
                }
                
                if (!commandSetView.isPop)
                {
                    commandSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        commandSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += commandSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            if ((setting.showView & ViewType.MoveAcceptance) != 0)
            {
                if (!moveAcceptanceListView.isPop)
                {
                    moveAcceptanceListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        moveAcceptanceListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += moveAcceptanceListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!moveAcceptanceSetView.isPop)
                {
                    moveAcceptanceSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        moveAcceptanceSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += moveAcceptanceSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            if ((setting.showView & ViewType.Attack) != 0)
            {
                if (!attackListView.isPop)
                {
                    attackListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        attackListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += attackListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!attackSetView.isPop)
                {
                    attackSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        attackSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += attackSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            if ((setting.showView & ViewType.ActionAttackBox) != 0)
            {
                if (!actionAttackBoxListView.isPop)
                {
                    actionAttackBoxListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionAttackBoxListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionAttackBoxListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!actionAttackBoxSetView.isPop)
                {
                    actionAttackBoxSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionAttackBoxSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionAttackBoxSetViewRectWidth;
                    hasNextView = true;
                }

            }
            if ((setting.showView & ViewType.ActionHitBox) != 0)
            {
                if (!actionHitBoxListView.isPop)
                {
                    actionHitBoxListViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionHitBoxListViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionHitBoxListViewRectWidth;
                    hasNextView = true;
                }
                
                if (!actionHitBoxSetView.isPop)
                {
                    actionHitBoxSetViewRect = new Rect(
                        nextPosX + space,
                        nextPosY + space,
                        actionHitBoxSetViewRectWidth - space,
                        itemHeight - space * 2);
                    nextPosX += actionHitBoxSetViewRectWidth;
                    hasNextView = true;
                }

            }
            
            #endregion calc size

            #region draw

            menuView.Draw(menuViewRect);
            
            if ((setting.showView & ViewType.State) != 0 && !stateListView.isPop)
            {
                stateListView.Draw(stateListViewRect);
            }

            if (hasNextView)
            {
                Rect position = new Rect(startPosX + space, startPosY, width - space, height);
                Rect view = new Rect(startPosX + space, startPosY, nextPosX - startPosX - space, itemHeight);
                setting.otherViewScrollPos = GUI.BeginScrollView(position, setting.otherViewScrollPos, view, true, false);

                
                if ((setting.showView & ViewType.StateSet) != 0 && !stateSetView.isPop)
                {
                    stateSetView.Draw(stateSetViewRect);
                }
                
                if ((setting.showView & ViewType.Tool) != 0 && !toolView.isPop)
                {
                    toolView.Draw(toolViewRect);
                }
                
                if ((setting.showView & ViewType.Cancel) != 0)
                {
                    if (!cancelTagListView.isPop)
                    {
                        cancelTagListView.Draw(cancelTagViewRect);
                        
                    }
                    if (!cancelTagSetView.isPop)
                    {
                        cancelTagSetView.Draw(cancelTagSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.BeCancel) != 0)
                {
                    if (!beCancelTagListView.isPop)
                    {
                        beCancelTagListView.Draw(beCancelTagListViewRect);
                        
                    }
                    if (!beCancelTagSetView.isPop)
                    {
                        beCancelTagSetView.Draw(beCancelTagSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.TempBeCancel) != 0)
                {
                    if (!tempBeCancelTagListView.isPop)
                    {
                        tempBeCancelTagListView.Draw(tempBeCancelTagListViewRect);
                        
                    }
                    if (!tempBeCancelTagSetView.isPop)
                    {
                        tempBeCancelTagSetView.Draw(tempBeCancelTagSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.Command) != 0)
                {
                    if (!commandListView.isPop)
                    {
                        commandListView.Draw(commandListViewRect);
                        
                    }
                    if (!commandSetView.isPop)
                    {
                        commandSetView.Draw(commandSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.MoveAcceptance) != 0)
                {
                    if (!moveAcceptanceListView.isPop)
                    {
                        moveAcceptanceListView.Draw(moveAcceptanceListViewRect);
                        
                    }
                    if (!moveAcceptanceSetView.isPop)
                    {
                        moveAcceptanceSetView.Draw(moveAcceptanceSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.Attack) != 0)
                {
                    if (!attackListView.isPop)
                    {
                        attackListView.Draw(attackListViewRect);
                        
                    }
                    if (!attackSetView.isPop)
                    {
                        attackSetView.Draw(attackSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.ActionAttackBox) != 0)
                {
                    if (!actionAttackBoxListView.isPop)
                    {
                        actionAttackBoxListView.Draw(actionAttackBoxListViewRect);
                        
                    }
                    if (!actionAttackBoxSetView.isPop)
                    {
                        actionAttackBoxSetView.Draw(actionAttackBoxSetViewRect);
                        
                    }
                }
                
                if ((setting.showView & ViewType.ActionHitBox) != 0)
                {
                    if (!actionHitBoxListView.isPop)
                    {
                        actionHitBoxListView.Draw(actionHitBoxListViewRect);
                        
                    }
                    if (!actionHitBoxSetView.isPop)
                    {
                        actionHitBoxSetView.Draw(actionHitBoxSetViewRect);
                        
                    }
                }
                
                GUI.EndScrollView();
            }

            #endregion draw
        }

        #endregion Draw view
    }
    
    
    public class ViewWindow : EditorWindow
    {
        protected IView _view;
        protected ActionEditorWindow _win;
        protected string _viewTypeName;

        public IView view
        {
            get
            {
                if (_view != null && _win != null) { return _view; }

                Type viewType = Type.GetType(_viewTypeName, false);
                if (viewType == null) { return null; }

                if (!HasOpenInstances<ActionEditorWindow>()) { return null; }

                _win = GetWindow<ActionEditorWindow>();
                _view = _win.views.Find(t => t.GetType() == viewType);
                _view.popWindow = this;
                return _view;
            }
            set
            {
                _view = value;
                _win = value.win;
                _viewTypeName = value.GetType().FullName + "," + value.GetType().Assembly.FullName;
                _view.popWindow = this;
            }
        }

        public static ViewWindow Show(IView view, Rect rect)
        {
            var win = EditorWindow.CreateWindow<ViewWindow>(view.title);
            win.position = rect;
            win.view = view;
            win.Show();
            return win;
        }

        private void OnEnable()
        {
            autoRepaintOnSceneChange = true;
        }

        private void OnDisable()
        {
        }

        private void OnDestroy()
        {
            view?.OnPopDestroy();
        }

        private void OnGUI()
        {
            if (view == null)
            {
                return;
            }

            Rect contentRect = new Rect(Vector2.zero, this.position.size);
            view.Draw(contentRect);

            Repaint();
        }
    }



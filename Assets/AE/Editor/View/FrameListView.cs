   using System;
   using System.Collections.Generic;
   using UnityEditor;
   using UnityEngine;
   using XMLib;

    /// <summary>
    /// FrameListView
    /// </summary>
    [Serializable]
    public class FrameListView : IView
    {
   public override string title => $"帧序列({win.setting.frameRate}s)";
   public override bool useAre => false;
   
   private Vector2 actionViewScroll = Vector2.zero;
   
   private bool playFrame = false;
   
   private float playTimer = 0f;
   private float playSpeed = 1f;
   
   private float toolHeight = 22f;
   
   protected override void OnGUI(Rect rect)
   {
       var frames = win.currentFrames;
       var cancels = win.currentCancels;
       if (null == frames || frames.Count == 0)
       {
           EditorGUI.LabelField(rect, "没有帧可编辑");
           return;
       }
       
       Rect frameRt = new Rect(rect.x, rect.y, rect.width, rect.height - toolHeight);
       Rect toolRt = new Rect(rect.x, rect.y + frameRt.height, rect.width, toolHeight);
       
       DrawFrames(frames, cancels, frameRt);
       // DrawTool(frames, toolRt);
   }
   
   private void DrawFrames(List<FrameInfo> frames, List<CancelTag> cancels, Rect rect)
   {
       int frameCnt = frames.Count;
   
        //横行左端标题
       float headWidth = 50f;
       float topHeight = 30f;
   
       float frameWidth = win.setting.frameWidth;
       float frameSpace = 4f;
   
       float cancelHeight = 30f;
       float cancelSpace = 4f;
       float cancelOffset = 0f;
   
       float barSize = 16f;
   
       float viewHeight = rect.height - barSize - topHeight;
       float viewWidth = frameWidth * frameCnt;
   
       float minViewWidth = rect.width - barSize - headWidth;
       if (viewWidth < minViewWidth)
       {
           viewWidth = minViewWidth;
       }
   
       float cancelsHeight = cancelHeight * cancels.Count;
       if (cancelsHeight > viewHeight)
       {
           viewHeight = cancelsHeight + cancelHeight;
       }
   
       Rect framePosition = new Rect(rect.x + headWidth, rect.y, rect.width - headWidth - barSize, rect.height - barSize);
       Rect frameView = new Rect(framePosition.x, framePosition.y, viewWidth, rect.height - barSize);
   
       Rect actionIdPosition = new Rect(rect.x, rect.y + topHeight, headWidth, rect.height - barSize - topHeight);
       Rect actionIdView = new Rect(actionIdPosition.x, actionIdPosition.y, headWidth, viewHeight);
   
       Rect actionPosition = new Rect(rect.x + headWidth, rect.y + topHeight, rect.width - headWidth, rect.height - topHeight);
       Rect actionView = new Rect(actionPosition.x, actionPosition.y, viewWidth, viewHeight);
   
       Rect beginFrameRt = new Rect(frameView.x, frameView.y, frameWidth, frameView.height);
       Rect beginActionIdRect = new Rect(actionIdView.x, actionIdView.y, headWidth, cancelHeight);
   
       Rect beginActionRt = new Rect(actionView.x, actionView.y, actionView.width, cancelHeight);
       Rect beginActionBgRt = new Rect(actionView.x, actionView.y, actionView.width, cancelHeight);
   
       Rect beginFrameBtnRt = beginFrameRt;
       beginFrameBtnRt.height = topHeight;
       Rect beginFrameBgRt = beginFrameRt;
       beginFrameBgRt.y += topHeight;
       beginFrameBgRt.height = frameView.height - topHeight;
   
       //-----------------------------------------------------
   
       #region 帧
   
       GUI.BeginScrollView(framePosition, Vector2.right * actionViewScroll.x, frameView, GUIStyle.none, GUIStyle.none);
       for (int i = 0; i < frameCnt; i++)
       {
           Rect btnRt = beginFrameBtnRt;
           btnRt.x += frameWidth * i;
           btnRt.width -= frameSpace;
   
           Rect bgRt = beginFrameBgRt;
           bgRt.x += frameWidth * i;
           bgRt.width -= frameSpace;
   
           bool selected = win.frameSelectIndex == i;
   
           FrameInfo config = win.currentFrames[i];
   
           // string title = string.Format("{0}\n{1}|{2}",
           //  i, 
           //  config.stayAttackRange ? "←" : (config.attackRanges?.Count ?? 0).ToString(),
           //  config.stayBodyRange ? "←" : (config.bodyRanges?.Count ?? 0).ToString());
           if (GUI.Button(btnRt, $"{i}", selected ? XMLib.AM.AEStyles.item_head_select : XMLib.AM.AEStyles.item_head_normal))
           {
               win.frameSelectIndex = selected ? -1 : i;
           }
           GUI.Box(bgRt, GUIContent.none, selected ? XMLib.AM.AEStyles.item_body_select : XMLib.AM.AEStyles.item_body_normal);
       }
       GUI.EndScrollView();
   
       #endregion 帧
   
       //-----------------------------------------------------
   
       #region 控制条
   
       actionViewScroll = GUI.BeginScrollView(actionPosition, actionViewScroll, actionView, true, true);
       for (int i = 0; i < cancels.Count; i++)
       {
            var action = cancels[i];
       
           int beginFrame = 0;
           int endFrame = frameCnt - 1;
           if (frameCnt > 0)
           {
               beginFrame = (int)(action.startFromPercentage * frameCnt);
           }
       //
       //     IHoldFrames holdFrames = action as IHoldFrames;
       //     if (holdFrames != null)
       //     {
       //         beginFrame = holdFrames.GetBeginFrame();
       //         endFrame = holdFrames.GetEndFrame();
       //     }
       //
            //minMaxSlider
            Rect rt = beginActionRt;
            rt.y += cancelHeight * i + cancelOffset;
            rt.height -= (cancelSpace + cancelOffset);
       
            bool selected = win.cancelSelectIndex == i;
       
            float beginValue = beginFrame * frameWidth;
            float endValue = (endFrame + 1) * frameWidth - 2 * EditorGUIEx.minMaxThumbWidth - frameSpace;
       
            bool clicked = EditorGUIEx.MinMaxSlider(rt, ref beginValue, ref endValue, 0, viewWidth - 2 * EditorGUIEx.minMaxThumbWidth);
           if (clicked)
           {
               win.cancelSelectIndex = i;
           }

           if (frameCnt > 0)
           {
               //校验
               beginFrame = Mathf.RoundToInt(beginValue / frameWidth);
               endFrame = Mathf.RoundToInt((endValue + 2 * EditorGUIEx.minMaxThumbWidth + frameSpace) / frameWidth - 1);
               beginFrame = Mathf.Clamp(beginFrame, 0, frameCnt - 1);
               endFrame = Mathf.Clamp(endFrame, 0, frameCnt - 1);
               if (endFrame < beginFrame)
               {
                   endFrame = beginFrame;
               }


               action.startFromPercentage = (float)beginFrame / frameCnt;
           }
       }
       GUI.EndScrollView();
   
       #endregion 控制条
   
       //-----------------------------------------------------
   
       #region 动作序号
   
       GUI.BeginScrollView(actionIdPosition, Vector2.up * actionViewScroll.y, actionIdView, GUIStyle.none, GUIStyle.none);
       for (int i = 0; i < cancels.Count; i++)
       {
           Rect rt = beginActionIdRect;
           rt.y += cancelHeight * i;
           rt.height -= cancelSpace;
           
           bool selected = win.cancelSelectIndex == i;
           
           if (GUI.Button(rt, $"{i}:取消", selected ? XMLib.AM.AEStyles.item_head_select : XMLib.AM.AEStyles.item_head_normal))
           {
               win.cancelSelectIndex = selected ? -1 : i;
           }
       }
       GUI.EndScrollView();
   
       #endregion 动作序号
   
       //-------------------------------------------
   
       #region 动作信息
   
       // GUI.BeginScrollView(actionPosition, actionViewScroll, actionView, true, true);
       // for (int i = 0; i < actions.Count; i++)
       // {
       //     object action = actions[i];
       //
       //     //label
       //     Rect labelRt = beginActionBgRt;
       //     labelRt.y += actionHeight * i;
       //     labelRt.height -= actionSpace;
       //     labelRt.width = viewWidth;
       //
       //     bool selected = win.actionSelectIndex == i;
       //
       //     if (GUI.Button(labelRt, $"{action.ToString()}", XMLib.AM.AEStyles.frame_label))
       //     {
       //         win.actionSelectIndex = selected ? -1 : i;
       //     }
       // }
       // GUI.EndScrollView();
   
       #endregion 动作信息
   }
   //
   //      private void DrawTool(List<FrameConfig> configs, Rect rect)
   //      {
   //          int maxFrames = configs.Count;
   //
   //          GUILayout.BeginArea(rect);
   //          GUILayout.BeginHorizontal(XMLib.AM.AEStyles.list_tool_bg, GUILayout.Height(18f), GUILayout.ExpandHeight(false));
   //
   //          if (GUILayout.Button("上一帧", GUILayout.Width(65)))
   //          {
   //              int index = win.frameSelectIndex - 1;
   //              win.frameSelectIndex = Mathf.Clamp(index, -1, maxFrames - 1);
   //          }
   //          if (GUILayout.Button("下一帧", GUILayout.Width(65)))
   //          {
   //              int index = win.frameSelectIndex + 1;
   //              win.frameSelectIndex = Mathf.Clamp(index, -1, maxFrames - 1);
   //          }
   //          if (GUILayout.Button(playFrame ? "停止" : "播放"))
   //          {
   //              playTimer = 0f;
   //              playFrame = !playFrame;
   //          }
   //
   //          GUILayout.Space(5);
   //          playSpeed = EditorGUILayout.Slider(playSpeed, 0f, 1f, GUILayout.MaxWidth(150));
   //          GUILayout.Space(10);
   //          win.frameSelectIndex = EditorGUILayout.IntSlider(win.frameSelectIndex, -1, maxFrames - 1, GUILayout.MaxWidth(300f));
   //          GUILayout.FlexibleSpace();
   //          GUILayout.EndHorizontal();
   //          GUILayout.EndArea();
   //      }
   
       public override void OnUpdate()
       {
   //          if (!playFrame)
   //          {
   //              return;
   //          }
   //          int maxIndex = win.currentFrames?.Count ?? -1;
   //          if (maxIndex < 0)
   //          {
   //              return;
   //          }
   //
   //          playTimer += Time.deltaTime * playSpeed;
   //
   //          int index = win.frameSelectIndex;
   //
   //          while (playTimer > win.setting.frameRate)
   //          {
   //              playTimer -= win.setting.frameRate;
   //              index += 1;
   //
   //              if (index >= maxIndex)
   //              {
   //                  index = 0;
   //              }
   //          }
   //          win.frameSelectIndex = index;
   //
   //          win.Repaint();
        }
   }
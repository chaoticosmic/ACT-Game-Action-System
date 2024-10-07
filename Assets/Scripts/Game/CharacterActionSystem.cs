
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionSystem : MonoBehaviour
{
    private static CharacterActionSystem _instance;

    public static CharacterActionSystem GetInstance() => _instance;

    public static List<ActionController> Controllers = new();
    
    public static void EnsureCreation()
    {
        if (_instance == null)
        {
            GameObject systemGameObject = new GameObject("CharacterActionSystem");
            _instance = systemGameObject.AddComponent<CharacterActionSystem>();

            systemGameObject.hideFlags = HideFlags.NotEditable;
            _instance.hideFlags = HideFlags.NotEditable;

            GameObject.DontDestroyOnLoad(systemGameObject);
        }
    }
    public static void RegisterController(ActionController actionController)
    {
        Controllers.Add(actionController);
    }
    
    public static void UnRegisterController(ActionController actionController)
    {
        Controllers.Remove(actionController);
    }
    
    // This is to prevent duplicating the singleton gameobject on script recompiles
    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
    
    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// 逻辑帧
    /// </summary>
    public static void LogicUpdate(float delta)
    {
        for (int i = 0; i < Controllers.Count; i++)
        {
            ActionController controller = Controllers[i];
            controller.Tick(delta);
        }
    }
}

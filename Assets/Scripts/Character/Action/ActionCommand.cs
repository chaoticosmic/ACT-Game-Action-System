using System;
using System.Collections.Generic;

/// <summary>
/// 根据命令得出的操作
/// </summary>
[Serializable]
public class ActionCommand
{
    /// <summary>
    /// 按键顺序
    /// </summary>
    public List<KeyMap> keySequence;

    /// <summary>
    /// 检查的按键最远的一次操作距离现在的最远时间（秒）
    /// </summary>
    public float validInSec;
}



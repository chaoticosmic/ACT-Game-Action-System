using System;
using System.Collections.Generic;

/// <summary>
/// 脚本函数的信息
/// </summary>
[Serializable]
public class ScriptMethodInfo
{
    /// <summary>
    /// 函数名
    /// </summary>
    public string method;

    /// <summary>
    /// 参数，使用string[]得策划自己在脚本（Methods）翻译其用意
    /// </summary>
    public List<string> param = new();
}
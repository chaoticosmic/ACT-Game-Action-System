using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动作的RootMotion的移动函数
/// 参数1:float：当前动作进行到的百分比
/// 参数2:string[]：配置在ActionInfo表里actionInfo.rootMotionTween的param部分
/// 返回值：Vector3，偏移量，假设起始的时候坐标为zero，到normalized==参数float的时候，当时的偏移值
/// </summary>
public static class RootMotionMethod
{
    public static Dictionary<string, Func<float, List<string>, Vector3>> Methods =
        new ()
        {
            //---------------------------直线向前----------------------------------------
            {
                "GoStraight",
                (pec, param) =>
                {
                    float totalDis = param.Count > 0 ? float.Parse(param[0]) : 0;
                    float startPec = param.Count > 1 ? float.Parse(param[1]) : 0;
                    float endPec = param.Count > 2 ? float.Parse(param[2]) : 1;
                    return pec <= startPec ? Vector3.zero :
                        pec >= endPec ? new Vector3(totalDis, 0, 0) : new Vector3(pec * totalDis, 0, 0);
                }
            },
            
        };
}
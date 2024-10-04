
using System;

//新添加



[Serializable]
public class FrameInfo
{
    /// <summary>
    /// 每一帧编号
    /// </summary>
    public int id;

    /// <summary>
    /// 关键帧
    /// 存在一个“同步”问题，也就是说，逻辑需要告诉动画，你最多可以播放到哪儿，如果我逻辑还没有去下一帧，
    /// 那么你动画播放到这里就应该“卡住不动”了。
    /// </summary>
    public float keyFrame;

    /// <summary>
    /// 循环帧数
    /// 一个动作帧的下一帧直接是自己这一帧的次数，这个循环帧数通常都是1，但是格斗游戏中的受伤动作，是动态算出来的。
    /// 这正是《街霸》这些游戏中玩家可以看到一些+6、-30之类的数据的来源之一，正因为受伤动作中某些Frame的循环次数增多了，
    /// 所以动作“变得更慢了”或者“硬直更久了”，以至于这个（玩家理解的）“加减帧”数据发生了变化。 
    /// </summary>
    public int autoPlayCount;
}

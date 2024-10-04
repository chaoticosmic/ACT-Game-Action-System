


//新添加。。取消了


/// <summary>
/// CancelTag和CancelData是决定2个动作切换的一个关键信息，他们之间的关系就像是锁跟钥匙的关系，有一种对应关系，
/// 并且因为这种对应关系产生联动。在一个动作的动作帧本身有一些CancelTag，他们就像是锁孔一样，本身没有直接的作用，
/// 但是因为其他动作有CancelData，这些有CancelData的动作就仿佛钥匙能开锁一般的，和这个动作帧产生关联，最后可以形成“动作的切换”。
/// </summary>
public class CancelData
{
    
}

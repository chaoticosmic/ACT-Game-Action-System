using UnityEngine;

public class AnimSyncTest : MonoBehaviour
{
    private Animator animator;  // 你的 Animator 组件
    private float animationTime = 0f;  // 当前动画时间进度

    private void FixedUpdate()
    {
        // 计算当前物理帧的时间步与动画帧速率之间的比例
        float frameTime = 1f / 30f; // 动画每帧对应的时间
        float animationProgressPerFixedUpdate = Time.fixedDeltaTime / frameTime; // 每个FixedUpdate应推进的动画进度

        // 更新动画进度
        animationTime += animationProgressPerFixedUpdate;

        // 将时间范围限制在 [0, 1] 之间，如果是循环动画可使用 % 1 来保证在范围内
        animationTime %= 1f;

        // 设置 Animator 的播放进度（假设你有一个控制动画状态的 float 参数，叫做 "PlaybackTime"）
        animator.Play("YourAnimationStateName", 0, animationTime);
    }
}

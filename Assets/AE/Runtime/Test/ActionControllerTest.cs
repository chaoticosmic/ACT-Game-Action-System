using UnityEditor;
using UnityEngine;
using XMLib;

/// <summary>
/// TestActionMachine
/// </summary>
[RequireComponent(typeof(AnimatorTest))]
public class ActionControllerTest : MonoBehaviour
{
    public TextAsset config;
    public UnityEngine.Matrix4x4 localToWorldMatrix => transform.localToWorldMatrix;
    public bool destroyOnPlay;

    private void Awake()
    {
        if (destroyOnPlay)
        {
            Destroy(gameObject);
        }
    }
}
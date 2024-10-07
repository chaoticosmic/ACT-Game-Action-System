
using System;
using KinematicCharacterController;
using UnityEngine;

public class ActionCharacterController : MonoBehaviour, ICharacterController
{
    public KinematicCharacterMotor Motor;
    private void Awake()
    {
        Motor = GetComponent<KinematicCharacterMotor>();
        Motor.CharacterController = this;
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
    {
    }
    // public Vector3 ThisTickMove(float delta)
    // {
    //     //硬直时候不做移动
    //     if (action.Freezing) return Vector3.zero;
    //     //有强制位移有限强制位移
    //     if (UnderForceMove)
    //     {
    //         Vector3 fMove = _forceMove.MoveTween(_forceMove);
    //         _forceMove.Update(delta);
    //         return fMove;
    //     }
    //     //下落重量的增幅，不应该在Update做，而是外部调用update的地方去做
    //     if (_falling) _curWeight += weight * delta;
    //     else _curWeight = 0;
    //     //有强制位移的时候强制位移，没有的时候自然移动
    //     return  NatureMove(delta);
    // }
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
    {
        if (Motor.GroundingStatus.IsStableOnGround)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;

            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;
        }
    }

    public void BeforeCharacterUpdate(float deltaTime)
    {
        
    }

    public void PostGroundingUpdate(float deltaTime)
    {
        
    }

    public void AfterCharacterUpdate(float deltaTime)
    {
        
    }

    public bool IsColliderValidForCollisions(Collider coll)
    {
        return false;
    }

    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
        ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
        Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
    {
        
    }

    public void OnDiscreteCollisionDetected(Collider hitCollider)
    {
        
    }
}

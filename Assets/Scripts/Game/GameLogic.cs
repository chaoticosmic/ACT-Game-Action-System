
using System;
using KinematicCharacterController;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _instance;
    public static void EnsureCreation()
    {
        if (_instance == null)
        {
            GameObject systemGameObject = new GameObject("GameLogic");
            _instance = systemGameObject.AddComponent<GameLogic>();

            systemGameObject.hideFlags = HideFlags.NotEditable;
            _instance.hideFlags = HideFlags.NotEditable;

            GameObject.DontDestroyOnLoad(systemGameObject);
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        EnsureCreation();
        KinematicCharacterSystem.EnsureCreation();
        CharacterActionSystem.EnsureCreation();

        KinematicCharacterSystem.Settings.AutoSimulation = false;
        KinematicCharacterSystem.Settings.Interpolate = false;
    }

    // This is to prevent duplicating the singleton gameobject on script recompiles
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    void MoveLogic(float deltaTime)
    {

        if (KinematicCharacterSystem.Settings.Interpolate)
        {
            KinematicCharacterSystem.PreSimulationInterpolationUpdate(deltaTime);
        }

        KinematicCharacterSystem.Simulate(deltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);

        if (KinematicCharacterSystem.Settings.Interpolate)
        {
            KinematicCharacterSystem.PostSimulationInterpolationUpdate(deltaTime);
        }
        
    }

    void ActionLogic(float deltaTime)
    {
        CharacterActionSystem.LogicUpdate(deltaTime);
    }
    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        MoveLogic(deltaTime);
        ActionLogic(deltaTime);
    }
}

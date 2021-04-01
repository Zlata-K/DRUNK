using UnityEngine;

public struct NPCsGlobalVariables
{
    //NPC State Machine variables
    public const float MaxChaseDistance = 25.0f;
    
    //Look where you are going variables
    public const float MaxAngleChange = 150.0f;
    
    //Wandering behavior variables
    public const int WanderCircleDistance = 5;
    public const int WanderCircleRadius = 10;
    public const float WanderMaxVelocity = 1.0f;
    public const float WanderWaitTime = 10.0f;
    
    //Chasing behavior variables
    public const int ChasePredictionMultiplier = 2;
    public const float ChaseMaxVelocity = 3.0f;
    public const float ChaseAcceleration = 1.8f;
    
    //Animator speed variable names
    public static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
    public static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
}

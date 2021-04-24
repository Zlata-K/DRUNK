using UnityEngine;

namespace NPCs
{
    public static class NpcGlobalVariables
    {
        //NPC State Machine variables
        public const float MaxChaseDistance = 40.0f;

        //Look where you are going variables
        public const float MaxAngleChange = 360.0f;
    
        //Wandering behavior variables
        public const int WanderCircleDistance = 5;
        public const int WanderCircleRadius = 10;
        public const float WanderMaxVelocity = 1.0f;
        public const float WanderWaitTime = 10.0f;
        public const float WanderObstacleAvoidanceMultiplier = 0.2f;
        public const float WanderNpcAvoidanceMultiplier = 4.0f;

        //Chasing behavior variables
        public const float ChaseMaxVelocity = 2.5f;
        public const float ChaseAcceleration = 1.6f;
        public const float FieldOfView = 40.0f;
        public const float ChasePlayerRange = 5f;
        public const float WithinTargetNodeRange = 0.5f;
        public const float ChaseObstacleAvoidanceMultiplier = 4.0f;
        public const float ChaseNpcAvoidanceMultiplier = 1.0f;
        public static readonly Vector3 DefaultInitialVector = new Vector3(0,100,0);
        public const byte NextElementInPath = 1;

        //Animator speed variable names
        public static readonly int VelocityXHash = Animator.StringToHash("Velocity X");
        public static readonly int VelocityZHash = Animator.StringToHash("Velocity Z");
    
        //Buffer distances
        public const float ChaseFlockBuffer = 0.2f;
    }
}

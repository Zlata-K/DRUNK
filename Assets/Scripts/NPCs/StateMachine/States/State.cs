using NPCs.Flocking;
using NPCs.Flocking.Behaviour;
using UnityEngine;

namespace NPCs.StateMachine.States
{
    public abstract class State
    {
        protected NPCManager NpcManager;
        protected CollisionAvoidance AvoidObstacles;
        protected FlockBehaviour AvoidNpcs;

        public abstract void Move();
    }
}

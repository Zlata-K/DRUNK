using NPCs.Flocking;

namespace NPCs.StateMachine.States
{
    public abstract class State
    {
        protected NPCManager NpcManager;
        protected FlockBehaviour AvoidObstacles;
        protected FlockBehaviour AvoidNpcs;

        public abstract void Move();
    }
}

using NPCs.Flocking;
using NPCs.Flocking.Behaviour;
using NPCs.StateMachine.States;
using UnityEngine;

namespace NPCs.StateMachine
{
    public class NPCStateMachine : MonoBehaviour
    {
        public State CurrentState { get; set; }

        public WanderState Wander;
        public IdleState Idle;
        public ChaseState Chase;
        public FlockingState Flocking;

        private NPCManager _npcManager;
        private FlockManager _flockManager;

        [SerializeField] private CollisionAvoidance avoidObstacles;
        [SerializeField] private FlockBehaviour avoidNpCs;

        void Start()
        {
            _npcManager = GetComponent<NPCManager>();
            _flockManager = Indestructibles.FlockManagerInstance;
            
            Wander = new WanderState(_npcManager, avoidObstacles, avoidNpCs);
            Chase = new ChaseState(_npcManager, avoidObstacles);
            Idle = new IdleState();
            Flocking = new FlockingState();

            CurrentState = Wander;
        }

        public void StartWandering()
        {
            CurrentState = Wander;
            
            if (_flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.RemoveNpcFromFlock(_npcManager);
            }
        }

        public void StartChasing()
        {
            CurrentState = Chase;
            
            if (_flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.RemoveNpcFromFlock(_npcManager);
            }
        }

        public void StartIdle()
        {
            CurrentState = Idle;
            
            if (_flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.RemoveNpcFromFlock(_npcManager);
            }
        }

        public void StartFlocking()
        {
            CurrentState = Flocking;
            
            if (!_flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.AddNpcToFlock(_npcManager);
            }
        }
    }
}
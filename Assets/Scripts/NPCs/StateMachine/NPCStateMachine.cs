using NPCs.Flocking;
using NPCs.Flocking.Behaviour;
using NPCs.StateMachine.States;
using UnityEngine;

namespace NPCs.StateMachine
{
    public class NPCStateMachine : MonoBehaviour
    {
        public State CurrentState { get; set; }

        private State _wander;
        private State _idle;
        private State _chase;
        private State _flocking;

        private NPCManager _npcManager;
        private FlockManager _flockManager;

        [SerializeField] private CollisionAvoidance avoidObstacles;
        [SerializeField] private FlockBehaviour avoidNpCs;

        void Start()
        {
            _npcManager = GetComponent<NPCManager>();
            _flockManager = Indestructibles.FlockManagerInstance;

            _wander = new Wander(_npcManager, avoidObstacles, avoidNpCs);
            _chase = new Chase(_npcManager, avoidObstacles, avoidNpCs);
            _idle = new Idle();
            _flocking = new global::NPCs.StateMachine.States.Flocking();

            CurrentState = _chase;
        }

        private void Update()
        {
            CurrentState.Move();
            float distanceFromPlayer = Vector3.Distance(Indestructibles.Player.transform.position, transform.position);
            CheckPlayerOutOfRange(distanceFromPlayer);
            CheckPlayerBackInRange(distanceFromPlayer);
            CheckFlockingOrChasing();
            AddToFlock();
        }

        //----- State change functions -----

        /*
     * If the player get out of range during chase, start wandering.
     */
        private void CheckPlayerOutOfRange(float distanceFromPlayer)
        {
            if (CurrentState == _chase &&
                distanceFromPlayer > NpcGlobalVariables.MaxChaseDistance)
            {
                CurrentState = _wander;
                _npcManager.LookingForPlayer = true;
            }
        }

        /*
         * If the player is back in range and was previously chased by the NPC, chase again.
         */
        private void CheckPlayerBackInRange(float distanceFromPlayer)
        {
            if (CurrentState == _wander &&
                _npcManager.LookingForPlayer &&
                distanceFromPlayer < NpcGlobalVariables.MaxChaseDistance)
            {
                CurrentState = _chase;
            }
        }

        public void StartWandering()
        {
            CurrentState = _wander;
        }

        public void StartChasing()
        {
            CurrentState = _chase;
        }

        public void StartIdle()
        {
            CurrentState = _idle;
        }

        /*
         * If player is in NPC chase range, make the npc chase the player directly
         * Else, make the NPC follow the flock
         */
        private void CheckFlockingOrChasing()
        {
            if (CurrentState == _wander || CurrentState == _idle) return;

            if (CurrentState == _flocking &&
                _npcManager.GetDistanceWithPlayer() < NpcGlobalVariables.ChasePlayerRange)
            {
                CurrentState = _chase;
            }

            if (CurrentState == _chase &&
                _npcManager.GetDistanceWithPlayer() >
                NpcGlobalVariables.ChasePlayerRange + NpcGlobalVariables.ChaseFlockBuffer)
            {
                CurrentState = _flocking;
            }
        }

        private void AddToFlock()
        {
            if (CurrentState == _flocking && !_flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.AddNpcToFlock(_npcManager);
            }
            else if (CurrentState != _flocking && _flockManager.IsNpcInFlock(_npcManager))
            {
                _flockManager.RemoveNpcFromFlock(_npcManager);
            }
        }
    }
}
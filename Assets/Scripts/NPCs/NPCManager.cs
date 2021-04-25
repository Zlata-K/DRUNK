using System.Collections.Generic;
using NPCs.Decision_Tree;
using NPCs.StateMachine;
using NPCs.StateMachine.States;
using Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPCs
{
    public class NPCManager : MonoBehaviour
    {
        public NpcData _npcData;
        private Rigidbody _playerRigidbody;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private AudioSource _audioSource;
        private Collider _agentCollider;
        private int _punchLayerIndex;

        //For some reason, some models are faster than others
        [SerializeField] private float ModelSpeedMultiplier;
        [SerializeField] private AudioClip[] bumpingSounds;

        public Rigidbody PlayerRigidbody => _playerRigidbody;
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;
        public AudioSource AudioSource => _audioSource;
        public int PunchLayerIndex => _punchLayerIndex;
        public Collider AgentCollider => _agentCollider;

        private void Start()
        {
            _playerRigidbody = Indestructibles.Player.GetComponent<Rigidbody>();
        }

        void Awake()
        {
            _npcData = new NpcData();
            _agentCollider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            _npcData.StateMachine = GetComponent<NPCStateMachine>();
            _punchLayerIndex = _animator.GetLayerIndex("Punch Layer");
            _npcData.PreviousLocation = transform.position;
            Invoke("GotStuck", 0.5f);
        }

        private void Update()
        {
            var stateInfo = Animator.GetCurrentAnimatorStateInfo(PunchLayerIndex);

            // To let the punching animation finish 
            if (!_npcData.Punching && stateInfo.normalizedTime % 1 < 0.1f)
            {
                Animator.SetLayerWeight(PunchLayerIndex, 0.0f);
            }

            if (Indestructibles.PlayerData.IsKnockedOut)
            {
                _npcData.StateMachine.StartWandering();
            }
            
            NpcDecisionTree.NpcDetermineAction(transform.position, _npcData);
            _npcData.StateMachine.CurrentState.Move();
        }

        public void SetAnimatorVelocity(Vector3 velocity)
        {
            Animator.SetFloat(NpcGlobalVariables.VelocityXHash, velocity.x);
            Animator.SetFloat(NpcGlobalVariables.VelocityZHash, velocity.z);
        }

        public float LookWhereYouAreGoing(Vector3 direction)
        {
            Quaternion goalRotation = Quaternion.LookRotation(direction);
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.RotateTowards(currentRotation, goalRotation,
                NpcGlobalVariables.MaxAngleChange * Time.deltaTime);
            return Quaternion.Angle(currentRotation, goalRotation);
        }

        public float GetModelSpeed(float normalSpeed)
        {
            return normalSpeed * ModelSpeedMultiplier;
        }

        public float GetDistanceWithPlayer()
        {
            return Vector3.Distance(Indestructibles.Player.transform.position, transform.position);
        }

        public bool IsChasing()
        {
            var state = _npcData.StateMachine != null ? _npcData.StateMachine.CurrentState : null;

            return state != null && state.GetType() == typeof(ChaseState);
        }

        public void StartPunching()
        {
            if (_npcData.CanPunch && !_npcData.Punching)
            {
                _npcData.Punching = true;
                Animator.Play("Punch", PunchLayerIndex, 0f);
                Animator.SetLayerWeight(PunchLayerIndex, 1.0f);
            }
        }

        public void StopPunching()
        {
            _npcData.Punching = false;
        }

        public void OnPlayerHit()
        {
            _npcData.StateMachine.StartWandering();
            StopPunching();

            _npcData.CanChase = false;
            Invoke(nameof(ChaseCooldown), 3.0f);
        }

        private void ChaseCooldown()
        {
            _npcData.CanChase = true;
        }

        private void PunchCooldown()
        {
            _npcData.CanChase = true;
        }

        /*
     * If the player collides with a NPC that is not chasing him, start chase.
     * For now, the collider is of type trigger because there is no navmesh.
     */
        void OnTriggerEnter(Collider collision)
        {
            if (_npcData.CanChase && !IsChasing() && collision.gameObject.CompareTag($"Player"))
            {
                if (bumpingSounds.Length > 0)
                {
                    AudioSource.PlayOneShot(bumpingSounds[Random.Range(0, bumpingSounds.Length)]);
                }

                Animator.SetTrigger(Animator.StringToHash($"Get Hit"));
                _npcData.LookingForPlayer = true;

                _npcData.CanPunch = false;
                Invoke(nameof(PunchCooldown), 1.0f);
            }
        }

        public List<Transform> GetNearbyObjects(float neighborRadius)
        {
            List<Transform> context = new List<Transform>();
            Collider[] contextColliders = Physics.OverlapSphere(transform.position, neighborRadius);

            foreach (Collider contextCollider in contextColliders)
            {
                if (contextCollider != AgentCollider)
                {
                    context.Add(contextCollider.transform);
                }
            }

            return context;
        }

        public void StartChasing()
        {
            _npcData.StateMachine.StartChasing();

            _npcData.CanPunch = false;
            Invoke(nameof(PunchCooldown), 1.0f);
        }

        private void GotStuck()
        {
            if (_npcData.StateMachine.CurrentState.GetType() == typeof(WanderState))
                return;

            if (Vector3.Distance(_npcData.PreviousLocation, transform.position) < 0.2f)
            {
                _npcData.Stuck = true;
            }
            else
            {
                _npcData.Stuck = false;
            }
        }
            
        public void SpawnChasing()
        {
            //I know people don't like Invoke but we need this to run 1 frame after start
            Invoke("StartChasing", 0.1f);
        }
    }
}


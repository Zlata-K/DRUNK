using UnityEngine;


public class NPCStateMachine: MonoBehaviour
{
    private State _currentState;

    private State _wander;
    private State _idle;
    private State _chase;
    
    private GameObject _player;
    private NPCManager _npcManager;

    [SerializeField] private AudioClip[] bumpingSounds;

    void Start()
    {
        _wander = new Wander();
        _chase = new Chase();
        _idle = new Idle();

        _currentState = _wander;
        
        _player = GameObject.FindWithTag("Player");
        _npcManager = GetComponent<NPCManager>();
    }

    private void Update()
    {
        _currentState.Move(_player, gameObject);
        
        float distanceFromPlayer = Vector3.Distance(_player.transform.position, transform.position);
        CheckPlayerOutOfRange(distanceFromPlayer);
        CheckPlayerBackInRange(distanceFromPlayer);
    }

    //----- State change functions -----
    
    
    /*
     * If the player collides with a NPC that is not chasing him, start chase.
     */
    public void CollidedWithPlayer()
    {
        if (_currentState != _chase)
        {
            GetComponent<AudioSource>().PlayOneShot(bumpingSounds[0]);
            GetComponent<Animator>().SetTrigger(Animator.StringToHash("Get Hit"));
            _currentState = _chase;
        }
    }
    
    /*
     * If the player get out of range during chase, start wandering.
     */
    private void CheckPlayerOutOfRange(float distanceFromPlayer)
    {
        if ( _currentState == _chase &&
             distanceFromPlayer > NPCsGlobalVariables.MaxChaseDistance)
        {
            _currentState = _wander;
            _npcManager.LookingForPlayer = true;
        }
    }
    
    /*
     * If the player is back in range and was previously chased by the NPC, chase again.
     */
    private void CheckPlayerBackInRange(float distanceFromPlayer)
    {
        if (_currentState == _wander &&
                 _npcManager.LookingForPlayer &&
                 distanceFromPlayer < NPCsGlobalVariables.MaxChaseDistance)
        {
            _currentState = _chase;
        }
    }
}

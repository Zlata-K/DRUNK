using UnityEngine;


public class NPCStateMachine: MonoBehaviour
{
    private State _currentState;

    private State _wander;
    private State _idle;
    private State _chase;
    
    private NPCManager _npcManager;

    [SerializeField] private AudioClip[] bumpingSounds;

    void Start()
    {
        _npcManager = GetComponent<NPCManager>();
        
        _wander = new Wander(_npcManager);
        _chase = new Chase(_npcManager);
        _idle = new Idle(_npcManager);

        _currentState = _wander;
    }

    private void Update()
    {
        _currentState.Move();
        
        float distanceFromPlayer = Vector3.Distance( Indestructibles.Player.transform.position, transform.position);
        CheckPlayerOutOfRange(distanceFromPlayer);
        CheckPlayerBackInRange(distanceFromPlayer);
    }

    //----- State change functions -----
    
    
    /*
     * If the player collides with a NPC that is not chasing him, start chase.
     * For now, the collider is of type trigger because there is no navmesh.
     */
    public void OnTriggerEnter(Collider collider)
    {
        if (_currentState != _chase && collider.gameObject.CompareTag("Player"))
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

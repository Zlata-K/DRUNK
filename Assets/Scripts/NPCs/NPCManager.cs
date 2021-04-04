using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private Rigidbody _playerRigidbody;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    
    //For some reason, some models are faster than others
    [SerializeField] private float ModelSpeedMultiplier;

    private bool _lookingForPlayer = false;

    public Rigidbody PlayerRigidbody => _playerRigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    public Animator Animator => _animator;
    public AudioSource AudioSource => _audioSource;

    public bool LookingForPlayer
    {
        get => _lookingForPlayer;
        set => _lookingForPlayer = value;
    }

    void Start()
    {
        _playerRigidbody = Indestructibles.Player.GetComponent<Rigidbody>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetAnimatorVelocity(Vector3 velocity)
    {
        Animator.SetFloat(NPCsGlobalVariables.VelocityXHash, velocity.x);
        Animator.SetFloat(NPCsGlobalVariables.VelocityZHash, velocity.z);
    }
    
    public float LookWhereYouAreGoing(Vector3 direction)
    {
        Quaternion goalRotation = Quaternion.LookRotation(direction);
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(currentRotation, goalRotation, NPCsGlobalVariables.MaxAngleChange * Time.deltaTime);
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
}

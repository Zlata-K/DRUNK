using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private Rigidbody _playerRigidbody;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private bool _lookingForPlayer = false;
    
    public Rigidbody PlayerRigidbody => _playerRigidbody;
    public Rigidbody Rigidbody => _rigidbody;
    public Animator Animator => _animator;

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
    }
}

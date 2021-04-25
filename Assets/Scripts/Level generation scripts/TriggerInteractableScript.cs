using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TriggerInteractableScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _mapTime = 2;
    private static readonly int Activated = Animator.StringToHash("Activated");
    [SerializeField] private float cooldown;
    [SerializeField] private AudioClip barrelFall;
    
    private TMP_Text _indicator;
    
    private float _timer = 0;
    private bool _triggered = false;
    private bool _mapTriggered = false;
    private AudioSource _audioSource;

    private void Awake()
    {
        _indicator = GameObject.Find("InteractIndicator").GetComponent<TMP_Text>();
        _indicator.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player"))
        {
            if (_triggered)
            {
                _animator.SetBool(Activated, true);
                _timer = 0.0f;
                PlayBarrelFallSound();
                StartCoroutine(RefreshCluster(_mapTime));
            }
            else
            {
                _indicator.enabled = false;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player"))
        {
            if (Input.GetButtonDown("Jump"))
            {
                _triggered = true;
                _indicator.enabled = false;
            }
            else
            {
                _indicator.enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player") && Input.GetButtonDown("Jump"))
        {
            _triggered = true;
            _indicator.enabled = false;
        }
    }

    private void Update()
    {
        if (_animator.GetBool(Activated))
        {
            if (_timer >= cooldown)
            {
                _animator.SetBool(Activated, false);
                _triggered = false;
                StartCoroutine(RefreshCluster(0.1f));
            }

            _timer += Time.deltaTime;
        }
    }
    
    private IEnumerator RefreshCluster(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        NavigationGraph.ReGenerateClusterLinks(transform.position);
    }

    private void PlayBarrelFallSound()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(barrelFall);
    }
}

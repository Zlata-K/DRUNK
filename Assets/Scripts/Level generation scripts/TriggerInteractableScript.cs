using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractableScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _mapTime = 2;
    private static readonly int Activated = Animator.StringToHash("Activated");
    [SerializeField] private float cooldown;
    private float _timer = 0;
    
    private bool _triggered = false;
    private bool _mapTriggered = false;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player") && _triggered)
        {
            _animator.SetBool(Activated, true);
            _timer = 0.0f;
            StartCoroutine(RefreshCluster(_mapTime));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player") && Input.GetButtonDown("Jump"))
        {
            _triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.ToLower().Contains("player") && Input.GetButtonDown("Jump"))
        {
            _triggered = true;
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
}

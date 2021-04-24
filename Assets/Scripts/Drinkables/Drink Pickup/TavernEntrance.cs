using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEntrance : MonoBehaviour
{
    [SerializeField] private float tavernCoolDown = 30;

    private bool tavernOpen;
    private float cooldown;
    private GameObject lights;
    
    private void Start()
    {
        tavernOpen = true;
        cooldown = 0;
        lights = transform.Find("Illumination").gameObject;
    }

    private void Update()
    {
        if (!tavernOpen)
        {
            cooldown += Time.deltaTime;
            lights.SetActive(false);
            if (cooldown > tavernCoolDown)
            {
                tavernOpen = true;
                cooldown = 0;
                lights.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tavernOpen && other.CompareTag("Player"))
        {
            tavernOpen = false;
            GameObject.Find("DrinkChoiceCanvas").GetComponent<DrinkChoice>().EnterTheBar(transform.position);
        }
    }
}

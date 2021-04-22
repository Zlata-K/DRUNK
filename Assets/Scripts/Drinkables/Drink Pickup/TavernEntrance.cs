using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEntrance : MonoBehaviour
{
    [SerializeField] private float tavernCoolDown = 30;

    private bool tavernOpen;
    private float cooldown;

    private void Start()
    {
        tavernOpen = true;
        cooldown = 0;
    }

    private void Update()
    {
        if (!tavernOpen)
        {
            cooldown += Time.deltaTime;
            this.transform.Find("Illumination").gameObject.SetActive(false);
            if (cooldown > tavernCoolDown)
            {
                tavernOpen = true;
                cooldown = 0;
                this.transform.Find("Illumination").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tavernOpen && other.CompareTag("Player"))
        {
            tavernOpen = false;
            GameObject.Find("DrinkChoiceCanvas").GetComponent<DrinkChoice>().EnterTheBar();
        }
    }
}

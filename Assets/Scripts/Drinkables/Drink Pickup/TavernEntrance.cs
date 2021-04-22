using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("DrinkChoiceCanvas").GetComponent<DrinkChoice>().EnterTheBar();
        }
    }
}

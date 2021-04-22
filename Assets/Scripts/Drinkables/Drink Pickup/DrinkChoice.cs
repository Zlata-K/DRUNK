using System;
using System.Collections;
using System.Collections.Generic;
using Drinkables;
using UnityEngine;

public class DrinkChoice : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject buffaloDrink;
    [SerializeField] private GameObject clearlyDrink;
    [SerializeField] private GameObject flippedDrink;
    [SerializeField] private float timeLimit;
    //[SerializeField] private GameObject water;
    
    public bool isActive;
    private float timeCounter = 0;
    
    void Update()
    {
        if (isActive)
        {
            Time.timeScale = 0;
            menuPanel.SetActive(isActive);
            timeCounter += Time.unscaledDeltaTime;

            if (timeCounter > timeLimit)
            {
                isActive = false;
                menuPanel.SetActive(isActive);
                Time.timeScale = 1;
                timeCounter = 0;
            }
        }
    }

    public void ConsumeBuffalo()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        Time.timeScale = 1;
        Instantiate(buffaloDrink);
    }

    public void ConsumeClearlyThere()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        Time.timeScale = 1;
        Instantiate(clearlyDrink);
    }

    public void ConsumeUpsideDown()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        Time.timeScale = 1;
        Instantiate(flippedDrink);
    }
    
}

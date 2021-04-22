using System;
using System.Collections;
using System.Collections.Generic;
using Drinkables;
using UnityEngine;
using Random = UnityEngine.Random;


//using Random = System.Random;

public class DrinkChoice : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject buffaloDrink;
    [SerializeField] private GameObject clearlyDrink;
    [SerializeField] private GameObject flippedDrink;
    [SerializeField] private GameObject waterDrink;
    [SerializeField] private GameObject beerDrink;
    [SerializeField] private float timeLimit;
    [SerializeField] private GameObject buffaloButton;
    [SerializeField] private GameObject clearlyButton;
    [SerializeField] private GameObject flippedButton;
    
    private int randomDrink;
    public bool isActive;
    private float timeCounter = 0;
    private bool randomIsChosen;
    
    void Update()
    {
        if (isActive)
        {
            Time.timeScale = 0;
            menuPanel.SetActive(isActive);
            GenerateRandomDrink();
            timeCounter += Time.unscaledDeltaTime;

            if (timeCounter > timeLimit)
            {
                isActive = false;
                menuPanel.SetActive(isActive);
                clearlyButton.SetActive(false);
                buffaloButton.SetActive(false);
                flippedButton.SetActive(false);
                Time.timeScale = 1;
                timeCounter = 0;
                randomIsChosen = false;
            }
        }
    }

    private void GenerateRandomDrink()
    {
        if (!randomIsChosen)
        {
            randomDrink = Random.Range(0, 3);
            if (randomDrink == 0)
            {
                buffaloButton.SetActive(true);
            }
            else if (randomDrink == 1)
            {
                clearlyButton.SetActive(true);
            }
            else
            {
                flippedButton.SetActive(true);
            }

            randomIsChosen = true;
        }
    }

    public void ConsumeBuffalo()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        Time.timeScale = 1;
        Instantiate(buffaloDrink);
        randomIsChosen = false;
    }

    public void ConsumeClearlyThere()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        Time.timeScale = 1;
        Instantiate(clearlyDrink);
        randomIsChosen = false;
    }

    public void ConsumeUpsideDown()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        Time.timeScale = 1;
        Instantiate(flippedDrink);
        randomIsChosen = false;
    }

    public void ConsumeWater()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        Time.timeScale = 1;
        Instantiate(waterDrink);
        randomIsChosen = false;
    }

    public void ConsumeBeer()
    {
        isActive = false;
        menuPanel.SetActive(isActive);
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        Time.timeScale = 1;
        Instantiate(beerDrink);
        randomIsChosen = false;
    }
    
}

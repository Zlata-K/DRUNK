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

    private float timeCounter = 0;

    void Update()
    {
        if (menuPanel.activeSelf)
        {
            timeCounter += Time.unscaledDeltaTime;

            if (timeCounter > timeLimit)
            {
                LeaveBar();
            }
        }
    }

    private void GenerateRandomDrink()
    {
        var randomDrink = Random.Range(0, 3);
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
    }

    public void ConsumeBuffalo()
    {
        LeaveBar();
        Instantiate(buffaloDrink);
    }

    public void ConsumeClearlyThere()
    {
        LeaveBar();
        Instantiate(clearlyDrink);
    }

    public void ConsumeUpsideDown()
    {
        LeaveBar();
        Instantiate(flippedDrink);
    }

    public void ConsumeWater()
    {
        LeaveBar();
        Instantiate(waterDrink);
    }

    public void ConsumeBeer()
    {
        LeaveBar();
        Instantiate(beerDrink);
    }

    public void EnterTheBar()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        timeCounter = 0;
        menuPanel.SetActive(true);
        GenerateRandomDrink();
    }

    public void LeaveBar()
    {
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Drinkables;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public bool wasActive;
    private GameObject countdownPanel;
    private Vector3 LastEntrencePos;

    private void Start()
    {
        countdownPanel = transform.Find("Menu").Find("Countdown").gameObject;
    }

    private void Update()
    {
        if (GameObject.Find("Canvas").transform.Find("MainPanel").gameObject.activeSelf && menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
            wasActive = true;
        }
        else if (menuPanel.activeSelf)
        {
            wasActive = false;
            timeCounter += Time.unscaledDeltaTime;
            countdownPanel.GetComponent<Text>().text = Math.Round(timeLimit - timeCounter).ToString();

            if (timeCounter > timeLimit)
            {
                countdownPanel.GetComponent<Text>().text = "";
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

    public void EnterTheBar(Vector3 pos)
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        timeCounter = 0;
        menuPanel.SetActive(true);
        GenerateRandomDrink();
        pos.y = 0; // Ensure the NPC is on the ground
        LastEntrencePos = pos;
    }

    public void LeaveBar()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        
        clearlyButton.SetActive(false);
        buffaloButton.SetActive(false);
        flippedButton.SetActive(false);
        menuPanel.SetActive(false);
        Time.timeScale = 1;
        // Spawn NPCs when player leave the bar (even if they don't order anything, the bartender will be pissed the player didn't order !)
        Invoke(nameof(SpawnNPCs), 0.5F);
    }

    public void SpawnNPCs()
    {
        for (int i = 0; i < 2; i++) { // Spawn 2 npcs, could be tweaked based on difficulty or whatever
            var obj = TerrainGeneratorScript.SpawnNPC(LastEntrencePos);

            obj.GetComponent<NPCManager>().StartChasing();
        }
    }
}
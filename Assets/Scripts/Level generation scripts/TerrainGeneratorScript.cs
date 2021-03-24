﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGeneratorScript : MonoBehaviour
{
    private readonly float _tileWidth = 10.0f;
    
    [SerializeField] private int blocks = 2; //number of blocks being made, block x block
    [SerializeField] private int size = 5;
    [SerializeField] private float tavernPercent = 0.25f;
    
    private RoadGenerationScript _roadScript;
    private CrossGenerationScript _crossRoadScript;
    private TavernGenerationScript _tavernScript;
    private FillerGenerationScript _fillerScript;

    void OnValidate()
    {
        blocks = Mathf.Max(blocks, 1);
        size = Mathf.Max(size, 3);
        tavernPercent = Mathf.Min(tavernPercent, 1);
        tavernPercent = Mathf.Max(tavernPercent, 0);
    }

    private void Awake()
    {
        //Initialize tile scripts
        _roadScript = GetComponent<RoadGenerationScript>();
        _roadScript.Size = size;
        _roadScript.Width = _tileWidth;
        _roadScript.Blocks = blocks;
        
        _crossRoadScript = GetComponent<CrossGenerationScript>();
        _crossRoadScript.Size = size;
        _crossRoadScript.Width = _tileWidth;
        _crossRoadScript.Blocks = blocks;
        
        _tavernScript = GetComponent<TavernGenerationScript>();
        _tavernScript.Size = size;
        _tavernScript.Width = _tileWidth;
        _tavernScript.Blocks = blocks;
        
        _fillerScript = GetComponent<FillerGenerationScript>();
        _fillerScript.Size = size;
        _fillerScript.Width = _tileWidth;
        _fillerScript.Blocks = blocks;
    }

    // Start is called before the first frame update
    void Start()
    {
        //example we want 1 block to be 4x4 and there to be 2x2 number of blocks
        //n = 4
        //b = 2
        //(n-1)*b+1
        //we would need 7 by 7 tiles to generate this level
        var temp = (size - 1) * blocks + 1;
        for (int i = 0; i < temp; i++)
        {
            for (int j = 0; j < temp; j++)
            {
                if (i % (size - 1) == 0 && j % (size - 1) == 0)
                {
                    _crossRoadScript.Generate(i, j);
                }
                else if (i % (size - 1) == 0 || j % (size - 1) == 0)
                {
                    _roadScript.Generate(i, j);
                }

                else if ((i % (size - 1) == 1 || j % (size - 1) == 1 || i % (size - 1) == size - 2 ||
                          j % (size - 1) == size - 2) && Random.value > (1-tavernPercent))
                {
                    _tavernScript.Generate(i, j);
                }
                else
                {
                    _fillerScript.Generate(i, j);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileGenerationScript : MonoBehaviour
{
    public int Size { get; set; }
    
    public List<GameObject> tiles;

    public float Width { get; set; }

    public int Blocks { get; set; }

    public abstract GameObject Generate(int i, int j);
}
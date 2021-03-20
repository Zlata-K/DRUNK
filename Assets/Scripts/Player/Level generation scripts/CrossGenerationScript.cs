using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossGenerationScript : TileGenerationScript
{
    public override GameObject Generate(int i, int j)
    {
        //will add logic to check if the crossroad is a map edge or corner

        //set as crossroad

        return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width), Quaternion.identity);
    }
}
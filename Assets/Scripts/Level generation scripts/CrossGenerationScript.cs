using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrossGenerationScript : TileGenerationScript
{
    public override GameObject Generate(int column, int row)
    {
        //set as crossroad
        //TODO::add logic to check if the crossroad is a map edge or corner
        return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width), Quaternion.identity);
    }
}
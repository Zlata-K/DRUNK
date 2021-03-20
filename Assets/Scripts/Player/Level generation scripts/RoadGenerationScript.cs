using System.Collections.Generic;
using UnityEngine;

public class RoadGenerationScript : TileGenerationScript
{

    public override GameObject Generate(int i, int j)
    {
        //will add logic to check if the road is a map edge

        //set as road
        //assuming road points west

        if (i % (Size - 1) == 0)
        {
            return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width), Quaternion.Euler(0, 90, 0));
        }

        return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width), Quaternion.identity);
    }
}
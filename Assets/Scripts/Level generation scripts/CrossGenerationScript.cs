using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CrossGenerationScript : TileGenerationScript
{
    private int? _area = null;

    //set as crossroad
    public override GameObject Generate(int column, int row)
    {
        if (_area == null)
        {
            _area = ((Size - 1) * Blocks + 1) - 1;
        }

        //Bottom left corner
        if (row == 0 && column == 0)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, -90, 0));
        }
        
        //top left corner
        if (row == _area && column == 0)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 0, 0));
        }
        
        //top right corner
        if (row == _area && column == _area)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 90, 0));
        }
        
        //bottom right corner
        if (row == 0 && column == _area)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 180, 0));
        }

        //bottom row
        if (row == 0)
        {
            return Instantiate(tiles[1], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 180, 0));
        }
        
        //top row
        if (row == _area)
        {
            return Instantiate(tiles[1], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 0, 0));
        }

        //left column
        if (column == 0)
        {
            return Instantiate(tiles[1], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, -90, 0));
        }
        
        //right column
        if (column == _area)
        {
            return Instantiate(tiles[1], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 90, 0));
        }
        
        //default - we are not along an edge
        return Instantiate(tiles[2], new Vector3(column * Width, 0, row * Width),
            Quaternion.identity);
    }
}
using UnityEngine;

public class TavernGenerationScript : TileGenerationScript
{
    public override GameObject Generate(int column, int row)
    {
        //we have the tavern tiles.
        if (column % (Size - 1) == 1)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width), 
                Quaternion.identity);
        }
        
        if (row % (Size - 1) == 1)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 270, 0));
        }
        
        if (column % (Size - 1) == Size - 2)
        {
            return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
                Quaternion.Euler(0, 180, 0));
        }

        return Instantiate(tiles[0], new Vector3(column * Width, 0, row * Width),
            Quaternion.Euler(0, 90, 0));
    }
}
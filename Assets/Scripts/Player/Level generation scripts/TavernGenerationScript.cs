using UnityEngine;

public class TavernGenerationScript : TileGenerationScript
{
    public override GameObject Generate(int i, int j)
    {
        //we have the tavern tiles.
        if (i % (Size - 1) == 1)
        {
            return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width), 
                Quaternion.identity);
        }
        
        if (j % (Size - 1) == 1)
        {
            return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width),
                Quaternion.Euler(0, 270, 0));
        }
        
        if (i % (Size - 1) == Size - 2)
        {
            return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width),
                Quaternion.Euler(0, 90, 0));
        }

        return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width),
            Quaternion.Euler(0, 90, 0));
    }
}
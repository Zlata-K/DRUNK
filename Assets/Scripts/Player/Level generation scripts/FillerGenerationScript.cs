using UnityEngine;

public class FillerGenerationScript : TileGenerationScript
{

    public override GameObject Generate(int i, int j)
    {
        return Instantiate(tiles[0], new Vector3(i * Width, 0, j * Width), Quaternion.identity);
    }
}

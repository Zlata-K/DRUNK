using UnityEngine;

public class FillerGenerationScript : TileGenerationScript
{

    public override GameObject Generate(int column, int row)
    {
        //TODO::if fillers have rules required to place add the logic
        return Instantiate(tiles[Random.Range(0, tiles.Count)], new Vector3(column * Width, 0, row * Width), Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorScript : MonoBehaviour
{
    public List<GameObject> roads;
    public List<GameObject> crossroads;
    public List<GameObject> fillers;
    public List<GameObject> taverns;

    private float tileWidth = 10.0f;

    public int blocks = 2; //number of blocks being made, block x block
    private int size = 5;
    private string[,] level; //for logic [y-axis, x-axis]

    // Start is called before the first frame update
    void Start()
    {
        //example we want 1 block to be 4x4 and there to be 2x2 number of blocks
        //n = 4
        //b = 2
        //(n-1)*b+1
        //we would need 7 by 7 tiles to generate this level
        var temp = (size - 1) * blocks + 1;
        level = new string[temp, temp];
        for (int i = 0; i < temp; i++)
        {
            for (int j = 0; j < temp; j++)
            {
                if (i % (size - 1) == 0 && j % (size - 1) == 0)
                {
                    //set as crossroad
                    level[i, j] = "C";
                    Instantiate(crossroads[0], new Vector3(i * tileWidth, 0, j * tileWidth), Quaternion.identity);
                }
                else if (i % (size - 1) == 0 || j % (size - 1) == 0)
                {
                    //set as road
                    level[i, j] = "R";
                    Instantiate(roads[0], new Vector3(i * tileWidth, 0, j * tileWidth), Quaternion.identity);
                }
                else if ((i % (size - 1) == 1 || j % (size - 1) == 1 || i % (size - 1) == size - 2 ||
                          j % (size - 1) == size - 2) && Random.value > 0.75)
                {
                    //we have the block tiles.
                    level[i, j] = "T";
                    Instantiate(taverns[0], new Vector3(i * tileWidth, 0, j * tileWidth), Quaternion.identity);
                }
                else
                {
                    level[i, j] = "F";
                    Instantiate(fillers[0], new Vector3(i * tileWidth, 0, j * tileWidth), Quaternion.identity);

                }
            }
        }

        Debug.Log(level);
    }
}
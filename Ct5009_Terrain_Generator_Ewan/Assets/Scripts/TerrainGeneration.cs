using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//References
//
public class TerrainGeneration : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGen PerlinNoiseGen;

    public GameObject Block;
    public GameObject Parent;

    public GameObject[,] CurrentTerrain;
    public GameObject[,] NewTerrain;

    public void GenerateTerrain(float[,] Map)
    {
        //Create a grid of gameobjects that are manipulated to represent terrain
        CurrentTerrain = new GameObject[PerlinNoiseGen.NewWidth+1, PerlinNoiseGen.NewHeight+1];
        //Use the Perlin Noise Value stored in the MapArray to generate a height map based on the values
        for (int x = 0; x <= PerlinNoiseGen.NewWidth; x++)
        {
            for (int y = 0; y <= PerlinNoiseGen.NewHeight; y++)
            {
                //Instantiate a prefab on each point of the grid
                CurrentTerrain[x,y] = Instantiate(Block, new Vector3(x-(PerlinNoiseGen.NewWidth / 2),0,y - (PerlinNoiseGen.NewHeight / 2)),new Quaternion(0,0,0,0), Parent.transform);
                
            }
        }

    }

    public void UpdateTerrain(float Width, float Height, float[,] Elevation)
    {
        //Move the already generated grid of gameObjects according to the values of the Perlin Noise
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CurrentTerrain[x, y].transform.position = new Vector3(x - (Width / 2)+Parent.transform.position.x, Elevation[x,y]*10 + Parent.transform.position.y, y - (Height / 2) + Parent.transform.position.z);
            }
        }
    }
    public void ClearTerrain()
    {
        //Remove every Game Object
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Destroy(Parent.transform.GetChild(i).gameObject);
        }
    }
}

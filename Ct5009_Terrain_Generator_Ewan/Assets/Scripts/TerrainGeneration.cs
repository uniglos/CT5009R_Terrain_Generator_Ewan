using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGen PerlinNoiseGen;
    public GameObject Block;
    public GameObject Parent;
    public List<GameObject> Nodes;
    public GameObject[,] CurrentTerrain;
    public GameObject[,] NewTerrain;
    private bool Renew = false;
    private bool DoneOnce = false;

    private float PositionOffset = 110.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        //Create a grid of gameobjects that are manipulated to represent terrain
       // GenerateTerrain(PerlinNoiseGen.NewWidth, PerlinNoiseGen.NewHeight);
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    public void GenerateTerrain(float[,] Map)
    {
        CurrentTerrain = new GameObject[PerlinNoiseGen.NewWidth+1, PerlinNoiseGen.NewHeight+1];
        //Use the Perlin Noise Value stored in the MapArray to generate a height map based on the values
        for (int x = 0; x <= PerlinNoiseGen.NewWidth; x++)
        {
            for (int y = 0; y <= PerlinNoiseGen.NewHeight; y++)
            {
                CurrentTerrain[x,y] = Instantiate(Block, new Vector3(x-(PerlinNoiseGen.NewWidth / 2),0,y - (PerlinNoiseGen.NewHeight / 2)),new Quaternion(0,0,0,0), Parent.transform);
                
            }
        }

    }

    public void UpdateTerrain(float Width, float Height, float[,] Elevation)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CurrentTerrain[x, y].transform.position = new Vector3(x - (Width / 2)-PositionOffset, Elevation[x,y]*10, y - (Height / 2));

            }
        }
    }
    public void ClearTerrain()
    {
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Destroy(Parent.transform.GetChild(i).gameObject);
        }
    }
}

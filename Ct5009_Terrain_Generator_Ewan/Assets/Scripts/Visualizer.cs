using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGen PerlinNoiseGen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Renderer MapRenderer = GetComponent<Renderer>();
        MapRenderer.material.mainTexture = Visualize(PerlinNoiseGen.MapArray);
    }

    Texture2D Visualize(float[,] Map)
    {
        //Make a texture for the perlin noise to be displayed on
        Texture2D MapTexture = new Texture2D(Map.GetLength(0),Map.GetLength(1));
        for (int x = 0; x < Map.GetLength(0); x++)
        {
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                //Visualize Perlin Noise through colour on the plane
                MapTexture.SetPixel(x, y, new Color(Map[x, y], Map[x, y], Map[x, y]));
            }
        }
        MapTexture.filterMode = FilterMode.Point;
        MapTexture.Apply();
        return MapTexture;
    }
}

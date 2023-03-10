using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//References
//BearTheCoder - Perlin Noise and Unity - https://www.youtube.com/watch?v=BO7x58NwGaU
public class Visualizer : MonoBehaviour
{
    //Set the script we are getting our noiseMap from
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
        //Render the noise map using the 2D array from Perlin Noise Generator
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

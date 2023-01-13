using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseGen : MonoBehaviour
{
    private int CurrentWidth = 100;
    [SerializeField]
    public int NewWidth;
    
    private int CurrentHeight = 100;
    [SerializeField]
    public int NewHeight;
    

    public float Scale;
    [Range(0, 5)]
    public int Octaves;
    public bool NormalizeOctave = true;
    [Range(0, 1.5f)]
    public float Persistance;
    [Range(0, 5)]
    public float Lacunarity;
    [SerializeField]
    [Range(-0.1f, 0.1f)]
    private float YOffsetRatio = 0.01f;
    private float YOffset;
    [SerializeField]
    [Range(-0.1f,0.1f)]
    private float XOffsetRatio = 0.01f;
    private float XOffset;

    public float[,] MapArray;
    // Start is called before the first frame update
    void Start()
    {
        NewHeight = CurrentHeight;
        NewWidth = CurrentWidth;
        MapArray = new float[CurrentWidth+1, CurrentHeight+1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Increasing offset to make the noise map scroll
        XOffset += XOffsetRatio;
        YOffset += YOffsetRatio;
    }

    public void  P_Noise_Generator()
    {
        //Make sure the height and width of the Noise Map doeesn't go below zero
        if(NewHeight <= 0)
        {
            NewHeight = 1;
        }
        if (NewWidth <= 0)
        {
            NewWidth = 1;
        }
        //Be able to change the height/width of the Map in Runtime
        if (CurrentHeight != NewHeight || CurrentWidth != NewWidth)
        {
            ChangeSize();
        }

        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;
        for (int x = 0; x <= CurrentWidth; x++)
        {
            for (int y = 0; y <= CurrentHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 1;
                for (int i = 0; i < Octaves; i++)
                {
                    //As we increment through the coordinates of our map
                    //Generate a colour between white and black using perlin noise
                    float XCoord = ((((float)x / CurrentWidth) * (Scale / 2)) * frequency) + XOffset;
                    float YCoord = ((((float)y / CurrentHeight) * (Scale / 2)) * frequency) + YOffset;
                    float PerlinNoise = Mathf.PerlinNoise(XCoord, YCoord) * 2 - 1;
                    noiseHeight += PerlinNoise * amplitude;
                    

                    amplitude *= Persistance;
                    frequency *= Lacunarity;

                    if (noiseHeight > maxNoise)
                    {
                        maxNoise = noiseHeight;
                    }
                    else if (noiseHeight < minNoise)
                    {
                        minNoise = noiseHeight;
                    }
                    MapArray[x, y] = noiseHeight;
                }

                //Old terrain generation function using Instantiated blocks
                //TerrainGeneration.GenerateTerrain(x,y,PerlinNoise);

            }
        }
        for (int x = 0; x <= CurrentWidth; x++)
        {
            for (int y = 0; y <= CurrentHeight; y++)
            {
                if (NormalizeOctave)
                {
                    MapArray[x, y] = Mathf.InverseLerp(minNoise, maxNoise, MapArray[x, y]);
                }
                
            }
        }
    }
    

    public void ChangeSize()
    {
        CurrentHeight = NewHeight;
        CurrentWidth = NewWidth;
        MapArray = new float[CurrentWidth + 1, CurrentHeight + 1];
    }
}

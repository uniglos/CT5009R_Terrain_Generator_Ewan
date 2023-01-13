using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField]
    PerlinNoiseGen PerlinNoiseGen;
    [SerializeField]
    TerrainGeneration Terrain;
    float[,] MapArray; 
    Mesh mesh;
    MeshCollider collider;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    bool DoneOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        collider = GetComponent<MeshCollider>();
        MapArray = PerlinNoiseGen.MapArray;
        Debug.Log(MapArray);
        //PerlinNoiseGen.P_Noise_Generator();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PerlinNoiseGen.P_Noise_Generator();
        CreateShape(PerlinNoiseGen.MapArray);
        if (!DoneOnce)
        {
            Terrain.GenerateTerrain(PerlinNoiseGen.MapArray);
            DoneOnce = true;
        }
        if (Terrain.Parent.transform.childCount != PerlinNoiseGen.MapArray.Length)
        {
            Debug.Log("Not enough blocks");
            Terrain.ClearTerrain();
            Terrain.GenerateTerrain(PerlinNoiseGen.MapArray);

        }
        //Pass in our generated perlin noise array to terrain generator
        Terrain.UpdateTerrain(PerlinNoiseGen.NewWidth+1, PerlinNoiseGen.NewHeight+1, PerlinNoiseGen.MapArray);
        
        UpdateMesh();
    }

    void CreateShape(float[,] MapArray)
    {
        //https://youtu.be/64NblGkAabk?t=258
        {
            vertices = new Vector3[(PerlinNoiseGen.NewWidth + 1) * (PerlinNoiseGen.NewHeight + 1)];
            colors = new Color[(PerlinNoiseGen.NewWidth + 1) * (PerlinNoiseGen.NewHeight + 1)];
            for (int i = 0, y = 0; y <= PerlinNoiseGen.NewHeight; y++)
            {
                for (int x = 0; x <= PerlinNoiseGen.NewWidth; x++)
                {
                    vertices[i] = new Vector3(x, PerlinNoiseGen.MapArray[x, y] * 10, y);
                    colors[i] = new Color(PerlinNoiseGen.MapArray[x, y], PerlinNoiseGen.MapArray[x, y], PerlinNoiseGen.MapArray[x, y]);
                    i++;

                }
            }
            int vert = 0;
            int tris = 0;
            triangles = new int[PerlinNoiseGen.NewWidth * PerlinNoiseGen.NewHeight * 6];
            for (int y = 0; y < PerlinNoiseGen.NewHeight; y++)
            {
                for (int x = 0; x < PerlinNoiseGen.NewWidth; x++)
                {

                    triangles[tris] = vert + 0;
                    triangles[tris + 1] = vert + (PerlinNoiseGen.NewWidth + 1);
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + (PerlinNoiseGen.NewWidth + 1);
                    triangles[tris + 5] = vert + (PerlinNoiseGen.NewWidth + 2);

                    vert++;
                    tris += 6;
                }
                vert++;

            } 
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;


        mesh.RecalculateNormals();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//References
//Brackeys - Procedural Terrain in Unity - https://youtu.be/64NblGkAabk?t=258
//Brackeys - Mesh Generation in Unity - https://youtu.be/eJEpeUH1EMg
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
    }

    // Update is called once per frame
    void Update()
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
            //When the grid changes size, clear the grid and remake it to the correct size
            Terrain.ClearTerrain();
            Terrain.GenerateTerrain(PerlinNoiseGen.MapArray);

        }
        //Pass in our generated perlin noise array to terrain generator
        Terrain.UpdateTerrain(PerlinNoiseGen.NewWidth+1, PerlinNoiseGen.NewHeight+1, PerlinNoiseGen.MapArray);
        
        UpdateMesh();
    }

    void CreateShape(float[,] MapArray)
    {
        {
            //Create a 2D array of vertices
            if (vertices == null || vertices.Length != (PerlinNoiseGen.NewWidth + 1) * (PerlinNoiseGen.NewHeight + 1))
            {
                vertices = new Vector3[(PerlinNoiseGen.NewWidth + 1) * (PerlinNoiseGen.NewHeight + 1)];
            }
            for (int i = 0, y = 0; y <= PerlinNoiseGen.NewHeight; y++)
            {
                for (int x = 0; x <= PerlinNoiseGen.NewWidth; x++)
                {
                    //For each vertex, change its Y value to represent the Perlin Noise
                    if (vertices[i] != new Vector3(x, PerlinNoiseGen.MapArray[x, y] * 10, y))
                    {
                        vertices[i] = new Vector3(x, PerlinNoiseGen.MapArray[x, y] * 10, y);
                    }
                    vertices[i].y = PerlinNoiseGen.MapArray[x, y] * 10;
                    i++;

                }
            }
            int vert = 0;
            int tris = 0;
            //Create a new array of Triangles in order to draw the mesh
            if (triangles == null || triangles.Length != PerlinNoiseGen.NewWidth * PerlinNoiseGen.NewHeight * 6)
            {
                triangles = new int[PerlinNoiseGen.NewWidth * PerlinNoiseGen.NewHeight * 6];
            }
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

        //Clear the mesh and apply the vertices and triangles then recalculate the normals 
        //so everything is facing the right way
        mesh.vertices = vertices;
        mesh.triangles = triangles;


        mesh.RecalculateNormals();
    }
}

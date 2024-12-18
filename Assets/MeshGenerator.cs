using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    public Slider slider;

    Vector3[] vertices;
    int[] triangles;

    public float scale = 2;

    public int xSize = 20;
    public int zSize = 20;

    public float strength = 0.3f;

    void Start()
    {
        slider.value = scale;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    public void ButtonGenerate()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    public void SliderStrength()
    {
        scale = slider.value;
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int octaves = 3;
        float amplitude = 1f;
        float frequency = 1f;
        float persistence = 0.5f;
        float lacunarity = 2f;

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = 0f;

                float tempAmplitude = amplitude;
                float tempFrequency = frequency;

                for (int o = 0; o < octaves; o++)
                {
                    y += Mathf.PerlinNoise(x * tempFrequency * strength, z * tempFrequency * strength) * tempAmplitude;

                    tempAmplitude *= persistence;
                    tempFrequency *= lacunarity;
                }

                y *= scale;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }


    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
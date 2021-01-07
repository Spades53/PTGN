using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves]; //This way we can generate multiple different versions.

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        //Sentinel Values 
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x; //Heightmap Values
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // Doma talking BS

                    //This allows us to create valleys/dunes
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //Mathf.PerlinNoise range [0,1] => by " * 2 - 1" we change the range to [-1,+1]
                    noiseHeight += perlinValue * amplitude;


                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;                
            }
        }

        // getting height map values
        string path = @"C:\Users\Marco\Desktop\Visual Computing Data\NoiseData" + seed + ".csv";

        if (!File.Exists(path))
        {
            string[] noiseMapData = new string[noiseMap.Length+1];
            int i = 0;
            noiseMapData[i] = "Seed Number: " + seed.ToString() + ";";
            i++;

            foreach (float data in noiseMap)
            {
                noiseMapData[i] = data.ToString().Replace(",",".");
                i++;
            }

            File.WriteAllLines(path, noiseMapData);
        }


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                //normalizing noiseMap
            }
        }

        
        return noiseMap;
    }
}

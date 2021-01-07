using System.Collections;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float [,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        int vertexIndex = 0;

        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 :  levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;


        MeshData meshData = new MeshData(verticesPerLine,verticesPerLine);

        for (int y = 0; y < height; y+= meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x+= meshSimplificationIncrement)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                int TOP_LEFT = vertexIndex;
                int TOP_RIGHT = vertexIndex + 1;
                int BOTTOM_RIGHT = vertexIndex + verticesPerLine + 1;
                int BOTTOM_LEFT = vertexIndex + verticesPerLine;

                if (x < width - 1 && y < height - 1)//ignoring right and bottom edge of map
                {
                    meshData.AddTriangle(TOP_LEFT, BOTTOM_RIGHT, BOTTOM_LEFT);
                    meshData.AddTriangle(BOTTOM_RIGHT, TOP_LEFT, TOP_RIGHT);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }

}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    int triangleIndex;
    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
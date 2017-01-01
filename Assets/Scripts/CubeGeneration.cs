using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeshFace
{
    Front,
    Back,
    Both
}

public static class CubeGeneration  {

	public static GameObject Generate(Vector3 center, Vector3 size, Material material) {

        GameObject cube = new GameObject("cube");
        GameObject cubeFace = null;
        Vector3 direction = Vector3.zero;

		direction = Vector3.up + Vector3.right + Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.up, Vector3.right, material, GenerateTexture(Color.red));
        cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.up + Vector3.right - Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.up, Vector3.right, material, GenerateTexture(Color.white));
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

		direction = Vector3.up - Vector3.right - Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.up, (-1f) * Vector3.forward, material, GenerateTexture(Color.yellow));
		cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

		direction = Vector3.up + Vector3.right + Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.up, Vector3.forward, material, GenerateTexture(Color.blue));
		cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.down + Vector3.right + Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.right, Vector3.forward, material, GenerateTexture(Color.green));
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.up + Vector3.right + Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), Vector3.right, Vector3.forward, material, GenerateTexture(Color.magenta));
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

        // direction = Vector3.up + Vector3.right
        // cubeFace = GenerateFace(center + (0.5f) * Vector3.Scale(size, direction), direction, GenerateTexture(Color.red));
        // cubeFace.transform.SetParent(cube.transform);

        return cube;
    }

	public static GameObject GenerateFace(Vector3 position, 
										  Vector3 direction1, 
                                          Vector3 direction2, 
                                          Material material, 
                                          Texture texture) 
    {
		MeshBuilder meshBuilder = new MeshBuilder ();

		meshBuilder.Vertices.Add (position);
		meshBuilder.Vertices.Add (position + direction1);
		meshBuilder.Vertices.Add (position + direction2);
		meshBuilder.Vertices.Add (position + direction1 + direction2);
        meshBuilder.AddQuadTriangles(MeshFace.Both, 0, 1, 2, 3);

		Mesh mesh = meshBuilder.CreateMesh ();

		GameObject cubeFace = new GameObject ("cubeFace");
		MeshFilter meshFilter = cubeFace.AddComponent<MeshFilter> ();
		MeshRenderer meshRenderer = cubeFace.AddComponent<MeshRenderer> ();

		meshFilter.mesh = mesh;
		meshRenderer.material = material;
		meshRenderer.material.mainTexture = texture;

		return cubeFace;
    }

    public static void AddQuadTriangles(this MeshBuilder meshBuilder, MeshFace meshFace,
                                        int index0, int index1, int index2, int index3)
    {
        if (meshFace == MeshFace.Front || meshFace == MeshFace.Both) {
            meshBuilder.AddTriangle(index0, index1, index2);
            meshBuilder.AddTriangle(index1, index3, index2);
        }
        if(meshFace == MeshFace.Back || meshFace == MeshFace.Both){
            meshBuilder.AddTriangle(index1, index0, index2);
            meshBuilder.AddTriangle(index1, index2, index3);
        }
    }

    public static Texture GenerateTexture(Color color, int sizeX = 100, int sizeY = 100)
    {
        Texture2D texture = new Texture2D(sizeX, sizeY);

        for (int x = 0; x < sizeX; x++) {
            for (int y = 0; y < sizeY; y++) {
                texture.SetPixel(x, y, color);
            }
        }

		texture.Apply ();

        return texture;
    }
}

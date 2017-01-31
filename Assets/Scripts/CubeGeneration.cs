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

        Vector3 up = Vector3.Scale(size, Vector3.up),
                down = Vector3.Scale(size, Vector3.down),
                left = Vector3.Scale(size, Vector3.left),
                right = Vector3.Scale(size, Vector3.right),
                back = Vector3.Scale(size, Vector3.back),
                forward = Vector3.Scale(size, Vector3.forward);


		direction = Vector3.up + Vector3.right + Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), up, right, material, Color.red);
        cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.up + Vector3.right - Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), up, right, material, Color.white);
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

		direction = Vector3.up - Vector3.right - Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), up, back, material, Color.yellow);
		cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

		direction = Vector3.up + Vector3.right + Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), up, forward, material, Color.blue);
		cubeFace.transform.SetParent(cube.transform);
		cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.down + Vector3.right + Vector3.forward;
        cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), right, forward, material, Color.green);
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

        direction = Vector3.up + Vector3.right + Vector3.forward;
		cubeFace = GenerateFace(center + (-0.5f) * Vector3.Scale(size, direction), right, forward, material, Color.magenta);
        cubeFace.transform.SetParent(cube.transform);
        cubeFace.transform.localPosition = Vector3.zero;

        cube.AddComponent<BoxCollider>().size = size;
        cube.AddComponent<CubeSelection>();

        return cube;
    }

	public static GameObject GenerateFace(Vector3 position, 
										  Vector3 direction1, 
                                          Vector3 direction2, 
                                          Material material, 
                                          Color faceColor) 
    {
		MeshBuilder meshBuilder = new MeshBuilder ();

        float edgeFactor = 0.15f;
        Vector3 middleVector = edgeFactor * (direction1 + direction2);

		meshBuilder.Vertices.Add (position);
        meshBuilder.Vertices.Add (position + middleVector);
        meshBuilder.Vertices.Add (position + direction2);
        meshBuilder.Vertices.Add (position + middleVector + (1f - 2*edgeFactor) * direction2);
        meshBuilder.Vertices.Add (position + direction1);
        meshBuilder.Vertices.Add (position + middleVector + (1f - 2*edgeFactor) * direction1);
        meshBuilder.Vertices.Add (position + direction1 + direction2 - middleVector);
        meshBuilder.Vertices.Add (position + direction1 + direction2);

        meshBuilder.AddQuadTriangles(MeshFace.Both, 0, 1, 2, 3);
        meshBuilder.AddQuadTriangles(MeshFace.Both, 0, 4, 1, 5);
        meshBuilder.AddQuadTriangles(MeshFace.Both, 5, 4, 6, 7);
        meshBuilder.AddQuadTriangles(MeshFace.Both, 3, 6, 2, 7);
        meshBuilder.AddQuadTriangles(MeshFace.Both, 1, 3, 5, 6);

        meshBuilder.UVs.Add(new Vector2(0f, 0f));
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));
        meshBuilder.UVs.Add(new Vector2(0f, 0f));
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));
        meshBuilder.UVs.Add(new Vector2(0f, 0f));
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));
        meshBuilder.UVs.Add(new Vector2(0f, 0f));

		Mesh mesh = meshBuilder.CreateMesh ();

		GameObject cubeFace = new GameObject ("cubeFace");
		MeshFilter meshFilter = cubeFace.AddComponent<MeshFilter> ();
		MeshRenderer meshRenderer = cubeFace.AddComponent<MeshRenderer> ();

		meshFilter.mesh = mesh;
		meshRenderer.material = material;
		meshRenderer.material.SetColor("_FaceColor", faceColor);

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
}

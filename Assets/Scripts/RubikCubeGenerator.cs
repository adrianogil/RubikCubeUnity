﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public interface ICubeSelectionManager
{
	void Selected(int x, int y, int z);
}

public class RubikCubeGenerator : MonoBehaviour {

	public GameObject cubePrefab;
	public Material material;

	private GameObject[,,] cubeMatrix;

	public Vector3 cubeSize;

	public Vector3 cubePosition; 
	public Vector3 rotationFace;
	public float angle;

	public Vector3 sumCube;

	public Vector3 cubesPerAxis;

	void Awake() {
		cubeMatrix = new GameObject[3, 3, 3];
		cubeSize = Vector3.one;
		// cubesPerAxis = 3 * Vector3.one;
	}

	// Use this for initialization
	void Start () {
		for (int x = 0; x < cubesPerAxis.x; x++) {
			for (int y = 0; y < cubesPerAxis.y; y++) {
				for (int z = 0; z < cubesPerAxis.z; z++) {
					cubeMatrix [x, y, z] = CubeGeneration.Generate(Vector3.zero, cubeSize, material);
					cubeMatrix [x, y, z].transform.SetParent (transform);
					cubeMatrix [x, y, z].transform.localPosition = (-1f) * cubeSize + Vector3.Scale(cubeSize, new Vector3 (x, y, z));
					cubeMatrix [x, y, z].GetComponent<CubeSelection>().SetCubePosition(x, y, z);
				}
			}
		}
	}

	void UpdateCubeMatrix()
	{
		Vector3 cubePosition;
		int x = 0, y = 0, z = 0;
		for (int i = 0; i < transform.childCount; i++) {
			cubePosition = transform.GetChild (i).localPosition;

			x = (int) cubePosition.x;
			y = (int) cubePosition.y;
			z = (int) cubePosition.z;

			cubeMatrix[x, y, z] = transform.GetChild(i).gameObject;
		}
	}
	
	public List<CubeSelection> GetCubesFrom(Vector3 cubePosition, Vector3 rotationFace)
	{
		List<CubeSelection> cubes = new List<CubeSelection>();

		Vector3 rotationSet = Vector3.one - rotationFace; // (1,1,1) - (0,1,0)
		cubePosition.Scale(rotationFace);

		int posx = 0, posy = 0, posz = 0;

		sumCube = Vector3.zero;

		CubeSelection cubeSelection = null;

		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				for (int z = 0; z < 3; z++) {
					posx = (int)(rotationSet.x * x + cubePosition.x);
					posy = (int)(rotationSet.y * y + cubePosition.y);
					posz = (int)(rotationSet.z * z + cubePosition.z);

					cubeSelection = cubeMatrix [posx, posy, posz].GetComponent<CubeSelection>();

					if (!cubes.Contains(cubeSelection))
						cubes.Add(cubeSelection);
				}
			}
		}

		return cubes;
	}

	public void RotateCubes(Vector3 cubePosition, Vector3 rotationFace, float angle)
	{
		GameObject rotateTempObject = new GameObject ("temp");
		rotateTempObject.transform.SetParent (transform);

		Vector3 rotationSet = Vector3.one - rotationFace; // (1,1,1) - (0,1,0)
		cubePosition.Scale(rotationFace);

		int posx = 0, posy = 0, posz = 0;

		sumCube = Vector3.zero;

		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				for (int z = 0; z < 3; z++) {
					posx = (int)(rotationSet.x * x + cubePosition.x);
					posy = (int)(rotationSet.y * y + cubePosition.y);
					posz = (int)(rotationSet.z * z + cubePosition.z);

					sumCube += new Vector3 (posx, posy, posz);
				}
			}
		}

		sumCube = (1 / 27f) * sumCube;

		rotateTempObject.transform.localPosition = sumCube - cubeSize;
		rotateTempObject.transform.localEulerAngles = Vector3.zero;

		for (int x = 0; x < 3; x++) {
			for (int y = 0; y < 3; y++) {
				for (int z = 0; z < 3; z++) {
					posx = (int)(rotationSet.x * x + cubePosition.x);
					posy = (int)(rotationSet.y * y + cubePosition.y);
					posz = (int)(rotationSet.z * z + cubePosition.z);

					cubeMatrix [posx, posy, posz].transform.SetParent (rotateTempObject.transform);
				}
			}
		}

		rotateTempObject.transform.Rotate (angle * rotationFace, Space.Self);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(cubePosition+transform.position-cubeSize, new Vector3(1, 1, 1));
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(RubikCubeGenerator))]
public class RubikCubeGeneratorEditor : Editor {


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		RubikCubeGenerator rGen = target as RubikCubeGenerator;

		if (rGen == null) return;

		if (GUILayout.Button("Rotate")) {
			rGen.RotateCubes (rGen.cubePosition, rGen.rotationFace, rGen.angle);
		}
	}

}
#endif

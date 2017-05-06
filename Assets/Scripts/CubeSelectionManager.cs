using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelectionManager : MonoBehaviour {

    enum SelectionMode
    {
        AxisX, AxisY, AxisZ, None
    }

    SelectionMode[] _modes;
    Vector3[] _faceDirectionByAxis;

    int _currentMode = 0;

    RubikCubeGenerator cubeGenerator;

    List<CubeSelection> lastCubes = null;

	// Use this for initialization
	void Start () {
		_modes = new SelectionMode[] {
            SelectionMode.None,
            SelectionMode.AxisX,
            SelectionMode.AxisY,
            SelectionMode.AxisZ
        };

        _faceDirectionByAxis = new Vector3[] {
            Vector3.zero,
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 1f)
        };

        _currentMode = 0;

        cubeGenerator = GetComponent<RubikCubeGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
            Debug.Log("GilLog - CubeSelectionManager::Update - Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit) {
                Debug.Log("GilLog - CubeSelectionManager::Update - hit " + hitInfo.collider.gameObject.name);

                CubeSelection cubeSelection = null;

                if ((cubeSelection = hitInfo.collider.gameObject.GetComponent<CubeSelection>()) != null) {
                    UnselectLastCubes();
                    SelectCubes(cubeSelection.CubePosition);
                }
            }
        }
	}

    void UnselectLastCubes()
    {
        if (lastCubes == null) return;

        for (int i = 0; i < lastCubes.Count; i++)
        {
            lastCubes[i].ToggleCubeSelection();
        }
    }

    void SelectCubes(Vector3 cubePosition)
    {
        List<CubeSelection> cubes = cubeGenerator.GetCubesFrom(cubePosition, _faceDirectionByAxis[1]);

        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].ToggleCubeSelection();
        }

        lastCubes = cubes;
    }

}

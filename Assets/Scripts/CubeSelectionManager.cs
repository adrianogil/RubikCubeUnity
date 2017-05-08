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

    bool isBackgroundMove = false;
    Vector3 backgroundMoveStart, lastMove;

    public float pixelToRotationFactor = 0.5f;

    Vector3 lastCubePosition;

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
        if (isBackgroundMove)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isBackgroundMove = false;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 diff = Input.mousePosition - lastMove;
                Vector3 diffFromStart = Input.mousePosition - backgroundMoveStart;

                if ( Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                {
                    transform.Rotate(Vector3.up, pixelToRotationFactor * diff.x, Space.World);
                } else if ( Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) {
                    transform.Rotate(Vector3.right, pixelToRotationFactor * diff.y, Space.World);
                }

                // if ( Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                // {
                //     transform.Rotate(transform.up, pixelToRotationFactor * diff.x);
                // } else if ( Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) {
                //     transform.Rotate(transform.right, pixelToRotationFactor * diff.y);
                // }
            }

            lastMove = Input.mousePosition;
        }
		else { 
            if (lastCubes != null) 
            {
               if (Input.GetMouseButtonUp(0))
                {
                    isBackgroundMove = false;
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 diff = Input.mousePosition - lastMove;
                    Vector3 diffFromStart = Input.mousePosition - backgroundMoveStart;

                    if ( Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                    {
                        cubeGenerator.RotateCubes(lastCubePosition, _faceDirectionByAxis[1], pixelToRotationFactor * diff.x);
                        //transform.Rotate(Vector3.up, pixelToRotationFactor * diff.x, Space.Self);
                    } else if ( Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) {
                        //transform.Rotate(Vector3.right, pixelToRotationFactor * diff.y, Space.Self);
                        cubeGenerator.RotateCubes(lastCubePosition, _faceDirectionByAxis[1], pixelToRotationFactor * diff.y);
                    }

                    // if ( Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                    // {
                    //     transform.Rotate(transform.up, pixelToRotationFactor * diff.x);
                    // } else if ( Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) {
                    //     transform.Rotate(transform.right, pixelToRotationFactor * diff.y);
                    // }
                }

                lastMove = Input.mousePosition;
            }
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
                } else {
                    UnselectLastCubes();
                    isBackgroundMove = true;

                    backgroundMoveStart = Input.mousePosition;
                    lastMove = Input.mousePosition;
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

        lastCubes = null;
    }

    void SelectCubes(Vector3 cubePosition)
    {
        lastCubePosition = cubePosition;

        List<CubeSelection> cubes = cubeGenerator.GetCubesFrom(cubePosition, _faceDirectionByAxis[1]);

        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].ToggleCubeSelection();
        }

        lastCubes = cubes;
    }

}

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

    private float accumulated_Angles;
    private const float MAX_ANGLE_VELOCITY = 15f;
    private int m_lastAxisRotation = -1;

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
                    // CubeFaceRotation(pixelToRotationFactor * diff.y, 2);
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
                    UnselectLastCubes();
                    SnapCubeRotation();
                    lastCubes = null;
                    isBackgroundMove = false;

                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 diff = Input.mousePosition - lastMove;
                    Vector3 diffFromStart = Input.mousePosition - backgroundMoveStart;

                    if ( Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                    {
                        CubeFaceRotation(pixelToRotationFactor * diff.x, 1);
                        // cubeGenerator.RotateCubes(lastCubePosition, _faceDirectionByAxis[1], pixelToRotationFactor * diff.x);
                        //transform.Rotate(Vector3.up, pixelToRotationFactor * diff.x, Space.Self);
                    } else if ( Mathf.Abs(diff.y) > Mathf.Abs(diff.x)) {
                        CubeFaceRotation(pixelToRotationFactor * diff.y, 1);
                        //transform.Rotate(Vector3.right, pixelToRotationFactor * diff.y, Space.Self);
                        // cubeGenerator.RotateCubes(lastCubePosition, _faceDirectionByAxis[2], pixelToRotationFactor * diff.y);
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
            else if (Input.GetMouseButtonDown(0)) {
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
                    lastCubes = null;
                    isBackgroundMove = true;

                    backgroundMoveStart = Input.mousePosition;
                    lastMove = Input.mousePosition;
                }
            }
        }
	}

    void CubeFaceRotation(float angles, int axis)
    {
        Vector3 faceRotation = _faceDirectionByAxis[axis];

        faceRotation =  transform.rotation * faceRotation;

        if (faceRotation.x > faceRotation.y && faceRotation.x > faceRotation.z)
        {
            faceRotation = _faceDirectionByAxis[1];
        } else if (faceRotation.y > faceRotation.x && faceRotation.y > faceRotation.z)
        {
            faceRotation = _faceDirectionByAxis[2];
        } else {
            faceRotation = _faceDirectionByAxis[3];
        }

        // ACCUMULATES ROTATIONS
        if (Mathf.Abs(angles) < MAX_ANGLE_VELOCITY)
        {
            cubeGenerator.RotateCubes(lastCubePosition, faceRotation, angles);    

            accumulated_Angles += angles;
            m_lastAxisRotation = axis;
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

    void SnapCubeRotation()
    {
        if (m_lastAxisRotation != -1 && Mathf.Abs(accumulated_Angles) > 0.01f)
        {
            Vector3 faceRotation = _faceDirectionByAxis[m_lastAxisRotation];

            faceRotation =  transform.rotation * faceRotation;

            if (faceRotation.x > faceRotation.y && faceRotation.x > faceRotation.z)
            {
                faceRotation = _faceDirectionByAxis[1];
            } else if (faceRotation.y > faceRotation.x && faceRotation.y > faceRotation.z)
            {
                faceRotation = _faceDirectionByAxis[2];
            } else {
                faceRotation = _faceDirectionByAxis[3];
            }

            float currentAngles = accumulated_Angles;
            float nextAngle = Mathf.Round(currentAngles / 90f) * 90f;
            currentAngles = nextAngle - accumulated_Angles;

            Debug.Log("GilLog - CubeSelectionManager::SnapCubeRotation - currentAngles " + currentAngles + " ");

            cubeGenerator.RotateCubes(lastCubePosition, faceRotation, currentAngles);

            accumulated_Angles = 0;
            m_lastAxisRotation = -1;
        }
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

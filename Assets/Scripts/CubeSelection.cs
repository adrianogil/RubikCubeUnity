using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelection : MonoBehaviour {

    public int x, y, z;

    public Vector3 CubePosition { get { return new Vector3((float)x, (float)y, (float)z); }}

    private bool _selected;
    public bool IsSelected { get { return _selected; }}

    public void SetCubePosition(int indexX, int indexY, int indexZ) {
        x = indexX;
        y = indexY;
        z = indexZ;
    }

    private float _currenColorAnimationProgress;

    private MeshRenderer[] faceRenderers;

    public void Awake()
    {
        faceRenderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void ToggleCubeSelection()
    {
        if (_selected)
            UnselectCube();
        else 
            SelectCube();
    }

    public void SelectCube()
    {        
        _selected = true;

        _currenColorAnimationProgress = 0f;
    }

    public void UnselectCube()
    {
        _selected = false;
        SetEdgeColor(Color.black);
    }

    void Update()
    {
        if (_selected)
        {
            SetEdgeColor(Color.Lerp(Color.black, Color.white, Mathf.PingPong(Time.time, 1f)));
        }
    }

    private void SetEdgeColor(Color color)
    {
        for (int i = 0; i < faceRenderers.Length; i++)
        {
            if (faceRenderers[i] != null) {
                faceRenderers[i].material.SetColor("_EdgeColor", color);
            }
        }
    }
}

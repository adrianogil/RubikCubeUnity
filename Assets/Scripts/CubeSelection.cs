using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelection : MonoBehaviour {

    public ICubeSelectionManager selectionManager;
    public int x, y, z;

    private bool _selected;

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

    public void OnSelected()
    {
        selectionManager.Selected(x, y, z);
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

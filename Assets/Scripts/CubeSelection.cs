using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelection : MonoBehaviour {

    public Texture selectedTexture;
    public Texture unselectedTexture;

    public MeshRenderer renderer;

    public void SelectCube()
    {
        // renderer.material.texture = selectedTexture;
    }

    public void UnselectCube()
    {
        // renderer.material.texture = unselectedTexture;
    }


}

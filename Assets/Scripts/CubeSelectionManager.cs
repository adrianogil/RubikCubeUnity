using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSelectionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
                    cubeSelection.ToggleCubeSelection();
                }
            }
        }
	}
}

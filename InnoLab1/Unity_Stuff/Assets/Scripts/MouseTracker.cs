using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTracker : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;

    public Vector3 mouseTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //https://forum.unity.com/threads/raycast-layermask-parameter.944194/

        int layerMask = 1 << 0 | 1 << 3 | 1 << 7;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, layerMask))
        {
            mouseTarget = raycastHit.point;
            //Debug.Log("Direction X: " + mouseTarget.x);
            //Debug.Log("Direction Z: " + mouseTarget.z);
        }
    }
}

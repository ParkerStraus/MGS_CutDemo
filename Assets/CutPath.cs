using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPath : MonoBehaviour
{
    public float Sensitivity = 1f;
    public static CutPath instance;
    public bool CutActive=false;
    public MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(!CutActive) { 
            renderer.enabled = false;
            return;
        }
        renderer.enabled = true;
        RotatePlate();
    }

    void RotatePlate()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (-Input.GetAxisRaw("Mouse X") * Vector3.forward * Sensitivity));
    }
}

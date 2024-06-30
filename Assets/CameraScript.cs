using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Angle_Normal;
    public GameObject Angle_Aim;
    public static bool Aiming = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Aiming)
            {
                Angle_Normal.SetActive(true);
                Angle_Aim.SetActive(false);
                CutPath.instance.CutActive = false;
                Aiming = false;
            }
            else
            {
                Angle_Normal.SetActive(false);
                Angle_Aim.SetActive(true);
                CutPath.instance.CutActive = true;
                Aiming = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float sensitivity;
    public static FollowTarget instance;
    public Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += sensitivity * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        Cursor.lockState = CursorLockMode.Locked;

    }

    public void newFollowType(GameObject obj, Vector3 offset)
    {
        this.gameObject.transform.parent = obj.transform;
        Offset = offset;
        gameObject.transform.localPosition = Offset;
    }
}

using EzySlice;
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
            Time.timeScale = 1f;
            return;
        }
        renderer.enabled = true;
        Time.timeScale = 0.25f;
        RotatePlate();
        if(Input.GetMouseButtonDown(0))
        {
            Cut();
        }
    }

    void RotatePlate()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + (-Input.GetAxisRaw("Mouse X") * Vector3.forward * Sensitivity));
    }

    void Cut()
    {
        print("Slice and dice");
        Collider[] Hits = Physics.OverlapBox(this.gameObject.transform.position, new Vector3(10, 0.1f, 10), this.transform.rotation, 1 << LayerMask.NameToLayer("Cuttable"));

        if (Hits.Length <= 0) return;

        for(int i = 0; i < Hits.Length; i++)
        {
            Material mat = Hits[i].GetComponent<Cuttable>().internalMaterial;
            SlicedHull hull = SliceObject(Hits[i].gameObject, mat);
            if(hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(Hits[i].gameObject, mat);
                GameObject top = hull.CreateUpperHull(Hits[i].gameObject, mat);
                Destroy(Hits[i].gameObject);

                AddHullComponents(bottom, mat);
                AddHullComponents(top, mat);

            }
        }
    }

    SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(this.transform.position, transform.up, crossSectionMaterial);
    }

    void AddHullComponents(GameObject obj, Material crossSectionMaterial)
    {
        obj.layer = LayerMask.NameToLayer("Cuttable");
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider col = obj.AddComponent<MeshCollider>();
        col.convex = true;
        Cuttable cuttable = obj.AddComponent<Cuttable>();
        cuttable.internalMaterial = crossSectionMaterial;
        rb.AddExplosionForce(100, obj.transform.position, 10);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject testobj;

    void Start()
    {
        
        //MeshFilter sourceMeshFilter = testobj.GetComponent<MeshFilter>();
        //
        //
        //// MeshFilter�� �߰��ϰ� sharedMesh�� �����մϴ�.
        //MeshFilter meshfilter = gameObject.AddComponent<MeshFilter>();
        //meshfilter.sharedMesh = sourceMeshFilter.sharedMesh;
        //
        //// �޽� ������ ����
        //MeshRenderer sourceMeshRenderer = testobj.GetComponent<MeshRenderer>();
        //if (sourceMeshRenderer != null)
        //{
        //    MeshRenderer meshrenderer = gameObject.AddComponent<MeshRenderer>();
        //    meshrenderer.sharedMaterials = sourceMeshRenderer.sharedMaterials;
        //}
        //
        //// �޽� �ݶ��̴� ����
        ////MeshCollider sourceCollider = testobj.GetComponent<MeshCollider>();
        //Collider sourceCollider = testobj.GetComponent<Collider>();
        //
        //if(sourceCollider is BoxCollider)
        //{
        //
        //}
        //if(sourceCollider is MeshCollider meshCollider)
        //{
        //    
        //    MeshCollider meshcol = gameObject.AddComponent<MeshCollider>();
        //    meshcol.sharedMesh = meshCollider.sharedMesh;
        //    meshcol.convex = true;
        //}
    }
}

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
        //// MeshFilter를 추가하고 sharedMesh를 설정합니다.
        //MeshFilter meshfilter = gameObject.AddComponent<MeshFilter>();
        //meshfilter.sharedMesh = sourceMeshFilter.sharedMesh;
        //
        //// 메시 렌더러 복사
        //MeshRenderer sourceMeshRenderer = testobj.GetComponent<MeshRenderer>();
        //if (sourceMeshRenderer != null)
        //{
        //    MeshRenderer meshrenderer = gameObject.AddComponent<MeshRenderer>();
        //    meshrenderer.sharedMaterials = sourceMeshRenderer.sharedMaterials;
        //}
        //
        //// 메시 콜라이더 복사
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

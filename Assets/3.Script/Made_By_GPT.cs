using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Made_By_GPT : MonoBehaviour
{
    public GameObject sourceObject; // �Ž��� ������ ������ ���� ������Ʈ

    void Start()
    {
        if (sourceObject == null)
        {
            Debug.LogError("Source object is not assigned.");
            return;
        }

        // ���� ������Ʈ���� MeshFilter�� MeshRenderer ������Ʈ�� �����ɴϴ�.
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();
        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();

        // MeshFilter ����
        if (currentMeshFilter != null)
        {
            MeshFilter sourceMeshFilter = sourceObject.GetComponent<MeshFilter>();
            if (sourceMeshFilter != null && sourceMeshFilter.sharedMesh != null)
            {
                currentMeshFilter.sharedMesh = sourceMeshFilter.sharedMesh;
                Debug.Log("MeshFilter sharedMesh copied.");
            }
            else
            {
                Debug.LogWarning("Source MeshFilter or its sharedMesh is missing.");
            }
        }
        else
        {
            Debug.LogWarning("Current MeshFilter is missing.");
        }

        // MeshRenderer ����
        if (currentMeshRenderer != null)
        {
            MeshRenderer sourceMeshRenderer = sourceObject.GetComponent<MeshRenderer>();
            if (sourceMeshRenderer != null && sourceMeshRenderer.sharedMaterials != null)
            {
                currentMeshRenderer.sharedMaterials = sourceMeshRenderer.sharedMaterials;
                Debug.Log("MeshRenderer sharedMaterials copied.");
            }
            else
            {
                Debug.LogWarning("Source MeshRenderer or its sharedMaterials is missing.");
            }
        }
        else
        {
            Debug.LogWarning("Current MeshRenderer is missing.");
        }

        // Collider ����
        Collider currentCollider = GetComponent<Collider>();
        if (currentCollider != null)
        {
            Collider sourceCollider = sourceObject.GetComponent<Collider>();
            if (sourceCollider != null)
            {
                if (sourceCollider is BoxCollider sourceBoxCollider)
                {
                    if (currentCollider is BoxCollider boxCollider)
                    {
                        boxCollider.center = sourceBoxCollider.center;
                        boxCollider.size = sourceBoxCollider.size;
                        Debug.Log("BoxCollider properties copied.");
                    }
                }
                else if (sourceCollider is MeshCollider sourceMeshCollider)
                {
                    if (currentCollider is MeshCollider meshCollider)
                    {
                        meshCollider.sharedMesh = sourceMeshCollider.sharedMesh;
                        meshCollider.convex = sourceMeshCollider.convex; // Optional: Copy other properties if needed
                        Debug.Log("MeshCollider properties copied.");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Source Collider is missing.");
            }
        }
        else
        {
            Debug.LogWarning("Current Collider is missing.");
        }
    }
}
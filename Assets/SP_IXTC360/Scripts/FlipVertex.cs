using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;
[ExecuteInEditMode]

public class FlipVertex : MonoBehaviour
{
    public bool Flip = false; //YT : To flip collider in execute mode
    // Start is called before the first frame update
    void Start()
    {
        //this.gameObject.SetActive(false);
    }

    // Update is called once per frame
   
    void Update()
    {
        if (Flip)
        {
            Debug.Log("Invert");
            CreateInvertedMeshCollider();
            Flip = false;
        }
    }

    public bool removeExistingColliders = true;

    public void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
            RemoveExistingColliders();

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
            DestroyImmediate(colliders[i]);
    }

    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}

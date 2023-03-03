using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeShader : MonoBehaviour
{

    public Material element;
    public List<Material> MyMaterial = new List<Material>();
  
    //public Material switchelement;


    //public Vector2 theoffset = new Vector2();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //ON HOVER THE SPHERE WILL CHANGE TO THE OPEN DOOR SHADER
    public void switchshader()
    {

        this.GetComponent<MeshRenderer>().material = MyMaterial[0];
        Debug.Log(MyMaterial[0]);
        //Hotspot.SetActive(true);

    }

    public void switchbackshader()
    {


        this.GetComponent<MeshRenderer>().material = element;
        //Hotspot.SetActive(false);


    }

}



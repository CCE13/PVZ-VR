using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SP_HelpPanel : MonoBehaviour
{

    public GameObject helppanel;

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //    if (helppanel.activeSelf == false)
        //    {
        //        Debug.Log("mouse enter");
        //        OnMouseEnter();
        //    }
        //    //if the details window is open, hide the label panel 
        //    else
        //    {
        //        OnMouseExit();
        //    }
        //   ;
    }

 /*   public void OnMouseEnter()
    {

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("click to close detailswindow");
            helppanel.SetActive(true);
        }
    }

    public void OnMouseExit()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("click to close detailswindow");
            helppanel.SetActive(false);
        }
    }*/
    public void onclick()
    {
        Debug.Log("click to close detailswindow");
        bool visible = helppanel.activeSelf;
        if (visible)
        {
            helppanel.SetActive(false);
        }
        else
        {
            helppanel.SetActive(true);
        }


    }


}


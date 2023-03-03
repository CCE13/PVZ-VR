using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SP_Startbutton : SP_ClickableUI
{

    public GameObject SplashScreen;
    public GameObject StartButton;
    public GameObject Exitbutton;
    public GameObject helpbutton;
    public GameObject helpPanel;
    public GameObject Hotspot;
    public GameObject Controls;

    //Color32 color = new Color32(7, 255, 0, 255);


    public void Click()
    {
        Time.timeScale = 1f;
        SplashScreen.SetActive(false);
        Exitbutton.SetActive(true);
        helpbutton.SetActive(true);
        StartButton.SetActive(false);
        Hotspot.SetActive(true);
        Controls.SetActive(true);

        //Play the Video when Enter button is clicked
        //Find the sphere that the camera is currently in
        //Find the position of the SP_player, the sphere with the exact same coords is the sphere
        GameObject cam = GameObject.Find("SP_Player");
        Vector3 camPosition = cam.GetComponent<Transform>().position;
        Debug.Log(camPosition);
        //Find the Gameobject that has the same coords as the came + contains a videoplayer component
        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {           
            if (obj.GetComponent<Transform>().position == camPosition && obj.GetComponent<VideoPlayer>()!=null)
            {
                Debug.Log("found something");
                obj.GetComponent<VideoPlayer>().Play();             
            }
            else
            {
                Debug.Log("found nothing");
            }            
        }              
    }

   void Start()
    {
        Time.timeScale = 0f;
        SplashScreen.SetActive(true);
        StartButton.SetActive(true);
        Hotspot.SetActive(false);
        Controls.SetActive(false);

    }
    //public override void OnEnter()
    //{
    //    this.gameObject.GetComponent<Image>().color = color;

    //}

    //public override void OnExit()
    //{
    //    if (this.gameObject.GetComponent<Image>().color == color)

    //    {
    //        this.gameObject.GetComponent<Image>().color = Color.white;
    //    }


    //}

}

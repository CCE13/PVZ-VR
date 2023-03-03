using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;


public class SP_VideoEditorCapture : EditorWindow
{
    string framenumber;
    float hslidervalue = 0.0F;
    float previoushslidervalue;
    float Duration = 0.0F;
    string PlayStop = "Play Video";
    bool playstoptoggle = false;
    float Hr = 0;
    float Mins = 0;
    float Secs = 0;
    GameObject SelectedVideoSphere;
    bool IsPlaying = false;
    bool isScrubbing = false;
    Texture2D screenshotviewer;
    VideoPlayer SelectedVideoSphereVP;

    //Add menu item SP IXTC 360/Save Video Frame
    //only static functions can be used for menu items
    [MenuItem("SP IXTC 360/Save Video Frame", false, 3)]
    static void SHOWWINDOW()
    {

        //Get the type for the gameview
        System.Type T = Type.GetType("UnityEditor.GameView,UnityEditor");
        //If a game window aleready exsist, close it
        EditorWindow gamewindow = GetWindow(Type.GetType("UnityEditor.GameView,UnityEditor"));
        gamewindow.Close();

        SP_VideoEditorCapture window = GetWindow<SP_VideoEditorCapture>("Save Video Frame");
        //get a new game window to appear beside this window
        EditorWindow newgamewindow = GetWindow(Type.GetType("UnityEditor.GameView,UnityEditor"), false, "Video Scrub");
       
        Vector2 windowsize = new Vector2(280, 300);
        window.minSize = windowsize;
        newgamewindow.minSize = new Vector2(720, 360);

    }
    //Input the selected video into the video player
    public void PreviewinGame()
    {
        //Load the actaul clip that was created in the resources 
        string SelectedVideoSphereclip = SelectedVideoSphere.GetComponent<VideoPlayer>().clip.name;
        GameObject VP = GameObject.Find("Video Player");
        VP.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>(SelectedVideoSphereclip);
        //Store Video Player of selected sphere, to capture image
        SelectedVideoSphereVP = VP.GetComponent<VideoPlayer>();
    }

    //Converts the hslidervalue into the exact time in hr, min and seconds
    public void Converttotime(float hslidervalue)
    {
        Secs = Mathf.Floor(hslidervalue % 60); //.....get the remainder of seconds, of current duration, rounding down to the lower int    
        Mins = Mathf.Floor(hslidervalue / 60);//.....get the maximum mintues of current hslidervalue, rounding down to the lower int    
        Hr = Mathf.Floor(Mins / 60);//.....get the maximum hrs of current hslidervalue, rounding down to the lower int
    }

    public void CaptureFrame()
    {
        //Texture of a video player is a render texture by default
        Texture rt = SelectedVideoSphereVP.texture;
        Debug.Log(SelectedVideoSphere);
        if (rt)
        {
            //cast(convert) rt into a current render texture, only only active rendertexture can replace the main camea screen
            //rendertexture.active is the current active screen, so it wont capture the players view.
            RenderTexture.active = (RenderTexture)rt;
            //create new texture slot for the screenshot, using the rt height and width, follow the actual video size base on the import settings
            Texture2D Screenshot = new Texture2D(rt.width, rt.height);
            //read pixels only work for texture 2d, which captures the whatevers on screen into a saved data(texture 2d)               
            Screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, true);
            Screenshot.Apply();
            screenshotviewer = Screenshot;
            Debug.Log(rt.width);
            Debug.Log(rt.height);
            SaveImage();
        }
        else
        {
            Debug.Log(rt);
        }

    }

    //save texture in user's selected folder
    public void SaveImage()
    {
        byte[] fileData;
        fileData = screenshotviewer.EncodeToPNG();
        //save file in wherer ever user selects
        string userselectedpath = EditorUtility.SaveFilePanel("Save Captured Image", "", "Placeholder", "png");

        if (userselectedpath.Length > 0)
        {
            File.WriteAllBytes(userselectedpath, fileData);
            Debug.Log("save Image");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log("Cancelled save");
        }
    }
    void OnGUI()
    {
        //To Input a video sphere in object field 
        GUI.Label(new Rect(10, 20, 200, 20), "Select Sphere From Hierachy");
        
        SelectedVideoSphere = (GameObject)EditorGUI.ObjectField(new Rect(10, 40, 200, 16), SelectedVideoSphere, typeof(GameObject), true);
        
        //Play and Stop button toggle
        if (GUI.Button(new Rect(10, 80, 100, 20), PlayStop))
        {
            //if objectfeild is empty amd user tries to play, prompt user to input a video sphere
            if (SelectedVideoSphere == null)
            {
                EditorUtility.DisplayDialog("No video detected", "You need to input a video Sphere from the hierachy", "OK");
            }
            else
            {               
                    IsPlaying = true;
                    playstoptoggle = true;                      
            }

        }

        if (GUI.Button(new Rect(10, 180, 200, 50), "Save Image"))
        {

            if (SelectedVideoSphere == null)
            {
                EditorUtility.DisplayDialog("No video detected", "You need to input a video Sphere from the hierachy", "OK");
            }
            else
            {
                Debug.Log("Save button click");
                CaptureFrame();
            }

        }

        //Display Horizontal Slider of current video timelime
        hslidervalue = GUI.HorizontalSlider(new Rect(10, 105, 400, 20), hslidervalue, 0.0F, Duration);
        //Display Timing of the current video playback
        GUI.Label(new Rect(10, 120, 100, 20), Hr.ToString("00") + ":" + Mins.ToString("00") + ":" + Secs.ToString("00"));
        //Display Framenumber the current video playback
        GUI.Label(new Rect(10, 140, 200, 20), framenumber);
    }

    void Update()
    {
        //Check play button is play or stop
        if (PlayStop == "Play Video")
        {
            //Check if play button has been clicked, clicked means it become a stop
            if (playstoptoggle)
            {
                GameObject Playbutton = GameObject.Find("PlayButton");
                Playbutton.GetComponent<Button>().onClick.Invoke();//....Invoke PlayVideo()
                GameObject VP = GameObject.Find("Video Player");
                hslidervalue = (float)VP.GetComponent<VideoPlayer>().time;//....hslidervalue follows the time of the Video
                IsPlaying = true;
                //Get the total duration in seconds
                Duration = (float)(VP.GetComponent<VideoPlayer>().clip.frameCount / VP.GetComponent<VideoPlayer>().clip.frameRate);
                
                PlayStop = "Stop Video";
                playstoptoggle = false;//.....CHange to false so it will only run once
               
            }
        }

        if (PlayStop == "Stop Video")
        {            
            if (playstoptoggle)
            {
                GameObject StopButton = GameObject.Find("StopButton");
                StopButton.GetComponent<Button>().onClick.Invoke();//....Invoke StopVideo()
                isScrubbing = true;
                IsPlaying = false;
                previoushslidervalue = hslidervalue;// Every stopVideo(), stores the current hslidervalue
                PlayStop = "Play Video";
                playstoptoggle = false;//.....Change to false so it will only run once
            }
        }

        if (IsPlaying)
        {
            //Hslidervalue is following the video timing, get the timing of the video
            GameObject VP = GameObject.Find("Video Player");
            hslidervalue = (float)VP.GetComponent<VideoPlayer>().time;
            Converttotime(hslidervalue);
            //Get the Frame index of the current video.texture
            framenumber = VP.GetComponent<VideoPlayer>().frame.ToString();
        }
        //Only after you click stop, then scrubbing enabled, 
        //Use else if, possible not to hv new hslidervalue, user may click play again 
        else if (isScrubbing)
        {
            //Update the GUI framenumber 
            //Input the current Frame index from the Frame Number GameObject text
            GameObject VP = GameObject.Find("Video Player");
            framenumber = VP.GetComponent<VideoPlayer>().frame.ToString();
            Debug.Log(framenumber);

            //Check if theres any new scrub
            if (previoushslidervalue != hslidervalue)
            {
                GameObject Slider = GameObject.Find("Slider");
                Slider.GetComponent<Slider>().onValueChanged.Invoke(hslidervalue);//....This invokes seekvaluewithstop(hslidervalue)
                Converttotime(hslidervalue);
                Debug.Log("scrubbed in editor at" + hslidervalue);
                previoushslidervalue = hslidervalue;//....replace the previoushslidervalue so it wont invoke function once only
            }
        }

        //If a sphere is dragged into the object field
        if (SelectedVideoSphere)
        {
            //check if the sphere has a video component 
            if (SelectedVideoSphere.GetComponent<VideoPlayer>() == null)
            {
                EditorUtility.DisplayDialog("Video Sphere", "Select a sphere with a video component", "OK");                
                SelectedVideoSphere = null;
            }
            else
            {
                GameObject spVideoCanvas = GameObject.Find("SP_VideoCanvas");
                //Checked if there already exsits a "SP_VideoCanvas"
                if (!spVideoCanvas)
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_VideoCanvas.prefab", typeof(GameObject));
                    spVideoCanvas = Instantiate(prefab) as GameObject;
                    spVideoCanvas.name = "SP_VideoCanvas";
                }

                PreviewinGame();
            }
           
        }
    }
}


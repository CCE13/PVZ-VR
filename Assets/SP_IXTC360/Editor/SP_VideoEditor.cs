using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class SP_VideoEditor : EditorWindow
{
    string selected360VideoFileName = "";
    Material materialFileName;
    string sphereName = "Input Name";
    Texture2D tempTex;
    bool previewassetclick = false;
    Texture2D thumbnail;
    float hslidervalue = 0.0F;
    float previoushslidervalue;
    float Hr = 0;
    float Mins = 0;
    float Secs = 0;
    GameObject SelectedVideoSphere;
    VideoClip myclip;
    public string addButton = "Create Waypoint";
    string wayPointLabel = "Click to create a new arrow";
    bool inCreation = false;
    bool isCreating = false;

    void ResetMaterial()
    {
        materialFileName = null;
    }

    void Reset()
    {
        addButton = "Add Arrow";
        wayPointLabel = "Click to create a new arrow";
        inCreation = false;
        isCreating = false;

    }

    //Preview thumbnail of selected Video
    public void preview()
    {
        UnityEngine.Object mp4 = Resources.Load<VideoClip>(selected360VideoFileName);
        //Get thumbnail (as show in project) of object
        tempTex = UnityEditor.AssetPreview.GetMiniThumbnail(mp4);
        if (tempTex == null)
        {

            Debug.Log("Icon is null");
            tempTex = Resources.Load<Texture2D>("NoPreview");

        }
        else
        {
            Debug.Log("asste has a thumbnail");
        }

        previewassetclick = true;
    }

    //Gets back string of materials path, create the mateiral and put the rendetxture as the main texture
    public void CreateMaterial(string path)
    {

        // Create a simple material asset
        Material material = new Material(Shader.Find("Insideout"));
        //create a new render texture
        RenderTexture tempRTex = new RenderTexture(1440, 720, 24);
        tempRTex.name = selected360VideoFileName;
        //Save the name of the path of the craeted rendertexture and maeterial
        string saverendertexturePath = "Assets/Resources/" + selected360VideoFileName + "_rt" + ".rendertexture";
        //string savePath = "Assets/SP_IXTC360/Materials/" + tempselected360VideoFileName + ".mat";
        string savePath = "Assets/Resources/" + selected360VideoFileName + ".mat";
        //create material at the given path
        AssetDatabase.CreateAsset(material, savePath);
        Debug.Log("create render texture material");
        //create rendertexture at the given path
        AssetDatabase.CreateAsset(tempRTex, saverendertexturePath);
        Debug.Log("create render texture ");
        AssetDatabase.Refresh();

    }



    public void CreateVideo(string filepath)
    {
        //string selectedvideoname = Path.GetFileNameWithoutExtension(filepath);
        //Get the bytes data of the file
        byte[] fileData;
        fileData = File.ReadAllBytes(filepath);
        //create file in resource folder
        File.WriteAllBytes("Assets/Resources/" + Path.GetFileNameWithoutExtension(filepath) + ".mov", fileData);
        Debug.Log("Create video");
        AssetDatabase.Refresh();

    }




    public void browse()
    {
        //opens window explorer, looking only for mp4s(videos)
        string[] filters = { "mp4", "mp4", "mov", "mov" };
        string path = EditorUtility.OpenFilePanelWithFilters("Select mp4 Video", "", filters);

        //check that something was selected
       //selected360VideoFileName = Path.GetFileNameWithoutExtension(path);

        if (path.Length != 0)
        {
            selected360VideoFileName = Path.GetFileNameWithoutExtension(path);
            //Check if the video is already in resources folder
            string videoexsits = "Assets/Resources/" + selected360VideoFileName + ".mp4";

            if (!File.Exists(videoexsits))
            {
                //create video in resources
                CreateVideo(path);

            }
            else
            {
                Debug.Log("video already exsits");
            }

            //check if material already exsits, in case reselect same video
            string materialexsitspath = "Assets/Resources/" + selected360VideoFileName + ".mat";
            if (File.Exists(materialexsitspath))
            {
                //if exsits give back the same material that already exists
                materialFileName = Resources.Load<Material>(selected360VideoFileName);
                Debug.Log("material already exsits");
            }
            else
            {
                //Create the new material of for video
                Debug.Log("material dont exsits, create material");
                CreateMaterial(path);
            }

            previewassetclick = true;
        }
        else
        {
            previewassetclick = true;
        }
    }

    //put the selected video into the video player
    public void PreviewinGame()
    {
        string SelectedVideoSphereclip = SelectedVideoSphere.GetComponent<VideoPlayer>().clip.name;
        GameObject VP = GameObject.Find("Video Player");
        VP.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>(SelectedVideoSphereclip);
    }

    void OnGUI()
    {
        sphereName = GUI.TextField(new Rect(100, 10, 120, 20), sphereName, 25);
        GUI.Label(new Rect(10, 10, 100, 20), "Sphere Name");
        GUI.Label((new Rect(10, 50, 200, 100)), thumbnail);

        // Browse button to import 360 video
        if (GUI.Button(new Rect(10, 170, 200, 50), "Browse Video"))
        {
            browse();
        }

        // Create Sphere button to import 360 video
        if (GUI.Button(new Rect(10, 230, 200, 50), "Create Sphere"))
        {
            //if there is a selected360VideoFileName detected 
            if (selected360VideoFileName.Length > 0)
            {
                //Instantiate the sphere
                UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_Sphere.prefab", typeof(GameObject));
                GameObject sphr = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                sphr.name = sphereName;
                //add vid component in sphere
                sphr.AddComponent<VideoPlayer>();
                //add video clip of selectedselected360VideoFileName
                UnityEngine.Object tempClipinstance = Resources.Load<UnityEngine.Object>(selected360VideoFileName);
                if (tempClipinstance)
                {
                    sphr.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>(selected360VideoFileName);
                    //change render mode to rendertexture
                    sphr.GetComponent<VideoPlayer>().renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
                    //add render texture of selected360VideoFileName
                    sphr.GetComponent<VideoPlayer>().targetTexture = Resources.Load<RenderTexture>(selected360VideoFileName + "_rt");
                    //set play on awake to false
                    sphr.GetComponent<VideoPlayer>().playOnAwake = false;
                }
                else
                {
                    Debug.Log("temp cliip instance is null");
                }

                //Load the created material 
                Material m = Resources.Load<Material>(selected360VideoFileName);
                //Load the created rendertexture
                RenderTexture rt = Resources.Load<RenderTexture>(selected360VideoFileName + "_rt");
                //load the rendertexture into material diffuse
                m.mainTexture = rt;
                //Load material into sphere
                sphr.GetComponent<MeshRenderer>().material = m;
                ResetMaterial();

            }
            else
            {
                //if no video is selected
                bool option = EditorUtility.DisplayDialog("Create Sphere?",
              "Select a video before creating a sphere ", "Browse Video", "OK");

                if (option)
                {
                    browse();
                }
            }         
        }

        if (GUI.Button(new Rect(10, 300, 200, 50), addButton))
        {
            inCreation = !inCreation;
            if (inCreation)
            {
                wayPointLabel = "Select Location to place arrow";
                addButton = "Cancel";
            }
            else
            {
                wayPointLabel = "Click to create a new arrow";
                addButton = "Add Arrow";
            }
        }

        GUI.Label(new Rect(10, 358, 240, 20), wayPointLabel);
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sv)
    {
        if (inCreation)
        {
            if (Event.current.type == EventType.MouseDown && (Event.current.button == 0) && !isCreating)
            {
                isCreating = true;

                Debug.Log("Left-Mouse Down");
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Creating hotspot");
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_Teleport.prefab", typeof(GameObject));
                    GameObject waypoint = Instantiate(prefab, hit.point, Quaternion.identity) as GameObject;



                    Vector3 lookAtAngle = hit.point - hit.transform.gameObject.transform.position;
                    lookAtAngle.y = 0;
                    waypoint.transform.LookAt(waypoint.transform.position + lookAtAngle);

                    waypoint.transform.parent = hit.transform.parent;
                    //waypoint.transform.rotation = Quaternion.FromToRotation()
                    Reset();
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Location", "You need to select on a valid location", "Ok");
                    Reset();
                }
            }

        }
    }

    void Update()
    {
        //Check if a video was selected
        //input the thumbnail of the video
        if (previewassetclick)
        {
            myclip = Resources.Load<VideoClip>(selected360VideoFileName);
            thumbnail = AssetPreview.GetMiniThumbnail(myclip);     
        }
        else
        {
            thumbnail = Resources.Load<Texture2D>("NoPreview");
        }

       
        
    }
}


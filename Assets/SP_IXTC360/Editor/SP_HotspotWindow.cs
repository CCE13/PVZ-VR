

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;


public class SP_HotspotWindow : EditorWindow
{

    bool createHotspot = false;
    string status = "Click on the Create button to add hotspot";
    string addButton = "Create";
    float lastFrameTime = 0.0f;
    int capturedFrame = 0;
    bool create = false;
    bool isCreating = false;
    public GameObject hotspotprefab;
    Texture2D tempTex;
    string selectedTextureFileName = "";
    string hotspotinformation = "Input hotspot information here";

    [MenuItem("SP IXTC 360/Input Information", false, 4)]
    static void Init()
    {
        SP_HotspotWindow window = (SP_HotspotWindow)EditorWindow.GetWindow(typeof(SP_HotspotWindow), false, "Input Information");
    }


    public string hotspotName = "Hello World";

    void ResetSelection()
    {
        isCreating = false;
        status = "Click on the Create button to add hotspot";
        addButton = "Create";
        createHotspot = false;
    }
    void Reset()
    {
        isCreating = false;
        status = "Click on the Create button to add hotspot";
        addButton = "Create";
        createHotspot = false;
        hotspotinformation = "Input hotspot information here";
        tempTex = null;
        selectedTextureFileName = "";

    }


    //GUI.DrawTexture(new Rect(0, 400, 100, 100), tex);
    public Texture2D LoadPNG(string filePath)
    {
        Texture2D tempTex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tempTex = new Texture2D(2, 2);
            tempTex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tempTex;
    }

    public void CreateTexture(string path)
    {
        Debug.Log("Create Texture");
        selectedTextureFileName = Path.GetFileNameWithoutExtension(path);
        Texture2D tex = LoadPNG(path);
        File.WriteAllBytes("Assets/Resources/" + selectedTextureFileName + ".jpg", tex.EncodeToJPG());
    }

    void OnGUI()
    {
        hotspotName = GUI.TextField(new Rect(120, 10, 100, 20), hotspotName, 25);
        GUI.Label(new Rect(10, 10, 100, 20), "Hot Spot Name :");

        //GUI.Label(new Rect(150, 20, 250, 250), "Description");
        hotspotinformation = GUI.TextArea(new Rect(120, 40, 250, 250), hotspotinformation, 2000);



        if (GUI.Button(new Rect(10, 150, 100, 20), "Browse Image"))
        {
            string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "jpg");
            Debug.Log(path);
            if (path.Length != 0)
            {
                CreateTexture(path);
                AssetDatabase.Refresh();
            }
        }

        if (selectedTextureFileName.Length > 0)
        {
            tempTex = Resources.Load(selectedTextureFileName, typeof(Texture2D)) as Texture2D;
            if (tempTex)
            {
                GUI.Label(new Rect(10, 40, 100, 100), tempTex);
                Debug.Log("Draw Image");
            }
        }
        else
        {
            tempTex = Resources.Load("NoPreview", typeof(Texture2D)) as Texture2D;
            if (tempTex)
            {
                GUI.Label(new Rect(10, 40, 100, 100), tempTex);
                Debug.Log("Draw Image");
            }
        }

        GUI.Label(new Rect(80, 310, 250, 20), status);
        if (GUI.Button(new Rect(150, 340, 100, 20), addButton))
        {
            if (!createHotspot)
            {
                addButton = "Cancel";
                createHotspot = true;
                status = "Select the location to create hotspot";
            }
            else
            {
                Reset();
            }
        }


        if (GUI.Button(new Rect(150, 370, 100, 20), "Close"))
        {
            this.Close();
        }



        SceneView.duringSceneGui += OnSceneGUI;

    }

    void OnDestroy()
    {
        Reset();
    }

    private void OnSceneGUI(SceneView sv)
    {
        if (createHotspot)
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
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_Hotspot.prefab", typeof(GameObject));
                    GameObject hotspot = Instantiate(prefab, hit.point, hit.transform.rotation) as GameObject;
                    hotspot.name = hotspotName;
                    hotspot.GetComponent<SP_Hotspot>().myInfo.hotspotName = hotspotName;
                    hotspot.GetComponent<SP_Hotspot>().myInfo.displayText = hotspotinformation;
                    //hotspot.GetComponent<SP_Hotspot>().myInfo.displayImage.texture = Resources.Load(selectedTextureFileName, typeof(Texture2D)) as Texture2D;
                    hotspot.transform.parent = hit.transform.parent.transform;
                    hotspot.GetComponent<SP_Hotspot>().myInfo.displayImage = Sprite.Create(tempTex, new Rect(0.0f, 0.0f, tempTex.width, tempTex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    Reset();
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Location", "You need to select on a valid location", "Ok");
                    ResetSelection();
                }
            }
        }
    }

    void Update()
    {

    }

}


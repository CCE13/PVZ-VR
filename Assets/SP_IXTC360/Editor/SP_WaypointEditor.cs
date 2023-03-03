
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;


public class SP_WaypointEditor : EditorWindow
{
    [MenuItem("SP IXTC 360/Create 360 Sphere", false, 1)]

    static void Init()
    {
        SP_WaypointEditor window = GetWindow<SP_WaypointEditor>("360 Image Sphere");
        //Dock SP_VideoEditor window next to SP_WaypointEditor
        SP_VideoEditor videowindow = GetWindow<SP_VideoEditor>("360 Video Sphere", typeof(SP_WaypointEditor));
        //Maked SP_WaypointEditor window the focused tab
        window.Focus();
        Vector2 windowsize = new Vector2(280, 400);
        window.minSize = windowsize;

    }

    public string addButton = "Create Waypoint";
    string wayPointLabel = "Click to create a new arrow";
    bool inCreation = false;
    bool isCreating = false;
    string materialFileName = "";
    string selectedTextureFileName = "";
    string sphereName = "Input Name";
    Texture2D tempTex;

    void ResetMaterial()
    {
        materialFileName = "";
        tempTex = null;
        selectedTextureFileName = "";
    }

    void Reset()
    {
        addButton = "Add Arrow";
        wayPointLabel = "Click to create a new arrow";
        inCreation = false;
        isCreating = false;
    }

    private string CreateMaterial()
    {
        // Create a simple material asset
        tempTex = Resources.Load(selectedTextureFileName, typeof(Texture2D)) as Texture2D;
        Material material = new Material(Shader.Find("Insideout"));
        string savePath = "Assets/SP_IXTC360/Materials/" + selectedTextureFileName + ".mat";
        material.mainTexture = tempTex;
        AssetDatabase.CreateAsset(material, savePath);
        return AssetDatabase.GetAssetPath(material);
    }

    public Texture2D LoadPhoto(string filePath)
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
        Texture2D tex = LoadPhoto(path);
        File.WriteAllBytes("Assets/Resources/" + selectedTextureFileName + ".jpg", tex.EncodeToJPG());
    }

    void OnGUI()
    {
        sphereName = GUI.TextField(new Rect(100, 10, 120, 20), sphereName, 25);
        GUI.Label(new Rect(10, 10, 100, 20), "Sphere Name");

        if (selectedTextureFileName.Length > 0)
        {
            tempTex = Resources.Load(selectedTextureFileName, typeof(Texture2D)) as Texture2D;
            if (tempTex)
            {
                GUI.Label(new Rect(10, 50, 200, 100), tempTex);
                materialFileName = CreateMaterial();
                //Debug.Log("Draw Image");
            }
        }
        else
        {
            tempTex = Resources.Load("NoPreview", typeof(Texture2D)) as Texture2D;
            if (tempTex)
            {
                GUI.Label(new Rect(10, 50, 200, 100), tempTex);
            }
        }


        if (GUI.Button(new Rect(10, 170, 200, 50), "Browse Image"))
        {
            string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "jpg");
            Debug.Log(path);
            if (path.Length != 0)
            {
                CreateTexture(path);
                AssetDatabase.Refresh();
            }
        }

        if (GUI.Button(new Rect(10, 230, 200, 50), "Create Sphere"))
        {
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_Sphere.prefab", typeof(GameObject));
            GameObject sphr = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            sphr.name = sphereName;
            Debug.Log(materialFileName);
            Material m = (Material)AssetDatabase.LoadAssetAtPath(materialFileName, typeof(Material));
            sphr.GetComponent<MeshRenderer>().material = m;
            ResetMaterial();
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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

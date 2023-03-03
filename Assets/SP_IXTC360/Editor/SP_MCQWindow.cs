using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
public class SP_MCQWindow : EditorWindow
{
    bool createHotspot = false;
    string status = "Click on the Create button to add hotspot";
    string addButton = "Create";
    bool isCreating = false;
    public GameObject hotspotprefab;
    List<string> Options = new List<string>();
    List<bool> Answers = new List<bool>();
    string Question = "Ask a Question";
    ReorderableList inList;
    [MenuItem("SP IXTC 360/Create MCQ", false, 6)]
    static void Init()
    {
        SP_MCQWindow window = (SP_MCQWindow)EditorWindow.GetWindow(typeof(SP_MCQWindow), false, "Set the MCQ");
    }
    private void OnEnable()
    {
        Options.Clear();
        Answers.Clear();
        Options.Add("Enter an Option...");
        Options.Add("Enter an Option...");

        Answers.Add(true);
        Answers.Add(false);

        inList = new ReorderableList(Options, typeof(string), true, true, true, true);
        inList.onAddCallback += OnAddCallback;
        inList.onRemoveCallback += OnRemoveCallback;
        inList.onSelectCallback += OnSelectCallback;
        inList.drawHeaderCallback += DrawHeaderCallbackIn;
        inList.onReorderCallback += DrawReOrderCallBack;
    }
    private void OnAddCallback(ReorderableList list)
    {
        if (Options.Count < 4)
        {
            Options.Add("Enter an Option...");
            Answers.Add(false);
        }
    }
    //private float DrawelementHeightCallback(int index)
    //{
    //    float height = 0;

    //    return height;
    //}
    int selectedIndex = -1;
    private void OnSelectCallback(ReorderableList list)
    {
        if (list == inList)
        {
            selectedIndex = list.index;
        }
    }
    private void OnRemoveCallback(ReorderableList list)
    {
        if (list.index == -1 || Options.Count <=2)
            return;

        Options.RemoveAt(list.index);
        Answers.RemoveAt(list.index);
        list.index = -1;

        OnSelectCallback(list);
    }

    private void DrawHeaderCallbackIn(Rect rect)
    {
    }
    private void DrawReOrderCallBack(ReorderableList list)
    {

    }
    public string hotspotName = "Hello MCQ";

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
        hotspotName = "Hello MCQ";
        Question = "Ask a Question";
        //tempTex = null;
        //selectedTextureFileName = "";
        Options.Clear();
        Answers.Clear();

        Options.Add("Enter an Option...");
        Options.Add("Enter an Option...");

        Answers.Add(true);
        Answers.Add(false);

    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        hotspotName = EditorGUILayout.TextField("MCQ Hot Spot Name:", hotspotName);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Question:");
        Question = EditorGUILayout.TextArea(Question, GUILayout.Height(50));
        EditorGUILayout.Space();
        inList.drawHeaderCallback =
         (Rect rect) =>
         {
             EditorGUI.LabelField(rect, "Answer Options                                               Correct", EditorStyles.boldLabel);
         };

        inList.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 2;
            rect.width = 250;
            rect.height = 50;
            Options[index] = EditorGUI.TextArea(rect, Options[index]);
            rect.x = 310;
            rect.y += 15;
            rect.height = 20;
            rect.width = 20;
            Answers[index] = EditorGUI.ToggleLeft(rect, "",Answers[index]);

        };
        inList.elementHeight = 55f;
        inList.list = Options;
        inList.DoLayoutList();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(status);
        EditorGUILayout.Space();

        if (GUILayout.Button(addButton)){
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
                    //Debug.Log("Creating hotspot");
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_MCQ.prefab", typeof(GameObject));
                    GameObject hotspot = Instantiate(prefab, hit.point, hit.transform.rotation) as GameObject;
                    hotspot.name = hotspotName;

                    SP_MCQ spMCQ = hotspot.GetComponent<SP_MCQ>();
                    spMCQ.spMCQInfo.hotspotname = hotspotName;
                    for(int i = 0; i < Options.Count; i++)
                    {
                        spMCQ.spMCQInfo.Options.Add(Options[i]);
                    }
                    for (int j = 0; j < Answers.Count; j++)
                    {
                        spMCQ.spMCQInfo.Answers.Add(Answers[j]);
                    }
                    spMCQ.spMCQInfo.Question = Question;

                    hotspot.transform.parent = hit.transform.parent.transform;
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



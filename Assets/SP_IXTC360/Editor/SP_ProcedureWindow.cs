using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class SP_ProcedureWindow : EditorWindow
{
    GameObject _Procedures;
    ReorderableList inList;
    List<SP_Procedure> procedures;
    bool showProcedureList = true;
    GameObject number;
    Sprite circleplus, circle;

    [MenuItem("SP IXTC 360/Create Procedural Steps", false, 5)]
    static void Init()
    {
        SP_ProcedureWindow window = (SP_ProcedureWindow)EditorWindow.GetWindow(typeof(SP_ProcedureWindow), false, "Set the order of the hotspots");
      
    }
    private void OnEnable()
    {
        _Procedures = GameObject.Find("_Procedures_");
        if (_Procedures == null)
        { _Procedures = new GameObject("_Procedures_"); }
        
        procedures = new List<SP_Procedure>(_Procedures.GetComponents<SP_Procedure>());
    
        number = Resources.Load("SP_Order") as GameObject;
        circleplus = Resources.Load<Sprite>("plus_512_white_shadow");
        circle = Resources.Load<Sprite>("bg");
        //order of a particular procedure
        inList = new ReorderableList(null, typeof(string), true, true, true, true);
        inList.onAddCallback += OnAddCallback;
        inList.onRemoveCallback += OnRemoveCallback;
        inList.onSelectCallback += OnSelectCallback;
        inList.drawHeaderCallback += DrawHeaderCallbackIn;
        inList.onReorderCallback += DrawReOrderCallBack;
        //inList.elementHeightCallback += DrawelementHeightCallback;
    }
    private void OnAddCallback(ReorderableList list)
    {
        //currentsP_Procedure = procedures.Find(x => x.spProcedureInfo.procedureName == procedureNames[selected]);
        //currentsP_Procedure.spProcedureInfo.OrderedObjects.Add(null);
        procedures[selected].spProcedureInfo.OrderedObjects.Add(null);
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
        if (list.index == -1)
            return;

        if (procedures[selected].spProcedureInfo.OrderedObjects[selectedIndex])
        {
            procedures[selected].spProcedureInfo.OrderedObjects[selectedIndex].GetComponent<SpriteRenderer>().sprite = circleplus;
            Transform order = previousObjects[selectedIndex].transform.Find("SP_Order(Clone)");
            if (order)
                DestroyImmediate(order.gameObject);
        }
        procedures[selected].spProcedureInfo.OrderedObjects.RemoveAt(selectedIndex);

        for (int i = 0; i < list.count; i++)
        {
            TextMesh[] txts = ((GameObject)(list.list[i])).GetComponentsInChildren<TextMesh>();
            for (int t = 0; t < txts.Length; t++)
            {
                txts[t].text = (i + 1).ToString();
            }
        }

        list.index = -1;

        OnSelectCallback(list);
    }

    private void DrawHeaderCallbackIn(Rect rect)
    {
    }
    private void DrawReOrderCallBack(ReorderableList list)
    {
        for (int i = 0; i < list.count; i++)
        {
            TextMesh[] txts = ((GameObject)(list.list[i])).GetComponentsInChildren<TextMesh>();
            for (int t = 0; t < txts.Length; t++)
            {
                txts[t].text = (i + 1).ToString();
            }
        }
    }
    private void OnDestroy()
    {
        if (selected < procedures.Count)
        {
            if (procedures[selected].spProcedureInfo.OrderedObjects.Count <= 0)
            {
                SP_Procedure temp = procedures[selected];
                procedures.RemoveAt(selected);
                DestroyImmediate(temp);
                showProcedureList = true;
            }
        }
    }
    int selected = 0;
    Color darkgrey = new Color(0.3f, 0.3f, 0.3f, 1);
    List<GameObject> previousObjects=new List<GameObject>();
    private void OnGUI()
    {
        if (showProcedureList)
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Procedures:");
            EditorGUILayout.Space();
            for (int i = 0; i < procedures.Count; i++)
            {
                GUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(procedures[i].spProcedureInfo.procedureName);
                if (GUILayout.Button("Edit"))
                {
                    selected = i;
                    inList.list = procedures[i].spProcedureInfo.OrderedObjects;
                    previousObjects.Clear();
                    previousObjects.AddRange(procedures[i].spProcedureInfo.OrderedObjects);
                    showProcedureList = false;
                }
                if (GUILayout.Button("X"))
                {
                    if (EditorUtility.DisplayDialog("Delete procedure", "Are you sure you want to remove this procedure?", "Remove", "Do Not Remove"))
                    {
                        SP_Procedure temp = procedures[i];
                        procedures.RemoveAt(i);
                        DestroyImmediate(temp);
                    }
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
          
            //adding new procedure

            if (GUILayout.Button("Create Procedure"))
            {
                showProcedureList = false;
                SP_Procedure p = _Procedures.AddComponent<SP_Procedure>();
                procedures.Add(p);
                previousObjects.Clear();
                selected = procedures.Count - 1;
            }

        }
        else
        {
            //procedure steps
            EditorGUILayout.Space();
            EditorGUILayout.Space();
           
            procedures[selected].spProcedureInfo.procedureName = EditorGUILayout.TextField(procedures[selected].spProcedureInfo.procedureName);
            EditorGUILayout.Space();
            procedures[selected].spProcedureInfo.clrRep = EditorGUILayout.ColorField(procedures[selected].spProcedureInfo.clrRep);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            procedures[selected].spProcedureInfo.Assessment = EditorGUILayout.Toggle("Assessment Mode", procedures[selected].spProcedureInfo.Assessment);
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.wordWrap = true;
            style.normal.textColor = darkgrey;
            EditorGUILayout.LabelField("Assessment Mode: Hotspots appear all at once. User has to decide the order in which to click the hotspot.", style, GUILayout.Height(30), GUILayout.Width(350));
            EditorGUILayout.LabelField("Non-Assessment Mode: After clicking on one Hotspot, the next Hotspot appears.", style, GUILayout.Height(30), GUILayout.Width(350));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();
            //Display our list to the inspector window
            inList.drawHeaderCallback =
           (Rect rect) =>
           {
               EditorGUI.LabelField(rect, "Steps", EditorStyles.boldLabel);
           };

            inList.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                // Display the property fields in two ways.
                //2 : Full custom GUI Layout <-- Choose me I can be fully customized with GUI options.
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.BeginChangeCheck();
                procedures[selected].spProcedureInfo.OrderedObjects[index] = (GameObject)EditorGUI.ObjectField(rect, (Object)procedures[selected].spProcedureInfo.OrderedObjects[index], typeof(GameObject), true);

                //update the color
                if (procedures[selected].spProcedureInfo.OrderedObjects[index])
                {
                    TextMesh[] txts = procedures[selected].spProcedureInfo.OrderedObjects[index].GetComponentsInChildren<TextMesh>();
                    for (int t = 0; t < txts.Length; t++)
                    {
                        txts[t].color = procedures[selected].spProcedureInfo.clrRep;
                    }
                }

                if (EditorGUI.EndChangeCheck())
                {
                   
                    //check for duplicate
                    if (procedures[selected].spProcedureInfo.OrderedObjects[index] != null && procedures[selected].spProcedureInfo.OrderedObjects.FindAll(x => x == procedures[selected].spProcedureInfo.OrderedObjects[index]).Count > 1)
                    {
                        procedures[selected].spProcedureInfo.OrderedObjects[index] = (GameObject)EditorGUI.ObjectField(rect, previousObjects[index], typeof(GameObject), true);
                        EditorUtility.DisplayDialog("Warning", "There shouldnt be duplicate objects in the procedure", "OK");
                    }

                    //Check if its a suitable object
                    if (procedures[selected].spProcedureInfo.OrderedObjects[index] != null && !procedures[selected].spProcedureInfo.OrderedObjects[index].GetComponent<SP_Hotspot>())
                    {
                        procedures[selected].spProcedureInfo.OrderedObjects[index] = (GameObject)EditorGUI.ObjectField(rect, previousObjects[index], typeof(GameObject), true);
                        EditorUtility.DisplayDialog("Warning", "This is not a hotspot", "OK");
                    }

                    //check if there is a change in the object
                    if (procedures[selected].spProcedureInfo.OrderedObjects[index] != previousObjects[index])
                    {
                        if (procedures[selected].spProcedureInfo.OrderedObjects[index])
                        {
                            Transform torder = procedures[selected].spProcedureInfo.OrderedObjects[index].transform.Find("SP_Order(Clone)");
                            GameObject order;
                            if (torder == null)
                                order = Instantiate(number, procedures[selected].spProcedureInfo.OrderedObjects[index].transform);
                            else
                                order = torder.gameObject;
                            procedures[selected].spProcedureInfo.OrderedObjects[index].GetComponent<SpriteRenderer>().sprite = circle;
                            TextMesh[] txts = order.GetComponentsInChildren<TextMesh>();
                            for(int t=0; t < txts.Length; t++)
                            {
                                txts[t].text = (index + 1).ToString();
                            }

                        }

                        if (previousObjects[index])
                        {
                            previousObjects[index].GetComponent<SpriteRenderer>().sprite = circleplus;
                            Transform order = previousObjects[index].transform.Find("SP_Order(Clone)");
                            if (order)
                                DestroyImmediate(order.gameObject);
                        }
                        Debug.Log("Object Changed" + index);
                    }
                    
                }
                rect.y += EditorGUIUtility.singleLineHeight + 2f;

            };
            inList.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
            inList.list = procedures[selected].spProcedureInfo.OrderedObjects;
            inList.DoLayoutList();
            if(procedures[selected].spProcedureInfo.OrderedObjects.Contains(null))
             EditorGUILayout.HelpBox("The fields should not be left null", MessageType.Warning);
            if (GUILayout.Button("Back"))
            {

                //dont create the procedure if its empty
                if (procedures[selected].spProcedureInfo.OrderedObjects.Count <= 0)
                {
                    if (EditorUtility.DisplayDialog("Info", "The procedure is not created if the list is empty", "OK"))
                    {
                        SP_Procedure temp = procedures[selected];
                        procedures.RemoveAt(selected);
                        DestroyImmediate(temp);
                        showProcedureList = true;
                    }
                }
                else showProcedureList = true;
            }

            previousObjects.Clear();
            previousObjects.AddRange(procedures[selected].spProcedureInfo.OrderedObjects);

        }
    }
}

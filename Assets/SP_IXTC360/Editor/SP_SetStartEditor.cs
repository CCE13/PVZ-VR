using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(SP_SetAsStartSphere))]
public class SP_SetStartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SP_SetAsStartSphere myScript = (SP_SetAsStartSphere)target;
        if (GUILayout.Button("Select As Start Position"))
        {
 
            myScript.SetStartPosition();

            GameObject maincamera = GameObject.Find("Main Camera");
            if (maincamera)
            {
                DestroyImmediate(maincamera);
            }

            GameObject DirectionalLight = GameObject.Find("Directional Light");
            if (DirectionalLight)
            {
                DestroyImmediate(DirectionalLight);
            }
        }
    }
}







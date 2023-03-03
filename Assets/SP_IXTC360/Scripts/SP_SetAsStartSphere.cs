using UnityEngine;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class SP_SetAsStartSphere : MonoBehaviour
{
#if UNITY_EDITOR
    public void SetStartPosition()
    {
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/SP_IXTC360/Prefabs/SP_Player.prefab", typeof(GameObject));
        SP_PlayerManager spMgr = SP_PlayerManager.Instance;

        GameObject spPlayer = GameObject.Find("SP_Player");
        if (!spPlayer)
        {
            spPlayer = Instantiate(prefab) as GameObject;
            spPlayer.name = "SP_Player";     
        }

        spPlayer.transform.position = this.transform.position;
    }
#endif
}

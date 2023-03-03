using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParanomicCapture : MonoBehaviour
{
    public bool stereoscopic=false;
    public Camera targetCamera;
    public RenderTexture cubemapleft, cubemapright;
    public RenderTexture equirectRT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Capture();
        }
    }
    public void Capture()
    {
        if (!stereoscopic)
        {
            targetCamera.RenderToCubemap(cubemapleft);
            cubemapleft.ConvertToEquirect(equirectRT);
        }
        else
        {
            targetCamera.stereoSeparation = 0.065f;
            targetCamera.RenderToCubemap(cubemapleft, 63, Camera.MonoOrStereoscopicEye.Left);
            targetCamera.RenderToCubemap(cubemapright, 63, Camera.MonoOrStereoscopicEye.Right);


            cubemapleft.ConvertToEquirect(equirectRT, Camera.MonoOrStereoscopicEye.Left);
            cubemapright.ConvertToEquirect(equirectRT, Camera.MonoOrStereoscopicEye.Right);
        }
        Debug.Log("print");
        Save(equirectRT);
    }
    public void Save(RenderTexture rt)
    {
        Texture2D tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToJPG();
        string path = Application.dataPath + "/Panaroma" + ".jpg";
        System.IO.File.WriteAllBytes(path, bytes);
    }
    
}

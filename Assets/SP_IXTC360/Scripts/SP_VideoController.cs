using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.IO;

[ExecuteAlways]
public class SP_VideoController : MonoBehaviour
{
    public VideoPlayer video;
    public Slider slider;
    float slidervalue = 0.0F;
    bool IsPlaying = true;
    public GameObject StopButton;
    public GameObject PlayButton;
    public GameObject CaptureButton;
    public Text videoframe;
    Texture2D screenshotviewer;
    bool isScrubbing = false;

    //Return a float duration in seconds of the selected video
    public float Duration
    {
        //Framcount divide by Framerate(frame/sec), gets duratuion of video in secs
        get { return video.frameCount / video.frameRate; }
    }

    void OnEnable()
    {
        //Change the slider's max value to the duration of the video
        slider.maxValue = Mathf.RoundToInt(Duration);
    }

    //Coroutine to play() and pause() video to get the the frame
    IEnumerator WaitAfterSeek()
    {
        Debug.Log("wait for 0.02 seconds then pause");
        yield return new WaitForSeconds(0.02F);
        //Input the current Frame index in the Frame Number GameObject text
        GameObject VF = GameObject.Find("Frame Number");
        VF.GetComponent<Text>().text = video.frame.ToString();
        Debug.Log(VF.GetComponent<Text>().text);
    }

    //Pause at the selected scrubbed position(value) on the Video Capture Editor Window slider
    public void Seekvaluewithstop(float value)
    {
        //Video needs to play() first before it can be paused
        Debug.Log("seeking value");
        video.Play();
        //Video time is going to continue playing at where it pause, the old value
        //Input the new scrubbed value for the new time
        video.time = value;
        Debug.Log("seek value" + value);
        //start the coroutine to pause it immediately after it plays
        StartCoroutine(WaitAfterSeek());
        video.Pause();
    }

    public void PlayVideo()
    {
        video.Play();
        //isScrubbing = false;
        Debug.Log("play the video");
        StopButton.SetActive(true);
        PlayButton.SetActive(false);
        //IsPlaying = true;
    }

    public void StopVideo()
    {
        video.Pause();
        //Current slider value stored, after every StopVideo(), so the next scrubbed value will be differnet
        slidervalue = slider.value;
        //isScrubbing = true;
        //IsPlaying = false;
        Debug.Log("Stop the video");
        StopButton.SetActive(false);
        PlayButton.SetActive(true);
    }

    //public void CaptureFrame()
    //{
    //    //Texture of a video player is a render texture by default
    //    Texture rt = video.texture;

    //    if (rt)
    //    {
    //        //cast(convert) rt into a current render texture, only only active rendertexture can replace the main camea screen
    //        //rendertexture.active is the current active screen, so it wont capture the players view.
    //        RenderTexture.active = (RenderTexture)rt;
    //        //create new texture slot for the screenshot, using the rt height and width, follow the actual video size base on the import settings
    //        Texture2D Screenshot = new Texture2D(rt.width, rt.height);
    //        //read pixels only work for texture 2d, which captures the whatevers on screen into a saved data(texture 2d)               
    //        Screenshot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, true);
    //        Screenshot.Apply();
    //        screenshotviewer = Screenshot;
    //        Debug.Log(rt.width);
    //        Debug.Log(rt.height);

    //        SaveImage();
    //    }

    //}

    ////save texture in user's selected folder
    //public void SaveImage()
    //{
    //    byte[] fileData;
    //    fileData = screenshotviewer.EncodeToPNG();
    //    //save file in wherer ever user selects
    //    string userselectedpath = EditorUtility.SaveFilePanel("Save Captured Image", "", "Placeholder", "png");

    //    if (userselectedpath.Length > 0)
    //    {
    //        File.WriteAllBytes(userselectedpath, fileData);
    //        Debug.Log("save Image");
    //        AssetDatabase.Refresh();
    //    }
    //    else
    //    {
    //        Debug.Log("Cancelled save");
    //    }
    //}
}


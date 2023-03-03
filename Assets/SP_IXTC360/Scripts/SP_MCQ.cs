using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SP_MCQInformation
{
    public string hotspotname = "";
    public string Question = "Ask a Question";
    public List<string> Options = new List<string>();
    public List<bool> Answers = new List<bool>();
}

public class SP_MCQ : MonoBehaviour
{
    public SP_MCQInformation spMCQInfo = new SP_MCQInformation();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

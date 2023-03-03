using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SP_MCQDisplayInformation : MonoBehaviour, IPointerExitHandler
{
    public Transform AnswerOptionsParent;
    public GameObject optionPrefab;
    public Button SubmitBtn;
    GameObject MCQHotspot;
    public Text Question;
    public List<bool> CorrectAnswers;

    // Start is called before the first frame update

    public void SetAnswerOptions(List<string> opt, GameObject MCQhotspotobj)
    {
        MCQHotspot = MCQhotspotobj;
        foreach (Transform child in AnswerOptionsParent)
        { GameObject.Destroy(child.gameObject);}

        SP_Status status = MCQHotspot.GetComponent<SP_Status>();
        if (!status)
        {
            for (int i = 0; i < opt.Count; i++)
            {
                GameObject AnswerBtn = Instantiate(optionPrefab, AnswerOptionsParent);
                AnswerBtn.GetComponentInChildren<Text>().text = opt[i];
                Button ansbtn = AnswerBtn.GetComponent<Button>();
                ansbtn.onClick.AddListener(() => { Clicked(AnswerBtn); });
                ansbtn.enabled =true;
                if (CorrectAnswers[i]) AnswerBtn.AddComponent<SP_CorrectAnswer>();
            }
            SubmitBtn.enabled = true;
            SubmitBtn.transform.Find("Submit").gameObject.SetActive(true);
            SubmitBtn.transform.Find("Correct").gameObject.SetActive(false);
            SubmitBtn.transform.Find("Wrong").gameObject.SetActive(false);
        }
        else
        {
            while (MCQHotspot.transform.childCount > 0)
            {
                Button btn = MCQHotspot.transform.GetChild(MCQHotspot.transform.childCount - 1).GetComponent<Button>();
                if (btn)
                {
                    btn.enabled = false;
                }
                MCQHotspot.transform.GetChild(MCQHotspot.transform.childCount - 1).SetParent(AnswerOptionsParent, false);
            }
            SubmitBtn.transform.Find("Submit").gameObject.SetActive(false);
            SubmitBtn.transform.Find("Correct").gameObject.SetActive(status.Correct);
            SubmitBtn.transform.Find("Wrong").gameObject.SetActive(!status.Correct);

            SubmitBtn.enabled = false;
        }
        
    }


    public void Clicked(GameObject obj)
    {
        GameObject blueobj = obj.transform.Find("blueborder").gameObject;
        blueobj.SetActive(!blueobj.activeSelf);
    }

    public void Submit()
    {
        bool NoAnswerSelected = true;
        for (int j = 0; j < AnswerOptionsParent.childCount; j++)
        {
            Transform AnswerChild = AnswerOptionsParent.GetChild(j);
            GameObject blueborder = AnswerChild.Find("blueborder").gameObject;
            if (blueborder.activeSelf)
            {
                NoAnswerSelected = false;
                break;
            }
        }
        if (NoAnswerSelected) return;

        bool finalAnswer =true;
        for (int i = 0; i < AnswerOptionsParent.childCount; i++)
        {
            Transform AnswerChild = AnswerOptionsParent.GetChild(i);
            GameObject blueborder =AnswerChild.Find("blueborder").gameObject;
            AnswerChild.GetComponent<Button>().enabled = false;
            if(blueborder.activeSelf)
            {
                if (CorrectAnswers[i])
                {
                    AnswerChild.Find("greenborder").gameObject.SetActive(true);
                    finalAnswer &= true;
                }
                else
                {
                    AnswerChild.Find("redborder").gameObject.SetActive(true);
                    finalAnswer &= false;
                }
            }
            else
            {
                if (CorrectAnswers[i])
                {
                    AnswerChild.Find("greenborder").gameObject.SetActive(true);
                    finalAnswer &= false;
                }
                else
                {
                    AnswerChild.Find("redborder").gameObject.SetActive(true);
                    finalAnswer &= true;
                }
            }
        }

        SubmitBtn.transform.Find("Submit").gameObject.SetActive(false);
        SubmitBtn.transform.Find("Correct").gameObject.SetActive(finalAnswer);
        SubmitBtn.transform.Find("Wrong").gameObject.SetActive(!finalAnswer);

        SubmitBtn.enabled = false;
        if (MCQHotspot)
        {
            SP_Status spstatus = MCQHotspot.AddComponent<SP_Status>();
            spstatus.Correct = finalAnswer;
            MCQHotspot.transform.Find("tick").gameObject.SetActive(finalAnswer);
            MCQHotspot.transform.Find("cross").gameObject.SetActive(!finalAnswer);
            MCQHotspot.transform.Find("qn").gameObject.SetActive(false);
            if (finalAnswer) SP_SoundManager.Instance.PlaySound("Correct");
            else SP_SoundManager.Instance.PlaySound("Incorrect");

        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OffMCQ()
    {
        if (MCQHotspot.GetComponent<SP_Status>())
        {
            while (AnswerOptionsParent.childCount > 0)
            {
                AnswerOptionsParent.GetChild(AnswerOptionsParent.childCount - 1).SetParent(MCQHotspot.transform, false);
            }
        }

        this.gameObject.SetActive(false);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OffMCQ();
    }
}

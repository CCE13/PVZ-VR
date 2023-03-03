using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SP_ProcedureInformation
{
    public string procedureName = "Procedure Name";
    public bool Assessment = false;
    public Color clrRep = Color.black;
    public List<GameObject> OrderedObjects = new List<GameObject>();
}
public class SP_Procedure : MonoBehaviour
{
    public SP_ProcedureInformation spProcedureInfo = new SP_ProcedureInformation();
    int currentstep = 0;
    //AudioSource correct, wrong;
    int colorstate = 0;
    GameObject ring;
    GameObject Correct, Wrong;
    // Start is called before the first frame update
    void Start()
    {
        if (spProcedureInfo.Assessment)
        {
            for (int i = 0; i < spProcedureInfo.OrderedObjects.Count; i++)
            {
                if (spProcedureInfo.OrderedObjects[i])
                {
                    spProcedureInfo.OrderedObjects[i].SetActive(true);
                    TextMesh[] _txts = spProcedureInfo.OrderedObjects[i].GetComponentsInChildren<TextMesh>();
                    for (int t = 0; t < _txts.Length; t++)
                    {
                        _txts[t].text = "?";
                        _txts[t].color = spProcedureInfo.clrRep;
                    }
                }
                else
                {
                    spProcedureInfo.OrderedObjects.RemoveAt(i); //remove the null
                    i -= 1;
                }
            }

            Correct = Instantiate(Resources.Load<GameObject>("SP_Correct"));
            Wrong = Instantiate(Resources.Load<GameObject>("SP_InCorrect"));
            Correct.SetActive(false);
            Wrong.SetActive(false);
            //correct = this.gameObject.AddComponent<AudioSource>();
            //correct.clip = Resources.Load<AudioClip>("Sounds/Correct");
            //wrong = this.gameObject.AddComponent<AudioSource>();
            //wrong.clip = Resources.Load<AudioClip>("Sounds/Incorrect");
        }
        else
        {
            for (int i = 0; i < spProcedureInfo.OrderedObjects.Count; i++)
            {
                if (spProcedureInfo.OrderedObjects[i])
                {
                    spProcedureInfo.OrderedObjects[i].SetActive(false);
                    TextMesh[] _txts = spProcedureInfo.OrderedObjects[i].GetComponentsInChildren<TextMesh>();
                    for (int t = 0; t < _txts.Length; t++)
                    {
                        _txts[t].text = (i + 1).ToString();
                        _txts[t].color = spProcedureInfo.clrRep;
                    }

                }
                else
                {
                    spProcedureInfo.OrderedObjects.RemoveAt(i);
                    i -= 1;
                }
            }
            if (spProcedureInfo.OrderedObjects[0])
            {
                spProcedureInfo.OrderedObjects[0].SetActive(true);
                ring = Instantiate(Resources.Load<GameObject>("SP_Ring"), spProcedureInfo.OrderedObjects[0].transform);
                if (spProcedureInfo.clrRep == Color.black)
                    ring.GetComponent<SpriteRenderer>().color = Color.white;
                else
                    ring.GetComponent<SpriteRenderer>().color = spProcedureInfo.clrRep;
            }
        }
        SP_MainCamera.HotSpotClicked += HotSpotClicked;
    }

    void HotSpotClicked(GameObject gameObject)
    {
        Debug.Log("Clicked hotspot");
        //check if object belongs to prcoedure
        if (!spProcedureInfo.OrderedObjects.Contains(gameObject)) return; 
        if (spProcedureInfo.Assessment)
        {
            if (SP_MainCamera.Instance){ SP_MainCamera.Instance.filterRaycast = true; }
            if (spProcedureInfo.OrderedObjects[currentstep] == gameObject)
            {
                SpriteRenderer currentsprite = gameObject.GetComponent<SpriteRenderer>();
                currentsprite.enabled = false;
                //currentsprite.color = Color.green;
                Correct.transform.SetParent(gameObject.transform);
                Correct.transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
                Correct.transform.localPosition = Vector3.zero;
                Correct.transform.localEulerAngles = Vector3.zero;
                Correct.SetActive(true);
                TextMesh[] txts = gameObject.GetComponentsInChildren<TextMesh>();
                for (int t = 0; t < txts.Length; t++)
                {
                    txts[t].text = (currentstep + 1).ToString();
                    txts[t].gameObject.SetActive(false);
                }
                Debug.Log("Correct");
                SP_SoundManager.Instance.PlaySound("Correct");
                currentstep++;
                StartCoroutine(EnableRaycast(txts, currentsprite, 1.1f));// currentsprite));

                //show that its done
                gameObject.AddComponent<SP_Status>();
            }
            else
            {
                SP_SoundManager.Instance.PlaySound("Incorrect");
                SpriteRenderer currentsprite = gameObject.GetComponent<SpriteRenderer>();
                currentsprite.enabled = false;
                //currentsprite.color = Color.green;
                Wrong.transform.SetParent(gameObject.transform);
                Wrong.transform.localScale = new Vector3(-1.3f, 1.3f, 1.3f);
                Wrong.transform.localPosition = Vector3.zero;
                Wrong.transform.localEulerAngles = Vector3.zero;
                Wrong.SetActive(true);
                TextMesh[] txts = gameObject.GetComponentsInChildren<TextMesh>();
                for (int t = 0; t < txts.Length; t++)
                {
                    txts[t].gameObject.SetActive(false);
                }
                StartCoroutine(EnableRaycast(txts, currentsprite, 0.8f));
                //colorstate = 1;
                //StartCoroutine(FlashRed(currentsprite));
                Debug.Log("Wrong");
            }
        }
        else
        {
            if (spProcedureInfo.OrderedObjects[currentstep] == gameObject)
            {
                //spProcedureInfo.OrderedObjects[currentstep].SetActive(false);
                currentstep++;

                if (currentstep < spProcedureInfo.OrderedObjects.Count)
                {
                    spProcedureInfo.OrderedObjects[currentstep].SetActive(true);
                    ring.transform.SetParent(spProcedureInfo.OrderedObjects[currentstep].transform);
                    ring.transform.localPosition = Vector3.zero;
                    ring.transform.localEulerAngles = Vector3.zero;
                }
                else ring.SetActive(false);
            }
        }
    }


    private IEnumerator EnableRaycast(TextMesh[] txts, SpriteRenderer currentsprite, float delay)
    {
        yield return new WaitForSeconds(delay);
        currentsprite.enabled = true;
        Correct.SetActive(false);
        Wrong.SetActive(false);
        for (int t = 0; t < txts.Length; t++)
        {
            txts[t].gameObject.SetActive(true);
        }
        //currentsprite.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        SP_MainCamera.Instance.filterRaycast = false;
    }
    private IEnumerator FlashRed(SpriteRenderer currentsprite)
    {
        while (colorstate != 0)
        {
            if (colorstate == 1)
            {
                if (currentsprite)
                {
                    currentsprite.color = Color.Lerp(Color.white, Color.red, 1f);
                    if (currentsprite.color == Color.red) { colorstate = 2; }
                }
            }
            else if (colorstate == 2)
            {
                currentsprite.color = Color.Lerp(Color.red, Color.white, 1f);
                if (currentsprite.color == Color.white) { colorstate = 0; }
            }
            yield return new WaitForSeconds(0.3f);
        }
        
        SP_MainCamera.Instance.filterRaycast = false;

    }
    // Update is called once per frame
    void Update()
    {
      
    }
}

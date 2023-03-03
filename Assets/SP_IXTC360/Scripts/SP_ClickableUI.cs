using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SP_ClickableUI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public virtual void OnClick()
    {

    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }

    bool mouseIsWithinImage()
    {
        //Debug.Log("Rect position "+ this.GetComponent<RectTransform>().position + ", mouse pos " + Input.mousePosition);

        Vector2 mousePos = Input.mousePosition;

        float RectPosX = this.GetComponent<RectTransform>().position.x;
        float RectPosy = this.GetComponent<RectTransform>().position.y;
        float RectWidth = this.GetComponent<RectTransform>().rect.width * 0.5f;
        float RectHeight = this.GetComponent<RectTransform>().rect.height * 0.5f;

        Debug.Log(RectPosX + " , " + RectPosy + ", " + RectWidth + " , " + RectHeight);

        bool withinX = false;
        bool withinY = false;
        if ((mousePos.x < (RectPosX + RectWidth)) && (mousePos.x > (RectPosX - RectWidth)))
        {
            withinX = true;
        }
        else
        {
            return false;
        }
        if ((mousePos.y < (RectPosy + RectHeight)) && (mousePos.y > (RectPosy - RectHeight)))
        {
            withinY = true;
        }
        else
        {
            return false;
        }

        if (withinX && withinY)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

        if (mouseIsWithinImage())  
        {
            OnEnter();

            if (Input.GetMouseButtonDown(0))
            {
               
                OnClick();
            }
        }
        else
        {
            OnExit();
        }


    }



}

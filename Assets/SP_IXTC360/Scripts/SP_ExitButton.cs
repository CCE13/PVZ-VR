using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SP_ExitButton : SP_ClickableUI
{

    Color32 color = new Color32(197, 197, 197, 255);
    public override void OnClick()
    {

        Application.Quit();
        Debug.Log("exit button click");
    }

    public override void OnEnter()
    {
        this.gameObject.GetComponent<Image>().color = color;

    }

    public override void OnExit()
    {
        if (this.gameObject.GetComponent<Image>().color == color)

        {
            this.gameObject.GetComponent<Image>().color = Color.white;
        }


    }
}


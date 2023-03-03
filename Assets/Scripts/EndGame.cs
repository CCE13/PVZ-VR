using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndGame : MonoBehaviour,IAttackable
{
    public static event Action GameOver;

    public void OnAttacked()
    {
        //Invoke the game over event when a zombie attacks the house
        GameOver?.Invoke();
        //AudioManager.Instance.Play("NOOOO");
    }
}

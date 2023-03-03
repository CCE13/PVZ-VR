using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundSound;
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

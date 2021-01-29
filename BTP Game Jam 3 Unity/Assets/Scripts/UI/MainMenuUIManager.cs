using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject credits;


    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

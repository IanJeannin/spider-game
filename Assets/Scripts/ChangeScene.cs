using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string mainMenu="MainMenu";
    [SerializeField]
    private string gameScene="LevelOne";
    [SerializeField]
    private string credits="Credits";
    [SerializeField]
    private string victory="Victory";

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(credits);
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void GoToVictory()
    {
        SceneManager.LoadScene(victory);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GoToVictory();
    }
}

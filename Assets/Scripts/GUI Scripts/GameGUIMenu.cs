using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUIMenu : MonoBehaviour
{
    private bool _levels;
    public Canvas LevelCanvas;
    public Canvas MainMenuCanvas;
    public Button levelsButton;
    public Button QuitButton;

    // remove o canvas dos niveis
    public void LevelCanvasRemove()
    {
        LevelCanvas.enabled = false;
    }

    
    public void LevelsPressed()
    {
        // se o jogo ja estiver em pause, cancelar a pausa
        if (_levels)
        {
            LevelCanvas.enabled = false;
            MainMenuCanvas.enabled = true;
            _levels = false;

        }
    
        // se nao estiver pausado, dar pause
        else
         {
                 LevelCanvas.enabled = true;
                 MainMenuCanvas.enabled = false;
                _levels = true;

         }

      }
    // se estiver em modo quit
    public void QuitPressed()
    {
        LevelCanvas.enabled = false;
        MainMenuCanvas.enabled = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadScene3()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadScene4()
    {
        SceneManager.LoadScene(4);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
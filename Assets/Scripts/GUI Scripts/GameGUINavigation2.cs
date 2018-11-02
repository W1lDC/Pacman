using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUINavigation2 : MonoBehaviour {


	private bool _paused;
    private bool quit;
    public float startdelay;
    private GameManager2 gm;
    public GameObject MainMenuCanvas;
    public GameObject maze;
    public GameObject canvasTitle;
    public Canvas PauseCanvas;
    public Canvas GameOverCanvas;
	public Canvas QuitCanvas;
    public Canvas YouWinCanvas;
    public Button yesButton;
    public Button cancelButton;
    public AudioClip beggining;


    public void Start ()  // parametros de inicio de jogo
	{
        Time.timeScale = 0;
        MainMenuCanvas.SetActive(true);
        maze.SetActive(true);
        canvasTitle.SetActive(false);

        gm = GameObject.Find("Game Manager2").GetComponent<GameManager2>();

    }
	
    public void startGame() // parametros necessarios para iniciar o jogo
    {
        StartCoroutine("TimeToStart", startdelay);
        MainMenuCanvas.SetActive(false);
        PauseCanvas.enabled = false;
        canvasTitle.SetActive(true);
        Time.timeScale = 1;
        GetComponent<AudioSource>().Play();
    }

   

    public IEnumerator TimeToStart(float seconds) // passa o tempo de espera antes de iniciar o jogo
	{
		GameManager2.gameState = GameManager2.GameState.Initiate;
		yield return new WaitForSeconds(seconds);
		GameManager2.gameState = GameManager2.GameState.Game;
		
	}

    public void PauseCanvasRemove()
    {
        PauseCanvas.enabled = false;
    }

	public void PausePressed()
	{
        // se o jogo ja estiver em pause, cancelar a pausa
		if(_paused)
		{
			Time.timeScale = 1;
			PauseCanvas.enabled = false;
			_paused = false;
            
        }

        // se nao estiver pausado, dar pause
        else
		{
			PauseCanvas.enabled = true;
			Time.timeScale = 0.0f;
			_paused = true;
			
		}

	}

    public void QuitPressed()
    {
        if (quit)
        {
            PauseCanvas.enabled = true;
            QuitCanvas.enabled = false;
            quit = false;
            
        }

        else
        {
            QuitCanvas.enabled = true;
            PauseCanvas.enabled = false;
            quit = true;
        }
    }

    

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(3);
    }


    public void GetGameOver() // passa os parametros necessarios para quando o user perde
    {
        GameOverCanvas.enabled = true;
        maze.SetActive(false);
        canvasTitle.SetActive(false);
        
        gm.DestroyThem();
    }
    public void GetYouWin() // passa os parametros necessarios para quando o user perde
    {
        YouWinCanvas.enabled = true;
        GameOverCanvas.enabled = false;
        maze.SetActive(false);
        canvasTitle.SetActive(false);
        gm.DestroyThem();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}

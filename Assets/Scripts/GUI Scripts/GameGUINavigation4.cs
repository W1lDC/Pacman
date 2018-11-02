using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUINavigation4 : MonoBehaviour {


	private bool _paused;
    private bool quit;
    public float startdelay;
    private GameManager4 gm;
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


    public void Start ()  // passa os valores para iniciar o jogo
	{
        Time.timeScale = 0;
        MainMenuCanvas.SetActive(true);
        maze.SetActive(true);
        canvasTitle.SetActive(false);

        gm = GameObject.Find("Game Manager4").GetComponent<GameManager4>();

    }
	
    public void startGame() // parametros necessarios para o inicio de jogo
    {
        StartCoroutine("TimeToStart", startdelay);
        MainMenuCanvas.SetActive(false);
        PauseCanvas.enabled = false;
        canvasTitle.SetActive(true);
        Time.timeScale = 1;
        GetComponent<AudioSource>().Play();
    }

   

    public IEnumerator TimeToStart(float seconds) // tempo de espera antes de começar a jogar
	{
		GameManager4.gameState = GameManager4.GameState.Initiate;
		yield return new WaitForSeconds(seconds);
		GameManager4.gameState = GameManager4.GameState.Game;
		
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

    public void QuitPressed() // se estiver em modo quit
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

    public void GetGameOver() // parametros passados para quando o user perde
    {
        GameOverCanvas.enabled = true;
        maze.SetActive(false);
        canvasTitle.SetActive(false);
        gm.DestroyThem();
    }
    public void GetYouWin() // parametros passados para quando o user perde
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

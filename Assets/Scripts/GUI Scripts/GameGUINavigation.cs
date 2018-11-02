using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class GameGUINavigation : MonoBehaviour {


	private bool _paused;
    private bool quit;
    public float startdelay;
    private GameManager gm;
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


    public void Start () 
	{
        Time.timeScale = 0; // inicia o tempo do jogo a zero
        MainMenuCanvas.SetActive(true); // ativa o canvas
        maze.SetActive(true);// ativa o maze
        canvasTitle.SetActive(false); // desativa o canvas
        
        gm = GameObject.Find("Game Manager").GetComponent<GameManager>();

    }
	
    public void startGame() // passa os parametros necessarios para iniciar o jogo
    {
        StartCoroutine("TimeToStart", startdelay);
        MainMenuCanvas.SetActive(false);
        PauseCanvas.enabled = false;
        canvasTitle.SetActive(true);
        Time.timeScale = 1;
        GetComponent<AudioSource>().Play();
    }

   

    public IEnumerator TimeToStart(float seconds) // passa o tempo de espera para o jogo começar e muda o estado de jogo
	{
		GameManager.gameState = GameManager.GameState.Initiate;
		yield return new WaitForSeconds(seconds);
		GameManager.gameState = GameManager.GameState.Game;
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

    public void LoadNextScene()
    {
        SceneManager.LoadScene(2);
    }

    public void GetGameOver() // parametros de quando o user perder
    {
        GameOverCanvas.enabled = true;
        YouWinCanvas.enabled = false;
        maze.SetActive(false);
        canvasTitle.SetActive(false);
        
        gm.DestroyThem();
    }

    public void GetYouWin() // parametros de quando o user ganha
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

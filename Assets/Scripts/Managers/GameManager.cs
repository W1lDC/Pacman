using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public enum GameState { mainMenu, Initiate, Game, Dead }
    public static GameState gameState;
    public GameObject pacman;
    private GameObject blinky;
    private GameObject pinky;
    private GameObject inky;
    private GameObject clyde;
    public GameObject canvasTitle;
    public GameObject maze;
    public GameObject ghosts;
    private GameGUINavigation gui;
    public Sprite[] pacmanLifes;
    public Image lifesPacman;
    public static bool scared;
    private int currentLives;
    public float scareLength;
    private float _timeToCalm;
    public AudioClip scaresound;
    

    void Awake() // chama a funçao quando o script é chamado
    {
        DesignateGhosts();
    }

    void Start() 
    {
        gui = GameObject.Find("UI Manager").GetComponent<GameGUINavigation>();
        gui.startGame();
        gameState = GameState.Initiate;
    }

    void Update()
    {
        if (scared && _timeToCalm <= Time.time) // se os ghosts tiverem no modo scared e o tempo para 
            CalmGhosts();
    }

    public void ResetVarScene()
    {
        CalmGhosts();
        //dar reset a todas as posiçoes dos ghosts e inicia os ghosts e a posiçao de inicio do pacman
        pacman.transform.position = new Vector3(15f, 11f, 0f);
        blinky.transform.position = new Vector3(15f, 20f, 0f);
        pinky.transform.position = new Vector3(14.5f, 17f, 0f);
        inky.transform.position = new Vector3(16.5f, 17f, 0f);
        clyde.transform.position = new Vector3(12.5f, 17f, 0f);
        pacman.GetComponent<PlayerController>().ResetDestination();
        blinky.GetComponent<GhostMove>().InitGhosts();
        pinky.GetComponent<GhostMove>().InitGhosts();
        inky.GetComponent<GhostMove>().InitGhosts();
        clyde.GetComponent<GhostMove>().InitGhosts();
        
        gameState = GameState.Initiate;
        gui.startGame();
    }

    private void ResetVarGhosts() // dá reset nas variaveis a ver com os ghosts
    {
        _timeToCalm = 0.0f;
        scared = false;
    }

    public void ToggleScare()// se os ghosts nao estiverem no modo scare, atribui o mesmo... se estiverem, atribui o modo calm
    {
        if (!scared) ScareGhosts();
        else CalmGhosts();
    }

    public void ScareGhosts() // passa o metodo Scare() a todos os ghosts e inicia o som
    {
        scared = true;
        blinky.GetComponent<GhostMove>().Scare();
        pinky.GetComponent<GhostMove>().Scare();
        inky.GetComponent<GhostMove>().Scare();
        clyde.GetComponent<GhostMove>().Scare();
        _timeToCalm = Time.time + scareLength;
        GetComponent<AudioSource>().Play();


        Debug.Log("Ghosts Scared");
    }

    public void CalmGhosts() // os ghosts deixam de fugir do pacman, caso o estejam a fazer
    {
        scared = false;
        blinky.GetComponent<GhostMove>().Calm();
        pinky.GetComponent<GhostMove>().Calm();
        inky.GetComponent<GhostMove>().Calm();
        clyde.GetComponent<GhostMove>().Calm();
        GetComponent<AudioSource>().Stop();
    }

    public void updateGameState()// dá update ao estado do jogo, bem como os componentes que devem estar ativos ou nao
    {
        switch (gameState)
        {
            case GameState.mainMenu:
                Cursor.visible = true;
                canvasTitle.SetActive(false);
                maze.SetActive(false);
                ghosts.SetActive(false);
                pacman.SetActive(false);
                break;

            case GameState.Initiate:
                Cursor.visible = false;
                canvasTitle.SetActive(true);
                maze.SetActive(true);
                ghosts.SetActive(true);
                pacman.SetActive(true);
                break;
        }
    }

    void DesignateGhosts()
    {
        // procurar pelos game objects pelos nomes e atribui cada um
        clyde = GameObject.Find("clyde");
        pinky = GameObject.Find("pinky");
        inky = GameObject.Find("inky");
        blinky = GameObject.Find("blinky");
        pacman = GameObject.Find("pacman");

        if (clyde == null || pinky == null || inky == null || blinky == null) Debug.Log("One of the ghosts are NULL");
        if (pacman == null) Debug.Log("Pacman is NULL");

        gui = GameObject.FindObjectOfType<GameGUINavigation>();

        if (gui == null) Debug.Log("GUI Null!");
    }

    public void LoseLife()// remove uma vida ao numero de vidas restante do pacman, bem como a sua representaçao visual
    {
        pacman.GetComponent<PlayerController>().lives--;
        currentLives = pacman.GetComponent<PlayerController>().lives;
        gameState = GameState.Dead;
        lifesPacman.sprite = pacmanLifes[currentLives];
    }

    public void DestroyThem()// destroi os fantasmas e o pacman
    {
        ghosts.SetActive(false);
        pacman.SetActive(false);
    }
}

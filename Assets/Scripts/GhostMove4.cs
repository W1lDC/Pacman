using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GhostMove4 : MonoBehaviour
{

    private Vector3 waypoint;           
    private Queue<Vector3> waypoints;  //  waypoints usados para o Initiate e Scatter

    public Vector3 _direction;
    public Vector3 direction
    {
        get
        {
            return _direction;
        }

        set
        {
            _direction = value;
            Vector3 pos = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
            waypoint = pos + _direction;
        }
    }

    public float speed = 0.3f;
    public float scatterLength = 5f;
    public float waitLength = 0.0f;

    private float timeToEndScatter;
    private float timeToEndWait;

    enum State { Wait, Init, Scatter, Chase, Run };
    State state;

    private Vector3 _startPos;
    private float _timeToWhite;
    private float _timeToToggleWhite;
    private float _toggleInterval;
    private bool isWhite = false;

    public GameGUINavigation4 GUINav;
    public PlayerController4 pacman;
    private GameManager4 _gm;

    public AudioClip eatGhost;

    void Awake()
    {
        GetComponent<AudioSource>().clip = eatGhost;
        GetComponent<AudioSource>().playOnAwake = false;
    }

    void Start()
    {
        _gm = GameObject.Find("Game Manager4").GetComponent<GameManager4>();
        _toggleInterval = _gm.scareLength * 0.33f * 0.20f;
        InitGhosts();
    }

    public float DISTANCE;

    void FixedUpdate()
    {
        DISTANCE = Vector3.Distance(transform.position, waypoint);

        if (GameManager4.gameState == GameManager4.GameState.Game)
        {
            animate();

            switch (state)
            {
                case State.Wait:
                    Wait();
                    break;

                case State.Init:
                    Initiate();
                    break;

                case State.Scatter:
                    Scatter();
                    break;

                case State.Chase:
                    ChaseAI();
                    break;

                case State.Run:
                    RunAway();
                    break;
            }
        }
    }


    public void InitGhosts()// posiciona os ghosts no maze, passa o valor do tempo de espera antes do jogo começar
    {
        _startPos = getStartPosAccordingToName();
        waypoint = transform.position;  
        state = State.Wait;
        timeToEndWait = Time.time + waitLength + GUINav.startdelay;
        InitializeWaypoints(state);
    }

    public void InitGhosts(Vector3 pos)// passa o valor do vetor para a posiçao dos ghosts
    {
        transform.position = pos;
        waypoint = transform.position;	
        state = State.Wait;
        timeToEndWait = Time.time + waitLength + GUINav.startdelay;
        InitializeWaypoints(state);
    }


    private void InitializeWaypoints(State st)
    {
        // Coordenadas do Initiate e do Scatter separadas por uma linha em branco

        // waypoints hardcoded de acordo com o nome do fantasma
        string data = "";
        switch (name)
        {
            case "blinky":
                data = @"19 20
22 20
22 17
25 17

25 23
27 23
27 30
22 30";
                break;
            case "pinky":
                data = @"14.5 17
14 17
14 20
10 20
10 17
10 11
7 11

4 11
4 23
2 23
2 30";
                break;
            case "inky":
                data = @"16.5 17
15 17
15 20
19 20
19 11
2 11
2 5

13 5
13 8
19 8
19 5
19 2
16 2
10 2
27 2
27 5
22 5";
                break;
            case "clyde":
                data = @"12.5 17
14 17
14 20
10 20
10 17
10 11
7 11

2 11
2 8
22 8
22 2
13 2
2 2
2 5
2 11";
                break;

        }

        // receber dos waypoints hardcoded
        string line;

        waypoints = new Queue<Vector3>();
        Vector3 wp;

        if (st == State.Init)
        {
            using (StringReader reader = new StringReader(data))// le os valores do método InitializeWaypoints() e faz o parse dos mesmos
            {
                // se chegar a linha em branco, para de receber
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 0) break;

                    string[] values = line.Split(' ');
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);

                    wp = new Vector3(x, y, 0);
                    waypoints.Enqueue(wp);
                }
            }
        }

        if (st == State.Scatter)
        {
            // passa a frente ate a linha em branco chegar, e le as coordenadas depois disso
            bool scatterWps = false;	// waypoints dos ghosts se espalharem

            using (StringReader reader = new StringReader(data))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        scatterWps = true; // alcançou-se um waypoint do estado Scatter
                        continue; // nao le a linha em branco e passa para a seguinte
                    }

                    if (scatterWps)
                    {
                        string[] values = line.Split(' ');
                        int x = Int32.Parse(values[0]);
                        int y = Int32.Parse(values[1]);

                        wp = new Vector3(x, y, 0);
                        waypoints.Enqueue(wp);
                    }
                }
            }
        }

        // se se encontrar no estado Wait
        if (st == State.Wait)
        {
            Vector3 pos = transform.position;

            // o clyde e o inky começam a andar de baixo para cima
            if (transform.name == "inky" || transform.name == "clyde")
            {
                waypoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
                waypoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
            }
            //enquanto que o pinky começa a andar de cima para baixo
            else
            {
                waypoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
                waypoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
            }
        }

    }

    private Vector3 getStartPosAccordingToName()// valor das posiçoes dos ghosts no inicio do jogo
    {
        switch (gameObject.name)
        {
            case "blinky":
                return new Vector3(15f, 20f, 0f);

            case "pinky":
                return new Vector3(14.5f, 17f, 0f);

            case "inky":
                return new Vector3(16.5f, 17f, 0f);

            case "clyde":
                return new Vector3(12.5f, 17f, 0f);
        }

        return new Vector3();
    }

  
    void animate()// passa a animaçao de acordo com a direçao
    {
        Vector3 dir = waypoint - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
        GetComponent<Animator>().SetBool("Run", false);
    }

    void OnTriggerEnter2D(Collider2D other)// metodo de colisao que verifica se é perdida uma vida caso os ghosts nao estiverem no estado run, ou se estiverem, aumenta o score e destroy os ghosts
    {
        if (other.name == "pacman")
        {
            if (state == State.Run)
            {
                Calm();
                InitGhosts(_startPos);
                var SCORE = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<ScoreCounter4>();
                SCORE.updateScoreGhosts();
                GetComponent<AudioSource>().Play();
                Debug.Log("Ghost Killed");
            }

            else
            {
                _gm.LoseLife();
            }

        }
    }

    
    void Wait()
    {
        if (Time.time >= timeToEndWait)
        {
            state = State.Init;
            waypoints.Clear();
            InitializeWaypoints(state);
        }
        
        MoveToWaypoint(true);// move o ghost para a posição de inicio
    }

    void Initiate()
    {
        _timeToWhite = 0;

        // se a fila estiver livre, mudar o estado
        if (waypoints.Count == 0)
        {
            state = State.Scatter;

            // recebe a direçao de acordo com o nome
            string name = GetComponent<SpriteRenderer>().sprite.name;
            if (name[name.Length - 1] == '0' || name[name.Length - 1] == '1') direction = Vector3.right;
            if (name[name.Length - 1] == '2' || name[name.Length - 1] == '3') direction = Vector3.left;
            if (name[name.Length - 1] == '4' || name[name.Length - 1] == '5') direction = Vector3.up;
            if (name[name.Length - 1] == '6' || name[name.Length - 1] == '7') direction = Vector3.down;

            InitializeWaypoints(state);
            timeToEndScatter = Time.time + scatterLength;

            return;
        }

        MoveToWaypoint();
    }

    void Scatter()
    {
        if (Time.time >= timeToEndScatter)// se o tempo for maior ou igual ao tempo dos ghosts se espalharem
        {
            waypoints.Clear();// limpa os waypoints dos ghosts se espalharem
            state = State.Chase;// inicia o estado de perseguiçao por parte dos ghosts
            return;
        }

        MoveToWaypoint(true);
    }

    void ChaseAI()
    {

        // se nao estiver no waypoint, movimenta-se para la
        if (Vector3.Distance(transform.position, waypoint) > 0.000000000001)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }

        // se estiver no waypoint, corre o script AI
        else GetComponent<AI4>().AILogic();

    }

    void RunAway()
    {
        GetComponent<Animator>().SetBool("Run", true);// inicia a animação dos ghosts quando fogem do pacman

        if (Time.time >= _timeToWhite && Time.time >= _timeToToggleWhite) ToggleBlueWhite();// se o tempo for maior que o tempo de ficar branco ou que o tempo até ficar branco, passa o metodo ToggleBlueWhite()

        if (Vector3.Distance(transform.position, waypoint) > 0.000000000001)// move os ghosts de acordo com os waypoints
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        else GetComponent<AI4>().RunLogic();// inicia a lógica de fugir
    }

    
    void MoveToWaypoint(bool loop = false)
    {
        waypoint = waypoints.Peek();		
        if (Vector3.Distance(transform.position, waypoint) > 0.000000000001)    // se nao chegou la
        {                                                                       // movimenta-se para la
            _direction = Vector3.Normalize(waypoint - transform.position);  
            Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        else    // se chegou ao waypoint, retira-se da fila
        {
            if (loop) waypoints.Enqueue(waypoints.Dequeue());
            else waypoints.Dequeue();
        }
    }

    public void Scare()// passa o estado para fuga, passa os valores que fazem a mudança de estado e cor dos ghosts, de acordo com o tempo de fuga depois de um powerup ser devorado
    {
        state = State.Run;
        _direction *= -1;

        _timeToWhite = Time.time + _gm.scareLength * 0.66f;
        _timeToToggleWhite = _timeToWhite;
        GetComponent<Animator>().SetBool("Run_White", false);

    }

    public void Calm()
    {
        // se o fantasma nao se estiver a mexer, nao faz nada
        if (state != State.Run) return;
        // limpa os waypoints e os ghosts deixam de estar a fugir do pacman, e passam a tentar apanha-lo novamente
        waypoints.Clear();
        state = State.Chase;
        _timeToToggleWhite = 0;
        _timeToWhite = 0;
        GetComponent<Animator>().SetBool("Run_White", false);
        GetComponent<Animator>().SetBool("Run", false);
    }

    public void ToggleBlueWhite()// ativa a animaçao de quando os ghosts estao quase a deixar de fugir do pacman
    {
        isWhite = !isWhite;
        GetComponent<Animator>().SetBool("Run_White", isWhite);
        _timeToToggleWhite = Time.time + _toggleInterval;
    }

}

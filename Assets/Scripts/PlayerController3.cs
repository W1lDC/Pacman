using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController3 : MonoBehaviour
{
    public int lives = 3;
    private Vector2 touchOrigin = -Vector2.one;
    Vector2 touchEnd = -Vector2.one;
    public float speed = 0.4f;
    Vector2 _dest = Vector2.zero;
    Vector2 _dir = Vector2.zero;
    Vector2 _nextDir;
    Vector2 currentswipe;
    public GameObject pacman;
    private GameManager3 GM;
    private bool _deadPlaying = false;
    private GameGUINavigation3 GN;
    public AudioClip deathsound;

    void Awake()
    {
        GetComponent<AudioSource>().clip = deathsound;
        GetComponent<AudioSource>().playOnAwake = false;
    }

    void Start()
    {
        GM = GameObject.Find("Game Manager3").GetComponent<GameManager3>();
        GN = GameObject.Find("UI Manager").GetComponent<GameGUINavigation3>();
        _dest = transform.position;
    }

    void FixedUpdate()
    {
        switch (GameManager3.gameState)
        {
            case GameManager3.GameState.Game:
                ReadInputAndMove();
                Animate();
                break;

            case GameManager3.GameState.Dead:
                if (!_deadPlaying)
                    StartCoroutine("PlayDeadAnimation");
                break;
        }
    }

    IEnumerator PlayDeadAnimation()
    {
        _deadPlaying = true;

        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetBool("Die", true);
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetBool("Die", false);
        _deadPlaying = false;
       

        if (pacman.GetComponent<PlayerController3>().lives <= 0)// caso o user perca a ultima vida
        {
            GN.GetGameOver();// inicia o ecra de gameover
        }
        else// caso nao seja a ultima vida 
            GM.ResetVarScene();// reinicia o nivel com menos uma vida
    }

    void Animate()// passa a animaçao de acordo com a direçao
    {
        Vector2 dir = _dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool Valid(Vector2 direction)
    {
        // cria linha de perto do pacman para o pacman, um bocadinho acima do centro do quadrado
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
        return hit.collider.name == "pacdot" || (hit.collider == GetComponent<Collider2D>());
    }

    public void ResetDestination()// passa o pacman para a posiçao de inicio de jogo
    {
        _dest = new Vector2(15f, 14f);
        GetComponent<Animator>().SetFloat("DirX", 1);
        GetComponent<Animator>().SetFloat("DirY", 0);
    }

    void ReadInputAndMove()
    {
        // aproxima se do destino
        Vector2 p = Vector2.MoveTowards(transform.position, _dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // script do touch mobile
        // se houver pelo menos um toque no ecra
        if (Input.touches.Length > 0)
        {
            // guarda o toque recebido
            Touch myTouch = Input.GetTouch(0);

            if (myTouch.phase == TouchPhase.Began)
            {
                // touchorigin = a posiçao do toque acima
                touchOrigin = new Vector2(myTouch.position.x, myTouch.position.y);
            }

            // se o touch phase nao for Began, e for igual ao Ended e o x do touch origin for maior ou igual a 0
            else if (myTouch.phase == TouchPhase.Ended /*&& touchOrigin.x >= 0 && touchOrigin.y >= 0*/)
            {
                touchEnd = new Vector2(myTouch.position.x, myTouch.position.y);

                currentswipe = new Vector3(touchEnd.x - touchOrigin.x, touchEnd.y - touchOrigin.y);
                currentswipe.Normalize();
                if (currentswipe.y > 0 && currentswipe.x > -0.5f && currentswipe.x < 0.5f)
                {
                    _nextDir = Vector2.up;
                    Debug.Log("up swipe");
                }
                //swipe down
                if (currentswipe.y < 0 && currentswipe.x > -0.5f && currentswipe.x < 0.5f)
                {
                    _nextDir = Vector2.down;
                    Debug.Log("down swipe");
                }
                //swipe left
                if (currentswipe.x < 0 && currentswipe.y > -0.5f && currentswipe.y < 0.5f)
                {
                    _nextDir = Vector2.left;
                    Debug.Log("left swipe");
                }
                //swipe right
                if (currentswipe.x > 0 && currentswipe.y > -0.5f && currentswipe.y < 0.5f)
                {
                    _nextDir = Vector2.right;
                    Debug.Log("right swipe");
                }
            }
        }



        // recebe a proxima direçao do teclado
        if (Input.GetAxis("Horizontal") > 0) _nextDir = Vector2.right;
        if (Input.GetAxis("Horizontal") < 0) _nextDir = -Vector2.right;
        if (Input.GetAxis("Vertical") > 0) _nextDir = Vector2.up;
        if (Input.GetAxis("Vertical") < 0) _nextDir = -Vector2.up;

        // se o pacman estiver no centro do quadrado
        if (Vector2.Distance(_dest, transform.position) < 0.1f)
        {
            if (Valid(_nextDir))
            {
                _dest = (Vector2)transform.position + _nextDir;
                _dir = _nextDir;
            }
            else   //se a proxima direçao nao for valida
            {
                if (Valid(_dir))  // e a proxima direçao for
                    _dest = (Vector2)transform.position + _dir;   // continua nessa direçao
            }
        }
    }

    public Vector2 getDir()
    {
        return _dir;
    }
}

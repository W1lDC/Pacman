using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    private GameManager gm;

	void Start ()
	{
	    gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if( gm == null )    Debug.Log("Powerup nao encontrou o GameManager!");
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "pacman") // se o pacman colidir com um objeto que contenha este script (powerup)
        {
            gm.ScareGhosts(); // inicia o metodo scareghosts()
            Destroy(gameObject); // destroy o powerup em questao
        }
    }
}

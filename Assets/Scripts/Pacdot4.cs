using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacdot4 : MonoBehaviour {

    private GameGUINavigation4 GN;
    void Start()
    {
        GN = GameObject.Find("UI Manager").GetComponent<GameGUINavigation4>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "pacman")// se o pacman colide com um pacdot
        {
            Debug.Log("Pacdot Collision");
            GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");// procura por todos os gameobjects com a tag "pacdot"
            Destroy(gameObject);// destroi o gameobject em questao que colidiu
            var SCORE = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<ScoreCounter4>();// chama o componente do script scorecounter "ScoreText"
            SCORE.updateScore();// atualiza o metodo no scripts scorecounter
            if (pacdots.Length == 1)// caso todos os pacdots sejam devorados
            {
                GN.GetYouWin();// aparece o screen de ganhar o nivel
            }
        }
    }
}

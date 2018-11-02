﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pacdot : MonoBehaviour {
        
    private GameGUINavigation GN;
    void Start()
    {
        GN = GameObject.Find("UI Manager").GetComponent<GameGUINavigation>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "pacman") // se o pacman colide com um pacdot
        {
            Debug.Log("Pacdot Collision");
            GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot"); // procura por todos os gameobjects com a tag "pacdot"
            Destroy(gameObject); // destroi o gameobject em questao que colidiu
            var SCORE = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<ScoreCounter>(); // chama o componente do script scorecounter "ScoreText"
            SCORE.updateScore(); // atualiza o metodo no scripts scorecounter
            if (pacdots.Length == 1) // caso todos os pacdots sejam devorados
            {
               GN.GetYouWin(); // aparece o screen de ganhar o nivel
            }

        }
    }
}
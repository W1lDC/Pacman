using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMainMenu : MonoBehaviour {
    
    public Text Score;
    private int HigherScoreTotal = 0;
    public int HigherScore;
    public int HigherScore2; 
    public int HigherScore3;
    public int HigherScore4;

    // receber e atribuir todas as variaveis guardadas no playerprefs relativas ao score de cada nivel
    void Start()
    {
        //PlayerPrefs.DeleteAll(); // apaga todas as playerprefs
        HigherScoreTotal = 0; 
        HigherScore = PlayerPrefs.GetInt("Higher", 0); // importante dar o valor default para nao haver conflitos
        HigherScore2 = PlayerPrefs.GetInt("Higher2", 0);
        HigherScore3 = PlayerPrefs.GetInt("Higher3", 0);
        HigherScore4 = PlayerPrefs.GetInt("Higher4", 0);
    }

    // a cada frame é atualizada a informação
    void Update()
    {
        HigherScoreTotal = HigherScore + HigherScore2 + HigherScore3 + HigherScore4; // adiçao de todas as variaveis recebidas acima
        Score.text = "" + HigherScoreTotal;
        GameObject.Find("TotalScoreGet").GetComponent<Text>().text = string.Format("{0:00000}", PlayerPrefs.GetInt("TotalScoreGet")); // passa o valor para o gameobject
        PlayerPrefs.SetInt("TotalScoreGet", HigherScoreTotal); 
        PlayerPrefs.Save(); // guarda o valor
    }
}

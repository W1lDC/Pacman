using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

    public int score;
    public int HigherScore = 0;
    public Text Score;
    public int dotsValue = 10;
    public int ghostsValue = 200;
    public AudioClip eat;

    void Awake()
    {
        Score = GetComponent<Text>();
        HigherScore = PlayerPrefs.GetInt("Higher");
        score = 0;
        GetComponent<AudioSource>().clip = eat;
        GetComponent<AudioSource>().playOnAwake = false;
    }

    // Update is called once per frame
    void Update ()
    {
        Score.text = "" + score;

        if (score > HigherScore) // se o valor da variavel score for maior que a higherscore
        {
          HigherScore = score; // iguala as duas
        }

        //no jogo apresenta os valores
        GameObject.Find("Score").GetComponent<Text>().text = string.Format("{0:00000}", score);
        GameObject.Find("Higher").GetComponent<Text>().text = string.Format("{0:00000}", PlayerPrefs.GetInt("Higher"));

        //no gameover screen apresenta os valores 
        GameObject.Find("YourScoreOver").GetComponent<Text>().text = string.Format("{0:00000}", score);
        GameObject.Find("YourBestScoreOver").GetComponent<Text>().text = string.Format("{0:00000}", PlayerPrefs.GetInt("Higher"));

        //no gamewin screen apresenta os valores
        GameObject.Find("YourScoreWin").GetComponent<Text>().text = string.Format("{0:00000}", score);
        GameObject.Find("YourBestScoreWin").GetComponent<Text>().text = string.Format("{0:00000}", PlayerPrefs.GetInt("Higher"));
    }

    void OnDestroy() // quando o nivel acaba, o variavel higherscore é guardada
    {
        PlayerPrefs.SetInt("Higher", HigherScore);
        PlayerPrefs.Save();
    }

    public void updateScore()
    {
        //faz a soma dos valores dos "pontos" no score
        score += dotsValue; // adiciona o valor de cada vez que um pacdot e devorado ao score

        // insere o som
        if (score >= dotsValue)
        {
            if (GetComponent<AudioSource>().isPlaying == false)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        Debug.Log("Updated Score Pacdots");
    }

    public void updateScoreGhosts() // adiciona o valor de cada vez que um ghost e devorado ao score
    {
        score += ghostsValue;
        Debug.Log("Updated Score Ghosts");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingHandler : MonoBehaviour {

    public Text rankingNoText, playerNameText, scoreText;

    public string rankingNo, playerName, score;



    public void UpdateInfo()
    {
        rankingNoText.text = rankingNo;
        playerNameText.text = playerName;
        scoreText.text = score;
    }
}

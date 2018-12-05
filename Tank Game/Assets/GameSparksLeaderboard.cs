using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api;
using GameSparks.Api.Responses;


namespace Prototype.NetworkLobby
{
    public class GameSparksLeaderboard : MonoBehaviour
    {


        private List<GameObject> playersRankingList;

        public GameObject rankingPanelGO, rankingContentGO;

        private List<PlayerRankingInfo> playerRankingList;

        // Use this for initialization
        void Start()
        {

        }

        public void GetLeaderboard()
        {
            Debug.Log("Fetching Leaderboard Data...");
            playerRankingList = new List<PlayerRankingInfo>();
            DeleteLeaderboardEntry();
            new GameSparks.Api.Requests.LeaderboardDataRequest()
                .SetLeaderboardShortCode("SCORE_LEADERBOARD")
                .SetEntryCount(50) // we need to parse this text input, since the entry count only takes long   
                .Send((response) =>
                {
                //Debug.Log("Fetching Leaderboard Data gdasgasg...");
                if (!response.HasErrors)
                    {
                        Debug.Log("Found Leaderboard Data...");
                    //DeleteLeaderboardEntry();
                    //outputData.text = System.String.Empty; // first clear all the data from the output
                    foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) // iterate through the leaderboard data
                    {
                        //int rank = (int)entry.Rank; // we can get the rank directly
                        //string playerName = entry.UserName;
                        //string score = entry.JSONData["SCORE"].ToString(); // we need to get the key, in order to get the score
                        ////SetupLeaderboard(rank.ToString(), playerName, score);
                        //Debug.Log(rank.ToString() + playerName + score);
                        //outputData.text += rank + "   Name: " + playerName + "        Score:" + score + "\n"; // addd the score to the output text
                        PlayerRankingInfo newPlayer = new PlayerRankingInfo();
                            newPlayer.rankingNo = entry.Rank.ToString();
                            newPlayer.playerName = entry.UserName;
                            newPlayer.totalScore = entry.JSONData["SCORE"].ToString();
                            playerRankingList.Add(newPlayer);
                        }

                    }
                    else
                    {
                        Debug.Log("Error Retrieving Leaderboard Data...");
                    }

                });

            Invoke("SetupLeaderboard", 2f);
        }

        private void SetupLeaderboard()//string rank, string playername, string score)
        {
            if (playerRankingList != null)
            {
                Debug.Log("List total: " + playerRankingList.Count.ToString());
                if (playerRankingList.Count > 0)
                {
                    GameObject lobbyManager = GameObject.Find("LobbyManager");
                    GameObject playerInfo = GameObject.Find("LoginPanel");

                    foreach (PlayerRankingInfo player in playerRankingList)
                    {
                        GameObject itemGO = Instantiate(rankingPanelGO, rankingContentGO.transform);
                        itemGO.SetActive(true);
                        itemGO.GetComponent<RankingHandler>().rankingNo = player.rankingNo;//rank;

                        if (lobbyManager.GetComponent<LobbyManager>().playerName == player.playerName)
                        {
                            lobbyManager.GetComponent<LobbyManager>().playerScore = int.Parse(player.totalScore);
                            playerInfo.GetComponent<LoginUI>().rankText.text = "Rank " + player.rankingNo;
                        }
                        itemGO.GetComponent<RankingHandler>().playerName = player.playerName;//playername;
                        itemGO.GetComponent<RankingHandler>().score = player.totalScore;//score;
                        itemGO.GetComponent<RankingHandler>().UpdateInfo();

                        playersRankingList.Add(itemGO);
                    }
                }
            }
        }

        private void DeleteLeaderboardEntry()
        {
            if (playersRankingList != null)
            {
                if (playersRankingList.Count > 0)
                {
                    foreach (GameObject go in playersRankingList)
                    {
                        Destroy(go);
                    }
                }
            }
            playersRankingList = new List<GameObject>();

        }

        private class PlayerRankingInfo
        {
            public string rankingNo;
            public string playerName;
            public string totalScore;
        }

    }
}
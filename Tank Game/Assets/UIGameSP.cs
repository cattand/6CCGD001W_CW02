using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TanksMP
{
    /// <summary>
    /// UI script for all elements, team events and user interactions in the game scene.
    /// </summary>
    public class UIGameSP : MonoBehaviour
    {
        /// <summary>
        /// UI sliders displaying team fill for each team using absolute values.
        /// </summary>
        public Slider[] teamSize;

        /// <summary>
        /// UI texts displaying kill scores for each team.
        /// </summary>
        public Text[] teamScore;

        /// <summary>
        /// UI texts displaying kill scores for this local player.
        /// [0] = Kill Count, [1] = Death Count
        /// </summary>
        public Text[] killCounter;

        /// <summary>
        /// UI text for indicating player death and who killed this player.
        /// </summary>
        public Text deathText;

        /// <summary>
        /// UI text displaying the time in seconds left until player respawn.
        /// </summary>
        public Text spawnDelayText;

        /// <summary>
        /// UI text for indicating game end and which team has won the round.
        /// </summary>
        public Text gameOverText;

        /// <summary>
        /// UI window gameobject activated on game end, offering restart buttons and quit.
        /// </summary>
        public GameObject gameOverMenu;

        public GameObject pauseButton;

        public GameObject MenuPanel;

        private bool gamePaused;

        public GameObject MenuPauseButton;

        public GameObject GameOverTextToDisplay;

        public Text timerText;

        public Text gameOverResult;

        //initialize variables
        IEnumerator Start()
        {
            //wait until the network is ready
            while (GameManagerSinglePlayer.GetInstance() == null || GameManagerSinglePlayer.GetInstance().localPlayer == null)
                yield return null;

            //play background music
            AudioManager.PlayMusic(0);

            gamePaused = false;
        }

        /// <summary>
        /// This is an implementation for changes to the team fill, updating the slider values.
        /// Parameters: index of team which received updates.
        /// </summary>
        public void OnTeamSizeChanged(int index)
        {
            teamSize[index].value = GameManagerSinglePlayer.GetInstance().size[index];
        }

        /// <summary>
        /// This is an implementation for changes to the team score, updating the text values.
        /// Parameters: index of team which received updates.
        /// </summary>
        public void OnTeamScoreChanged(int index)
        {
            teamScore[index].text = GameManagerSinglePlayer.GetInstance().score[index].ToString();
            teamScore[index].GetComponent<Animator>().Play("Animation");
        }

        /// <summary>
        /// Sets death text showing who killed the player in its team color.
        /// Parameters: killer's name, killer's team
        /// </summary>
        public void SetDeathText(string playerName, TeamForSinglePlayer team)
        {
            //show killer name and colorize the name converting its team color to an HTML RGB hex value for UI markup
            deathText.text = "KILLED BY\n<color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + playerName + "</color>";
        }

        public void SetTimerString(float time)
        {
            string minutes = Mathf.Floor(time / 60).ToString("00");
            string seconds = (time % 60).ToString("00");



            timerText.text = minutes + ":" + seconds;

            if (time/60 < 1.0f)
            {
                timerText.color = Color.red;
            }
            else
            {
                timerText.color = Color.green;
            }

        }


        /// <summary>
        /// Set respawn delay value displayed to the absolute time value received.
        /// The remaining time value is calculated in a coroutine by GameManager.
        /// </summary>
        public void SetSpawnDelay(float time)
        {
            spawnDelayText.text = Mathf.Ceil(time) + "";
        }

        /// <summary>
        /// Hides any UI components related to player death after respawn.
        /// </summary>
        public void DisableDeath()
        {
            //clear text component values
            deathText.text = string.Empty;
            spawnDelayText.text = string.Empty;
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (gamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    ActivateGameMenu();
                }
            }
        }


        /// <summary>
        /// Set game end text and display winning team in its team color.
        /// </summary>
        public void SetGameOverText(TeamForSinglePlayer team, bool isADraw)
        {
            if (isADraw)
            {
                gameOverText.text = "IS A DRAW!";
            }
            else
            {
                //show winning team and colorize it by converting the team color to an HTML RGB hex value for UI markup
                gameOverText.text = "TEAM <color=#" + ColorUtility.ToHtmlStringRGB(team.material.color) + ">" + team.name + "</color> WINS!";
            }
        }

        /// <summary>
        /// Displays the game's end screen. Called by GameManager after few seconds delay.
        /// </summary>
        public void ShowGameOver()
        {
            //hide text but enable game over window
            gameOverText.gameObject.SetActive(false);
            pauseButton.SetActive(false);
            gameOverMenu.SetActive(true);
            UpdateResultsText();
        }

        public void ShowGameOverOption()
        {
            gameOverMenu.SetActive(false);
            MenuPanel.SetActive(true);        
            MenuPauseButton.SetActive(false);
            GameOverTextToDisplay.SetActive(true);
        }

        private void UpdateResultsText()
        {
       
            gameOverResult.text = gameOverText.text;
            gameOverResult.text += " \nKills = " + killCounter[0].text;
            gameOverResult.text += " \nDeaths = " + killCounter[1].text;
        }

        public void ActivateGameMenu()
        {
            pauseButton.SetActive(false);
            Time.timeScale = 0;
            MenuPanel.SetActive(true);
            gamePaused = true;
        }


        public void ResumeGame()
        {         
            Time.timeScale = 1;
            MenuPanel.SetActive(false);
            pauseButton.SetActive(true);
            gamePaused = false;
        }

        /// <summary>
        /// Restarts the scene
        /// </summary>
        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(4);
        }

        public void TankSelection()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(3);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }
    }
}

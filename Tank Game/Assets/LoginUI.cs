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
    public class LoginUI : MonoBehaviour
    {

        public Text userNameField, connectionInfoField, loginPanelInfo, buttonText, loginTextMessage, rankText;

        public InputField userNameInput, passwordInput;

        public GameObject loginPanel, usernameInfo, passwordInfo, loginMessage, playerInfoPanel, leaderBoardPanelGO, lobbyManager;

        private bool isLogin;

        private string playerIDNO, playerScore;

        protected string loginID, loginPassword;



        //void Start()
        //{
        //    CheckConnection();

        //    SwitchToLogin();
        //}

        /// <summary>The GameSparks Manager singleton</summary>
        private static LoginUI instance = null;

        void Awake()
        {
            if (instance == null) // check to see if the instance has a refrence
            {
                instance = this; // if not, give it a refrence to this class...
                DontDestroyOnLoad(this.gameObject); // and make this object persistant as we load new scenes
            }
            else // if we already have a refrence then remove the extra manager from the scene
            {
                Destroy(this.gameObject);
            }
            //GS.GameSparksAvailable += OnAvailable;
        }

        public static LoginUI GetInstance()
        {
            return instance;
        }


        private void OnEnable()
        {
            CheckConnection();

            SwitchToLogin();
        }

        private void CheckConnection()
        {
            GS.GameSparksAvailable += OnGameSparksConnected;

            Debug.Log("fushvo");
        }

        private void OnGameSparksConnected(bool _isConnected)
        {
            if (_isConnected)
            {
                connectionInfoField.text = "GameSparks Connceted...";

                connectionInfoField.color = Color.green;
            }
            else
            {
                connectionInfoField.text = "GameSparks Disconnceted...";

                connectionInfoField.color = Color.red;
                Debug.Log("LOM");
                Invoke("CheckConnection", 0.5f);
            }
        }

        public void UserAuthentication_Bttn()
        {
            loginID = userNameInput.text;
            loginPassword = passwordInput.text;

            if (isLogin)
            {
                PasswordValidation();
                //Debug.Log("Attempting User Login...");
                ////print out the username and password here just to check they are correct //
                //Debug.Log("User Name:" + userNameInput.text);
                //Debug.Log("Password:" + passwordInput.text);
                new GameSparks.Api.Requests.AuthenticationRequest()
                    .SetUserName(userNameInput.text)//set the username for login
                    .SetPassword(passwordInput.text)//set the password for login
                    .Send((auth_response) =>
                    { //send the authentication request
                    if (!auth_response.HasErrors)
                        { // for the next part, check to see if we have any errors i.e. Authentication failed
                        connectionInfoField.text = "GameSparks Authenticated...";
                        //userNameField.text = auth_response.DisplayName;
                        SetupPlayerInfo(auth_response.DisplayName, auth_response.UserId);

                        //Debug.Log(auth_response.DisplayName);
                    }
                        else
                        {
                        //Debug.LogWarning(auth_response.Errors.JSON); // if we have errors, print them out
                        if (auth_response.Errors.GetString("DETAILS") == "UNRECOGNISED")
                            { // if we get this error it means we are not registered, so let's register the user instead

                            DisplayRegisterAlert("Login unrecognised! \nPlease click on register button to register a new account.", Color.red);
                            }
                            else
                            {
                                DisplayRegisterAlert("Login unrecognised! \nPlease click on register button to register a new account.", Color.red);
                            }
                        }
                    });
            }
            else
            {
                PasswordValidation();
            }
        }

        public void UserAuthentication()
        {

            if (isLogin)
            {
                PasswordValidation();
  
                new GameSparks.Api.Requests.AuthenticationRequest()
                    .SetUserName(loginID)//set the username for login
                    .SetPassword(loginPassword)//set the password for login
                    .Send((auth_response) =>
                    { //send the authentication request
                        if (!auth_response.HasErrors)
                        { // for the next part, check to see if we have any errors i.e. Authentication failed
                            connectionInfoField.text = "GameSparks Authenticated...";
                            //userNameField.text = auth_response.DisplayName;
                            SetupPlayerInfo(auth_response.DisplayName, auth_response.UserId);

                            //Debug.Log(auth_response.DisplayName);
                        }
                        else
                        {
                            //Debug.LogWarning(auth_response.Errors.JSON); // if we have errors, print them out
                            if (auth_response.Errors.GetString("DETAILS") == "UNRECOGNISED")
                            { // if we get this error it means we are not registered, so let's register the user instead

                                DisplayRegisterAlert("Login unrecognised! \nPlease click on register button to register a new account.", Color.red);
                            }
                            else
                            {
                                DisplayRegisterAlert("Login unrecognised! \nPlease click on register button to register a new account.", Color.red);
                            }
                        }
                    });
            }
        }

            private void DisplayRegisterAlert(string messageToDisplay, Color colorToShow)
        {
            loginMessage.SetActive(true);

            loginTextMessage.text = messageToDisplay;

            loginTextMessage.color = colorToShow;

            Invoke("HideLoginPanel", 5);
        }

        private void RegisterNewAccount()
        {
            new GameSparks.Api.Requests.RegistrationRequest()
                                .SetDisplayName(userNameInput.text)
                                .SetUserName(userNameInput.text)
                                .SetPassword(passwordInput.text)
                                .Send((reg_response) =>
                                {
                                    if (!reg_response.HasErrors)
                                    {
                                        connectionInfoField.text = "GameSparks Authenticated...";
                                    //userNameField.text = reg_response.DisplayName;
                                    DisplayRegisterAlert("Register successfully! \nLogin in progress..", Color.green);
                                        SetupPlayerInfo(reg_response.DisplayName, reg_response.UserId);
                                        Invoke("UpdateScore", 1);
                                    }

                                    else
                                    {
                                    //Debug.LogWarning(auth_response.Errors.JSON); // if we have errors, print them out
                                }
                                });
        }


        public void UpdateScore()
        {
            Debug.Log("Posting Score To Leaderboard...");
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("SUBMIT_SCORE")
                .SetEventAttribute("SCORE", "0")
                .Send((response) => {

                    if (!response.HasErrors)
                    {
                        Debug.Log("Score Posted Sucessfully...");
                    }
                    else
                    {
                        Debug.Log("Error Posting Score...");
                    }
                });
        }


        private void SetupPlayerInfo(string playerDisplayName, string playerID)
        {
            playerInfoPanel.SetActive(true);

            userNameField.text = "Display Name: " + playerDisplayName;

            playerInfoPanel.GetComponent<ChangeDisplayName>().playerName = playerDisplayName;

            lobbyManager = GameObject.Find("LobbyManager");

            playerIDNO = playerID;

            lobbyManager.GetComponent<LobbyManager>().playerID = playerID;
            lobbyManager.GetComponent<LobbyManager>().playerName = playerDisplayName;
            Debug.Log("player ID: " + playerID);

            loginMessage.SetActive(true);

            leaderBoardPanelGO.SetActive(true);



            if (isLogin)
            {
                loginTextMessage.text = "Login Successfully! \nWelcome back " + playerDisplayName + "!";

                loginTextMessage.color = Color.green;
            }

            loginPanel.SetActive(false);

            //
            //Invoke("GetRanking", 1);
            Invoke("UpdateLeaderboard", 1);
            Invoke("HideLoginPanel", 5);
        }

        private void GetRanking()
        {
            //string[] temp1 = (S, C, O, R, E, _, L, E, A, D, E, R, B, O, A, R, D;

            //List <string> temp = ["SCORE_LEADERBOARD"];

            List<string> temp = new List<string>();

            temp.Add("SCORE_LEADERBOARD");

            new LeaderboardsEntriesRequest()
            .SetLeaderboards(temp)
            .SetPlayer(playerIDNO)
            .Send((response) => {
            GSData scriptData = response.ScriptData;
                //lobbyManager.GetComponent<LobbyManager>().playerScore = (int)response.JSONData["SCORE"];
                //rankText.text = "Rank " + response.JSONData["SCORE"].ToString();
                Debug.Log("Response Data: " + scriptData);
            });
        }

        private void UpdateLeaderboard()
        {
            leaderBoardPanelGO.GetComponent<GameSparksLeaderboard>().GetLeaderboard();
        }

        private void PasswordValidation()
        {

            string temp = passwordInput.text;

            bool containAlpha = Regex.IsMatch(temp, "[a-zA-Z][0-9]");

            if (temp.Length >= 8 && containAlpha)
            {
                RegisterNewAccount();

            }
            else
            {
                DisplayRegisterAlert("Error! \nPlease key in minimum 8 alphanumeric characters.", Color.red);
            }
        }

        public void HideLoginPanel()
        {


            loginMessage.SetActive(false);
        }

        public void SwitchToRegister()
        {
            userNameInput.ActivateInputField();

            playerInfoPanel.SetActive(false);

            leaderBoardPanelGO.SetActive(false);

            loginPanelInfo.text = "Register";

            buttonText.text = "Login";

            usernameInfo.SetActive(true);
            passwordInfo.SetActive(true);

            passwordInput.contentType = InputField.ContentType.Alphanumeric;

            isLogin = false;
        }

        public void SwitchToLogin()
        {
            userNameInput.ActivateInputField();

            playerInfoPanel.SetActive(false);

            leaderBoardPanelGO.SetActive(false);

            loginPanelInfo.text = "Login";

            buttonText.text = "Register";

            usernameInfo.SetActive(false);
            passwordInfo.SetActive(false);

            passwordInput.contentType = InputField.ContentType.Password;

            isLogin = true;
        }

        public void SwitcherToLoginOrRegister()
        {
            if (isLogin)
            {
                SwitchToRegister();
            }
            else
            {
                SwitchToLogin();
            }
        }


        public void GetLeaderboard()
        {
            Debug.Log("Fetching Leaderboard Data...");

            new GameSparks.Api.Requests.LeaderboardDataRequest()
                .SetLeaderboardShortCode("SCORE_LEADERBOARD")
                .SetEntryCount(50) // we need to parse this text input, since the entry count only takes long   
                .Send((response) =>
                {
                    Debug.Log("Fetching Leaderboard Data gdasgasg...");
                    if (!response.HasErrors)
                    {
                        Debug.Log("Found Leaderboard Data...");
                    //DeleteLeaderboardEntry();
                    //outputData.text = System.String.Empty; // first clear all the data from the output
                    foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) // iterate through the leaderboard data
                    {
                            int rank = (int)entry.Rank; // we can get the rank directly
                        string playerName = entry.UserName;
                            string score = entry.JSONData["SCORE"].ToString(); // we need to get the key, in order to get the score
                                                                               //SetupLeaderboard(rank.ToString(), playerName, score);
                                                                               //outputData.text += rank + "   Name: " + playerName + "        Score:" + score + "\n"; // addd the score to the output text
                        Debug.Log(rank.ToString() + playerName + score);
                        }
                    }
                    else
                    {
                        Debug.Log("Error Retrieving Leaderboard Data...");
                    }

                });
        }


        private void SubmitScore()
        {
            Debug.Log("Posting Score To Leaderboard...");
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("Submit_Score")
                .SetEventAttribute("SCORE", playerScore)
                .Send((response) => {

                    if (!response.HasErrors)
                    {
                        Debug.Log("Score Posted Sucessfully...");
                    }
                    else
                    {
                        Debug.Log("Error Posting Score...");
                    }
                });
        }


        public void UpdateScore(string score)
        {
            playerScore = score;

            Invoke("SubmitScore", 1f);
        }

    }
}
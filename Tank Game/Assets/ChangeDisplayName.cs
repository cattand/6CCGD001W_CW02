using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api;
using GameSparks.Api.Responses;

namespace Prototype.NetworkLobby
{
    public class ChangeDisplayName : MonoBehaviour
    {

        public GameObject newDisplayNameInputField, successInfoGO;

        public Text editButtonText, playerDisplayName, successInfoText;

        public InputField newEditDisplayInputField;

        private GameObject lobbyManager;

        private bool isEditing;

        public string playerName;

        // Use this for initialization
        void Start()
        {
            HideEditField();
        }

        public void HideEditField()
        {
            newDisplayNameInputField.SetActive(false);

            editButtonText.text = "Edit Display Name";

            isEditing = false;
        }

        public void ShowEditField()
        {
            newDisplayNameInputField.SetActive(true);

            editButtonText.text = "Done Editing";

            isEditing = true;
        }

        public void ShowHideEditField()
        {
            if (isEditing)
            {
                HideEditField();
                UpdateNewDisplayName();
            }
            else
            {
                ShowEditField();
            }
        }

        private void UpdateNewDisplayName()
        {
            if (newEditDisplayInputField.text != "")
            {
                SetupPlayerInfo(newEditDisplayInputField.text);
                new ChangeUserDetailsRequest()
                    .SetDisplayName(newEditDisplayInputField.text)
                               .Send((response) =>
                               {
                                   GSData scriptData = response.ScriptData;
                               });

                newEditDisplayInputField.text = "";
            }
        }

        private void SetupPlayerInfo(string playerDisplayNameInString)
        {
            playerDisplayName.text = "Display Name: " + playerDisplayNameInString;

            successInfoGO.SetActive(true);

            successInfoText.text = "Update Successfully! \nNew Display Name: " + playerDisplayNameInString + "!";

            successInfoText.color = Color.green;

            playerName = playerDisplayNameInString;

            lobbyManager = GameObject.Find("LobbyManager");

            lobbyManager.GetComponent<LobbyManager>().playerName = playerName;

            HideEditField();

            Invoke("HideLoginPanel", 5);
        }

        public void HideLoginPanel()
        {
            successInfoGO.SetActive(false);
        }




    }
}

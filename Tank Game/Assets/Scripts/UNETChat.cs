using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{

    public class UNETChat : Chat
    {
        //just a random number
        private const short chatMessage = 131;

        public GameObject LobbyPlayerGo;

        public List<LobbyPlayer> playersList = new List<LobbyPlayer>();

        public InputField inputTextField;

        private void Start()
        {
            GetPlayerList();

            //if the client is also the server
            if (NetworkServer.active)
            {
                //registering the server handler
                NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
            }

            //registering the client handler
            NetworkManager.singleton.client.RegisterHandler(chatMessage, ReceiveMessage);
        }

        private void GetPlayerList()
        {
            if (LobbyPlayerGo != null)
            {
                playersList = LobbyPlayerGo.GetComponent<LobbyPlayerList>()._players;
            }
            else
            {
                GameObject lobbyManager = GameObject.Find("LobbyManager");
                playersList = lobbyManager.GetComponent<LobbyManager>().playersList;
            }
        }

        private string GetPlayerName(int playerConnNumber)
        {
            string _playerName;

            GetPlayerList();

            _playerName = playersList[playerConnNumber].playerName;

            return _playerName;
        }

        private Color GetPlayerColor(int playerConnNumber, string text)
        {
            Color _playerColor = Color.black;

            GetPlayerList();

            foreach (LobbyPlayer player in playersList)
            {
                if (text.Contains(player.playerName))
                {
                    _playerColor = player.playerColor;
                    Debug.Log("Found PLayer Name.");
                    break;
                }        
            }
            return _playerColor;
        }

        private void ReceiveMessage(NetworkMessage message)
        {
            //reading message
            string text = message.ReadMessage<StringMessage>().value;

            AddMessage(text, GetPlayerColor(message.conn.connectionId, text));
        }

        private void ServerReceiveMessage(NetworkMessage message)
        {
            StringMessage myMessage = new StringMessage();

            string playerName = GetPlayerName(message.conn.connectionId);

            //we are using the connectionId as player name only to exemplify
            myMessage.value = playerName + ": " + message.ReadMessage<StringMessage>().value;
            
            //sending to all connected clients
            NetworkServer.SendToAll(chatMessage, myMessage);
        }

        public override void SendMessage(UnityEngine.UI.InputField input)
        {
            StringMessage myMessage = new StringMessage();

            if (input.text != "")
            {
                //getting the value of the input
                myMessage.value = input.text;

                //sending to server
                NetworkManager.singleton.client.Send(chatMessage, myMessage);

                input.text = "";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessage(inputTextField);
            }

           

        }

    }
}
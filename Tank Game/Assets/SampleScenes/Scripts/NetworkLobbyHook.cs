using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;
using TanksMP;
using System;

public class NetworkLobbyHook : LobbyHook 
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();

        Player tankInfo = gamePlayer.GetComponent<Player>();
        tankInfo.myName = lobby.playerName;
		tankInfo.tankColor = lobby.playerColor;
        tankInfo.teamIndex = lobby.slot;

        //Transform temp = manager.GetStartPosition();
        
        //temp.GetComponentInChildren<MeshRenderer>().material.color = tankInfo.tankColor;
    }
}

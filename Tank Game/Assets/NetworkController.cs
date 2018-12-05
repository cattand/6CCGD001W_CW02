using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkController : NetworkManager {

    //public Transform spawnPosition;
    public int curPlayer;

    public GameObject[] TankList;

    public Button Tank1Button;
    public Button Tank2Button;
    public Button Tank3Button;
    public Button Tank4Button;
    public Button Tank5Button;

    int tankIndex = 0;
    private GameObject playerPrefab;

    //Called on client when connect
    public override void OnClientConnect(NetworkConnection conn)
    {

        // Create message to set the player
        IntegerMessage msg = new IntegerMessage(curPlayer);

        // Call Add player and pass the message
        ClientScene.AddPlayer(conn, 0, msg);
    }

    // Server
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        //int id = 0;

        curPlayer = 0;

        AddListenertoAllButtons();

        playerPrefab = spawnPrefabs[curPlayer];

        // Read client message and receive index
        if (extraMessageReader != null)
        {
            var stream = extraMessageReader.ReadMessage<IntegerMessage>();
            curPlayer = stream.value;
            //id = stream.value;
            playerPrefab = spawnPrefabs[curPlayer];
        }

        //Select the prefab from the spawnable objects list
        //var playerPrefab = TankList[0];

        Debug.Log("iuhvosuhvosvgh");

        if (playerPrefab != null)
        {
            // Create player object with prefab
            var player = Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        else
        {
            playerPrefab = spawnPrefabs[0];
            var player = Instantiate(playerPrefab, new Vector3(0, 2, 0), Quaternion.identity) as GameObject;
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        // Add player object for connection
        //NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }


// Use this for initialization
void Start () {
        //Tank1Button.onClick.AddListener(delegate { TankPicker(Tank1Button.name);});
        //Tank2Button.onClick.AddListener(delegate { TankPicker(Tank2Button.name); });
        //Tank3Button.onClick.AddListener(delegate { TankPicker(Tank3Button.name); });
        //Tank4Button.onClick.AddListener(delegate { TankPicker(Tank4Button.name); });
        //Tank5Button.onClick.AddListener(delegate { TankPicker(Tank5Button.name); });
        AddListenertoAllButtons();
    }

    private void Awake()
    {
        AddListenertoAllButtons();
    }

    private void AddListenertoAllButtons()
    {
        playerPrefab = new GameObject();

        Tank1Button.onClick.AddListener(delegate { TankPicker(Tank1Button.name); });
        Tank2Button.onClick.AddListener(delegate { TankPicker(Tank2Button.name); });
        Tank3Button.onClick.AddListener(delegate { TankPicker(Tank3Button.name); });
        Tank4Button.onClick.AddListener(delegate { TankPicker(Tank4Button.name); });
        Tank5Button.onClick.AddListener(delegate { TankPicker(Tank5Button.name); });
    }

    private void TankPicker(string buttonName)
    {
        //playerPrefab = spawnPrefabs[int.Parse(buttonName)];

        curPlayer = int.Parse(buttonName);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //https://www.youtube.com/watch?v=2cYfGT2HK0g
}


using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;



public class GameUI : MonoBehaviour
{
    public Button HostButton;
    public Button ClientButton;
    public Button QuitButton;

    public Text StatusText;
    public Text WinnerText;

    public string ConnectionAddress = "127.0.0.1";

    private void Start()
    {
        HostButton.onClick.AddListener(StartHost);
        ClientButton.onClick.AddListener(StartClient);
        QuitButton.onClick.AddListener(QuitGame);

        WinnerText.text = "";
        StatusText.text = "Choose Host or Client";
    }

    private void Update()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null)
        {
            StatusText.text = "NetworkManager not found";
            return;
        }
            

        if (networkManager.IsHost)
            StatusText.text = "Connected as Host";
        else if (networkManager.IsClient && networkManager.IsConnectedClient)
            StatusText.text = "Connected as Client";
        else if (networkManager.IsListening)
            StatusText.text = "Connecting...";
        else
            StatusText.text = "Choose Host or Client";

        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.IsSpawned)
            return;

        if (!GameManager.Instance.IsMatchActive.Value)
        {

            WinnerText.text = "";
        }
        else
        {

            ulong winnerId = GameManager.Instance.WinnerClientId.Value;

            WinnerText.text = "P:" + (winnerId + 1) + " Wins!";
        }
    }

    private void StartHost()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null || networkManager.IsListening)
            return;

        bool Started = networkManager.StartHost();

        if (!Started)
            StatusText.text = "Failed to Start Host";
    }

    private void StartClient()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager == null || networkManager.IsListening)
            return;

        UnityTransport transport =networkManager.GetComponent<UnityTransport>();

        transport.ConnectionData.Address = ConnectionAddress;

        bool Started = networkManager.StartClient();

        if (!Started)
            StatusText.text = "Failed to start Client";
        else
            StatusText.text = "Connecting...";
    }

    private void QuitGame()
    {
        NetworkManager networkManager = NetworkManager.Singleton;

        if (networkManager != null && networkManager.IsListening)
            networkManager.Shutdown();

        Application.Quit();
    }
}

// Start is called once before the first execution of Update after the MonoBehaviour is created
//void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

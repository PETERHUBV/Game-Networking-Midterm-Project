using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FinishLine : NetworkBehaviour
{
    private bool HasFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkManager.Singleton == null)
            return;

        
        if (!NetworkManager.Singleton.IsServer)
            return;

        if (HasFinished)
            return;

        PlayerController player =other.GetComponentInParent<PlayerController>();

        if (player == null || !player.IsSpawned)
            return;

        StartRestart(player.OwnerClientId);
    }

    private void StartRestart(ulong winnerClientID)
    {
        if (HasFinished)
            return;

        HasFinished = true;

        if (GameManager.Instance != null)
            GameManager.Instance.EndMatch(winnerClientID);

        Debug.Log("Player " + (winnerClientID + 1) + " reached the finish line. Restarting in 5 seconds.");

        StartCoroutine(RestartAfterDelay());
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (NetworkManager.Singleton == null ||
            !NetworkManager.Singleton.IsServer)
            yield break;

        string CurrentScene =
            SceneManager.GetActiveScene().name;

        Debug.Log("Reloading scene: " + CurrentScene);

        NetworkManager.Singleton.SceneManager.LoadScene(CurrentScene,LoadSceneMode.Single);
    }


void Start()
        {

        }

       

        // Update is called once per frame
        void Update()
        {

        }
    }


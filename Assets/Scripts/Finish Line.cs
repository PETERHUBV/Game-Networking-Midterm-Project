using UnityEngine;
using Unity.Netcode;
using System.Collections;
public class FinishLine : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsServer)
            return;

        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        GameManager.Instance.EndMatch(player.OwnerClientId);
    }

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

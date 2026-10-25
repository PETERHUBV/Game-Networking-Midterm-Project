using UnityEngine;
using Unity.Netcode;
using System.Collections;
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public Transform[] SpawnPoints;

    public NetworkVariable<bool> IsMatchActive =
        new NetworkVariable<bool>(true);

    public NetworkVariable<ulong> WinnerClientId =
        new NetworkVariable<ulong>(ulong.MaxValue);

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            IsMatchActive.Value = true;
            WinnerClientId.Value = ulong.MaxValue;
        }
    }

    public void EndMatch(ulong winningClientId)
    {
        if (!IsServer || !IsMatchActive.Value)
            return;

        WinnerClientId.Value = winningClientId;
        IsMatchActive.Value = false;
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

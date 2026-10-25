
using System.Collections;
using Unity.Netcode;
using UnityEngine;


public class DisappearingObstacle : NetworkBehaviour
{
    public float VisibleTime = 4f;
    public float HiddenTime = 2f;

    private Renderer platformRenderer;
    private Collider platformCollider;

    private NetworkVariable<bool> IsVisible =
        new NetworkVariable<bool>(true);

    private void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider>();
    }

    public override void OnNetworkSpawn()
    {
        IsVisible.OnValueChanged += OnVisibilityChanged;

        ApplyVisibility(IsVisible.Value);

        if (IsServer)
        {
            StartCoroutine(PlatformCycle());
        }
    }

    public override void OnNetworkDespawn()
    {
        IsVisible.OnValueChanged -= OnVisibilityChanged;
    }

    private IEnumerator PlatformCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(VisibleTime);
            IsVisible.Value = false;

            yield return new WaitForSeconds(HiddenTime);
            IsVisible.Value = true;
        }
    }

    private void OnVisibilityChanged(
        bool oldValue,
        bool newValue)
    {
        ApplyVisibility(newValue);
    }

    private void ApplyVisibility(bool visible)
    {
        platformRenderer.enabled = visible;
        platformCollider.enabled = visible;
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

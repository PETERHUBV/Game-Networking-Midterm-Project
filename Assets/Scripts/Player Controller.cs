using UnityEngine;
using Unity.Netcode;
using System.Collections;
using Unity.Netcode.Transports.UTP;
public class PlayerController : NetworkBehaviour
{
    public float MoveSpeed = 10f;
    public float JumpHeight = 40f;
    public float Gravity = -10f;
    public float FallLimit = -10f;

    private CharacterController characterController;
    private Vector3 verticalVelocity;
    private bool HasJumped;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            int spawnIndex = (int)(OwnerClientId % (ulong) GameManager.Instance.SpawnPoints.Length);
        
            Transform spawnPoint = GameManager.Instance.SpawnPoints[spawnIndex];

            characterController.enabled = false;
            transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);
            characterController.enabled = true;
            verticalVelocity = Vector3.zero;
            HasJumped = false;
        }

        if (IsOwner)
        {
            Camera mainCamera = Camera.main;
            CameraFollowSide cameraFollow = mainCamera.GetComponent<CameraFollowSide>();

            cameraFollow.Target = transform;
        }
    }

    private void Update()
    {
        if (!IsOwner || !IsSpawned)
            return;

        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.IsMatchActive.Value)
            return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");


        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        MoveServerRpc(horizontalInput, verticalInput, jumpPressed);
    }

    [ServerRpc]
    private void MoveServerRpc(float horizontalInput, float verticalInput, bool jumpPressed)
    {
        if (GameManager.Instance == null && !GameManager.Instance.IsMatchActive.Value)
            return;

        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);
        if (characterController.isGrounded)
        {
            if (verticalVelocity.y < 0f)
                verticalVelocity.y = -2f;

            HasJumped = false;
        }


        if (jumpPressed && characterController.isGrounded && !HasJumped)
        {
            verticalVelocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);

            HasJumped = true;

           
        }

        verticalVelocity.y += Gravity * Time.deltaTime;

        Vector3 movement = new Vector3(horizontalInput * MoveSpeed,verticalVelocity.y,verticalInput * MoveSpeed);

        characterController.Move(movement * Time.deltaTime);

        if (transform.position.y < FallLimit)
            Respawn();
    }

    private void Respawn()
    {
        int spawnIndex = (int)(OwnerClientId %(ulong)GameManager.Instance.SpawnPoints.Length);

        Transform spawnPoint = GameManager.Instance.SpawnPoints[spawnIndex];

        characterController.enabled = false;

        transform.SetPositionAndRotation(spawnPoint.position,spawnPoint.rotation);

        characterController.enabled = true;

        verticalVelocity = Vector3.zero;
        HasJumped = false;
    }


// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {

    }
}
    
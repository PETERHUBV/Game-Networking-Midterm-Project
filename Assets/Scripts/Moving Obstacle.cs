
using System.Collections;
using Unity.Netcode;
using UnityEngine;
public class MovingObstacle : NetworkBehaviour
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public float MoveSpeed = 2f;

    private void Update()
    {
        if (!IsSpawned || !IsServer)
            return;

        float movement = Mathf.PingPong(
            Time.time * MoveSpeed,
            1f
        );

        transform.position = Vector3.Lerp(StartPosition,EndPosition,movement);
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

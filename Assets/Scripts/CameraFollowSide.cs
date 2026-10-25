using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class CameraFollowSide : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0f, 3f, -10f);
    public float FollowSpeed = 8f;

    private void LateUpdate()
    {
        if (Target == null)
            return;

        Vector3 targetPosition = Target.position + Offset;

        transform.position = Vector3.Lerp(transform.position,targetPosition,FollowSpeed * Time.deltaTime);

        transform.LookAt(Target.position + Vector3.up);
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

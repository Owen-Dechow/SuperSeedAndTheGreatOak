using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;
    [SerializeField] Vector2 positionClamp;
    Vector3 TargetPosition { get => new(target.position.x, target.position.y, transform.position.z); }

    void Start()
    {
        transform.position = TargetPosition;
    }

    void LateUpdate()
    {
        transform.position += (TargetPosition - transform.position) * followSpeed * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -positionClamp.x, positionClamp.x),
            Mathf.Clamp(transform.position.y, -positionClamp.y, positionClamp.y),
            transform.position.z
            );
    }
}

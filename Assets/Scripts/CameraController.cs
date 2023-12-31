using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;
    [SerializeField] Vector2 positionClamp;

    [Header("Background")]
    [SerializeField] Transform background;
    [SerializeField] Vector2 backgroundLeniency;
    [SerializeField] Vector3 backgroundSize;

    Vector3 TargetPosition { get => new(target.position.x, target.position.y, transform.position.z); }

    void Start()
    {
        if (GameManager.PlayerCollection != null && GameManager.PlayerCollection.PlayerLocationOnLoad != null)
        {
            Vector3 position = (Vector3)GameManager.PlayerCollection.PlayerLocationOnLoad;
            position.z = transform.position.z;
            transform.position = position;
        }
        else
        {
            transform.position = TargetPosition;
        }

        background.localScale = backgroundSize;
        UpdateBackgroundPosition();
    }

    void LateUpdate()
    {
        transform.position += (TargetPosition - transform.position) * followSpeed * Time.deltaTime;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -positionClamp.x, positionClamp.x),
            Mathf.Clamp(transform.position.y, -positionClamp.y, positionClamp.y),
            transform.position.z
            );

        UpdateBackgroundPosition();
    }

    void UpdateBackgroundPosition() => background.position = transform.position / backgroundLeniency;
}

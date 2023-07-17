using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    enum PlayerDirection
    {
        Right,
        Left,
    }

    [SerializeField] int levelNumber;
    [SerializeField] bool overridePlayerPositionOnLoad;
    [SerializeField] Vector3 playerPositionOnLoad;
    [SerializeField] PlayerDirection playerDirectionOnLoad;

    public static bool FlipPlayerOnLoad { get; private set; } = false;
    public static bool OverridePlayerPositionOnLoad { get; private set; } = false;
    public static Vector3 PlayerPositionOnLoad { get; private set; } = Vector3.zero;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.CollectPlayer(collision.collider.GetComponent<PlayerController>());
            SceneManager.LoadScene(levelNumber - 1);

            FlipPlayerOnLoad = playerDirectionOnLoad == PlayerDirection.Left;
            OverridePlayerPositionOnLoad = overridePlayerPositionOnLoad;
            PlayerPositionOnLoad = playerPositionOnLoad;
        }
    }
}

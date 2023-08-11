using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum PlayerDirection
    {
        Right,
        Left,
    }

    [SerializeField] GameManager.GameLevel level;
    [SerializeField] PlayerDirection playerDirectionOnLoad;
    [HideInInspector] public Vector2 playerPositionOnLoad;
    [HideInInspector] public bool overridePlayerPositionOnLoad;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Vector2? playerPositionOnLoad = overridePlayerPositionOnLoad ? this.playerPositionOnLoad : null;
            GameManager.CollectPlayer(collision.collider.GetComponent<PlayerController>(), playerDirectionOnLoad, playerPositionOnLoad);
            GameManager.LoadLevel(level);
        }
    }
}

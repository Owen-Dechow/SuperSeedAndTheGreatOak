using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] Bullet.PowerLevel powerLevel;
    [SerializeField] Vector2 phaseOffset;
    [SerializeField] Material greyScaleMaterial;
    [SerializeField] Material laserMaterial;
    [SerializeField] float unblockSeconds;
    float timeUnblocked;
    bool touchingPlayer;

    // Update is called once per frame
    private void Start()
    {
        if (powerLevel >= Bullet.PowerLevel.LaserBeam)
            GetComponent<Renderer>().material = laserMaterial;
        else
            GetComponent<Renderer>().material = greyScaleMaterial;
    }

    void Update()
    {
        if (powerLevel >= Bullet.PowerLevel.PhaserShot)
        {
            transform.position = new Vector2(
                Random.Range(-phaseOffset.x, phaseOffset.x),
                Random.Range(-phaseOffset.y, phaseOffset.y)
                );
        }
        else
        {
            transform.position = Vector3.zero;
        }

        if (Level.Blocked)
            return;

        timeUnblocked += Time.deltaTime;
        if (timeUnblocked >= unblockSeconds && !touchingPlayer)
        {
            SetBlocksClosed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet.powerLevel >= powerLevel)
                SetBlocksOpen();
        }

        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            touchingPlayer = false;
    }

    void SetBlocksOpen()
    {
        timeUnblocked = 0;
        Level.Blocked = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void SetBlocksClosed()
    {

        timeUnblocked = unblockSeconds;
        Level.Blocked = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}

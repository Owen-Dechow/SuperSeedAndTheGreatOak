using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Header("X Movement")]
    public bool lag;
    [SerializeField] float walkSpeed;
    [SerializeField] float xSpeedClamp;
    [SerializeField] float drag;


    [Header("Y Movement")]
    [SerializeField] float jumpVelocity;
    [SerializeField] float ySpeedClamp;
    [SerializeField] float maxJumpSeconds;
    [SerializeField] float airJumpLeewaySeconds;
    public bool canHighJump;
    float MaxJumpSeconds { get => canHighJump ? maxJumpSeconds : 0; }
    float jumpSeconds;
    float fallingSeconds;

    [Header("Wall Jumping")]
    public bool canWallJump;
    [SerializeField] Vector2 wallJumpVelocity;

    [Header("Dashing")]
    public bool canDash;
    [SerializeField] float dashVelocity;

    [Header("Morphing")]
    public bool canShrink;
    [SerializeField] Vector3 shrinkSize;
    [SerializeField] float shrinkAdaptionDistance;
    [SerializeField] float shrinkAdaptionCheckInterval;
    Vector3 grownSize;
    bool isShrunk;

    [Header("Shooting")]
    [SerializeField] GameObject bulletPrefab;
    public bool triShot;
    public bool laserBeam;
    public bool phaserShot;

    [Header("Stats")]
    public int life;
    public int maxLife;
    public bool shield;


    public static Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        playerTransform = transform;
        jumpSeconds = MaxJumpSeconds + 1;
        fallingSeconds = airJumpLeewaySeconds + 1;
        grownSize = transform.localScale;
        isShrunk = false;

        if (GameManager.PlayerCollection != null)
        {
            canHighJump = GameManager.PlayerCollection.CanHighJump;
            canWallJump = GameManager.PlayerCollection.CanWallJump;
            canDash = GameManager.PlayerCollection.CanDash;
            canShrink = GameManager.PlayerCollection.CanShrink;
            life = GameManager.PlayerCollection.Life;
            maxLife = GameManager.PlayerCollection.MaxLife;
            spriteRenderer.flipX = GameManager.PlayerCollection.PlayerDirectionOnLoad == Door.PlayerDirection.Left;
            if (GameManager.PlayerCollection.PlayerLocationOnLoad != null)
                transform.position = (Vector3)GameManager.PlayerCollection.PlayerLocationOnLoad;
        }
    }

    void Update()
    {
        if (lag)
        {
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            GameManager.GreyScale = !GameManager.GreyScale;
        }
 
        // Get player input
        Vector2 input = new(
            Input.GetAxisRaw(ControlMapping.MoveX),
            Input.GetAxisRaw(ControlMapping.MoveY));

        // Run physics
        PhysicsInfo physicsInfo = base.UpdatePosition();

        // Update falling
        velocity.y -= Level.Gravity * Time.deltaTime;
        if (physicsInfo.feetOnGround) fallingSeconds = 0;
        else if (physicsInfo.hitHead) fallingSeconds = airJumpLeewaySeconds + 1;
        else fallingSeconds += Time.deltaTime;

        // Jump/ wall jump
        if (input.y > 0)
        {
            if (jumpSeconds <= MaxJumpSeconds)
            {
                jumpSeconds += Time.deltaTime;
                velocity.y = jumpVelocity;
            }
            else if (physicsInfo.setForWallJump != 0 && canWallJump && Input.GetButtonDown(ControlMapping.MoveY) && !isShrunk)
            {
                velocity = new Vector2(wallJumpVelocity.x * physicsInfo.setForWallJump, wallJumpVelocity.y);
            }
        }
        else
        {
            if (fallingSeconds <= airJumpLeewaySeconds)
            {
                jumpSeconds = 0;
            }
            else
            {
                jumpSeconds = MaxJumpSeconds + 1;
            }
        }

        // Move x
        velocity.x += walkSpeed * input.x * Time.deltaTime;
        velocity.x += (0 - velocity.x) * drag * Time.deltaTime;
        if (Input.GetButtonDown(ControlMapping.Dash) && canDash)
            velocity.x = dashVelocity * (spriteRenderer.flipX ? -1 : 1);

        // Clamp Speed
        velocity = new(
            Mathf.Clamp(velocity.x, -xSpeedClamp, xSpeedClamp),
            Mathf.Clamp(velocity.y, -ySpeedClamp, ySpeedClamp));

        // Morph: Shrink/ Grow
        if (Input.GetButtonDown(ControlMapping.Morph) && input.y == -1 && canShrink)
        {
            if (isShrunk)
            {
                transform.localScale = grownSize;

                if (MorphReposition())
                    transform.localScale = shrinkSize;
                else
                    isShrunk = false;
            }
            else
            {
                transform.localScale = shrinkSize;
                isShrunk = true;
            }
        }

        // Set life meter
        life = Mathf.Clamp(life, 0, maxLife);
        GameUI.SetLifeMeter(life);

        // Shooting
        if (Input.GetButtonDown(ControlMapping.Fire) && triShot)
        {
            GameObject go = Instantiate(bulletPrefab);
            Bullet bullet = go.GetComponent<Bullet>();

            Bullet.PowerLevel powerLevel;
            if (phaserShot)
                powerLevel = Bullet.PowerLevel.PhaserShot;
            else if (laserBeam)
                powerLevel = Bullet.PowerLevel.LaserBeam;
            else
                powerLevel = Bullet.PowerLevel.TriShot;

            bullet.PlaceBullet(transform.position, spriteRenderer.flipX ? -1 : 1, powerLevel);
        }
    }

    private bool MorphReposition()
    {
        Vector3 startPoint = transform.position;

        for (float moveDis = 0; moveDis < shrinkAdaptionDistance; moveDis += PixelSize)
        {
            for (float rotateRadians = 0; rotateRadians <= (2 * Mathf.PI); rotateRadians += (2 * Mathf.PI) / shrinkAdaptionCheckInterval)
            {
                transform.position = startPoint;
                transform.Translate(Mathf.Sin(rotateRadians) * moveDis, Mathf.Cos(rotateRadians) * moveDis, 0);

                if (TouchingGroundPoint())
                    continue;

                if (TouchingGroundComplete())
                    continue;

                return false;
            }
        }

        transform.position = startPoint;
        return true;
    }

    public static class ControlMapping
    {
        public const string MoveX = "Horizontal";
        public const string MoveY = "Vertical";
        public const string Dash = "Fire1";
        public const string Fire = "Fire2";
        public const string MapToggle = "Fire3";
        public const string Morph = "Vertical";
    }
}

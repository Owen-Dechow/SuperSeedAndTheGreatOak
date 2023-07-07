using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Header("Speed")]
    [SerializeField] Vector2 speed;
    [SerializeField] Vector2 clampSpeed;
    [SerializeField] float airSpeed;

    [Header("Drag")]
    [SerializeField] float drag;
    [SerializeField] float airDrag;

    [Header("Jumping")]
    [SerializeField] float maxJumpSeconds;
    [SerializeField] float airJumpLeewaySeconds;
    float jumpSeconds;
    float fallingSeconds;

    [Header("Wall Jumping")]
    [SerializeField] bool canWallJump;
    [SerializeField] Vector2 wallJumpVelocity;

    [Header("Dashing")]
    [SerializeField] bool canDash;
    [SerializeField] float dashVelocity;

    [Header("Morphing")]
    [SerializeField] bool canShrink;
    [SerializeField] Vector3 shrinkSize;
    [SerializeField] float shrinkAdaptionDistance;
    [SerializeField] float shrinkAdaptionCheckInterval;
    Vector3 grownSize;
    bool isShrunk;

    public static Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        playerTransform = transform;
        jumpSeconds = maxJumpSeconds + 1;
        fallingSeconds = airJumpLeewaySeconds + 1;
        grownSize = transform.localScale;
        isShrunk = false;

        if (Door.OverridePlayerPositionOnLoad)
        {
            transform.position = Door.PlayerPositionOnLoad;
        }
        spriteRenderer.flipX = Door.FlipPlayerOnLoad;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameManager.GreyScale = !GameManager.GreyScale;
        }
 
        // Get player input
        Vector2 input = new(
            Input.GetAxisRaw(ControlMapping.MoveX),
            Input.GetAxisRaw(ControlMapping.MoveY));

        // Run physics
        Controller.PhysicsInfo physicsInfo = UpdatePosition(Time.deltaTime);

        // Update falling
        velocity.y -= 0.01f;
        if (physicsInfo.feetOnGround) fallingSeconds = 0;
        else if (physicsInfo.hitHead) fallingSeconds = airJumpLeewaySeconds + 1;
        else fallingSeconds += Time.deltaTime;

        // Jump/ wall jump
        if (input.y > 0)
        {
            if (jumpSeconds <= maxJumpSeconds)
            {
                jumpSeconds += Time.deltaTime;
                velocity.y = speed.y;
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
                jumpSeconds = maxJumpSeconds + 1;
            }
        }

        // Move x
        if (physicsInfo.feetOnGround)
        {
            velocity.x += speed.x * input.x;
            velocity.x *= drag;
        }
        else
        {
            if (Input.GetButtonDown(ControlMapping.Dash))
                velocity.x = dashVelocity * input.x;

            velocity.x += airSpeed * input.x;
            velocity.x *= airDrag;
        }

        // Clamp Speed
        velocity = new(
            Mathf.Clamp(velocity.x, -clampSpeed.x, clampSpeed.x),
            Mathf.Clamp(velocity.y, -clampSpeed.y, clampSpeed.y));

        // Morph: Shrink/ Grow
        if (Input.GetButtonDown(ControlMapping.Morph) && input.y == -1)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [SerializeField] Vector2 speed;
    [SerializeField] Vector2 clampSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] float drag;
    [SerializeField] float airDrag;
    [SerializeField] float maxJumpSeconds;
    [SerializeField] float airJumpLeewaySeconds;

    [SerializeField] bool canWallJump;
    [SerializeField] Vector2 wallJumpVelocity;

    [SerializeField] bool canDash;
    [SerializeField] float dashVelocity;

    float jumpSeconds;
    float fallingSeconds;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        jumpSeconds = maxJumpSeconds + 1;
        fallingSeconds = airJumpLeewaySeconds + 1;
    }

    void Update()
    {
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
            else if (physicsInfo.setForWallJump != 0 && canWallJump && Input.GetButtonDown(ControlMapping.MoveY))
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
    }

    public static class ControlMapping
    {
        public const string MoveX = "Horizontal";
        public const string MoveY = "Vertical";
        public const string Dash = "Fire1";
        public const string Fire = "Fire2";
        public const string MapToggle = "Fire3";
    }
}

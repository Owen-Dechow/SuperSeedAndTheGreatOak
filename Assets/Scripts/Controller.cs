using System.Collections;
using UnityEngine;
using static Controller;


public class Controller : MonoBehaviour
{
    [SerializeField] int maxSlopePerPixel;
    protected Vector2 velocity;
    protected BoxCollider2D boxCollider;
    protected SpriteRenderer spriteRenderer;
    protected float PixelSize { get => 1 / Level.PixelsPerUnit; }

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected Vector2 GetPointOnCollider(float x, float y)
    {
        Vector2 center = (Vector2)transform.position + boxCollider.offset * transform.localScale;
        Vector2 topLeft = center - boxCollider.size / 2 * transform.localScale;
        Vector2 point = topLeft + new Vector2(x, -y) * boxCollider.size * transform.localScale;
        point.y += boxCollider.size.y * transform.localScale.y;

        Debug.DrawLine(center, point, Color.white);

        return point;
    }

    protected bool BottomTouchingGround()
    {
        for (float f = 0; f <= 1; f += 0.1f)
        {
            if (Level.TouchingGround(GetPointOnCollider(f, 1)))
                return true;
        }
        return TouchingGround();
    }

    protected bool TopTouchingGround()
    {
        for (float f = 0; f <= 1; f += 0.1f)
        {
            if (Level.TouchingGround(GetPointOnCollider(f, 0)))
                return true;
        }
        return TouchingGround();
    }

    protected bool LeftTouchingGround()
    {
        for (float f = 0; f <= 1; f += 0.1f)
        {
            if (Level.TouchingGround(GetPointOnCollider(0, f)))
                return true;
        }
        return TouchingGround();
    }

    protected bool RightTouchingGround()
    {
        for (float f = 0; f <= 1; f += 0.1f)
        {
            if (Level.TouchingGround(GetPointOnCollider(1, f)))
                return true;
        }
        return TouchingGround();
    }

    protected bool TouchingGround()
    {
        return Level.TouchingGround(GetPointOnCollider(0, 0)) ||
            Level.TouchingGround(GetPointOnCollider(1, 0)) ||
            Level.TouchingGround(GetPointOnCollider(1, 1)) ||
            Level.TouchingGround(GetPointOnCollider(0, 1));
    }

    protected PhysicsInfo UpdatePosition(float deltaTime)
    {
        PhysicsInfo physicsInfo = new();

        // Flip player
        if (velocity.x > 0)
            spriteRenderer.flipX = false;
        else if (velocity.x < 0)
            spriteRenderer.flipX = true;

        // Move Y
        transform.Translate(0, velocity.y * deltaTime, 0);
        if (velocity.y < 0)
        {
            while (BottomTouchingGround())
            {
                transform.Translate(0, PixelSize * deltaTime, 0);
                velocity.y = 0;
                physicsInfo.feetOnGround = true;
            }
        }
        else
        {
            while (TopTouchingGround())
            {
                transform.Translate(0, -PixelSize * deltaTime, 0);
                velocity.y = 0;
                physicsInfo.hitHead = true;
            }
        }

        // Move X
        transform.Translate(velocity.x * deltaTime, 0, 0);
        if (velocity.x < 0)
        {
            MoveX(deltaTime, 1, LeftTouchingGround, ref physicsInfo);
        }
        else
        {
            MoveX(deltaTime, -1, RightTouchingGround, ref physicsInfo);
        }

        return physicsInfo;
    }

    private void MoveX(float deltaTime, float backOutDirection, System.Func<bool> sideCheckFunc, ref PhysicsInfo physicsInfo)
    {
        bool touchingGround = sideCheckFunc();
        if (touchingGround)
            touchingGround = ExitSlope(sideCheckFunc, deltaTime);

        while (touchingGround)
        {
            velocity.x = 0;
            transform.Translate(PixelSize * deltaTime * backOutDirection, 0, 0);
            touchingGround = sideCheckFunc();
            if (velocity.y < 0)
                physicsInfo.setForWallJump = -1;
        }
    }

    private bool ExitSlope(System.Func<bool> sideCheckFunc, float deltaTime)
    {
        bool touchingGround = true;
        int maxSlope = Mathf.CeilToInt(Mathf.Abs(velocity.x) * deltaTime / Level.PixelsPerUnit) * maxSlopePerPixel;

        for (int i = 0; i < maxSlope; i++)
        {
            transform.Translate(0, PixelSize, 0);
            if (TopTouchingGround() || sideCheckFunc())
                continue;

            touchingGround = false;
            break;
        }

        if (touchingGround)
            transform.Translate(0, -PixelSize * maxSlope, 0);

        return touchingGround;
    }

    public class PhysicsInfo
    {
        public bool feetOnGround;
        public int setForWallJump;
        public bool hitHead;

        public PhysicsInfo()
        {
            feetOnGround = false;
            setForWallJump = 0;
            hitHead = false;
        }
    }
}

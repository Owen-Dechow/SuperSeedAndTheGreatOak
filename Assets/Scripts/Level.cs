using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level instance;

    [SerializeField] Texture2D groundTexture;

    public static Vector2 GroundSize { get => instance.groundSize; }
    [SerializeField] Vector2 groundSize;

    public static float PixelsPerUnit { get => instance.pixelsPerUnit; }
    [SerializeField] float pixelsPerUnit;



    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    public static bool TouchingGround(Vector2 pos) => instance.InternalTouchingGround(pos);
    private bool InternalTouchingGround(Vector2 pos)
    {
        Vector2 offsetPosition = pos + (groundSize / pixelsPerUnit / 2);
        Vector2Int intPositionOnTexture = Vector2Int.FloorToInt(offsetPosition * pixelsPerUnit);
        return groundTexture.GetPixel(intPositionOnTexture.x, intPositionOnTexture.y).a != 0;
    }

}
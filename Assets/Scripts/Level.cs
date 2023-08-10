using System.Collections;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level instance;

    [SerializeField] Texture2D groundTexture;
    [SerializeField] Texture2D groundTextureBlocked;

    public static Vector2 GroundSize => instance.groundSize;
    [SerializeField] Vector2 groundSize;

    public static float PixelsPerUnit  => instance.pixelsPerUnit; 
    [SerializeField] float pixelsPerUnit;

    public static float Gravity  => instance.gravity; 
    [SerializeField] float gravity;

    public static bool Blocked { get; set; }

    // Use this for initialization
    void Start()
    {
        instance = this;
        Blocked = true;
    }

    public static bool TouchingGround(Vector2 pos) => instance.InternalTouchingGround(pos);
    private bool InternalTouchingGround(Vector2 pos)
    {
        Vector2 offsetPosition = pos + (groundSize / pixelsPerUnit / 2);
        Vector2Int intPositionOnTexture = Vector2Int.FloorToInt(offsetPosition * pixelsPerUnit);

        if (Blocked)
            return groundTextureBlocked.GetPixel(intPositionOnTexture.x, intPositionOnTexture.y).a != 0;
        else
            return groundTexture.GetPixel(intPositionOnTexture.x, intPositionOnTexture.y).a != 0;
        
    }
}
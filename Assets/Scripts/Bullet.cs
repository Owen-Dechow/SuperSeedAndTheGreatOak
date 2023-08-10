using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Controller
{
    public enum PowerLevel
    {
        TriShot = 1,
        LaserBeam = 3,
        PhaserShot = 6,
    }

    [SerializeField] float speed;
    [SerializeField] Sprite TriShotPowerLevelSprite;
    [SerializeField] Sprite LaserBeamPowerLevelSprite;
    [SerializeField] Sprite PhaserShotPowerLevelSprite;

    [SerializeField] Material greyScaleMaterial;
    [SerializeField] Material laserMaterial;

    int direction;
    public PowerLevel powerLevel;
    Vector3 size;

    public void PlaceBullet(Vector2 position, int direction, PowerLevel powerLevel)
    {
        transform.position = position;
        this.direction = direction;
        size = transform.localScale;

        this.powerLevel = powerLevel;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = direction < 0;
        spriteRenderer.material = powerLevel >= PowerLevel.LaserBeam ? laserMaterial : greyScaleMaterial;
        spriteRenderer.sprite = powerLevel switch
        {
            PowerLevel.TriShot => TriShotPowerLevelSprite,
            PowerLevel.LaserBeam => LaserBeamPowerLevelSprite,
            PowerLevel.PhaserShot => PhaserShotPowerLevelSprite,
            _ => throw new System.NotImplementedException(),
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (powerLevel >= PowerLevel.PhaserShot)
        {
            transform.localScale = new Vector3(
                size.x + Random.Range(-0.08f, 0.05f),
                size.y + Random.Range(-0.08f, 0.05f),
                size.z
                );

            spriteRenderer.flipY = Random.Range(0, 2) == 0;
        }

        transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);
        if (TouchingGroundPoint())
        {
            Destroy(gameObject, 0.1f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public static bool GreyScale { get => instance.greyScale; set => SetGreyScale(value); }
    public static PlayerCollectionData PlayerCollection { get; private set; }
    public static bool GameManagerExists { get => instance != null; }

    [SerializeField] Material[] greyScaleMaterials;
    [SerializeField] Material laserMaterial;
    [SerializeField] float laserMaterialShiftSpeed;
    bool greyScale;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetGreyScale(bool to)
    {
        instance.greyScale = to;
        foreach (Material material in instance.greyScaleMaterials)
        {
            material.SetFloat("_Saturation", to ? 0 : 1);
        }
    }

    public static void CollectPlayer(PlayerController player)
    {
        PlayerCollection = new PlayerCollectionData(player.canHighJump, player.canWallJump, player.canDash, player.canShrink, player.life, player.maxLife);
    }

    public class PlayerCollectionData
    {
        public bool CanHighJump { get; private set; }
        public bool CanWallJump {get; private set;}
        public bool CanDash { get; private set; }
        public bool CanShrink { get; private set; }
        public int Life { get; private set; }
        public int MaxLife { get; private set; }

        public PlayerCollectionData(bool canHighJump, bool canWallJump, bool canDash, bool canShrink, int life, int maxLife)
        {
            this.CanHighJump = canHighJump;
            this.CanWallJump = canWallJump;
            this.CanDash = canDash;
            this.CanShrink = canShrink;
            this.Life = life;
            this.MaxLife = maxLife;
        }
    }

    private void FixedUpdate()
    {
        laserMaterial.SetFloat("_Shift", Time.time * laserMaterialShiftSpeed % 1);
    }
}

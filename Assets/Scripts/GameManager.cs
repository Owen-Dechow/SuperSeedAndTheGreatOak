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
        PlayerCollection = new PlayerCollectionData(player.canHighJump, player.canWallJump, player.canDash, player.canShrink, player.life);
    }

    public class PlayerCollectionData
    {
        public bool canHighJump { get; private set; }
        public bool canWallJump {get; private set;}
        public bool canDash { get; private set; }
        public bool canShrink { get; private set; }
        public int life { get; private set; }

        public PlayerCollectionData(bool canHighJump, bool canWallJump, bool canDash, bool canShrink, int life)
        {
            this.canHighJump = canHighJump;
            this.canWallJump = canWallJump;
            this.canDash = canDash;
            this.canShrink = canShrink;
            this.life = life;
        }
    }
}

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

    public enum GameLevel
    {
        Tree = 0,
        Mine = 1,
        Factory = 2,
        Cloud = 3
    }

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

    public static void CollectPlayer(PlayerController player, Door.PlayerDirection playerDirectionOnLoad, Vector2? playerLocationOnLoad)
    {
        PlayerCollection = new PlayerCollectionData(
            player.stats,
            playerDirectionOnLoad,
            playerLocationOnLoad);
    }

    public class PlayerCollectionData
    {
        public PlayerController.PlayerStats Stats { get; private set; }
        public Door.PlayerDirection PlayerDirectionOnLoad { get; private set; }
        public Vector2? PlayerLocationOnLoad { get; private set; }

        public PlayerCollectionData(PlayerController.PlayerStats stats, Door.PlayerDirection playerDirectionOnLoad, Vector2? playerLocationOnLoad)
        {
            Stats = stats;
            PlayerDirectionOnLoad = playerDirectionOnLoad;
            PlayerLocationOnLoad = playerLocationOnLoad;
        }
    }

    private void FixedUpdate()
    {
        laserMaterial.SetFloat("_Shift", Time.time * laserMaterialShiftSpeed % 1);
    }

    public static void LoadLevel(GameLevel level)
    {
        instance.StartCoroutine(LoadLevelAnimated(level));
    }

    static IEnumerator LoadLevelAnimated(GameLevel level)
    {
        Time.timeScale = 0;
        yield return GameUI.ToggleSceneTransition(true);
        SceneManager.LoadScene((int)level);
        yield return GameUI.ToggleSceneTransition(false);
        Time.timeScale = 1;
    }
}

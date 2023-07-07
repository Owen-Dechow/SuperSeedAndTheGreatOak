using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public static bool GreyScale { get => GreyScaleMaterial.GetFloat("_Saturation") == 0; set => GreyScaleMaterial.SetFloat("_Saturation", value ? 0 : 1); }
    public static Material GreyScaleMaterial { get; set; }

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Build a Game Manager if none exists
    public static bool BuildGameManager()
    {
        if (instance)
            return false;

        _ = new GameObject("GameManager", typeof(GameManager));
        return true;
    }
}

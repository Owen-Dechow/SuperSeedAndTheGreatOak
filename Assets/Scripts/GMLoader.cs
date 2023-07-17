using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLoader : MonoBehaviour
{
    [SerializeField] GameObject gameManager;

    void Start()
    {
        if (!GameManager.GameManagerExists)
        {
            GameObject go = Instantiate(gameManager);
            go.name = "GameManager";
        }
        Destroy(gameObject);
    }
}

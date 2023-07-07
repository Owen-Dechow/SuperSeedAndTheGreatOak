using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMLoader : MonoBehaviour
{
    [SerializeField] Material greyScaleMaterial;

    void Start()
    {
        if (GameManager.BuildGameManager())
        {
            GameManager.GreyScaleMaterial = greyScaleMaterial;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    static GameUI instance;

    [SerializeField] Slider slider;
    [SerializeField] Material hueShiftMaterial;
    [SerializeField] float hueShiftSpeed;

    private void Start()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        hueShiftMaterial.SetFloat("_Shift", (hueShiftMaterial.GetFloat("_Shift") + hueShiftSpeed * Time.fixedDeltaTime) % 1);
    }

    public static void SetLifeMeter(int life)
    {
        instance.slider.value = life;
    }
}

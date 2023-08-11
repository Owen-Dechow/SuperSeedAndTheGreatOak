using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    static GameUI instance;

    [Header("Life bar")]
    [SerializeField] Slider slider;
    [SerializeField] Material hueShiftMaterial;
    [SerializeField] float hueShiftSpeed;

    [Header("Scene transition")]
    [SerializeField] CanvasGroup crossFadeCanvasGroup;
    [SerializeField] float transitionSeconds;

    [Header("Text box")]
    [SerializeField] GameObject textBox;
    [SerializeField] TMPro.TextMeshProUGUI textMeshPro;
    [SerializeField] float typingPauseSeconds;

    private void Start()
    {
        instance = this;
        textBox.SetActive(false);
    }

    void FixedUpdate()
    {
        hueShiftMaterial.SetFloat("_Shift", (hueShiftMaterial.GetFloat("_Shift") + hueShiftSpeed * Time.fixedDeltaTime) % 1);
    }

    public static void SetLifeMeter(int life)
    {
        instance.slider.value = life;
    }

    public static IEnumerator ToggleSceneTransition(bool onOff)
    {
        if (instance.transitionSeconds <= 0)
            instance.transitionSeconds = 0.0001f;

        if (onOff)
            yield return instance.ToggleSceneTransitionOn();
        else
            yield return instance.ToggleSceneTransitionOff();
    }

    IEnumerator ToggleSceneTransitionOn()
    {
        while (crossFadeCanvasGroup.alpha < 1)
        {
            crossFadeCanvasGroup.alpha += Time.unscaledDeltaTime / transitionSeconds;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ToggleSceneTransitionOff()
    {
        while (crossFadeCanvasGroup.alpha > 0)
        {
            crossFadeCanvasGroup.alpha -= Time.unscaledDeltaTime / transitionSeconds;
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerator TextBox(string text)
    {
        yield return new WaitForEndOfFrame();
        instance.textBox.SetActive(true);
        instance.textMeshPro.text = "";

        foreach (char letter in text)
        {
            instance.textMeshPro.text += letter;
            yield return new WaitForSecondsRealtime(instance.typingPauseSeconds);
        }

        yield return new WaitUntil(() => Input.GetButtonDown(PlayerController.ControlMapping.Select));
        instance.textBox.SetActive(false);
    }
}

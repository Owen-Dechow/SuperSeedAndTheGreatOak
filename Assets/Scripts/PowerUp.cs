using System.Collections;
using UnityEngine;
using System.Reflection;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    [SerializeField] string PlayerPropertyName;
    [SerializeField] string[] textMessage;

    void Update()
    {
        transform.Rotate(Time.deltaTime * turnSpeed * Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            PlayerController.PlayerStats stats = pc.stats;
            FieldInfo property = stats.GetType().GetField(PlayerPropertyName);
            property.SetValue(stats, true);
            pc.stats = stats;
            StartCoroutine(HitPlayer());
        }
    }

    IEnumerator HitPlayer()
    {
        Time.timeScale = 0;

        foreach (string text in textMessage)
        {
            yield return GameUI.TextBox(text);
        }

        Time.timeScale = 1;
        Destroy(gameObject);
    }

}

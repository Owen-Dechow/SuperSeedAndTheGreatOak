using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float turnSpeed;
    [SerializeField] string PlayerPropertyName;

    void Update()
    {
        transform.Rotate(Time.deltaTime * turnSpeed * Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            pc.GetType().GetField(PlayerPropertyName).SetValue(pc, true);
        }
        Destroy(gameObject);
    }
}

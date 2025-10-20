using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Events.OnEnemyHitPlayer?.Invoke(damage);
        }
        Destroy(gameObject);
    }
}

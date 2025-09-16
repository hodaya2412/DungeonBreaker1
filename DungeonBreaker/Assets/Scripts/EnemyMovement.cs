using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 2f;         
    [SerializeField] float moveDistance = 3f;  

    private Vector2 startPos;
    private int direction = 1; 

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (transform.position.x > startPos.x + moveDistance)
        {
            direction = -1; 
            Flip();
        }
        else if (transform.position.x < startPos.x - moveDistance)
        {
            direction = 1; 
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] Animator animator;
    public bool canMove = true;

    private int direction = 1; 

    private void Update()
    {
        if (!canMove)
        {
            if (animator != null)
                animator.SetFloat("Speed", 0);
            return;
        }

        
        transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);

        
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(speed));
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * (direction == 1 ? 1 : -1);
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            direction *= -1; 
            Flip();
        }
    }

    public void StopMoving()
    {
        canMove = false;
    }

    public void ResumeMoving()
    {
        canMove = true;
    }
}

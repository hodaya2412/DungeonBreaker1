using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;

    public PatrolState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
        
    }

    public override void Execute()
    {
        // קוד פשוט לפטרול:
        // האויב זז ימינה ושמאלה אוטומטית לפי קירות (כבר יש לך ב-EnemyMovement)
        // ניתן לבדוק אם השחקן נמצא בטווח ולשנות מצב ל-Chase
    }

    public override void Exit()
    {
        enemy.StopMoving();
    }
}



public class ChaseState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;


    public ChaseState(EnemyMovement enemy, Transform player)
    {
        this.enemy = enemy;
        this.player = player;
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (player == null) return;

        
        enemy.SetDirection(player.position.x < enemy.transform.position.x ? 1 : -1);

        
        enemy.transform.position +=  Vector3.left * enemy.transform.localScale.x * enemy.GetSpeed() * Time.deltaTime;

       
        if (Vector2.Distance(enemy.transform.position, player.position) < 1.5f)
        {
            enemy.ChangeState(enemy.attackState);
        }
    }

    public override void Exit()
    {
        enemy.StopMoving();
    }
}


public class AttackState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public AttackState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public override void Enter()
    {
        enemy.StopMoving();
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("IsAttacking");
    }

    public override void Execute()
    {
        // כאן אפשר להוסיף קוד לגרימת נזק לשחקן
        // אפשר לבדוק מרחק ולחזור ל-Chase אם השחקן התרחק
        if (player != null && Vector2.Distance(enemy.transform.position, player.position) > 2f)
        {
            enemy.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit()
    {
        enemy.ResumeMoving();
    }
}

public class SpecialAttackState : IEnemyState
{
    private EnemyMovement enemy;

    public SpecialAttackState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.StopMoving();
        
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("SpecialAttack");
    }

    public override void Execute()
    {
        // קוד למתקפה מיוחדת
    }

    public override void Exit()
    {
        enemy.ResumeMoving();
    }
}


public class DieState : IEnemyState
{
    private EnemyMovement enemy;

    public DieState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        enemy.StopMoving();
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("IsDead");

        // אפשר למחוק את האוייב אחרי אנימציית מוות
        Object.Destroy(enemy.gameObject, 2f);
    }

    public override void Execute() { }

    public override void Exit() { }
}


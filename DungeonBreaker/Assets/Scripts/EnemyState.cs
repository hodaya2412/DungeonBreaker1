using UnityEngine;

#region IdleState
public class IdleState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public IdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        enemy.StopMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        enemy.ResumeMoving();
    }
}
#endregion

#region PatrolState
public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public PatrolState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        // תזוזת פטרול מתבצעת ב-EnemyMovement
    }

    public override void Exit() { }
}
#endregion

#region ChaseState
public class ChaseState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public ChaseState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        // הכוון לשחקנית
        int dir = (player.position.x > enemy.transform.position.x) ? 1 : -1;
        enemy.SetDirection(dir);

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist > detection)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }
    }

    public override void Exit() { }
}
#endregion

#region AttackState

public class AttackState : IEnemyState
{
    private EnemyMovement enemy;
    private EnemyAttack attackComp;
    private Transform player;
    private bool attackedThisEntry = false;
    private float attackTime = 0f;
    private float postAttackDelay = 1f; // זמן חיכוי אחרי התקפה (בין התקפה לפטרול)

    public AttackState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        attackComp = enemy.GetComponent<EnemyAttack>();
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        attackedThisEntry = false;
        attackTime = 0f;
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;

        // אם עדיין לא התקפנו — הפעל התקפה
        if (!attackedThisEntry && attackComp != null && attackComp.CanAttack() && dist <= attackRange)
        {
            attackComp.TriggerAttack();
            attackedThisEntry = true;
            attackTime = Time.time; // רשום את זמן ההתקפה
            return;
        }

        // אם התקפה בוצעה, חכה כמה שניות לפני המעבר לפטרול
        if (attackedThisEntry)
        {
            if (Time.time - attackTime >= postAttackDelay)
            {
                enemy.ChangeState(enemy.patrolState);
                return;
            }
            // אחרת נשאר כאן ומחכה
            return;
        }

        // אם השחקנית יצאה מטווח דיטקשן — חזור לפטרול
        if (dist > detection)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }
    }

    public override void Exit()
    {
        // הפטרול כבר יטפל בכיוונים
    }
}

#endregion

#region DieState
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
        var attack = enemy.GetEnemyAttack();
        if (attack != null) attack.enabled = false;
        enemy.GetAnimator()?.SetTrigger("IsDead");
    }

    public override void Execute() { }

    public override void Exit() { }
}
#endregion

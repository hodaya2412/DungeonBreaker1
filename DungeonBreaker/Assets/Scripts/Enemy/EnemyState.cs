using Unity.VisualScripting;
using UnityEngine;

#region SpiritIdleState
public class SpiritIdleState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public SpiritIdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=cyan>{enemy.name} entered SpiritIdleState</color>");
        // עצירה דרך Events
        Events.RequestMove(enemy, false);
    }

    public override void Execute()
    {
        Debug.Log($"<color=cyan>{enemy.name} executing SpiritIdleState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        // שינוי כיוון לפי מיקום השחקן
        int dir = (player.position.x > enemy.transform.position.x) ? -1 : 1;
        Events.RequestDirection(enemy, dir);

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;
        var attackComp = enemy.GetEnemyAttack();

        if (enemy.GetEnemyDate().enemyType == EnemyType.Spirit)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                Events.RequestStateChange(enemy, enemy.attackState);
                return;
            }

            if (dist <= detection)
            {
                Events.RequestStateChange(enemy, enemy.chaseState);
                return;
            }
        }

        if (enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                Events.RequestStateChange(enemy, enemy.attackState);
                return;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=cyan>{enemy.name} exited SpiritIdleState</color>");
        // חזרה לתנועה דרך Events
        Events.RequestMove(enemy, true);
    }
}

#endregion

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
        Debug.Log($"<color=yellow>{enemy.name} entered IdleState</color>");
        // במקום enemy.StopMoving()
        Events.RequestMove(enemy, false);
    }

    public override void Execute()
    {
        Debug.Log($"<color=yellow>{enemy.name} executing IdleState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            // במקום enemy.ChangeState(enemy.attackState)
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            // במקום enemy.ChangeState(enemy.chaseState)
            Events.RequestStateChange(enemy, enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=yellow>{enemy.name} exited IdleState</color>");
        // במקום enemy.ResumeMoving()
        Events.RequestMove(enemy, true);
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
        Debug.Log($"<color=green>{enemy.name} entered PatrolState</color>");
        // חזרה לתנועה דרך Events
        Events.RequestMove(enemy, true);
    }

    public override void Execute()
    {
        Debug.Log($"<color=green>{enemy.name} executing PatrolState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            // בקשה למעבר למצב התקפה
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            // בקשה למעבר למצב רדיפה
            Events.RequestStateChange(enemy, enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=green>{enemy.name} exited PatrolState</color>");
        // אין קריאה ישירה ל-EnemyMovement – הכל דרך Events אם צריך
    }
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
        Debug.Log($"<color=magenta>{enemy.name} entered ChaseState</color>");
        // חזרה לתנועה דרך Events
        Events.RequestMove(enemy, true);
    }

    public override void Execute()
    {
        Debug.Log($"<color=magenta>{enemy.name} executing ChaseState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        // שינוי כיוון דרך Events
        int dir = (player.position.x > enemy.transform.position.x) ? 1 : -1;
        Events.RequestDirection(enemy, dir);

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist > detection)
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=magenta>{enemy.name} exited ChaseState</color>");
        // אין קריאות ישירות ל-EnemyMovement – הכל דרך Events
    }
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
    private float postAttackDelay = 1f;

    public AttackState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        attackComp = enemy.GetComponent<EnemyAttack>();
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=red>{enemy.name} entered AttackState</color>");
        attackedThisEntry = false;
        attackTime = 0f;
    }

    public override void Execute()
    {
        Debug.Log($"<color=red>{enemy.name} executing AttackState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            ReturnToIdleOrPatrol();
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;

        // אם בטווח ויכול לתקוף – מבצע התקפה
        if (!attackedThisEntry && attackComp != null && attackComp.CanAttack() && dist <= attackRange)
        {
            Debug.Log($"<color=red>{enemy.name} ATTACK</color>");
            attackComp.TriggerAttack();
            attackedThisEntry = true;
            attackTime = Time.time;
            return;
        }

        // אם התקפה בוצעה – מחכים את זמן ה-delay לפני חזרה למצב אחר
        if (attackedThisEntry)
        {
            if (Time.time - attackTime >= postAttackDelay)
            {
                ReturnToIdleOrPatrol();
                return;
            }
            return;
        }

        // אם השחקן רחוק מדי
        if (dist > detection)
        {
            ReturnToIdleOrPatrol();
            return;
        }
    }

    private void ReturnToIdleOrPatrol()
    {
        // במקום enemy.ChangeState(...) משתמשים ב-Events
        if (enemy.GetEnemyDate().enemyType == EnemyType.Spirit || enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            Events.RequestStateChange(enemy, enemy.spiritIdleState);
        }
        else
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=red>{enemy.name} exited AttackState</color>");
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
        Debug.Log($"<color=grey>{enemy.name} entered DieState</color>");

        // עצירת תנועה דרך האירועים
        Events.RequestMove(enemy, false);

        // ביטול יכולת התקפה
        var attack = enemy.GetEnemyAttack();
        if (attack != null)
            attack.enabled = false;

        // הפעלת אנימציית מוות
        enemy.GetAnimator()?.SetTrigger("IsDead");

        // שליחת אירוע גלובלי לאויב מת (אם רוצים לעדכן מערכת ספירת אויבים למשל)
        Events.OnEnemyDeath?.Invoke(enemy.gameObject);
    }

    public override void Execute()
    {
        Debug.Log($"<color=grey>{enemy.name} executing DieState</color>");
        // אין צורך לעשות כאן כלום – האויב מת
    }

    public override void Exit()
    {
        Debug.Log($"<color=grey>{enemy.name} exited DieState</color>");
    }
}

#endregion

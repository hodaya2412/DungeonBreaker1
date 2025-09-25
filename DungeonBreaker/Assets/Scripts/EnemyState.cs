using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;

    public PatrolState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.ResumeMoving();
        
    }

    public void Execute()
    {
        // ��� ���� ������:
        // ����� �� ����� ������ �������� ��� ����� (��� �� �� �-EnemyMovement)
        // ���� ����� �� ����� ���� ����� ������ ��� �-Chase
    }

    public void Exit()
    {
        enemy.StopMoving();
    }
}



public class ChaseState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public ChaseState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void Enter()
    {
        enemy.ResumeMoving();
    }

    public void Execute()
    {
        if (player == null) return;

        
        enemy.SetDirection(player.position.x > enemy.transform.position.x ? 1 : -1);

        
        enemy.transform.position += Vector3.right * enemy.transform.localScale.x * enemy.GetSpeed() * Time.deltaTime;

       
        if (Vector2.Distance(enemy.transform.position, player.position) < 1.5f)
        {
            enemy.ChangeState(enemy.attackState);
        }
    }

    public void Exit()
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

    public void Enter()
    {
        enemy.StopMoving();
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("Attack");
    }

    public void Execute()
    {
        // ��� ���� ������ ��� ������ ��� �����
        // ���� ����� ���� ������ �-Chase �� ����� �����
        if (player != null && Vector2.Distance(enemy.transform.position, player.position) > 2f)
        {
            enemy.ChangeState(enemy.chaseState);
        }
    }

    public void Exit()
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

    public void Enter()
    {
        enemy.StopMoving();
        
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("SpecialAttack");
    }

    public void Execute()
    {
        // ��� ������ ������
    }

    public void Exit()
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

    public void Enter()
    {
        enemy.StopMoving();
        Animator anim = enemy.GetAnimator();
        if (anim != null)
            anim.SetTrigger("Die");

        // ���� ����� �� ������ ���� �������� ����
        Object.Destroy(enemy.gameObject, 2f);
    }

    public void Execute() { }

    public void Exit() { }
}


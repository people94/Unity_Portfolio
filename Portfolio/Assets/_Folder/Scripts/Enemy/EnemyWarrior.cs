using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWarrior : EnemyFSM
{
    public float _findRange = 15f;
    public float _moveRange = 30f;
    public float _attackRange = 2f;   

    private void OnEnable()
    {
        findRange = _findRange;
        moveRange = _moveRange;
        attackRange = _attackRange;
    }

    public override void Idle()
    {
        Debug.Log("EnemyWarrior : Idle");
        base.Idle();
    }


    public override void Move()
    {
        print("Warrior : Idle -> Move");
        base.Move();
    }

    public override void Attack()
    {
        print("Warrior : Move -> Attack");
        base.Attack();
    }

    public override void Return()
    {
        print("Warrior : Move -> Return");
        base.Return();
    }

    public override void Damaged()
    {
        print("Warrior : Damaged");
        base.Damaged();
    }

    public override void Die()
    {
        print("Warrior : Die");
        base.Die();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZoneControl : MonoBehaviour
{

    private Rigidbody2D parentRigidBody;
    public float attackCycleTime;
    public float attackPower;
    public float backForceByAttack;

    public float currentAttackCycle;

    private bool isAttacking;

    // Use this for initialization
    void Start()
    {
        parentRigidBody = GetComponentInParent<Rigidbody2D>();

        Debug.Log(parentRigidBody);

        attackCycleTime = 0.5f;
        attackPower = 5f;
        backForceByAttack = 10f;

        currentAttackCycle = 0;

        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacking)
        {
            currentAttackCycle += 0.1f * Time.deltaTime;

            if(currentAttackCycle >= attackCycleTime)
            {
                isAttacking = false;
                currentAttackCycle = 0f;
            }
        }
    }

    public void FixedUpdate()
    {

    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "enemy" && !isAttacking)
        {
            isAttacking = true;

            Vector2 direcVec = parentRigidBody.transform.position - collision.transform.position;
            direcVec.Normalize();
            parentRigidBody.AddForce(direcVec * backForceByAttack, ForceMode2D.Force);
        }
    }
}

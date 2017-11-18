using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitInfo : MonoBehaviour {

    public int hp = 0;
    int atk_damage = 10;
    Rigidbody2D rigidBody2D;

    bool isDead = false;
	
    void Start()
    {
        hp = 100;
        isDead = false;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void deal_damage(int damage)
    {
        hp -= damage;

        if (hp <= 0) isDead = true;
    }

    public bool getIsDead()
    {
        return isDead;
    }
}

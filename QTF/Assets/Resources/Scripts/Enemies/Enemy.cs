using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : FallingObject, IDamagable
{
    public AudioClip deathSound;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        currentHP = 1;
        damage = 1;
    }

    public int currentHP
    {
        get;
        protected set;
    }

    protected int damage;

    private void DealDamageToPlayer()
    {
        Player.ApplyDamage(damage);
    }

    public void DealDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Game.instance.RewardPlayer(100);
        Audio.instance.PlaySoundOnce(deathSound);
    }

    protected override void OnFall()
    {
        DealDamageToPlayer();
        Destroy(gameObject);
    }

    protected override void OnCollide(Collision2D collision)
    {
        DealDamageToPlayer();
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Enemy
{
    protected override void Init()
    {
        currentHP = 2;
        damage = 2;
    }

    private void DealDamageToPlayer()
    {
        Player.ApplyDamage(2);
    }

    protected override void OnDeath()
    {
        Game.instance.RewardPlayer(400);
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

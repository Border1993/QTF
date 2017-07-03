using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : Enemy
{
    public AudioClip catchSound;

    protected override void OnDeath()
    {
        Player.instance.DealDamage(1);
        Game.instance.RewardPlayer(-100);
        Audio.instance.PlaySoundOnce(deathSound);
    }

    protected override void OnCollide(Collision2D collision)
    {
        Game.instance.RewardPlayer(250);
        Destroy(gameObject);
    }

    protected override void OnFall()
    {
        Game.instance.RewardPlayer(50);
    }
}

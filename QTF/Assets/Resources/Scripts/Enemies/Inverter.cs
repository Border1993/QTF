using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Enemy
{
    public AudioClip shootSound;
    public GameObject inverterBullet;
    private float shootCooldownRemaining;
    static private GameObject bulletRoot;

    public void Shoot()
    {
        GameObject bullet = Instantiate(inverterBullet);
        bullet.transform.SetParent(bulletRoot.transform);
        bullet.transform.position = transform.position;
        Audio.instance.PlaySoundOnce(shootSound);
    }

    public static void SetBulletRoot(GameObject bulletRoot)
    {
        Inverter.bulletRoot = bulletRoot;
    }

    protected override void OnDeath()
    {
        Game.instance.RewardPlayer(200);
        Audio.instance.PlaySoundOnce(deathSound);
    }

    protected override void Init()
    {
        shootCooldownRemaining = 1.0f;
        base.Init();
    }

    private void Update()
    {
        shootCooldownRemaining -= Time.deltaTime;
        if(shootCooldownRemaining <= 0.0f)
        {
            Shoot();
            shootCooldownRemaining += 1.0f + Random.Range(0, 2);
        }
    }
}

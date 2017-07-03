using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : FallingObject, IDamagable
{
    public AudioClip collectSound;
    public AudioClip destroySound;

    public EBonusType bonusType
    {
        get;
        protected set;
    }

    public void CreateBonus(EBonusType type)
    {
        bonusType = type;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite sprite = null;

        switch (bonusType)
        {
            case EBonusType.POINTS:
                sprite = Resources.Load<Sprite>("textures/bonusPoints");
                break;

            case EBonusType.LIFE:
                sprite = Resources.Load<Sprite>("textures/bonusHealth");
                break;

            case EBonusType.CONTAINER:
                sprite = Resources.Load<Sprite>("textures/bonusHealthContainer");
                break;
        }

        spriteRenderer.sprite = sprite;

    }

    protected override void OnFall()
    {
        Destroy(gameObject);
    }

    protected override void OnCollide(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Game.instance.RewardPlayer(this);
        }
        Audio.instance.PlaySoundOnce(collectSound);
        Destroy(gameObject);
    }

    public void DealDamage(int amount)
    {
        Destroy(gameObject);
        Audio.instance.PlaySoundOnce(destroySound);
    }
}

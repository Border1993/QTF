using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public GameObject bulletPrefab;
    public float speed;

    public AudioClip deathSound;
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip invertedSound;

    private List<Vector3> positions;
    private int positionIndex;
    private bool moving;

    public Vector2 screenResolution
    {
        set;
        private get;
    }
    private GameObject playerObject;

    public float shootingDelay;
    private float currentShootingDelay;

    static public Player instance
    {
        get;
        private set;
    }

    static Player()
    {
        
        instance = null;
    }

    static public void DeleteInstance()
    {
        if(instance != null) Destroy(instance.gameObject);
        instance = null;
    }


    static public void ApplyDamage(int amount)
    {
        instance.DealDamage(amount);
    }

    private int maxHPValue;
    private int currentHPValue;

    public void PlayInverterHit()
    {
        Audio.instance.PlaySoundOnce(invertedSound);
    }

    public int maxHP
    {
        private set
        {
            maxHPValue = value;
            if (maxHPValue > 5) maxHPValue = 5;
            Game.instance.ChangeHealthDisplayed(currentHPValue, maxHPValue);
        }
        get
        {
            return maxHPValue;
        }
    }

    public int currentHP
    {
        private set
        {
            currentHPValue = value;
            if (currentHPValue > maxHPValue) currentHPValue = maxHPValue;
            Game.instance.ChangeHealthDisplayed(currentHPValue, maxHPValue);
            if (currentHPValue <= 0) Audio.instance.PlaySoundOnce(deathSound);
        }
        get
        {
            return currentHPValue;
        }
    }

    private void Update()
    {
        currentShootingDelay -= Time.deltaTime;
    }


    // Use this for initialization
    void Start ()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        screenResolution = new Vector2(480, 800);

        positions = new List<Vector3>();

        Vector3 centerPosition = new Vector3(screenResolution.x / 2, 100, 1);
        

        for (int i = 2; i >= 1; i--)
        {
            positions.Add(new Vector3(centerPosition.x - i * (screenResolution.x / 5), centerPosition.y, 1));
        }

        positions.Add(centerPosition);

        for (int i = 1; i <= 2; i++)
        {
            positions.Add(new Vector3(centerPosition.x + i * (screenResolution.x / 5), centerPosition.y, 1));
        }

        playerObject = gameObject;
        transform.position = positions[2];
        positionIndex = 2;
        moving = false;

        maxHP = 3;
        currentHP = 3;
        shootingDelay = 0.1f;
        currentShootingDelay = 0;
    }

    public void AddBonus(Bonus bonus)
    {
        switch (bonus.bonusType)
        {
            case EBonusType.CONTAINER:
                maxHP++;
                break;

            case EBonusType.LIFE:
                currentHP++;
                break;

            case EBonusType.POINTS:
                Game.instance.RewardPlayer(1000);
                break;
        }

    }

    public void MoveLeft()
    {
        if (moving || positionIndex == 0) return;
        positionIndex--;
        //StartCoroutine(MoveCoRoutine(positions[positionIndex], speed));
        moving = true;
        MoveInstantly(positions[positionIndex]);
    }

    public void MoveRight()
    {
        if (moving || positionIndex == positions.Count - 1) return;
        positionIndex++;
        //StartCoroutine(MoveCoRoutine(positions[positionIndex], speed));
        moving = true;
        MoveInstantly(positions[positionIndex]);
    }

    public void Shoot(GameObject bulletsRoot)
    {
        if (moving) return;
        if (currentShootingDelay > 0) return;

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.SetParent(bulletsRoot.transform);
        bullet.GetComponent<Bullet>().SetOwner(gameObject);
        bullet.transform.position = transform.position;
        currentShootingDelay = shootingDelay;

        Audio.instance.PlaySoundOnce(shootSound);
    }

    private void MoveInstantly(Vector3 destination)
    {
        transform.position = destination;
        moving = false;
    }

    private IEnumerator MoveCoRoutine(Vector3 destination, float speed)
    {
        moving = true;

        while (transform.position.x != destination.x || transform.position.y != destination.y)
        {
            float timeDelta = Time.deltaTime;
            Vector3 direction = destination - transform.position;
            Vector3 movement = direction.normalized * speed * timeDelta;
            if (movement.magnitude >= direction.magnitude) transform.position = destination;
            else transform.position += movement;

            yield return null;
        }

        moving = false;

        yield break;
    }

    public void DealDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP > maxHP) currentHP = maxHP;
        Game.instance.ChangeHealthDisplayed(currentHP, maxHP);
        Audio.instance.PlaySoundOnce(hitSound);
    }
}

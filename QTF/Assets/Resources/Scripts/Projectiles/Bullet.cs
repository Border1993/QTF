using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;

	void Start ()
    {
        StartCoroutine(Movement());
	}

    public void SetOwner(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.instance.GetComponent<BoxCollider2D>(), true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable otherObject = collision.gameObject.GetComponent<IDamagable>();
        if (otherObject != null)
        {
            otherObject.DealDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator Movement()
    {
        while(transform.position.y >= 50 && transform.position.y < 750)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}

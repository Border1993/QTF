using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverterBullet : MonoBehaviour
{

    public float speed;

    void Start()
    {
        StartCoroutine(Movement());
    }

    public void SetOwner(GameObject obj)
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Player.instance.GetComponent<BoxCollider2D>(), true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), collision.GetComponent<BoxCollider2D>());
            return;
        }

        Game.instance.ReverseControl();
    }

    private IEnumerator Movement()
    {
        while (transform.position.y >= 50 && transform.position.y < 750)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
        yield break;
    }
}

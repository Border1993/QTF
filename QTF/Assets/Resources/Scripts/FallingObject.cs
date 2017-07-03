using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FallingObject : MonoBehaviour
{
    protected float speed;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(StartMoving());
	}

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    protected abstract void OnFall();
    protected abstract void OnCollide(Collision2D collision);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FallingObject")
        {
            Physics2D.IgnoreCollision(
                gameObject.GetComponent<BoxCollider2D>(), 
                collision.gameObject.GetComponent<BoxCollider2D>());
        }
        else OnCollide(collision);
    }

    private IEnumerator StartMoving()
    {
        while (transform.position.y >= 75 && transform.position.y <= 800)
        {
            transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
            yield return null;
        }

        OnFall();
        Destroy(gameObject);
        yield break;
    }
}

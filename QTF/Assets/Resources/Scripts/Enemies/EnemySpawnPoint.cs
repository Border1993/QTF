using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private bool blocked;
    public bool isLocked
    {
        get { return blocked; }
    }

    public void Spawn(GameObject obj)
    {
        obj.transform.position = transform.position;
    }

	void Start ()
    {
        blocked = false;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        blocked = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        blocked = false;
    }
}

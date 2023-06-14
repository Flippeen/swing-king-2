using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstacle : MonoBehaviour
{
    [SerializeField] int positionOffset = 40;

    private Transform player;

    public static event Action OnPlayerPassed;

    private void LateUpdate()
    {
        if(player != null && (transform.position.x + positionOffset) < player.position.x)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            OnPlayerPassed.Invoke();
            player = collision.transform;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSpawner : MonoBehaviour
{
    [SerializeField] GameObject grabber;

    [SerializeField] Vector2 range;
    [SerializeField] float spawnerCD, spawnYOffset;

    float spawnTimer;

    private void Update()
    {
        //TODO change to spawn based on y position of player with an offset instead of being time based to instantly spawn everything a certain distance above the player.
        if (Time.time > spawnTimer + spawnerCD)
        {
            float x = Random.Range(range.x * -0.5f, range.x*0.5f);
            float y = Random.Range(range.y * -0.5f, range.y*0.5f);
            Vector2 spawnPosition = new Vector2(x, y + transform.position.y + spawnYOffset);

            if(!Physics2D.OverlapCircle(spawnPosition, 5))
            {
                spawnTimer = Time.time;
                Instantiate(grabber, spawnPosition, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, range);
    }
}

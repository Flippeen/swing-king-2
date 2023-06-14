using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab objects")]
    [SerializeField] GameObject pipePrefab;
    [SerializeField] GameObject spikeBallPrefab;
    [SerializeField] Transform gapIndicator;
    [SerializeField] Canvas UICanvas;

    [Header("Spawn delays")]
    [SerializeField] float spawnDelayDistance = 50f;
    [SerializeField] float spawnObjectDistance = 50f;

    [Header("Pipe")]
    [SerializeField] GameObject pipeMiddle;
    [SerializeField] float pipeMinHeight = -1f;
    [SerializeField] float pipeMaxHeight = 1f;
    [SerializeField] float pipeGapSize = 1f;

    [Header("Spike ball")]
    [SerializeField] float spikeBallMinHeight = -3f;
    [SerializeField] float spikeBallMaxHeight = 3f;
    [SerializeField] float pingPongSpeed = 1f;
    [SerializeField] float rotationSpeed = 0.5f;

    float xSpawnTimer;
    Transform player;
    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, 0);
    }

    private void Update()
    {
        if(transform.position.x > xSpawnTimer)
        {
            xSpawnTimer += spawnDelayDistance;
            SpawnPipe();
        }
    }

    private void SpawnObstacle()
    {
        int rng = Random.Range(0, 2);
        if (rng >= 1)
        {
            SpawnPipe();
        }
        else
        {
            SpawnSpikeBall();
        }
    }

    private void SpawnPipe()
    {
        // Calculate the height of the pipes.
        float gapPosition = Random.Range(pipeMinHeight + pipeGapSize, pipeMaxHeight - pipeGapSize);
        pipeGapSize = pipeGapSize - Random.Range(0, 1.5f);
        float topPipeHeight = gapPosition + (pipeGapSize*0.5f);
        float bottomPipeHeight = gapPosition - (pipeGapSize * 0.5f);

        Transform gapObj = Instantiate(pipeMiddle, new Vector3(transform.position.x + spawnObjectDistance, gapPosition, 0f), Quaternion.identity).transform;

        // Spawn the top pipe.
        GameObject topPipe = Instantiate(pipePrefab, new Vector3(transform.position.x + spawnObjectDistance, topPipeHeight, 0f), Quaternion.identity, gapObj);

        // Set the rotation of the top pipe to 180 degrees.
        topPipe.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));

        // Spawn the bottom pipe.
        GameObject bottomPipe = Instantiate(pipePrefab, new Vector3(transform.position.x + spawnObjectDistance, bottomPipeHeight, 0f), Quaternion.identity, gapObj);

        // Move the gap indicator to the gap's position.
        //gapIndicator.rectTransform.anchoredPosition = new Vector2(gapIndicator.rectTransform.anchoredPosition.x, gapPosition);
        GameObject newIndicator = Instantiate(gapIndicator.gameObject, new Vector3(0, gapPosition, 0f), Quaternion.identity, UICanvas.transform);
        newIndicator.GetComponent<DangerIndicator>().SetUp(gapObj, player, new Vector2(topPipeHeight, bottomPipeHeight), true);
        newIndicator.GetComponent<Image>().rectTransform.anchorMax = new Vector2(1f, 0.5f);
        newIndicator.GetComponent<Image>().rectTransform.anchorMin = new Vector2(1f, 0.5f);
        newIndicator.GetComponent<Image>().rectTransform.position = new Vector3(Camera.main.pixelWidth - newIndicator.GetComponent<Image>().rectTransform.localScale.x, Camera.main.WorldToScreenPoint(new Vector3(0, gapPosition, 0f)).y, 0);
    }

    private void SpawnSpikeBall()
    {
        // Calculate the height of the spike ball.
        float spikeBallHeight = Random.Range(spikeBallMinHeight, spikeBallMaxHeight);

        Transform gapObj = Instantiate(pipeMiddle, new Vector3(transform.position.x + spawnObjectDistance, spikeBallHeight, 0f), Quaternion.identity).transform;

        // Spawn the spike ball.
        GameObject spikeBall = Instantiate(spikeBallPrefab, new Vector3(transform.position.x + spawnObjectDistance, spikeBallHeight, 0f), Quaternion.identity, gapObj.transform);

        // Set the rotation of the spike ball to a random angle.
        spikeBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0f, 360f)));

        GameObject newIndicator = Instantiate(gapIndicator.gameObject, new Vector3(0, spikeBallHeight, 0f), Quaternion.identity, UICanvas.transform);
        newIndicator.GetComponent<DangerIndicator>().SetUp(spikeBall.transform, player, 2, false);
        newIndicator.GetComponent<Image>().rectTransform.anchorMax = new Vector2(1f, 0.5f);
        newIndicator.GetComponent<Image>().rectTransform.anchorMin = new Vector2(1f, 0.5f);
        newIndicator.GetComponent<Image>().rectTransform.position = new Vector3(Camera.main.pixelWidth, Camera.main.WorldToScreenPoint(new Vector3(0, spikeBallHeight, 0f)).y, 0);


        // Move the spike ball in a ping pong motion between two points.
        Vector3 startPos = new Vector3(spikeBall.transform.position.x, spikeBallMinHeight, 0f);
        Vector3 endPos = new Vector3(spikeBall.transform.position.x, spikeBallMaxHeight, 0f);
        StartCoroutine(PingPong(spikeBall.transform, startPos, endPos));
    }

    private IEnumerator PingPong(Transform spikeBall, Vector3 startPos, Vector3 endPos)
    {
        bool moveUp = Vector2.Distance(spikeBall.position, startPos) > Vector2.Distance(spikeBall.position, endPos);

        while (true)
        {
            spikeBall.rotation = Quaternion.Euler(new Vector3(0f, 0f, spikeBall.eulerAngles.z + rotationSpeed));
            if (moveUp)
            {
                spikeBall.position = Vector2.MoveTowards(spikeBall.position, startPos, pingPongSpeed * 0.1f);
                if (Vector2.Distance(spikeBall.position, startPos) < 0.01f)
                {
                    moveUp = false;
                }
            }
            else
            {
                spikeBall.position = Vector2.MoveTowards(spikeBall.position, endPos, pingPongSpeed * 0.1f);
                if(Vector2.Distance(spikeBall.position, endPos) < 0.01f)
                {
                    moveUp = true;
                }
            }
            yield return null;
        }
    }
}
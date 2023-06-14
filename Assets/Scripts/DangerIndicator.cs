using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerIndicator : MonoBehaviour
{
    [SerializeField] int indicatorDespawnOffset = 5;

    //Set-up values
    private Transform target;
    private Transform player;
    private float size;
    private bool isSafe;
    private Image indicator;

    public void SetUp(Transform _target, Transform _player, Vector2 _topAndBottomPos, bool _isSafe)
    {
        target = _target;
        player = _player;
        size = _topAndBottomPos.x - _topAndBottomPos.y;
        isSafe = _isSafe;
    }

    public void SetUp(Transform _target, Transform _player, int _size, bool _isSafe)
    {
        target = _target;
        player = _player;
        size = _size;
        isSafe = _isSafe;
    }

    private void Start()
    {
        indicator = GetComponent<Image>();
        indicator.rectTransform.sizeDelta = new Vector2(indicator.rectTransform.rect.width, Camera.main.WorldToScreenPoint(new Vector3(0, size, 0)).y);

        Obstacle.OnPlayerPassed += PlayerPassed;
    }
    private void Update()
    {
        indicator.rectTransform.position = new Vector3(Camera.main.pixelWidth, Camera.main.WorldToScreenPoint(new Vector3(0, target.position.y, 0f)).y, 0);
        indicator.rectTransform.sizeDelta = new Vector2(target.position.x - (player.position.x + indicatorDespawnOffset), indicator.rectTransform.sizeDelta.y);

        if (isSafe)
        {
            indicator.color = Color.green;
        }
        else
        {
            //indicator.color = gradient.Evaluate(player.position.x/ target.position.x);
            indicator.color = Color.red;
        }
    }

    private void PlayerPassed()
    {
        Obstacle.OnPlayerPassed -= PlayerPassed;
        Destroy(gameObject);
    }
}

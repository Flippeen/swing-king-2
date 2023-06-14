using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject anchorPoint;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] float swingSpeedMultiplier;
    SpringJoint2D ropeJoint;
    Rigidbody2D rb;
    LineRenderer lr;

    bool isSwinging = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ropeJoint = GetComponent<SpringJoint2D>();
        lr = GetComponent<LineRenderer>();
        ropeJoint.enabled = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Swing();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopSwing();
        }
        speedText.text = Mathf.RoundToInt(rb.velocity.x).ToString();
    }

    private void FixedUpdate()
    {
        if (isSwinging)
        {
            Debug.DrawRay(transform.position, rb.velocity * 2, Color.red);
            rb.velocity *= swingSpeedMultiplier;
        }
    }

    private void LateUpdate()
    {
        if (isSwinging)
        {
            lr.SetPosition(0, transform.position);
        }
    }

    private void Swing()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(1,1));

        if(hit != null)
        {
            isSwinging = true;
            lr.SetPosition(1, hit.point);
            lr.enabled = true;
            anchorPoint.transform.position = hit.point;
            ropeJoint.enabled = true;
        }
    }

    private void StopSwing()
    {
        isSwinging = false;
        lr.enabled = false;
        ropeJoint.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrab : MonoBehaviour
{
    [SerializeField] GameObject anchorPoint, hand;
    [SerializeField] TMPro.TextMeshProUGUI speedText;
    [SerializeField] float swingSpeedMultiplier, maxVelocity;
    [SerializeField] SpringJoint2D ropeJoint;
    [SerializeField] Rigidbody2D rb;
    LineRenderer lr;

    bool isSwinging = false;
    Vector3 grabPosition;
    Transform closestGrab;
    Rigidbody2D childBody;
    Vector2 savedVelocity;
    float swingStopTime;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        ropeJoint = GetComponentInChildren<SpringJoint2D>();
        lr = GetComponent<LineRenderer>();
        childBody = GetComponentsInChildren<Rigidbody2D>()[1];
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
        //speedText.text = Mathf.RoundToInt(rb.velocity.x).ToString();
        speedText.text = Mathf.RoundToInt(transform.position.y).ToString();
    }

    private void FixedUpdate()
    {
        if (isSwinging)
        {
            Debug.DrawRay(transform.position, rb.velocity * 2, Color.red);
            if(ropeJoint.distance < 2)
            {
                ropeJoint.distance += 0.05f;
            }
            if(rb.velocity.magnitude < maxVelocity)
            {
                rb.velocity *= swingSpeedMultiplier;
            }
        }
        else
        {

            if (rb.velocity.magnitude < (maxVelocity * 2) && Time.time < swingStopTime + 0.75f)
            {
                float direction = 1 + Mathf.Abs(childBody.angularVelocity) * 0.0001f;
                Debug.Log(direction);
                if((direction + rb.velocity.magnitude)< (maxVelocity * 2))
                {
                    rb.velocity *= direction;
                }
            }
            foreach (var grab in grabables)
            {
                if(closestGrab == null)
                {
                    closestGrab = grab;
                    grab.GetComponent<SpriteRenderer>().color = Color.green;
                    continue;
                }

                if(Vector2.Distance(grab.position, transform.position) < Vector2.Distance(closestGrab.position, transform.position))
                {
                    closestGrab.GetComponent<SpriteRenderer>().color = Color.blue;
                    grab.GetComponent<SpriteRenderer>().color = Color.green;
                    closestGrab = grab;
                }
            }
        }
        savedVelocity = rb.velocity;
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
        if(grabables.Count == 0)
        {
            return;
        }
        isSwinging = true;
        grabPosition = closestGrab.transform.position;
        lr.SetPosition(1, grabPosition);
        lr.enabled = true;
        anchorPoint.transform.position = grabPosition;
        ropeJoint.enabled = true;
    }

    private void StopSwing()
    {
        swingStopTime = Time.time;
        isSwinging = false;
        lr.enabled = false;
        ropeJoint.enabled = false;
    }

    List<Transform> grabables = new List<Transform>();
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Grab"))
        {
            grabables.Add(c.transform);
            c.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        grabables.Remove(c.transform);
        c.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.DrawRay(c.contacts[0].point, savedVelocity, Color.blue, 10);
        Debug.DrawRay(c.contacts[0].point, c.contacts[0].normal, Color.red, 10);
        Debug.DrawRay(c.contacts[0].point, Vector2.Reflect(savedVelocity, c.contacts[0].normal), Color.green, 10);
        rb.velocity = Vector2.Reflect(savedVelocity, c.contacts[0].normal);
    }
}

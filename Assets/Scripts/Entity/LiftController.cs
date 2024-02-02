using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private bool isUp;
    [SerializeField] private float speed;

    bool isMoving;
    Vector3 targetPosition;

    public void ToggleLift()
    {
        if (isMoving)
            return;

        if (isUp)
        {
            targetPosition = transform.localPosition - new Vector3(0, moveDistance, 0);

            isUp = false;
            isMoving = true;
        }
        else
        {
            targetPosition = transform.localPosition + new Vector3(0, moveDistance, 0);

            isUp = true;
            isMoving = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }
}

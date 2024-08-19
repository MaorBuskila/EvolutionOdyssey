using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.3f;
    public LayerMask obstacleLayer;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.up));
        }
        if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.left));
        }
        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.down));
        }
        if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        // Check if there is an obstacle in the direction
        RaycastHit2D hit = Physics2D.Raycast(origPos, direction, 1f, obstacleLayer);

        if (hit.collider != null)
        {
            // Check if the obstacle can be moved
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable != null && interactable.isPushable)
            {
                Vector3 obstacleTargetPos = hit.collider.transform.position + direction;
                RaycastHit2D obstacleHit = Physics2D.Raycast(hit.collider.transform.position, direction, 1f, obstacleLayer);
                if (obstacleHit.collider == null)
                {
                    // Move the obstacle
                    StartCoroutine(MoveObstacle(hit.collider.gameObject, obstacleTargetPos));
                }
                else
                {
                    // Can't move the obstacle, block the player movement
                    isMoving = false;
                    yield break;
                }
            }
            else
            {
                // Can't move, block the player movement
                isMoving = false;
                yield break;
            }
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private IEnumerator MoveObstacle(GameObject obstacle, Vector3 targetPos)
    {
        float elapsedTime = 0;
        Vector3 origPos = obstacle.transform.position;
        while (elapsedTime < timeToMove)
        {
            obstacle.transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obstacle.transform.position = targetPos;
    }
}

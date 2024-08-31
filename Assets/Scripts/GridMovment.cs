using System.Collections;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.05f;
    public LayerMask obstacleLayer;

    private Animator animator;
    private LevelManager levelManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>(); // Get reference to Level1Manager
    }

    void Update()
    {
        if (levelManager.IsGameOver()) return; // Stop movement if the game is over

        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.up));
        }
        else if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            animator.SetBool("isMovingLeft", true);
            animator.SetBool("isMovingRight", false);
            StartCoroutine(MovePlayer(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.down));
        }
        else if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
            StartCoroutine(MovePlayer(Vector3.right));
        }
        else
        {
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        // Cast a narrow box along the direction of movement, starting at the player's position
        Vector2 castDirection = new Vector2(direction.x, direction.y);
        Vector2 castOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f); // Adjust for the player's feet position
        RaycastHit2D hit = Physics2D.BoxCast(castOrigin, new Vector2(0.8f, 0.1f), 0, castDirection, 1f, obstacleLayer);

        // Debugging: Visualize the BoxCast
        Debug.DrawRay(castOrigin, castDirection * 1f, Color.red, 0.5f);

        if (hit.collider != null)
        {
            // Can't move, block the player movement
            isMoving = false;
            yield break;
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
}
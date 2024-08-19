using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;

    void Start()
    {
        movePoint.parent = null;
        Debug.Log("PlayerMovement initialized");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                Vector3 newPosition = movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                if (!Physics2D.OverlapCircle(newPosition, .2f, whatStopsMovement))
                {
                    movePoint.position = newPosition;
                    Debug.Log("Moved to: " + newPosition);
                }
                else
                {
                    Debug.Log("Movement blocked at: " + newPosition);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                Vector3 newPosition = movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                if (!Physics2D.OverlapCircle(newPosition, .2f, whatStopsMovement))
                {
                    movePoint.position = newPosition;
                    Debug.Log("Moved to: " + newPosition);
                }
                else
                {
                    Debug.Log("Movement blocked at: " + newPosition);
                }
            }
        }
    }
}

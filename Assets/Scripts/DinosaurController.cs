using UnityEngine;
using UnityEngine.SceneManagement;


public class DinosaurController : MonoBehaviour
{
    public float speed = 2f;
    public float moveDistance = 2f;
    private bool movingRight = true;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= startPos.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= startPos.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player trigger collision detected.");

            PlayerInventory playerInventory = collision.gameObject.GetComponent<PlayerInventory>();

            //if current level is L4 dystry dinosaur
            int currentLevel = int.Parse(SceneManager.GetActiveScene().name.Replace("L", ""));
            if (currentLevel == 4)
            {
                Destroy(gameObject);  // Destroy the Dinosaur
            }
            else
            {
                Debug.Log("Player touched the Dinosaur and died.");
                LevelManager levelManager = FindObjectOfType<LevelManager>();
                levelManager.GameOver();  // Trigger Game Over
            }
        }
    }
}

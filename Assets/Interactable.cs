using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isPushable = true;
    private bool isPickedUp = false;
    public string elementType;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with " + other.name);
        if (other.CompareTag("Player") && !isPickedUp)
        {
            Debug.Log("Player collided with " + elementType);
            isPickedUp = true;
            gameObject.SetActive(false);
            other.GetComponent<PlayerInventory>().AddToInventory(elementType);
        }
        else
        {
            Debug.Log("Collision detected but not with Player or already picked up");
        }
    }
}

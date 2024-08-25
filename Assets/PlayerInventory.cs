using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<string> inventory = new List<string>();

    public void AddToInventory(string item)
    {
        inventory.Add(item);
        Debug.Log(item + " added to inventory");
        CheckForCombination();
    }

    private void CheckForCombination()
    {
        if (inventory.Contains("Rock") && inventory.Contains("Wood"))
        {
            Debug.Log("Fire Created!");
            // Move to the next level or trigger the next action
            // Example: Call a method from another script that handles level progression
        }
        else
        {
            Debug.Log("Combination not yet complete");
        }
    }

    public bool HasItem(string item)
    {
        return inventory.Contains(item);
    }

    public List<string> GetInventoryItems()
    {
        return new List<string>(inventory);
    }
}

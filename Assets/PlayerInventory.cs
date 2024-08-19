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
            // Remove items from inventory
            inventory.Remove("Rock");
            inventory.Remove("Wood");
            // Move to next level or trigger next action
        }
        else
        {
            Debug.Log("Combination not yet complete");
        }
    }
}

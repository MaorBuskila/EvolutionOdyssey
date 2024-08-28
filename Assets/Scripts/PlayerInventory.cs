using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;  // Add this line


public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    
    public delegate void InventoryChangeHandler(string item, int count);
    public event InventoryChangeHandler OnInventoryChanged;

    [System.Serializable]
    public class CraftingRecipe
    {
        public string result;
        public Dictionary<string, int> ingredients;
    }

    public List<CraftingRecipe> craftingRecipes;

    public void AddToInventory(string item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory[item] = 1;
        }
        
        Debug.Log($"{item} added to inventory. Total: {inventory[item]}");
        OnInventoryChanged?.Invoke(item, inventory[item]);
        
        CheckForCrafting();
    }

    public void RemoveFromInventory(string item, int count = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= count;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
            OnInventoryChanged?.Invoke(item, inventory.ContainsKey(item) ? inventory[item] : 0);
        }
    }

    private void CheckForCrafting()
    {
        foreach (var recipe in craftingRecipes)
        {
            if (CanCraft(recipe))
            {
                CraftItem(recipe);
                break;  // Only craft one item at a time
            }
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        return recipe.ingredients.All(ingredient => 
            inventory.ContainsKey(ingredient.Key) && inventory[ingredient.Key] >= ingredient.Value);
    }

    private void CraftItem(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            RemoveFromInventory(ingredient.Key, ingredient.Value);
        }

        AddToInventory(recipe.result);
        Debug.Log($"{recipe.result} crafted!");
    }

    public bool HasItem(string item, int count = 1)
    {
        return inventory.ContainsKey(item) && inventory[item] >= count;
    }

    public Dictionary<string, int> GetInventoryItems()
    {
        return new Dictionary<string, int>(inventory);
    }

    public int GetItemCount(string item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

        public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Player Inventory:");
        
        if (inventory.Count == 0)
        {
            sb.AppendLine("  Empty");
        }
        else
        {
            foreach (var item in inventory)
            {
                sb.AppendLine($"  {item.Key}: {item.Value}");
            }
        }

        return sb.ToString();
    }
}


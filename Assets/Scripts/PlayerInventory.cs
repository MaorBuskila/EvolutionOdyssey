using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

[System.Serializable]
public class CraftingRecipe
{
    public string result;  // Name of the crafted item, e.g., "Arch"
    public Dictionary<string, int> ingredients;  // Required ingredients
}

public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public delegate void InventoryChangeHandler(string item, int count);
    public event InventoryChangeHandler OnInventoryChanged;

    public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>
    {
        new CraftingRecipe
        {
            result = "Arch",
            ingredients = new Dictionary<string, int>
            {
                { "Wood", 1 },
                { "Rock", 1 },
                { "Vine", 1 }
            }
        }
    };

    public Sprite archerSprite; // The new player sprite after crafting

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

    private void CheckForCrafting()
    {
        Debug.Log("Checking if crafting can be triggered...");

        foreach (var recipe in craftingRecipes)
        {
            Debug.Log($"Checking recipe: {recipe.result}");

            if (CanCraft(recipe))
            {
                Debug.Log($"Can craft {recipe.result}. Starting crafting process.");
                CraftItem(recipe);
                break;  // Only craft one item at a time
            }
            else
            {
                Debug.Log($"Cannot craft {recipe.result} yet. Missing items.");
            }
        }
    }

    private bool CanCraft(CraftingRecipe recipe)
    {
        bool canCraft = recipe.ingredients.All(ingredient =>
            inventory.ContainsKey(ingredient.Key) && inventory[ingredient.Key] >= ingredient.Value);

        foreach (var ingredient in recipe.ingredients)
        {
            Debug.Log($"Required: {ingredient.Key} x{ingredient.Value}, " +
                      $"In Inventory: {(inventory.ContainsKey(ingredient.Key) ? inventory[ingredient.Key] : 0)}");
        }

        Debug.Log($"Checking if can craft {recipe.result}: {canCraft}");
        return canCraft;
    }

    private void CraftItem(CraftingRecipe recipe)
    {
        Debug.Log($"Crafting {recipe.result}...");

        foreach (var ingredient in recipe.ingredients)
        {
            Debug.Log($"Removing {ingredient.Value}x {ingredient.Key} from inventory.");
            RemoveFromInventory(ingredient.Key, ingredient.Value);
        }

        AddToInventory(recipe.result);
        Debug.Log($"{recipe.result} crafted!");

        // Change the player sprite after crafting the Arch
        GetComponent<SpriteRenderer>().sprite = archerSprite;
        Debug.Log("Player sprite changed to Archer.");
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
            Debug.Log($"Removed {item} from inventory. Remaining: {GetItemCount(item)}");
        }
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

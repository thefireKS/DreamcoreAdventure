using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    
    private readonly List<Item> _items = new();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void AddItem(Item itemToAdd)
    {
        _items.Add(itemToAdd);
    }

    public void RemoveItem(Item itemToRemove)
    {
        _items.Remove(itemToRemove);
    }

    public bool CheckItem(Item itemToSearchInInventory)
    {
        return _items.Contains(itemToSearchInInventory);
    }
}

using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
    private Dictionary<string, int> _items = new Dictionary<string, int>();
    public int _max = 20;

    [Signal]
    public delegate void InventoryChangedEventHandler();

    public bool AddItem(string itemName, int quantity = 1)
    {
        if (GetTotalItems() + quantity > _max)
        {
            GD.Print("Not enough space in the bag.");
            return false;
        }

        if (_items.ContainsKey(itemName))
        {
            _items[itemName] += quantity;
        }
        else
        {
            _items[itemName] = quantity;
        }

        GD.Print($"Added {quantity}x {itemName} to the inventory.");
        EmitSignal(nameof(InventoryChanged));
        return true;
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        if (!_items.ContainsKey(itemName) || _items[itemName] < quantity)
        {
            GD.Print($"Not enough {itemName} to remove.");
            return false;
        }

        _items[itemName] -= quantity;

        if (_items[itemName] <= 0)
        {
            _items.Remove(itemName);
        }

        GD.Print($"Removed {quantity}x {itemName} from the inventory.");
        EmitSignal(nameof(InventoryChanged));
        return true;
    }

    public int GetItemQuantity(string itemName)
    {
        return _items.ContainsKey(itemName) ? _items[itemName] : 0;
    }

    public int GetTotalItems()
    {
        int total = 0;
        foreach (var quantity in _items.Values)
        {
            total += quantity;
        }
        return total;
    }

    public void PrintInventory()
    {
        GD.Print("Inventory:");
        foreach (var item in _items)
        {
            GD.Print($"{item.Key}: {item.Value}");
        }
    }

    public Dictionary<string, int> GetItems()
    {
        return new Dictionary<string, int>(_items);
    }
}

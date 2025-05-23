using Godot;
using System;
using System.Collections.Generic;

namespace IdleGame;

public partial class GameState : Node
{
    public static GameState Instance { get; private set; }
    
    private Dictionary<string, int> _inventory = new();
    private Dictionary<string, int> _employees = new();
    private float _money = 0;
    
    public event Action<string, int> InventoryChanged;
    public event Action MoneyChanged;
    public event Action<string, int> EmployeesChanged;
    
    public override void _Ready()
    {
        Instance = this;
        AddMoney(100); // Start with 100 money
    }
    
    public void AddResource(string resourceId, int quantity = 1)
    {
        if (!_inventory.ContainsKey(resourceId))
        {
            _inventory[resourceId] = 0;
        }
        
        _inventory[resourceId] += quantity;
        InventoryChanged?.Invoke(resourceId, _inventory[resourceId]);
    }
    
    public void AddMoney(float amount)
    {
        _money += amount;
        MoneyChanged?.Invoke();
    }
    
    public float GetMoney()
    {
        return _money;
    }
    
    public int GetResouceQuantity(string resourceId)
    {
        return _inventory.ContainsKey(resourceId) ? _inventory[resourceId] : 0;
    }

    public void AddEmployee(string resourceId)
    {
        if (!_employees.ContainsKey(resourceId))
        {
            _employees[resourceId] = 0;
        }
        
        _employees[resourceId]++;
        EmployeesChanged?.Invoke(resourceId, _employees[resourceId]);
    }

    public int GetEmployeeCount(string resourceId)
    {
        return _employees.ContainsKey(resourceId) ? _employees[resourceId] : 0;
    }
} 
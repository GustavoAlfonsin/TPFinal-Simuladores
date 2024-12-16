using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cocina : MonoBehaviour
{
    public List<dinner> _dishes;
    public List<Tray> barDishes;
    public Transform _position;

    public bool isCooking;

    private float time;
    private int lastOrder;

    public Action actualizarLista;
    public Action<List<dinner>, int> whenRegisteringOrders;
    public Func<dinner, bool> someoneWantsIt;
    void Start()
    {
        _dishes = new List<dinner>();
        barDishes = new List<Tray>();
        foreach (Transform child in transform)
        {
            Tray dish = child.GetComponent<Tray>();
            if (dish != null)
            {
                barDishes.Add(dish);
            }
        }
        foreach (Tray dish in barDishes) //desactivo los platos en la barra
        {
            dish._cubo.SetActive(false);
        }
        foodRandomizer.loadMenu();
        isCooking = false;
        _position = this.transform;
        lastOrder = 1;
        
    }

    void Update()
    {
        if (isCooking)
        {
            time += Time.deltaTime;
            foreach (dinner food in _dishes)
            {
                food.Cooking();
            }
        }

        putOnTheBar();
    }

    private void putOnTheBar()
    {
        foreach(Tray barTray in barDishes)
        {
            if (barTray.IsEmpty)
            {
                dinner readyFood = searchForDish();
                if (readyFood != null)
                {
                    barTray.assignOrder(readyFood);
                }
            }
        }
    }

    private dinner searchForDish()
    {
        foreach (dinner food in _dishes)
        {
            if (!isInBar(food) && food.State == Estados.foodInKitchen.Ready)
            {
                return food;
            }
        }
        return null;
    }

    private bool isInBar(dinner food)
    {
        foreach (Tray tray in barDishes)
        {
            if (!tray.IsEmpty && tray.Order.ID == food.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void getOrders(List<meal> orders, int tableId)
    {
        List<dinner> ordersList = new List<dinner>();
        foreach (meal ord in orders)
        {
            dinner food = new dinner(lastOrder, ord._name, ord._cookingTime,
                                 ord._cost, CicloDeDia.getCurrentTime());
            food.TableID = tableId;
            lastOrder++;
            _dishes.Add(food);
            ordersList.Add(food);
        }
        whenRegisteringOrders?.Invoke(ordersList, tableId);
        isCooking = _dishes.Count > 0;
    }

    public void foodDelivered(dinner food)
    {
        foreach (dinner d in _dishes)
        {
            if (d.ID == food.ID)
            {
                d.delivered();
                return;
            }
        }
    }

    public bool throwFood(dinner food)
    {
        if (someoneWantsIt != null && !someoneWantsIt.Invoke(food))
        {
            foreach (dinner d in _dishes)
            {
                dinner dish = _dishes.FirstOrDefault(x => x.ID == food.ID);
                if (dish != null)
                {
                    dish.throwFood();
                    return true;
                }
            }
        }
        return false;  
    }
   
}

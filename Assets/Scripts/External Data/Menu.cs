using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Menu 
{
    public List<meal> dishes;

    public Menu()
    {
        dishes = new List<meal>();
    }
}

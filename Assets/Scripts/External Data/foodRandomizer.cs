using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public static class foodRandomizer
{
    private static readonly string stringFilePath = Application.persistentDataPath + "/comidas.json";
    private static Menu _menuList;

    public static void loadMenu()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(stringFilePath))
        {
            string data = File.ReadAllText(stringFilePath);
            _menuList = JsonUtility.FromJson<Menu>(data);
            meal food = getRamdonMeal();
        }
        else
        {
            meal food = new meal("Milanesa", 15, 35);
            _menuList = new Menu();
            _menuList.dishes = new List<meal> { food };
            saveMenu();
        }
    }

    private static void saveMenu()
    {
        string JsonFile = JsonUtility.ToJson(_menuList);
        File.WriteAllText(stringFilePath, JsonFile);
    }

    public static meal getRamdonMeal()
    {
        if (_menuList.dishes.Count == 0) return null;
        return _menuList.dishes[Random.Range(0, _menuList.dishes.Count)];
    }
}

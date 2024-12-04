using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class nameRandomizer
{
    private static readonly string stringFilePath = Application.persistentDataPath + "/nombres.json";
    private static NameList _nameList;

    public static void loadNames()
    {
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(stringFilePath))
        {
            string data = File.ReadAllText(stringFilePath);
            _nameList = JsonUtility.FromJson<NameList>(data);
            Debug.Log("Fueron cargados los nombres desde el archivo Json");
        }
        else
        {
            Debug.LogWarning("Archivo Json no encontrado. Creando lista por defecto");
            _nameList = new NameList()
            {
                _names = new List<string>()
                {
                    "Sof�a", "Mateo", "Valentina", "Lucas", "Martina", "Santiago", "Camila", "Nicol�s", "Luc�a",
                    "Joaqu�n", "Emma","Tom�s", "Isabella", "Benjam�n", "Victoria", "Leonardo", "Julieta", "Mart�n",
                    "M�a", "Gabriel", "Mar�a", "Daniel", "Ana", "David", "Daniela", "Sebasti�n", "Elena", "Alejandro",
                    "Sara", "Pablo", "Carolina", "Diego", "Bianca", "Andr�s", "Gabriela", "Javier", "Clara", "Adri�n",
                    "Julia", "Agust�n","Natalia", "Manuel", "Laura", "Hugo", "Paula", "�lvaro", "Cecilia", "Rodrigo",
                    "Viviana", "Ignacio"
                },
            };
            saveNames();
        }
    }

    private static void saveNames()
    {
        string JsonFile = JsonUtility.ToJson(_nameList);
        File.WriteAllText(stringFilePath, JsonFile);
        Debug.Log("Nombre guardados en archivo Json");
    }

    public static string getRamdonName()
    {
        if (_nameList._names.Count == 0) return "Sin nombre";
        return _nameList._names[Random.Range(0, _nameList._names.Count)];
    }
}

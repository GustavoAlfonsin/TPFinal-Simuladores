using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMesas : MonoBehaviour
{
    [field: SerializeField]
    public List<Mesa> mesas { get; set; }
    void Start()
    {
        asignarNumeros();
    }

    void Update()
    {
        
    }

    private void asignarNumeros()
    {
        int i = 1;
        foreach (Mesa table in mesas)
        {
            table.asignarNumero(i);
            i++;
        }
    }


}

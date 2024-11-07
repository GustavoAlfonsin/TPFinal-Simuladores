using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatosMesa
{
    public int numero_mesa { get; set; }
    public List<comida> platos { get; set; } = new List<comida>();

    public bool listo { get; set; }

    public float startTime { get; set; }

    public void cocinarPlatos(float time)
    {
        float tiempoTranscurrido = time - startTime;
        foreach (comida plato in platos)
        {
            plato.cocinar(tiempoTranscurrido);
        }
        listo = revisarPlatos(tiempoTranscurrido, time);
    }

    private bool revisarPlatos(float time1, float time2)
    {
        foreach (comida plato in platos)
        {
            if (plato.estado != foodState.listoParaServir) 
            {
                return false;
            }
        }
        Debug.Log($"Empezo a las {startTime}, termino a los {time2} y pasaron {time1}");
        return true;
    }

    public override string ToString()
    {
        return $"Plato de la mesa {numero_mesa}";
    }
}

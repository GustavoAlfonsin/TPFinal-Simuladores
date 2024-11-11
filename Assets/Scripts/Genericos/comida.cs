using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comida
{
    public string nombre { get; set; }
    public float tiempoDeCoccion { get; set; }
    public float costo { get; set; }
    public Estados.food estado { get; set; }

    public void cocinar(float time)
    {
        if (time >= tiempoDeCoccion)
        {
            estado = Estados.food.Ready;
        }
    }

    public void terminarComida()
    {
        estado = Estados.food.Empty;
    }
}


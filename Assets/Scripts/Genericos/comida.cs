using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comida
{
    public string nombre { get; set; }
    public float tiempoDeCoccion { get; set; }
    public float costo { get; set; }
    public foodState estado { get; set; }

    public void cocinar(float time)
    {
        if (time >= tiempoDeCoccion)
        {
            estado = foodState.listoParaServir;
        }
    }

    public void terminarComida()
    {
        estado = foodState.platoVacio;
    }
}

public enum foodState
{
    cocinandose,
    listoParaServir,
    platoVacio
}

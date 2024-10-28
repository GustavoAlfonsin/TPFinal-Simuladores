using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina : MonoBehaviour
{
    public List<PlatosMesa> platosEnPreparacion;
    public List<PlatosMesa> platosListos;

    public bool cocinando;

    private float time;
    void Start()
    {
        platosEnPreparacion = new List<PlatosMesa>();
        platosListos = new List<PlatosMesa>();
        cocinando = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cocinando)
        {
            time += Time.deltaTime;
            foreach (PlatosMesa plato in platosEnPreparacion)
            {
                plato.cocinarPlatos(time);
                if (plato.listo)
                {
                    pasarAPlatosListos(plato);
                }
            }
        }
    }

    private void pasarAPlatosListos(PlatosMesa plato)
    {
        platosEnPreparacion.Remove(plato);
        platosListos.Add(plato);
        if (platosEnPreparacion.Count == 0)
        {
            cocinando = false;
        }
    }

    public void nuevoPlato(PlatosMesa plato)
    {
        platosEnPreparacion.Add(plato);
        cocinando = platosEnPreparacion.Count > 0;
    }
}

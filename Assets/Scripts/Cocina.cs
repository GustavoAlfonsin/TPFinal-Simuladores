using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocina : MonoBehaviour
{
    public List<PlatosMesa> platosEnPreparacion;
    public static List<PlatosMesa> platosListos;
    public Transform _position;

    public bool cocinando;

    private float time;

    public static Action actualizarLista;
    void Start()
    {
        platosEnPreparacion = new List<PlatosMesa>();
        platosListos = new List<PlatosMesa>();
        cocinando = false;
        _position = this.transform;
        //List<comida> platos = new List<comida>();
        //for (int i = 0; i < 4; i++)
        //{
        //    comida nuevaComida = new comida()
        //    {
        //        nombre = "Pastas",
        //        tiempoDeCoccion = 15f,
        //        estado = foodState.cocinandose
        //    };
        //    platos.Add(nuevaComida);
        //}
        //PlatosMesa Plato = new PlatosMesa()
        //{
        //    numero_mesa = 12,
        //    platos = platos,
        //    listo = false,
        //    startTime = Time.time
        //};
        //nuevoPlato(Plato);
    }

    void Update()
    {
        if (cocinando)
        {
            Debug.Log("Estamos cocinando");
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
        actualizarLista?.Invoke();
    }

    public void nuevoPlato(PlatosMesa plato)
    {
        Debug.Log("Se agrego un nuevo plato a la cocina");
        platosEnPreparacion.Add(plato);
        cocinando = platosEnPreparacion.Count > 0;
    }
}

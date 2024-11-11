using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControladorMesas : MonoBehaviour
{
    [field: SerializeField]
    public List<Mesa> mesas { get; set; }
    private float timer = 0f;
    public Cocina cocina;
    public TMP_Dropdown listaPlatosListos;
    private List<PlatosMesa> platosListos;
    void Start()
    {
        asignarNumeros();
        platosListos = new List<PlatosMesa>();
        Cocina.actualizarLista += mostrarPlatosListos;
    }

    void Update()
    {
        timer += Time.deltaTime;
        aumentarTiempo(timer);
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

    private void aumentarTiempo(float time)
    {
        foreach (Mesa m in mesas)
        {
            if (m.ocupada)
            {
                m.pasarTiempo(time);
            }
        }
    }

    private void mostrarPlatosListos()
    {
        listaPlatosListos.ClearOptions();
        List<string> mesasParaServir = new List<string>();
        foreach (PlatosMesa p in Cocina.platosListos)
        {
            mesasParaServir.Add($"Platos mesa {p.numero_mesa}: Listo");
            platosListos.Add(p);
            var mesaDelPlato = mesas.FirstOrDefault(x => x.numeroMesa == p.numero_mesa);
            if (mesaDelPlato != null && mesaDelPlato._state != Estados.table.toDeliver)
            {
                mesaDelPlato._state = Estados.table.toDeliver;
            }
        }
        listaPlatosListos.AddOptions(mesasParaServir);
    }

    public static PlatosMesa darPlato(int numeroMesa)
    {
        var plato = Cocina.platosListos.FirstOrDefault(x => x.numero_mesa == numeroMesa);
        Cocina.platosListos.Remove(plato);
        Cocina.actualizarLista();
        return plato;
    }

    public static int getNumberOfOrders()
    {
        return Cocina.platosListos.Count; 
    } 
}

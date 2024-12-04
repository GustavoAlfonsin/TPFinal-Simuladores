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
    //private List<PlatosMesa> platosListos;
    void Start()
    {
        asignarNumeros();
        hideCustomers();
        cocina.whenRegisteringOrders += registerTableOrders;
       // platosListos = new List<PlatosMesa>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        aumentarTiempo(timer);
    }
    private void hideCustomers()
    {
        foreach (Mesa table in mesas)
        {
            table.HideClients();
        }
    }
    private void asignarNumeros()
    {
        int i = 1;
        foreach (Mesa table in mesas)
        {
            table.asignarNumero(i);
            table.whenDeliveringTheFood += cocina.foodDelivered;
            table.whenLeavingThePlace += cocina.throwFood;
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
    private void registerTableOrders(List<dinner> orders, int tableId)
    {
        foreach (Mesa table in mesas)
        {
            if (table.numeroMesa == tableId)
            {
                table.identifyOrders(orders);
            }
        }
    }
    //public static PlatosMesa darPlato(int numeroMesa)
    //{
    //    var plato = Cocina.platosListos.FirstOrDefault(x => x.numero_mesa == numeroMesa);
    //    Cocina.platosListos.Remove(plato);
    //    Cocina.actualizarLista();
    //    return plato;
    //}

    //public static int getNumberOfOrders()
    //{
    //    return Cocina.platosListos.Count; 
    //} 
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Mesa : MonoBehaviour, IInteractions
{
    public int numeroMesa;

    public List<Button> botones;

    public List<GameObject> clientes;
    public TextMeshProUGUI txtMesa;
    public GameObject infoPanel;
    public TextMeshProUGUI txtOrdenes;
    public GameObject _familia;
    public List<TableFood> _dishes = new List<TableFood>(); 
    public GameObject mozo;
    public Image stateImage;
    private bool isAngry = false;
    public Sprite normalCall, angryCall, paycall;
    public bool ocupada { get; set; }
    public Estados.table _state { get; set; }

    [SerializeField]private float start_time, thinking_time, order_time, wait_time, eating_time;

    public Action<dinner> whenDeliveringTheFood;
    public Action<List<TableFood>> whenLeavingThePlace;
    public void asignarNumero(int i)
    {
        numeroMesa = i;
        txtMesa.text = $"{numeroMesa}";
    }

    public void ocuparMesa(float time, GameObject family)
    {
        ocupada = true;
        ShowClients();
        _state = Estados.table.Thinking;
        _familia = family;
        start_time = time;
    }

    private void ShowClients()
    {
        foreach (GameObject c in clientes)
        {
            c.SetActive(true);
        }
    }

    public void desocuparMesa()
    {
        ocupada = false;
        HideClients();
        _state = Estados.table.Thinking;
        _familia.SetActive(true);
        _familia.GetComponent<Client>().readyToLeave(SpawClientes.spawPoint);
        _familia = null;
        _dishes.Clear();
    }

    public void HideClients()
    {
        foreach (GameObject c in clientes)
        {
            c.SetActive(false);
            stateImage.gameObject.SetActive(false);
        }
    }

    public void mostrarAcciones()
    {
        if (ocupada)
        {
            Button btonIndicado;
            switch (_state)
            {
                case Estados.table.toOrder:
                    btonIndicado = botones.FirstOrDefault(x => x.CompareTag("Bton_tomarPedido"));
                    break;
                case Estados.table.toCollect:
                    btonIndicado = botones.FirstOrDefault(x => x.CompareTag("Bton_Cobrar"));
                    break;
                default:
                    btonIndicado = null;
                    break;
            }
            if (btonIndicado != null)
            {
                btonIndicado.gameObject.SetActive(true);
                Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
                btonIndicado.gameObject.transform.position = posicion;
            }
            else if (_state == Estados.table.Thinking)
            {
                Debug.Log("La mesa seleccionada no a hecho ningún llamado");
            }
        }
        else 
        {
            Debug.Log("La mesa indicada esta libre");
        }
    }

    public void ocultarAcciones()
    {
        foreach (Button bton in botones)
        {
            //bton.GetComponent<ColorBotones>().cabiarColorOut();
            Game_Manager.enElBoton = false;
            bton.gameObject.SetActive(false);
        }
    }

    public void pasarTiempo(float timer)
    {
        float tiempoTranscurrido = timer - start_time;
        Debug.Log($"Tiempo transcurrido: {tiempoTranscurrido:00} ");
        if (_state == Estados.table.Thinking)
        {
            Debug.Log("El cliente esta pensando");
            if (tiempoTranscurrido > thinking_time)
            {
                _state = Estados.table.toOrder;
                start_time = Time.time;
            }
        }else if (_state == Estados.table.toOrder)
        {
            Debug.Log("El cliente quiere ordenar");
            if (tiempoTranscurrido > order_time)
            {
                isAngry = true;
                CalcularTardanza(tiempoTranscurrido, order_time);
            }
        }else if (_state == Estados.table.Waiting)
        {
            Debug.Log("Estamos esperando la orden");
            if (tiempoTranscurrido > wait_time)
            {
                isAngry = true;
                CalcularTardanza(tiempoTranscurrido, wait_time);
            }
        }

        foreach (TableFood food in _dishes)
        {
            food.eating(timer);
        }
        if (haveYouFinishedEating() && _dishes.Count > 0)
        {
            _state = Estados.table.toCollect;
        }
    }

    private bool haveYouFinishedEating()
    {
        foreach (TableFood food in _dishes)
        {
            if (food.State == Estados.tableFood.Ontable || food.State == Estados.tableFood.NotYetDelivered)
            {
                return false;
            }
        }
        return true;
    }

    private void CalcularTardanza(float tiempoT, float time)
    {
        if (tiempoT - time > 15f && tiempoT - time < 30f)
        {
            Debug.Log("Se esta tardando mucho");
        }else if (tiempoT - time > 30f)
        {
            Debug.Log("Nos vamos");
            whenLeavingThePlace?.Invoke(_dishes);
            desocuparMesa();
            UIManager.numberOfDC++;
        }
    }

    public List<meal> pedidos()
    {
        List<meal> orders = new List<meal>();
        _dishes = new List<TableFood>();
        for (int i = 0; i < 4; i++)
        {
            meal order = foodRandomizer.getRamdonMeal();
            orders.Add(order);
            TableFood newdish = new TableFood(order._name,7.5f,order._cost);
            _dishes.Add(newdish);
        }
        return orders;
    }

    public void wasAttendedTo()
    {
        _state = Estados.table.Waiting;
        start_time = Time.time;
    }

    public bool deliverFood(dinner food)
    {
        foreach (TableFood dish in _dishes)
        {
            if (dish.Name == food._name)
            {
                dish.wasDelivered();
                whenDeliveringTheFood?.Invoke(food);
                if (allDelivered())
                {
                    _state = Estados.table.Eating;
                }
                return true;
            }
        }
        return false;
    }

    private bool allDelivered()
    {
        foreach (TableFood food in _dishes)
        {
            if (food.State == Estados.tableFood.NotYetDelivered)
            {
                return false;
            }
        }
        return true;
    }

    public void identifyOrders(List<dinner> orders)
    {
        foreach (TableFood food in _dishes)
        {
            dinner dish = orders.FirstOrDefault(o => o._name == food.Name);
            if (dish != null)
            {
                food.IDOrder = dish.ID;
            }
        }
    }

    public float payForDinner()
    {
        float total = 0;
        foreach (TableFood food in _dishes)
        {
            total += food.Cost;
        }
        return total;
    }

    public void showInfo()
    {
        if (ocupada && _dishes.Count > 0)
        {
            infoPanel.SetActive(true);
            txtOrdenes.text = string.Empty;
            foreach (TableFood food in _dishes)
            {
                txtOrdenes.text += food.Name + " \n";
            }
            Vector3 posicion = Input.mousePosition + (Vector3.up * 3) + (Vector3.right * 2);
            infoPanel.transform.position = posicion;
        }
    }

    public void hideInfo()
    {
        infoPanel?.SetActive(false);
    }

    public void callWaiter()
    {
        stateImage.gameObject?.SetActive(true);
        if (!isAngry)
        {
            stateImage.sprite = normalCall;
        }
        else
        {
            stateImage.sprite = angryCall;
        }
    }

    public void callToPay()
    {
        stateImage.gameObject?.SetActive(true);
        stateImage.sprite = paycall;
    }

    public bool someoneWants(dinner food)
    {
        foreach (TableFood dish in _dishes)
        {
            if (dish.Name == food._name)
            {
                return true;
            }
        }
        return false;
    }

    private void OnMouseEnter()
    {
        if (infoPanel != null)
        {
            showInfo();
        }
    }

    private void OnMouseExit()
    {
        if (infoPanel != null)
        {
            hideInfo();
        }
    }
}


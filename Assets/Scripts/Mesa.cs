using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mesa : MonoBehaviour, IInteractions
{
    public int numeroMesa { get; private set; }
    
    [field: SerializeField]
    public List<Button> botones { get; set; }

    [field: SerializeField]
    public List<GameObject> clientes { get; set; }
    [field: SerializeField]
    public TextMeshProUGUI txtMesa { get; set; }
    public GameObject _familia { get; set; }
    public PlatosMesa _plato;
    public GameObject mozo { get; set; }
    public bool ocupada { get; set; }
    public Estados.table _state { get; set; }

    [SerializeField]private float start_time, thinking_time, order_time, wait_time, eating_time;
    public void asignarNumero(int i)
    {
        numeroMesa = i;
        txtMesa.text = $"{numeroMesa}";
    }

    public void ocuparMesa(float time, GameObject family)
    {
        ocupada = true;
        foreach (GameObject c in clientes) 
        {
            c.SetActive(true);
        }
        _state = Estados.table.Thinking;
        _familia = family;
        start_time = time;
    }

    public void desocuparMesa()
    {
        ocupada = false;
        foreach (GameObject c in clientes)
        {
            c.SetActive(false);
        }
        _state = Estados.table.Thinking;
        _familia.SetActive(true);
        _familia.GetComponent<Client>().readyToLeave(SpawClientes.spawPoint);
        _familia = null;
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
                case Estados.table.toDeliver:
                    btonIndicado = botones.FirstOrDefault(x => x.CompareTag("Bton_EntregarPedido"));
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
            bton.GetComponent<ColorBotones>().cabiarColorOut();
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
                CalcularTardanza(tiempoTranscurrido, order_time);
            }
        }else if (_state == Estados.table.Waiting || _state == Estados.table.toDeliver)
        {
            Debug.Log("Estamos esperando la orden");
            if (tiempoTranscurrido > wait_time)
            {
                CalcularTardanza(tiempoTranscurrido, wait_time);
            }
        }else if (_state == Estados.table.Eating)
        {
            if (tiempoTranscurrido > eating_time)
            {
                Debug.Log("Ya podemos pagar la comida");
                _state = Estados.table.toCollect;
            }
        }
    }

    private void CalcularTardanza(float tiempoT, float time)
    {
        if (tiempoT - time > 15f && tiempoT - time < 30f)
        {
            Debug.Log("Se esta tardando mucho");
        }else if (tiempoT - time > 30f)
        {
            Debug.Log("Nos vamos");
            desocuparMesa();
            UIManager.numberOfDC++;
        }
    }

    public PlatosMesa pedidos()
    {
        List<comida> platos = new List<comida>();
        for (int i = 0; i < 4; i++)
        {
            comida nuevaComida = new comida()
            {
                nombre = "Pastas",
                tiempoDeCoccion = 1f,
                estado = Estados.food.cooking,
                costo = 12f
            };
            platos.Add(nuevaComida);
        }
        PlatosMesa nuevoPlato = new PlatosMesa()
        {
            numero_mesa = this.numeroMesa,
            platos = platos,
            listo = false,
            startTime = Time.time
        };

        _state = Estados.table.Waiting;
        start_time = Time.time;
        return nuevoPlato;
    }
}


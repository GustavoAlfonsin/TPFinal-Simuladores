using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Mesa : MonoBehaviour, IInteractions
{
    public int numeroMesa { get; private set; }
    
    [field: SerializeField]
    public List<Button> botones { get; set; }

    [field: SerializeField]
    public List<GameObject> clientes { get; set; }
    public GameObject mozo { get; set; }
    public bool ocupada { get; set; }
    public estado_mesa estado { get; set; }

    [SerializeField]private float start_time, thinking_time, order_time, wait_time, eating_time;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void asignarNumero(int i)
    {
        numeroMesa = i;
    }

    public void ocuparMesa(float time)
    {
        ocupada = true;
        foreach (GameObject c in clientes) 
        {
            c.SetActive(true);
        }
        estado = estado_mesa.Pensando;
        start_time = time;
    }
    public void mostrarAcciones()
    {
        if (ocupada)
        {
            Button btonIndicado;
            switch (estado)
            {
                case estado_mesa.ParaOrdenar:
                    btonIndicado = botones.FirstOrDefault(x => x.CompareTag("Bton_tomarPedido"));
                    break;
                case estado_mesa.ParaEntregar:
                    btonIndicado = botones.FirstOrDefault(x => x.CompareTag("Bton_EntregarPedido"));
                    break;
                case estado_mesa.ParaCobrar:
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
            else if (estado == estado_mesa.Pensando)
            {
                Debug.Log("La mesa seleccionada esta pensando la comida");
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
            bton.gameObject.SetActive(false);
            bton.GetComponent<ColorBotones>().cabiarColorOut();
        }
    }

    public void pasarTiempo(float timer)
    {
        float tiempoTranscurrido = timer - start_time;
        Debug.Log($"Tiempo transcurrido: {tiempoTranscurrido:00} ");
        if (estado == estado_mesa.Pensando)
        {
            Debug.Log("El cliente esta pensando");
            if (tiempoTranscurrido > thinking_time)
            {
                estado = estado_mesa.ParaOrdenar;
                start_time = Time.time;
            }
        }else if (estado == estado_mesa.ParaOrdenar)
        {
            Debug.Log("El cliente quiere ordenar");
            if (tiempoTranscurrido > order_time)
            {
                CalcularTardanza(tiempoTranscurrido, order_time);
            }
        }else if (estado == estado_mesa.Esperando || estado == estado_mesa.ParaEntregar)
        {
            if (tiempoTranscurrido > wait_time)
            {
                CalcularTardanza(tiempoTranscurrido, wait_time);
            }
        }else if (estado == estado_mesa.Comiendo)
        {
            if (tiempoTranscurrido > eating_time)
            {
                estado = estado_mesa.ParaCobrar;
            }
        }
    }

    private void CalcularTardanza(float tiempoT, float time)
    {
        if (tiempoT - time > 15f)
        {
            Debug.Log("Se esta tardando mucho");
        }else if (tiempoT - time > 30f)
        {
            Debug.Log("Nos vamos");
            //funcion para irse del restaurante
        }
    }
}

public enum estado_mesa
{
    Pensando,
    ParaOrdenar,
    Esperando,
    ParaEntregar,
    Comiendo,
    ParaCobrar
}

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
    public GameObject _familia { get; set; }
    public PlatosMesa _plato;
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

    public void ocuparMesa(float time, GameObject family)
    {
        ocupada = true;
        foreach (GameObject c in clientes) 
        {
            c.SetActive(true);
        }
        estado = estado_mesa.Pensando;
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
        estado = estado_mesa.Pensando;
        _familia.SetActive(true);
        _familia.GetComponent<grupo_cliente>().salir();
        _familia = null;
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
            Debug.Log("Estamos esperando la orden");
            if (tiempoTranscurrido > wait_time)
            {
                CalcularTardanza(tiempoTranscurrido, wait_time);
            }
        }else if (estado == estado_mesa.Comiendo)
        {
            if (tiempoTranscurrido > eating_time)
            {
                Debug.Log("Ya podemos pagar la comida");
                estado = estado_mesa.ParaCobrar;
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
                tiempoDeCoccion = 5f,
                estado = foodState.cocinandose,
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

        estado = estado_mesa.Esperando;
        start_time = Time.time;
        return nuevoPlato;
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

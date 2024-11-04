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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ocuparMesa()
    {
        ocupada = true;
        foreach (GameObject c in clientes) 
        {
            c.SetActive(true);
        }
        estado = estado_mesa.Pensando;
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
}

public enum estado_mesa
{
    Pensando,
    ParaOrdenar,
    ParaEntregar,
    ParaCobrar
}

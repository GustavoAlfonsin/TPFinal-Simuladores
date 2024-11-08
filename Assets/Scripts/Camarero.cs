using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Camarero : MonoBehaviour, IInteractions
{
    
    [SerializeField] private float _distancia = 4f;
    private NavMeshAgent _agente;
    private Vector3 _destino;
    private bool conGente;
    private PlatosMesa _plato;
    public int ID { get; private set; }
    [field: SerializeField]
    public GameObject player { get; set; }
    [field: SerializeField]
    public Cocina _cocina { get; set; }
    public GameObject objeto1 { get; set; }
    public GameObject objeto2 { get; set; }
    [field: SerializeField]
    public Button bton_accion { get ; set; }

    public estadoCamarero estado { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        _agente = player.GetComponent<NavMeshAgent>();
        estado = estadoCamarero.Esperando;
        _plato = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (estado == estadoCamarero.EnMovimiento)
        {
            mover();
        } else if (estado == estadoCamarero.Atendiendo)
        {
            atenderClientes();
        } else if (estado == estadoCamarero.TomandoPedido)
        {
            AtenderMesa();
        } else if (estado == estadoCamarero.EntregandoPedido)
        {
            entregandoElPedido();
        } else if (estado == estadoCamarero.CobrandoMesa)
        {
            cobrandoLaMesa();
        }
    }

    public void CamareroCamina(Vector3 destino)
    {
        _destino = destino;
        estado = estadoCamarero.EnMovimiento;
    }

    private void mover()
    {
        _agente.SetDestination(_destino);
        if (Vector3.Distance(transform.position, _destino) == 0)
        {
            estado = estadoCamarero.Esperando;
        }
    }
    public void atender(GameObject obj1, GameObject obj2)
    {
        objeto1 = obj1;
        objeto2 = obj2;
        objeto2.GetComponent<Mesa>().mozo = player;
        estado = estadoCamarero.Atendiendo;
    }
    private void atenderClientes()
    {
        if (!conGente)
        {
            _agente.SetDestination(objeto1.transform.position);
            if (Vector3.Distance(_agente.transform.position, objeto1.transform.position) <= _distancia)
            {
                conGente = true;
                objeto1.GetComponent<grupo_cliente>().asignarMesero(player);
                objeto1.GetComponent<grupo_cliente>().aMoverse();
            }
        }
        else
        {
            _agente.SetDestination(objeto2.transform.position);
            if (Vector3.Distance(_agente.transform.position, objeto2.transform.position) <= _distancia)
            {
                objeto1.SetActive(false);
                objeto2.GetComponent<Mesa>().ocuparMesa(Time.time, objeto1);
                conGente = false;
                Vector3 destino = objeto2.transform.position + (Vector3.left * 4);
                CamareroCamina(destino);
                objeto1 = null;
                objeto2 = null;
            }
        }
    }

    public void Llamando(GameObject mesa)
    {
        objeto1 = mesa;
        estado = estadoCamarero.TomandoPedido;
    }

    private void AtenderMesa()
    {
        if (Vector3.Distance(transform.position, objeto1.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto1.transform.position);
        }
        else
        {
            PlatosMesa encargo = objeto1.GetComponent<Mesa>().pedidos();
            _cocina.nuevoPlato(encargo);
            CamareroCamina(_cocina.transform.position);
            objeto1 = null;
        }
    }

    public void entregarPedido(GameObject mesa)
    {
        objeto1 = mesa;
        estado = estadoCamarero.EntregandoPedido;
    }

    private void entregandoElPedido()
    {
        if (Vector3.Distance(transform.position, _cocina.transform.position) > _distancia && _plato == null)
        {
            _agente.SetDestination(_cocina.transform.position);
        }
        else if(Vector3.Distance(transform.position, _cocina.transform.position) <= _distancia &&
                    _plato == null)
        {
            _plato = ControladorMesas.darPlato(objeto1.GetComponent<Mesa>().numeroMesa);

        }
        else if(_plato != null && Vector3.Distance(transform.position, objeto1.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto1.transform.position);
        }else if (Vector3.Distance(transform.position, objeto1.transform.position) <= _distancia)
        {
            objeto1.GetComponent<Mesa>()._plato = _plato;
            objeto1.GetComponent<Mesa>().estado = estado_mesa.Comiendo;
            CamareroCamina(_cocina.transform.position);
            objeto1 = null;
        }
    }

    public void LlamadaParaCobrar(GameObject mesa)
    {
        objeto1 = mesa;
        estado = estadoCamarero.CobrandoMesa;
    }

    private void cobrandoLaMesa()
    {
        if (Vector3.Distance(transform.position, objeto1.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto1.transform.position);
        }
        else
        {
            Game_Manager.dineroActual += objeto1.GetComponent<Mesa>()._plato.costoTotal;
            objeto1.GetComponent<Mesa>().desocuparMesa();
            CamareroCamina(_cocina.transform.position);
            objeto1 = null;
        }
    }
    public void mostrarAcciones()
    {
        bton_accion.gameObject.SetActive(true);
        Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
        bton_accion.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        bton_accion.gameObject.SetActive(false);
        bton_accion.GetComponent<ColorBotones>().cabiarColorOut();
    }
}
public enum estadoCamarero
{
    EnMovimiento,
    Esperando,
    Atendiendo,
    TomandoPedido,
    EntregandoPedido,
    CobrandoMesa
}


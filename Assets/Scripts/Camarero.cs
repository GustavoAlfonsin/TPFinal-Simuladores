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
    private dinner _plato;
    public int ID { get; private set; }
    [field: SerializeField]
    public GameObject player { get; set; }
    [field: SerializeField]
    public Cocina _cocina { get; set; }
    [field: SerializeField]
    public GameObject objeto1 { get; set; }
    [field: SerializeField]
    public GameObject objeto2 { get; set; }
    [field: SerializeField]
    public Button bton_accion1 { get ; set; }

    [field: SerializeField]
    public Button bton_accion2 { get; set; }
    [field: SerializeField]
    public Estados.waiter estado { get; set; }

    public Image imagenError;
    public Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        _agente = player.GetComponent<NavMeshAgent>();
        estado = Estados.waiter.Waiting;
    }

    // Update is called once per frame
    void Update()
    {
        if (estado == Estados.waiter.Walked)
        {
            mover();
        } 
        else if (estado == Estados.waiter.Attending)
        {
            atenderClientes();
        } 
        else if (estado == Estados.waiter.TakingOrder)
        {
            AtenderMesa();
        } 
        else if (estado == Estados.waiter.Delivering)
        {
            entregandoElPedido();
        } 
        else if (estado == Estados.waiter.ReceivePayment)
        {
            cobrandoLaMesa();
        }

        if (imagenError.gameObject.activeInHierarchy)
        {
            imagenError.gameObject.transform.forward = cam.transform.forward;
        }
    }

    public void CamareroCamina(Vector3 destino)
    {
        _destino = destino;
        estado = Estados.waiter.Walked;
    }

    private void mover()
    {
        _agente.SetDestination(_destino);
        if (Vector3.Distance(transform.position, _destino) <= _distancia)
        {
            estado = Estados.waiter.Waiting;
        }
    }
    public void atender(GameObject obj1, GameObject obj2)
    {
        objeto1 = obj1;
        objeto2 = obj2;
        objeto2.GetComponent<Mesa>().mozo = player;
        estado = Estados.waiter.Attending;
    }
    private void atenderClientes()
    {
        if (!conGente)
        {
            _agente.SetDestination(objeto1.transform.position);
            if (Vector3.Distance(_agente.transform.position, objeto1.transform.position) <= _distancia)
            {
                conGente = true;
                objeto1.GetComponent<Client>().readyToEnter(objeto2);
            }
        }
        else
        {
            _agente.SetDestination(objeto2.transform.position);
            if (Vector3.Distance(_agente.transform.position, objeto2.transform.position) <= _distancia)
            {
                objeto1.SetActive(false);
                objeto2.GetComponent<Mesa>().ocuparMesa(CicloDeDia.getCurrentTime(), objeto1);
                conGente = false;
                Vector3 destino = _cocina._position.position;
                destino.y = transform.position.y;
                CamareroCamina(destino);
                objeto1 = null;
                objeto2 = null;
            }
        }
    }

    public void Llamando(GameObject mesa)
    {
        objeto1 = mesa;
        estado = Estados.waiter.TakingOrder;
    }

    private void AtenderMesa()
    {
        if (Vector3.Distance(transform.position, objeto1.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto1.transform.position);
        }
        else
        {
            List<meal> encargos = objeto1.GetComponent<Mesa>().pedidos();
            int tableID = objeto1.GetComponent<Mesa>().numeroMesa;
            objeto1.GetComponent<Mesa>().wasAttendedTo();
            _cocina.getOrders(encargos, tableID);
            CamareroCamina(_cocina._position.position);
            objeto1 = null;
        }
    }

    public void entregarPedido(GameObject pedido, GameObject mesa)
    {
        objeto1 = pedido;
        objeto2 = mesa;
        estado = Estados.waiter.Delivering;
    }

    public void reEntregarElPedido(GameObject mesa)
    {
        objeto2 = mesa;
        estado = Estados.waiter.Delivering;
    }

    private void entregandoElPedido()
    {
        if (Vector3.Distance(transform.position, objeto1.transform.position) > _distancia && _plato == null)
        {
            _agente.SetDestination(objeto1.transform.position);
        }
        else if(Vector3.Distance(transform.position, objeto1.transform.position) <= _distancia &&
                    _plato == null)
        {
            _plato = objeto1.GetComponent<Tray>().receiveOrder();

        }
        else if(_plato != null && Vector3.Distance(transform.position, objeto2.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto2.transform.position);
        }
        else if (Vector3.Distance(transform.position, objeto2.transform.position) <= _distancia)
        {
            if (objeto2.GetComponent<Mesa>().deliverFood(_plato))
            {
                _plato = null;
                CamareroCamina(_cocina._position.position);
                objeto1 = null;
                objeto2 = null;
            }
            else
            {
                estado = Estados.waiter.incorrectDelivery;
                imagenError.gameObject?.SetActive(true);
            }
        }
    }

    public void LlamadaParaCobrar(GameObject mesa)
    {
        objeto1 = mesa;
        estado = Estados.waiter.ReceivePayment;
    }

    private void cobrandoLaMesa()
    {
        if (Vector3.Distance(transform.position, objeto1.transform.position) > _distancia)
        {
            _agente.SetDestination(objeto1.transform.position);
        }
        else
        {
            float tips;
            Game_Manager.dineroActual += objeto1.GetComponent<Mesa>().payForDinner(out tips);
            Game_Manager.tipsTotales += tips;
            objeto1.GetComponent<Mesa>().desocuparMesa();
            UIManager.numberOfClients++;
            CamareroCamina(_cocina._position.position);
            objeto1 = null;
        }
    }
    public void mostrarAcciones()
    {
        if (estado == Estados.waiter.Waiting)
        {
            bton_accion1.gameObject.SetActive(true);
            Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
            bton_accion1.gameObject.transform.position = posicion;
        }
        else if (estado == Estados.waiter.incorrectDelivery)
        {
            imagenError.gameObject?.SetActive(false);
            bton_accion2.gameObject.SetActive(true);
            Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
            bton_accion2.gameObject.transform.position = posicion;
        }
        
    }

    public void ocultarAcciones()
    {
        Game_Manager.enElBoton = false;
        bton_accion1.gameObject.SetActive(false);
        //bton_accion1.GetComponent<ColorBotones>().cabiarColorOut();

        bton_accion2.gameObject.SetActive(false);
        //bton_accion2.GetComponent<ColorBotones>().cabiarColorOut();
        if (estado == Estados.waiter.incorrectDelivery)
        {
            imagenError.gameObject.SetActive(true);
        }
    }
}



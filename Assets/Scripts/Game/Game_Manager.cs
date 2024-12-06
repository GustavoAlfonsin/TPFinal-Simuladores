using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public LayerMask capaTransitable; // capa donde me puedo mover con el personaje
    public LayerMask capaSeleccion; // capa donde estan los objetos que puedo seleccionar
    
    // Rayo que controla la capa transitable
    private Ray rayoPrincipal;
    private RaycastHit infoRayoPrincipal;
    
    // Rayo que controla la capa de objetos seleccionables
    private Ray rayoSecundario;
    private RaycastHit infoRayoSecundario;
    
    // Objeto donde puede impactar el rayo y es interactuable
    [SerializeField] private GameObject _target;
    
    // Objetos utilizables cuando se realiza una selección
    private GameObject objeto1;
    private GameObject objeto2;
    public TextMeshProUGUI txtAyuda;
    public GameObject panelAyuda;
    public Estados.selection selection_state;
    private int objetosSeleccionados = 0;
   
    // Para poder tocar el boton
    public static bool enElBoton;

    //otros
    public static float dineroActual;
    public TextMeshProUGUI txtDinero;

    // Start is called before the first frame update
    void Start()
    {
        selection_state = Estados.selection.Nothing;
        dineroActual = 0;
        objeto1 = null;
        objeto2 = null;
    }

    // Update is called once per frame
    void Update()
    {
        interaccionesDelMouse();
        txtDinero.text = "Dinero acumulado: $" + dineroActual;
    }

    private void interaccionesDelMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selection_state == Estados.selection.toAttendTo)
            {
                seleccionarObjetos();
            }
            else if (selection_state == Estados.selection.toDeliver)
            {
                deliverySelection();
            }
            else if (selection_state == Estados.selection.toReDeliver)
            {
                reDeliverySelection();
            }
            else
            {
                ObjetosYMovimiento();
            }
            
        }
    }

    private void ObjetosYMovimiento()
    {
        if (!enElBoton)
        {
            rayoPrincipal = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayoPrincipal, out infoRayoPrincipal, 150, capaTransitable))
            {
                Debug.Log(infoRayoPrincipal.collider.tag);
                if (infoRayoPrincipal.collider.CompareTag("Suelo"))
                {
                    if (_target != null)
                    {
                        if (_target.CompareTag("Player"))
                        {
                            if (_target.GetComponent<Camarero>().estado == Estados.waiter.Waiting)
                            {
                                Vector3 destino = infoRayoPrincipal.point;
                                _target.GetComponent<Camarero>().CamareroCamina(destino);
                            }
                            else
                            {
                                Debug.Log("El camarero esta ocupado");
                            }
                        }
                        _target.GetComponent<IInteractions>().ocultarAcciones();
                    }
                }
                else if (hasTheInterface<IInteractions>(infoRayoPrincipal.collider.gameObject))
                {
                    _target = infoRayoPrincipal.collider.gameObject;
                    _target.GetComponent<IInteractions>().mostrarAcciones();
                }
            }
        }
    }

    private bool hasTheInterface<T>(GameObject obj) where T : class
    {
        var components = obj.GetComponents<MonoBehaviour>();

        foreach (var component in components)
        {
            if (component is T)
            {
                return true;
            }
        }
        return false;
    }

    private void seleccionarObjetos()
    {
        rayoSecundario = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayoSecundario, out infoRayoSecundario, 150, capaSeleccion))
        {
            if (objetosSeleccionados == 0 && infoRayoSecundario.collider.CompareTag("Cliente"))
            {
                objeto1 = infoRayoSecundario.collider.gameObject;
                objetosSeleccionados++;
                txtAyuda.text = "Elije la mesa donde quieres llevar a los clientes";
                Debug.Log("Objeto 1 seleccionado: " + objeto1.tag);
            }
            else if (objetosSeleccionados == 1 && infoRayoSecundario.collider.CompareTag("Mesa"))
            {
                if (infoRayoSecundario.collider.GetComponent<Mesa>().ocupada)
                {
                    txtAyuda.text = "La mesa está ocupada, elija una mesa vacia";
                }
                else 
                {
                    objeto2 = infoRayoSecundario.collider.gameObject;
                    objetosSeleccionados++;
                    selection_state = Estados.selection.Nothing;
                    _target.GetComponent<Camarero>().atender(objeto1, objeto2);
                    panelAyuda.SetActive(false);
                    Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
                }
            }
        }
    }

    public void IniciarSeleccion()
    {
        selection_state = Estados.selection.toAttendTo;
        objeto1 = null;
        objeto2 = null;
        objetosSeleccionados = 0;
        _target.GetComponent<IInteractions>().ocultarAcciones();
        panelAyuda.SetActive(true);
        txtAyuda.text = "Elije a los clientes que quieras atender";
    }

    public void AtenderMesa()
    {
        _target.GetComponent<Mesa>().mozo.GetComponent<Camarero>().Llamando(_target);
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void EntregarPedido()
    {
        selection_state = Estados.selection.toDeliver;
        objeto1 = null; objeto2 = null;
        objetosSeleccionados = 0;
        panelAyuda.SetActive(true);
        txtAyuda.text = "Elija al mozo que entregara el pedido a la mesa";
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    private void deliverySelection()
    {
        rayoSecundario = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayoSecundario, out infoRayoSecundario, 150, capaSeleccion))
        {
            if (objetosSeleccionados == 0 && infoRayoSecundario.collider.CompareTag("Player"))
            {
                objeto1 = infoRayoSecundario.collider.gameObject;
                objetosSeleccionados++;
                txtAyuda.text = "Elije la mesa donde quieres entregar la orden";
                Debug.Log("Objeto 1 seleccionado: " + objeto1.tag);
            }
            else if (objetosSeleccionados == 1 && infoRayoSecundario.collider.CompareTag("Mesa"))
            {
                if (!infoRayoSecundario.collider.GetComponent<Mesa>().ocupada)
                {
                    txtAyuda.text = "La mesa está vacia \n elija una que este vacia";
                }
                else
                {
                    objeto2 = infoRayoSecundario.collider.gameObject;
                    objetosSeleccionados++;
                    selection_state = Estados.selection.Nothing;
                    objeto1.GetComponent<Camarero>().entregarPedido(_target, objeto2);
                    panelAyuda.SetActive(false);
                    Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
                }
            }
        }
    }

    public void reSelectTheTable()
    {
        selection_state = Estados.selection.toReDeliver;
        objeto1 = null; objeto2 = null;
        objetosSeleccionados = 0;
        panelAyuda.SetActive(true);
        txtAyuda.text = "Elija otra mesa donde quiera entregar la orden";
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    private void reDeliverySelection()
    {
        rayoSecundario = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayoSecundario, out infoRayoSecundario, 150, capaSeleccion))
        {
            if (objetosSeleccionados == 0 && infoRayoSecundario.collider.CompareTag("Mesa"))
            {
                if (!infoRayoSecundario.collider.GetComponent<Mesa>().ocupada)
                {
                    txtAyuda.text = "La mesa está vacia \n elija una que este vacia";
                }
                else
                {
                    objeto2 = infoRayoSecundario.collider.gameObject;
                    objetosSeleccionados++;
                    selection_state = Estados.selection.Nothing;
                    objeto1.GetComponent<Camarero>().reEntregarElPedido(objeto2);
                    panelAyuda.SetActive(false);
                    Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
                }
            }
        }
    }

    public void CobrarPedido()
    {
        _target.GetComponent<Mesa>().mozo.GetComponent<Camarero>().LlamadaParaCobrar(_target);
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void descartarPedido()
    {
        _target.GetComponent<Tray>().DiscardOrder();
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }
}

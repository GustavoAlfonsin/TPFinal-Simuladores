using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private bool seleccionando = false;
    private int objetosSeleccionados = 0;
   
    // Para poder tocar el boton
    public static bool enElBoton;

    //otros
    public static float dineroActual;
    public TextMeshProUGUI txtDinero;

    // Start is called before the first frame update
    void Start()
    {
        
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
            if (!seleccionando)
            {
                ObjetosYMovimiento();
            }
            else
            {
                seleccionarObjetos();
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
                            if (_target.GetComponent<Camarero>().estado == estadoCamarero.Esperando)
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
                else if (infoRayoPrincipal.collider.CompareTag("Player") ||
                            infoRayoPrincipal.collider.CompareTag("Mesa"))
                {
                    _target = infoRayoPrincipal.collider.gameObject;
                    _target.GetComponent<IInteractions>().mostrarAcciones();
                }
            }
        }
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
                    seleccionando = false;
                    _target.GetComponent<Camarero>().atender(objeto1, objeto2);
                    panelAyuda.SetActive(false);
                    Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
                }
            }
        }
    }

    public void IniciarSeleccion()
    {
        seleccionando = true;
        objeto1 = null;
        objeto2 = null;
        objetosSeleccionados = 0;
        _target.GetComponent<IInteractions>().ocultarAcciones();
        panelAyuda.SetActive(true);
        txtAyuda.text = "Elije a los clientes que quieras atender";
        Debug.Log("Modo de selección activado. Haz click en dos objetos");
    }

    public void AtenderMesa()
    {
        Debug.Log("Estoy apretando el boton");
        _target.GetComponent<Mesa>().mozo.GetComponent<Camarero>().Llamando(_target);
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void EntregarPedido()
    {
        _target.GetComponent<Mesa>().mozo.GetComponent<Camarero>().entregarPedido(_target);
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void CobrarPedido()
    {
        _target.GetComponent<Mesa>().mozo.GetComponent<Camarero>().LlamadaParaCobrar(_target);
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

}

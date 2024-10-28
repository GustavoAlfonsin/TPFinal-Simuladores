using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool seleccionando = false;
    private int objetosSeleccionados = 0;
    // Para poder tocar el boton
    public static bool enElBoton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interaccionesDelMouse();
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
                            _target.GetComponent<Mesero>().posicionFinal = infoRayoPrincipal.point;
                            _target.GetComponent<Mesero>().movimientoLibre = true;
                        }
                        _target.GetComponent<IInteractions>().ocultarAcciones();
                    }
                }
                else if (infoRayoPrincipal.collider.CompareTag("Player") ||
                            infoRayoPrincipal.collider.CompareTag("Mesa"))
                {
                    _target = infoRayoPrincipal.collider.gameObject;
                    _target.GetComponent<IInteractions>().mostrarAcciones();
                    asignarBotones();
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
                Debug.Log("Objeto 1 seleccionado: " + objeto1.tag);
            }
            else if (objetosSeleccionados == 1 && infoRayoSecundario.collider.CompareTag("Mesa"))
            {
                objeto2 = infoRayoSecundario.collider.gameObject;
                objetosSeleccionados++;
                seleccionando = false;
                objeto2.GetComponent<Mesa>().mesero = _target;
                _target.GetComponent<Mesero>().objeto1 = objeto1;
                _target.GetComponent<Mesero>().objeto2 = objeto2;
                _target.GetComponent<Mesero>().sinGente = true;
                _target.GetComponent<Mesero>().movimientoLibre = false;
                _target.GetComponent<Mesero>().atendiendo = true;
                Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
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
        Debug.Log("Modo de selección activado. Haz click en dos objetos");
    }

    public void AtenderMesa()
    {
        Debug.Log("Estoy apretando el boton");
        GameObject meseroMesa = _target.GetComponent<Mesa>().mesero;
        meseroMesa.GetComponent<Mesero>().objeto1 = _target;
        meseroMesa.GetComponent<Mesero>().tomandoPedido = true;
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void EntregarPedido()
    {
        GameObject meseroMesa = _target.GetComponent<Mesa>().mesero;
        meseroMesa.GetComponent<Mesero>().objeto1 = _target;
        meseroMesa.GetComponent<Mesero>().entregandoPedido = true;
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    public void CobrarPedido()
    {
        GameObject meseroMesa = _target.GetComponent<Mesa>().mesero;
        meseroMesa.GetComponent<Mesero>().objeto1 = _target;
        meseroMesa.GetComponent<Mesero>().cobrandoMesa = true;
        _target.GetComponent<IInteractions>().ocultarAcciones();
    }

    private void asignarBotones()
    {
        if (_target.CompareTag("Player"))
        {
            List<Button> botones = _target.GetComponent<Mesero>().buttons;
            var btonAtender = botones.FirstOrDefault(x => x.CompareTag("Bton_Atender"));
            btonAtender.onClick.AddListener(IniciarSeleccion);
        }else if (_target.CompareTag("Mesa"))
        {
            List<Button> botones = _target.GetComponent<Mesa>().buttons;
            foreach ( var button in botones)
            {
                switch (button.tag)
                {
                    case "Bton_tomarPedido":
                        button.onClick.AddListener(AtenderMesa);
                        break;
                    case "Bton_EntregarPedido":
                        button.onClick.AddListener(EntregarPedido);
                        break;
                    case "Bton_Cobrar":
                        button.onClick.AddListener(CobrarPedido);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

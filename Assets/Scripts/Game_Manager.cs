using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public LayerMask capaTransitable;
    public LayerMask capaSeleccion;
    private Ray miRayo;
    private RaycastHit infoRayo;
    private GameObject _target;

    private GameObject objeto1;
    private GameObject objeto2;
    private bool seleccionando = false;
    private int objetosSeleccionados = 0;

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
        miRayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(miRayo, out infoRayo, 150, capaTransitable))
        {
            Debug.Log(infoRayo.collider.tag);
            if (infoRayo.collider.CompareTag("Suelo"))
            {
                if (_target != null)
                {
                    if (_target.CompareTag("Player"))
                    {
                        _target.GetComponent<Mesero>().posicionFinal = infoRayo.point;
                        _target.GetComponent<Mesero>().movimientoLibre = true;
                    }
                    _target.GetComponent<IInteractions>().ocultarAcciones();
                }
            }
            else
            {
                _target = infoRayo.collider.gameObject;
                _target.GetComponent<IInteractions>().mostrarAcciones();
                asignarBotones();

            }
        }
    }

    private void seleccionarObjetos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 150, capaSeleccion))
        {
            if (objetosSeleccionados == 0 && hit.collider.CompareTag("Cliente"))
            {
                objeto1 = hit.collider.gameObject;
                objetosSeleccionados++;
                Debug.Log("Objeto 1 seleccionado: " + objeto1.tag);
            }
            else if (objetosSeleccionados == 1 && hit.collider.CompareTag("Mesa"))
            {
                objeto2 = hit.collider.gameObject;
                objetosSeleccionados++;
                seleccionando = false;
                _target.GetComponent<Mesero>().objeto1 = objeto1;
                _target.GetComponent<Mesero>().objeto2 = objeto2;
                _target.GetComponent<Mesero>().sinGente = true;
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

    private void asignarBotones()
    {
        if (_target.CompareTag("Player"))
        {
            List<Button> botones = _target.GetComponent<Mesero>().buttons;
            var btonAtender = botones.FirstOrDefault(x => x.CompareTag("Bton_Atender"));
            btonAtender.onClick.AddListener(IniciarSeleccion);
        }else if (_target.CompareTag("Mesa"))
        {

        }
    }
}

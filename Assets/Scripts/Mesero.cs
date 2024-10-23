using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mesero : MonoBehaviour, IInteractions
{
    private NavMeshAgent _agent;
    public GameObject _botonesMesero;
    public List<Button> buttons;

    private GameObject objeto1;
    private GameObject objeto2;

    public float distancia = 1.0f;

    private bool seleccionando = false;
    private int objetosSeleccionados = 0;
    // Start is called before the first frame update
    void Start()
    {
        // botonSeleccion.onClick.AddListener(IniciarSeleccion);
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (seleccionando)
        {
            SeleccionarObjetos();
        }

        if (objeto1 != null && objeto2 != null)
        {
            MoverMesero();
        }
    }
    public void IniciarSeleccion()
    {
        seleccionando = true;
        objeto1 = null;
        objeto2 = null;
        objetosSeleccionados = 0;
        Debug.Log("Modo de selección activado. Haz click en dos objetos");
    }
    private void MoverMesero()
    {
        if (Vector3.Distance(_agent.transform.position, objeto1.transform.position) > distancia)
        {
            _agent.SetDestination(objeto1.transform.position);
        }else if (Vector3.Distance(_agent.transform.position, objeto1.transform.position) <= distancia &&
                    Vector3.Distance(_agent.transform.position, objeto2.transform.position) > distancia)
        {
            _agent.SetDestination(objeto2.transform.position);
        }
    }

    private void SeleccionarObjetos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (objetosSeleccionados == 0)
                {
                    objeto1 = hit.collider.gameObject;
                    objetosSeleccionados++;
                    Debug.Log("Objeto 1 seleccionado: " + objeto1.tag);
                }else if (objetosSeleccionados == 1)
                {
                    objeto2 = hit.collider.gameObject;
                    objetosSeleccionados++;
                    seleccionando = false;
                    Debug.Log("Objeto 2 seleccionado: " + objeto2.tag);
                }
            }
        }
    }

    public void mover(Vector3 point)
    {
        _agent.SetDestination(point);
    }
    public void mostrarAcciones()
    {
        _botonesMesero.SetActive(true);
        Vector3 posicion = Input.mousePosition;
        buttons = _botonesMesero.GetComponentsInChildren<Button>().ToList();
        _botonesMesero.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        _botonesMesero.SetActive(false);
    }
}

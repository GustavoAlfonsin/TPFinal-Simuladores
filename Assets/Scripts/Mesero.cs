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
    public GameObject player;
    public List<Button> buttons;

    public Vector3 posicionFinal;
    public GameObject objeto1;
    public GameObject objeto2;

    public float distancia = 3.0f;

    public bool movimientoLibre = false, conGente = false, sinGente = false;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movimientoLibre)
        {
            mover();
        }
        else
        {
            MoverMesero();
        }
    }
    private void MoverMesero()
    {
        if (sinGente)
        {
            _agent.SetDestination(objeto1.transform.position);
            if (Vector3.Distance(_agent.transform.position, objeto1.transform.position) <= distancia)
            {
                sinGente = false;
                objeto1.GetComponent<grupo_cliente>().asignarMesero(player);
                objeto1.GetComponent<grupo_cliente>().aMoverse();
                conGente = true;
            }
        }else if (conGente)
        {
            _agent.SetDestination(objeto2.transform.position);
            if (Vector3.Distance(_agent.transform.position, objeto2.transform.position) <= distancia)
            {
                objeto1.GetComponent<grupo_cliente>().sentarse(objeto2);
                conGente = false;
                movimientoLibre = true;
                posicionFinal = objeto2.transform.position + Vector3.left * 5;
            }
        }
    }

    public void mover()
    {
        _agent.SetDestination(posicionFinal);
        movimientoLibre = Vector3.Distance(_agent.transform.position, posicionFinal) == 0;
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
        foreach (var button in buttons) 
        {
            button.onClick.RemoveAllListeners();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mesero : MonoBehaviour, IInteractions
{
    private NavMeshAgent _agent;
    public GameObject _botonesMesero;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mover(Vector3 point)
    {
        _agent.SetDestination(point);
    }
    public void mostrarAcciones()
    {
        _botonesMesero.SetActive(true);
        Vector3 posicion = Input.mousePosition;
        _botonesMesero.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        _botonesMesero.SetActive(false);
    }
}

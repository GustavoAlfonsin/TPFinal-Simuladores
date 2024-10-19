using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cliente : MonoBehaviour
{
    private float satisfaccion;
    private bool enMesa;
    public GameObject Mozo;
    public float followDistance = 2.0f, stoppingDistance = 1.0f;

    private NavMeshAgent clientAgent;
    // Start is called before the first frame update
    void Start()
    {
        clientAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mozo != null && !enMesa)
        {
            seguirMozo();
        }
    }

    private void seguirMozo()
    {
        Vector3 targetPosition = Mozo.gameObject.transform.position - 
                                    Mozo.gameObject.transform.forward * followDistance;
        clientAgent.SetDestination(targetPosition);
        if (Vector3.Distance(transform.position, Mozo.gameObject.transform.position) <= stoppingDistance)
        {
            clientAgent.isStopped = true;
        }
        else
        {
            clientAgent.isStopped = false;
        }
    }
}

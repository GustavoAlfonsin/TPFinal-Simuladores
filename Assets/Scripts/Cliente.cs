using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cliente : MonoBehaviour
{
    public bool enMovimiento;
    public GameObject followObject;
    public float followDistance = 2.0f, stoppingDistance = 1.0f;
    private Vector3 posInic;

    private NavMeshAgent clientAgent;

    // Start is called before the first frame update
    void Start()
    {
        clientAgent = GetComponent<NavMeshAgent>();
        enMovimiento = false;
        posInic = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (followObject != null && enMovimiento)
        {
            seguir();
        }
    }

    private void seguir()
    {
        Vector3 targetPosition = followObject.gameObject.transform.position - 
                                    followObject.gameObject.transform.forward * followDistance;
        clientAgent.SetDestination(targetPosition);
        
        if(Vector3.Distance(transform.position, followObject.gameObject.transform.position) <= stoppingDistance)
        {
            clientAgent.isStopped = true;
        }
        else
        {
            clientAgent.isStopped = false;
        }
    }

}

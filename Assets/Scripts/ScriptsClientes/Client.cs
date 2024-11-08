using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Client : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _whereToGo;
    private float followDistance = 2.0f, stoppingDistance = 1.0f;

    public  List<NavMeshAgent> _friends = new List<NavMeshAgent>();
    public  Estados.customer _state;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _whereToGo = null;
        _state = Estados.customer.Waiting;
    }

    void Update()
    {
        if (_state == Estados.customer.Entering)
        {
            goToTable();
        }else if (_state == Estados.customer.leaving)
        {
            goToExit();
        }
        
    }

    public void readyToEnter(GameObject table)
    {
        _whereToGo = table;
        _state = Estados.customer.Entering;
    }

    private void goToTable()
    {
        Vector3 endPosition = _whereToGo.transform.position -
                                (_whereToGo.transform.forward * followDistance);
        _agent.SetDestination(endPosition);
        moveFriends();
        if (Vector3.Distance(transform.position, _whereToGo.transform.position) <= stoppingDistance)
        {
            _agent.isStopped = true;
            _state = Estados.customer.Sitting;
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private void moveFriends()
    {
        for (int i = 0; i < _friends.Count; i++)
        {
            Vector3 friendPosition = friendDestiny(i);
            _friends[i].SetDestination(friendPosition);
        }
    }

    private Vector3 friendDestiny(int indice)
    {
        Vector3 back = -_agent.transform.forward;
        Vector3 offset = back * (followDistance * (indice + 1));
        return _agent.transform.position + offset;
    }

    public void readyToLeave(GameObject exit)
    {
        _whereToGo = exit;
        _state = Estados.customer.leaving;
    }
    private void goToExit()
    {
        Vector3 endPosition = _whereToGo.transform.position -
                                (_whereToGo.transform.forward * followDistance);
        _agent.SetDestination(endPosition);
        moveFriends();
        if (Vector3.Distance(transform.position, _whereToGo.transform.position) <= stoppingDistance)
        {
            _agent.isStopped = true;
            _state = Estados.customer.Waiting;
        }
        else
        {
            _agent.isStopped = false;
        }
    }
}

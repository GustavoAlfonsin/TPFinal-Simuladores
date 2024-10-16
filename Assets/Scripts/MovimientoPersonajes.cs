using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoPersonajes : MonoBehaviour
{
    public LayerMask capaTransitable;
    private NavMeshAgent miAgente;
    private Ray miRayo;
    private RaycastHit infoRayo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            miRayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(miRayo, out infoRayo, 150, capaTransitable))
            {
                if (infoRayo.collider.CompareTag("Player"))
                {
                    miAgente = infoRayo.collider.GetComponent<NavMeshAgent>();
                }
                else
                {
                    miAgente.SetDestination(infoRayo.point);
                }
                
            }
        }
    }
}

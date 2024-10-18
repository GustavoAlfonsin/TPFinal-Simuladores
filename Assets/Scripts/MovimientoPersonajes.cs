using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoPersonajes : MonoBehaviour
{
    public LayerMask capaTransitable;
    public GameObject _botonesMozo, _botonesMesa;
    private Vector3 _posicion;
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
                    _botonesMozo.gameObject.SetActive(true);
                    _posicion = Input.mousePosition;
                    _botonesMozo.gameObject.transform.position = _posicion;

                } else if (infoRayo.collider.CompareTag("Mesa"))
                {
                    _botonesMesa.gameObject.SetActive(true);
                    _posicion = Input.mousePosition;
                    _botonesMesa.gameObject.transform.position = _posicion;
                }
                else
                {
                    if (miAgente != null)
                    {
                        miAgente.SetDestination(infoRayo.point);
                    }
                    _botonesMozo.gameObject.SetActive(false);
                }
                
            }
        }
    }
}

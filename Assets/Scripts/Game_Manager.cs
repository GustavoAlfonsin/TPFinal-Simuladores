using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Game_Manager : MonoBehaviour
{
    public LayerMask capaTransitable;
    private Ray miRayo;
    private RaycastHit infoRayo;
    private GameObject _target;

    public GameObject prefab;
    public GameObject spaw;
    private float distancia = 2.0f;
    private float spawTime = 5.0f;
    private int maxClient = 3;
    public List<GameObject> clientes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateClients();
        //Vector3[] posiciones = new Vector3[]
        //{
        //    spaw.transform.position + Vector3.forward * distancia, // adelante
        //    spaw.transform.position + Vector3.right * distancia, // a la derecha
        //    spaw.transform.position + Vector3.left * distancia, // a la izquierda
        //    spaw.transform.position, //origen del spaw
        //};

        //foreach (Vector3 pos in posiciones)
        //{
        //    Instantiate(prefab, pos, Quaternion.identity);
        //}
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
                            _target.GetComponent<Mesero>().mover(infoRayo.point);
                        }
                        _target.GetComponent<IInteractions>().ocultarAcciones();
                    }
                }
                else
                {
                    _target = infoRayo.collider.gameObject;
                    _target.GetComponent<IInteractions>().mostrarAcciones();
                }
            }
        }
    }

    private void CreateClients()
    {
        for (int i = 1; i <= maxClient; i++)
        {
            GameObject grupo = new GameObject($"Grupo{i}");
            if(clientes.Count == 0)
            {
                grupo.transform.position = spaw.transform.position;
            }
            else
            {
                grupo.transform.position = clientes.Last().transform.position + Vector3.back * 2;
            }

            grupo.AddComponent<grupo_cliente>();

            for (int j = 0; j < 4; j++) 
            {
                GameObject nuevoCliente = Instantiate(prefab, grupo.transform.position
                                                        ,Quaternion.identity);
                nuevoCliente.transform.parent = grupo.transform;
                nuevoCliente.GetComponent<Cliente>().enMovimiento = false;
                grupo.GetComponent<grupo_cliente>().sumarCliente(nuevoCliente);
                switch (j)
                {
                    case 0:
                        nuevoCliente.transform.localPosition = Vector3.zero;
                        break;
                    case 1:
                        nuevoCliente.transform.localPosition = Vector3.right * distancia;
                        break;
                    case 2:
                        nuevoCliente.transform.localPosition = Vector3.left * distancia;
                        break;
                    case 3:
                        nuevoCliente.transform.localPosition = Vector3.back * distancia;
                        break;
                    default:
                        break;
                }
            }
            grupo.GetComponent<grupo_cliente>().conectarClientes();
            clientes.Add(grupo);
        }
    }
}

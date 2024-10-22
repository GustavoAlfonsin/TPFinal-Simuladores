using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grupo_cliente : MonoBehaviour
{
    [SerializeField] private GameObject[] clientes = new GameObject[4];
    public Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sumarCliente(GameObject c)
    {
        if (!lleno())
        {
            for (int i = 0; i < clientes.Length; i++)
            {
                if (clientes[i] == null)
                {
                    clientes[i] = c;
                    return;
                }
            }
        }
    }

    private bool lleno()
    {
        for (int i = 0; i < clientes.Length; i++)
        {
            if (clientes[i] == null)
                return false;
        }
        return true;
    }

    public void conectarClientes()
    {
        for(int i = 1;i < clientes.Length; i++)
        {
            clientes[i].GetComponent<Cliente>().followObject = clientes[i-1];
        }
    }
}

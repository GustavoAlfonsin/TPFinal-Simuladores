using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class grupo_cliente : MonoBehaviour
{
    public GameObject[] clientes = new GameObject[4];
    public Transform pos;
    private GameObject mesero;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void conectarClientes()
    {
        for(int i = 1;i < clientes.Length; i++)
        {
            clientes[i].GetComponent<Cliente>().followObject = clientes[i-1];
            clientes[i].GetComponent<Cliente>().enMovimiento = false;
        }
    }

    public void asignarMesero(GameObject mesero)
    {
        this.mesero = mesero;
        clientes[0].GetComponent<Cliente>().followObject = mesero;
    }

    public void aMoverse()
    {
        foreach (GameObject go in clientes)
        {
            go.GetComponent<Cliente>().enMovimiento = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawClientes : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spaw;
    //private float spawTime = 5.0f;
    private int maxClient = 3;
    public List<GameObject> clientes = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateClients();
        clientes[0].SetActive(true);
        clientes[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateClients()
    {
        for (int i = 1; i <= maxClient; i++)
        {
            Vector3 posicionIn;
            if (clientes.Count == 0)
            {
                posicionIn = spaw.transform.position;
            }
            else
            {
                posicionIn = clientes.Last().transform.position + Vector3.left * 4;
            }

            GameObject nuevoCliente = Instantiate(prefab, posicionIn, Quaternion.identity);

            //nuevoCliente.GetComponent<grupo_cliente>().conectarClientes();
            nuevoCliente.GetComponent<grupo_cliente>().spaw = spaw;
            clientes.Add(nuevoCliente);
            nuevoCliente.gameObject.SetActive(false);
        }
    }
}

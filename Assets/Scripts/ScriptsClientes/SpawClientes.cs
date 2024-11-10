using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawClientes : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spaw;

    private float spawTime = 5.0f;
    private float _time = 0f;
    private int maxClient = 10;
    private float _spaceBetweenClients = 3.0f;
    private int _row = 1;
    private float _hor = 1;

    public List<GameObject> clients = new List<GameObject>();
    private List<GameObject> activeClients = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateClients();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= spawTime && activeClients.Count < clients.Count)
        {
            NextClient();
            _time = 0;
            spawTime = Random.Range(5,11);
        }
        
        reorganizeClients();
    }

    private void CreateClients()
    {
        for (int i = 1; i <= maxClient; i++)
        {
            Vector3 start_position = spaw.transform.position;
            GameObject nuevoCliente = Instantiate(prefab, start_position, Quaternion.identity);
            clients.Add(nuevoCliente);
            nuevoCliente.gameObject.SetActive(false);
            
        }
    }

    private void NextClient()
    {
        GameObject client = clients.FirstOrDefault(x => !x.activeInHierarchy && 
                                x.GetComponent<Client>()._state == Estados.customer.Waiting);
        client.SetActive(true);
        client.transform.position = whichPosition(activeClients.Count);
        client.GetComponent<Client>().lineUpFriends();
        activeClients.Add(client);
    }

    private Vector3 whichPosition(int indice)
    {
        Vector3 basePosition = transform.position;
        Vector3 offset = Vector3.right * _hor * _spaceBetweenClients * indice;
        return basePosition + offset;
    }

    private void reorganizeClients()
    {
        if (activeClients.Count == 0) return;

        // Saco de la lista los clientes que se fueron a la mesa
        for (int i = 0; i < activeClients.Count; i++)
        {
            Vector3 expectedPosition = whichPosition(i);
            if (Vector3.Distance(activeClients[i].transform.position, expectedPosition) > 1f)
            {
                activeClients.RemoveAt(i);
            }
        }
        // Reorganizo la posición de los clientes en la fila
        for (int j = 0; j < activeClients.Count; j++) 
        {
            activeClients[j].transform.position = whichPosition(j);
        }
    }
}

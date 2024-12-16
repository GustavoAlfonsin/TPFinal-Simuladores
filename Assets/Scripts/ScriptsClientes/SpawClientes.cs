using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawClientes : MonoBehaviour
{
    [SerializeField] private Vector3 spawnAreaCenter, spawnAreaSize;
    public GameObject prefab;
    public GameObject spaw;
    public static GameObject spawPoint;

    private float spawTime = 5.0f;
    private TimeSpan lastSpaw;
    private int maxClient = 24;
    private float _spaceBetweenClients = 1.5f;

    public List<GameObject> clients = new List<GameObject>();
    void Start()
    {
        nameRandomizer.loadNames();
        spawPoint = spaw;
        lastSpaw = CicloDeDia.getCurrentTime();
        CreateClients();
    }

    void Update()
    {
        int passedTime = CicloDeDia.howMuchTimePassed(lastSpaw);
        if (passedTime > spawTime)
        {
            NextClient();
            lastSpaw = CicloDeDia.getCurrentTime();
            spawTime = UnityEngine.Random.Range(5, 9);
        }
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
        var waitingClients = clients.Where(c => c.activeInHierarchy &&
                                            c.GetComponent<Client>()._state == Estados.customer.Waiting).Count();
        if(waitingClients < 5)
        {
            GameObject client = clients.FirstOrDefault(x => !x.activeInHierarchy &&
                                x.GetComponent<Client>()._state == Estados.customer.Waiting);
            if (client != null)
            {
                Vector3 position;
                do
                {
                    position = GetRandomPositionInArea();
                } while (isPositionValid(position));
                client.SetActive(true);
                client.transform.position = position;
                client.GetComponent<Client>().lineUpFriends();
            }
        }
    }

    private Vector3 GetRandomPositionInArea()
    {
        float randomX = UnityEngine.Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = UnityEngine.Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        return spawnAreaCenter + new Vector3(randomX, 0, randomZ);
    }

    private bool isPositionValid(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            return false;
        }

        return !Physics.CheckSphere(position,_spaceBetweenClients);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(spawnAreaCenter, spawnAreaSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
    }
}

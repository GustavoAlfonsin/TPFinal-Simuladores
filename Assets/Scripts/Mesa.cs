using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesa : MonoBehaviour, IInteractions
{
    GameObject _botonesMesa;   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void mostrarAcciones()
    {
        _botonesMesa.SetActive(true);
        Vector3 posicion = Input.mousePosition;
        _botonesMesa.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        _botonesMesa.SetActive(false);
    }
}

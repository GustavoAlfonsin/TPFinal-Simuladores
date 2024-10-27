using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Mesa : MonoBehaviour, IInteractions
{
    public GameObject _botonesMesa;
    public List<Button> buttons;
    public GameObject[] clientes;

    public bool mesaOcupada = false;
    public GameObject mesero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ocuparMesa()
    {
        mesaOcupada = true;
        foreach (GameObject c in clientes) 
        {
            c.SetActive(true);
        }
    }
    public void mostrarAcciones()
    {
        _botonesMesa.SetActive(true);
        Vector3 posicion = Input.mousePosition;
        buttons = _botonesMesa.GetComponentsInChildren<Button>().ToList();
        _botonesMesa.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        _botonesMesa.SetActive(false);
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
            button.GetComponent<ColorBotones>().cabiarColorOut();
        }
    }
}

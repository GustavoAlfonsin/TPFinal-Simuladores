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
    public bool paraOrdenar = false, paraEntregar = false, paraCobrar = false; 


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
        paraOrdenar = true;
    }
    public void mostrarAcciones()
    {
        if (mesaOcupada)
        {
            _botonesMesa.SetActive(true);
            Vector3 posicion = Input.mousePosition;
            buttons = _botonesMesa.GetComponentsInChildren<Button>().ToList();
            Transform[] botones = _botonesMesa.GetComponentsInChildren<Transform>(true);

            foreach (Transform boton in botones)
            {
                if (boton != _botonesMesa.transform)
                {
                    if (!boton.CompareTag("Untagged"))
                    {
                        boton.gameObject.SetActive(false);
                    }
                }
            }
            
            if (paraOrdenar)
            {
                _botonesMesa.transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (paraEntregar)
            {
                _botonesMesa.transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (paraCobrar)
            {
                _botonesMesa.transform.GetChild(2).gameObject.SetActive(true);
            }
           
            _botonesMesa.gameObject.transform.position = posicion;
        }
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

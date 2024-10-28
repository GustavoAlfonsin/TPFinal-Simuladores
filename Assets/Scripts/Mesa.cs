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
            Button mainOption;
            if (paraOrdenar)
            {
                mainOption = buttons.FirstOrDefault(x => x.CompareTag("Bton_tomarPedido"));
            }else if (paraEntregar)
            {
                mainOption = buttons.FirstOrDefault(x => x.CompareTag("Bton_EntregarPedido"));
            }else if (paraCobrar)
            {
                mainOption = buttons.FirstOrDefault(x => x.CompareTag("Bton_Cobrar"));
            }
            else
            {
                mainOption = buttons.FirstOrDefault(x => x.CompareTag("Bton_tomarPedido"));
                mainOption.enabled = false;
            }
            mainOption.gameObject.SetActive(true);
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

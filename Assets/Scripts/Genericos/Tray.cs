using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tray : MonoBehaviour, IInteractions
{
    public GameObject panelButtons;
    public GameObject _cubo;
    public GameObject PanelInfo;
    public TextMeshProUGUI _txtOrderName;
    public Cocina _kitchen;

    private bool isEmpty;
    public bool IsEmpty
    {
        get { return isEmpty; }
    }

    private dinner _order;
    public dinner Order
    {
        get { return _order; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_cubo = this.GetComponent<GameObject>();
        isEmpty = true;
        this.gameObject.SetActive(false);
    }

    public void assignOrder(dinner food)
    {
        this._order = food;
        isEmpty = false;
    }

    public dinner receiveOrder()
    {
        removeOrder();
        return this._order;
    }

    public void removeOrder()
    {
        this._order = null;
        isEmpty = true;
        this.gameObject.SetActive(false);
    }

    public void showInfo()
    {
        if (!isEmpty)
        {
            PanelInfo.SetActive(true);
            _txtOrderName.text = "Plato pedido: " + _order._name;
            Vector3 posicion = Input.mousePosition + (Vector3.up * 3) + (Vector3.right * 2);
            PanelInfo.transform.position = posicion;
        }
    }

    public void hideInfo()
    {
        PanelInfo?.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (PanelInfo != null)
        {
            showInfo();
        }
    }

    private void OnMouseExit()
    {
        if (PanelInfo != null)
        {
            hideInfo();
        }
    }

    public void mostrarAcciones()
    {
        hideInfo();
        panelButtons.SetActive(true);
        Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
        panelButtons.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        panelButtons.SetActive(false);
        Game_Manager.enElBoton = false;
        //foreach (Button button in panelButtons.GetComponentsInChildren<Button>())
        //{
        //    button.GetComponent<ColorBotones>().cabiarColorOut();
        //}
    }

    public void DiscardOrder()
    {
        if (_kitchen.throwFood(Order))
        {
            removeOrder();
        }
        else
        {
            PanelInfo.SetActive(true);
            _txtOrderName.text = "Hay clientes que pueden querer esta comida";
        }
    }
}

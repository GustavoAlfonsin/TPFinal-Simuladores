using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tray : MonoBehaviour, IInteractions
{
    public Button deliverBtton;
    public GameObject _cubo;
    public GameObject PanelInfo;
    public TextMeshProUGUI _txtOrderName;

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
        _cubo = this.GetComponent<GameObject>();
        isEmpty = true;
    }

    public void assignOrder(dinner food)
    {
        this._order = food;
        isEmpty = false;
    }

    public dinner receiveOrder()
    {
        return this._order;
    }

    public void removeOrder()
    {
        this._order = null;
        isEmpty = true;
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

    public void mostrarAcciones()
    {
        hideInfo();
        deliverBtton.gameObject.SetActive(true);
        Vector3 posicion = Input.mousePosition + (Vector3.up * 3);
        deliverBtton.gameObject.transform.position = posicion;
    }

    public void ocultarAcciones()
    {
        deliverBtton.gameObject.SetActive(false);
        deliverBtton.GetComponent<ColorBotones>().cabiarColorOut();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorBotones : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image imagen;
    // Start is called before the first frame update
    void Start()
    {
        imagen = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cabiarColorIn()
    {
        Game_Manager.enElBoton = true;
        imagen.color = Color.green;
    }

    public void cabiarColorOut()
    {
        Game_Manager.enElBoton = false;
        imagen.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cabiarColorIn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cabiarColorOut();
    }
}

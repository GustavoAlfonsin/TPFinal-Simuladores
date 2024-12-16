using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button bton_ayuda, bton_volver, bton_avanzar, bton_retroceder;
    public GameObject panelTutorial;
    public TextMeshProUGUI txt_tutorial;
    private int paginaActual;

    void Start()
    {
        paginaActual = 0;
    }


    void Update()
    {
        if (paginaActual == 0)
        {
            txt_tutorial.text = "Puedes controlar a los mozos (las capsulas rojas) haciendo click sobre ello. \n" +
                                "Si tienes un mozo seleccionado puedes moverlo a cualquier parte del escenario indicando " +
                                "donde lo quieres llevar. \n Ademas, te pueden salir el boton 'atender' el cual le permite " +
                                "al mozo llevar a un cliente que elijas a una mesa disponible.";
        }else if (paginaActual == 1)
        {
            txt_tutorial.text = "Cuando lleves algún cliente a una mesa, despues de un rato te aparecera una señal que indica" +
                " que estan llamando al mozo. Cuando esto ocurra al hacer click sobre " +
                "la mesa aparecera el boton 'Tomar pedido' el cual llama al mozo. \n Si te tardas mucho en responder los clientes" +
                " pueden ser que se vayan del local. \n Una vez tomado el pedido, cada vez que pongas el mouse sobre la mesa, " +
                "en el cuadro que esta abajo a la derecha, te mostrara la comida que pidio esa mesa.";
        }else if (paginaActual == 2)
        {
            txt_tutorial.text = "Una vez tomado el pedido, la cocina se encargara de prepararlo. Cuando este listo aparecera en " +
                "la barra (los rectangulos amarillos). Al posar el mouse sobre ellos veras que pedido es. Cuando hagas click " +
                "sobre ellos que aparecera los botones de 'Entregar' y 'Desechar'. Si apretas 'Entregar' elije al mozo que lo " +
                "vaya a entregar y la mesa dondo lo quieras entregar. Si te confundes de mesa puede afectar el monto final de la " +
                "propina \n Si apretas el boton 'Desechar', la comida se tira.";
        }else if (paginaActual == 3)
        {
            txt_tutorial.text = "Si el mozo entrego el pedido a la mesa equivocada le aparecera arriba una señal de averterncia \n" +
                "Al pasar esto y hacer click sobre el mozo, te aparecera el boton de 'Reentregar' el cual te permite elegir otra mesa" +
                " para entregar la comida.";
        }else if (paginaActual == 4)
        {
            txt_tutorial.text = "Cuando los clientes terminaron de comer, les aparecera un carter que indica que quieren pagar " +
                "la comida. Haz click sobre la mesa y aparecera el boton de 'Cobrar Mesa' lo que hara que el mozo cobre el dinero " +
                "por la cena";
        }
    }

    public void mostrarTutorial()
    {
        Time.timeScale = 0f;
        panelTutorial.SetActive(true);
        paginaActual = 0;
    }

    public void cerrarTutorial()
    {
        panelTutorial.SetActive(false);
        Time.timeScale = 1f;
    }

    public void avanzarPagina()
    {
        paginaActual++;
        if (paginaActual == 4)
        {
            bton_avanzar.gameObject.SetActive(false);
        }else if (paginaActual == 1)
        {
            bton_retroceder.gameObject.SetActive(true);
        }

    }

    public void retrocederPagina() 
    {
        paginaActual--;
        if (paginaActual == 0)
        {
            bton_retroceder.gameObject.SetActive(false);
        }else if (paginaActual == 3)
        {
            bton_avanzar.gameObject.SetActive(true);
        }
    }
}

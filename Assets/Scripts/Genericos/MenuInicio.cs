using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInicio : MonoBehaviour
{
    public Button btonIniciarJuego, btonSalirJuego;
    
    public void iniciarJuego()
    {
        SceneManager.LoadScene("Game_level");
    }

    public void cerrarJuego()
    {
        Debug.Log("Cerraste el juego");
        Application.Quit();
    }
}

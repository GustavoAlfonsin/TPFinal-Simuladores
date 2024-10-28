using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDeDia : MonoBehaviour
{
    [SerializeField] private float duracionDiaEnSegundos = 840f;
    private int horaInicio = 10;
    private float timeOfDay = 0;
    public int diaActual = 1;
    
    
    void Update()
    {
        timeOfDay += Time.deltaTime;

        if (timeOfDay >= duracionDiaEnSegundos)
        {
            timeOfDay = 0;
            diaActual++;
            //terminarJuego
        }
    }

    public string GetTime()
    {
        int hours = horaInicio + Mathf.FloorToInt((timeOfDay / duracionDiaEnSegundos) * 14);
        int minutes = Mathf.FloorToInt(((timeOfDay/duracionDiaEnSegundos) * 1440f) % 60f);
        int seconds = Mathf.FloorToInt(((timeOfDay / duracionDiaEnSegundos) * 86400f) % 60f);

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}

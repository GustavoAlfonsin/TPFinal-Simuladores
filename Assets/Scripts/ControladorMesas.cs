using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMesas : MonoBehaviour
{
    [field: SerializeField]
    public List<Mesa> mesas { get; set; }
    private float timer = 0f;
    void Start()
    {
        asignarNumeros();
    }

    void Update()
    {
        timer += Time.deltaTime;
        aumentarTiempo(timer);
    }

    private void asignarNumeros()
    {
        int i = 1;
        foreach (Mesa table in mesas)
        {
            table.asignarNumero(i);
            i++;
        }
    }

    private void aumentarTiempo(float time)
    {
        foreach (Mesa m in mesas)
        {
            if (m.ocupada)
            {
                m.pasarTiempo(time);
            }
        }
    }
}

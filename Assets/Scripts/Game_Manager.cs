using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Game_Manager : MonoBehaviour
{
    public LayerMask capaTransitable;
    private Ray miRayo;
    private RaycastHit infoRayo;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        interaccionesDelMouse();
    }

    private void interaccionesDelMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            miRayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(miRayo, out infoRayo, 150, capaTransitable))
            {
                if (infoRayo.collider.CompareTag("Suelo"))
                {
                    if (_target != null && _target.CompareTag("Player"))
                    {
                        _target.GetComponent<Mesero>().mover(infoRayo.point);
                    }
                    _target.GetComponent<IInteractions>().ocultarAcciones();
                }
                else
                {
                    _target = infoRayo.collider.gameObject;
                    _target.GetComponent<IInteractions>().mostrarAcciones();
                }
            }
        }
    }
}

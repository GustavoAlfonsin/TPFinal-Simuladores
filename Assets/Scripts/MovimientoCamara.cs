using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    [SerializeField] private Vector3 posicion, movimiento;
    [SerializeField] private Camera cam;
    public float velocidad, velRotacion, velZoom;
    public AnimationCurve areaActiva;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            rotarCamara();
        }
        else
        {
            moverCamara();
            zoomCamara();
        }

        if (Input.GetMouseButtonDown(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void moverCamara()
    {
        posicion = Input.mousePosition;
        posicion.x /= Screen.width;
        posicion.y /= Screen.height;

        movimiento.x = areaActiva.Evaluate(posicion.x);
        movimiento.z = areaActiva.Evaluate(posicion.y);

        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }

    private void rotarCamara()
    {
        transform.Rotate(Vector3.up * velRotacion * Time.deltaTime * Input.GetAxis("Mouse X"));

    }

    private void zoomCamara()
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - Input.mouseScrollDelta.y * velZoom * Time.deltaTime, 10, 100);
    }
}

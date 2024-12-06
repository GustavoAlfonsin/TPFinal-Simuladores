using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    public LayerMask mainLayer;
    private Ray mainRay;
    private RaycastHit mainInfo;

    private GameObject _target;
    private GameObject _thing_one, _thing_two;
    private int selectedObject = 0;

    public static bool onTheButton;
    public Estados.selection gameState;

    void Start()
    {
        gameState = Estados.selection.Nothing;
        _thing_one = null; _thing_two = null;
    }


    void Update()
    {
        
    }
}

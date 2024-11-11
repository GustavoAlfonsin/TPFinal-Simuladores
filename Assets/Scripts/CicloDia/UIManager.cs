using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI currentHourTxt;

    public TextMeshProUGUI numberOfOrders;
    public TextMeshProUGUI amountEarned;
    public TextMeshProUGUI disstisfiedCustomers;
    public static int numberOfClients;
    public static int numberOfDC; //numero de clientes insatisfechos
    public GameObject EndGamePanel;

    public CicloDeDia hora;
    private bool endGame;

    void Start()
    {
        endGame = false;
        EndGamePanel.SetActive(false);
        Time.timeScale = 1f;
        if (CicloDeDia.finishDay == null)
        {
            CicloDeDia.finishDay += endDay;
        }
    }

    void Update()
    {
        if (!endGame) 
        {
            currentHourTxt.text = hora.GetTime();
            if (Input.GetKeyDown(KeyCode.T))
            {
                endGame = true;
            }
        }
        else
        {
            EndOfTheGame();
        }
       
    }

    private void EndOfTheGame()
    {
        Time.timeScale = 0f;
        EndGamePanel.SetActive(true);
        numberOfOrders.text = numberOfClients.ToString();
        amountEarned.text = Game_Manager.dineroActual.ToString();
        disstisfiedCustomers.text = numberOfDC.ToString();
    }

    public void restartLevel()
    {
        SceneManager.LoadScene("GameLevel");
    }
    private void endDay()
    {
        endGame = true;
    }
}

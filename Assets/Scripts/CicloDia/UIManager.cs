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
    public static int numberOfDC; //numero de clientes insatisfechos
    public GameObject EndGamePanel;

    public CicloDeDia hora;
    private bool endGame;
    // Start is called before the first frame update
    void Start()
    {
        endGame = false;
        EndGamePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
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
        numberOfOrders.text = ControladorMesas.getNumberOfOrders().ToString();
        amountEarned.text = Game_Manager.dineroActual.ToString();
        disstisfiedCustomers.text = numberOfDC.ToString();
    }

    public void restartLevel()
    {
        SceneManager.LoadScene("GameLevel");
    }
}

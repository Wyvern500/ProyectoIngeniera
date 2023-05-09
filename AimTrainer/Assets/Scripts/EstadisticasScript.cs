using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EstadisticasScript : MonoBehaviour
{

    private int tirosAcertados;
    private int tirosFallados;
    private int tirosTotales;
    private float punteria;

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void showStatistics()
    {
        Debug.Log(transform.GetChild(1).GetComponent<Text>().text = "Tiros Acertados: " + tirosAcertados);
        Debug.Log(transform.GetChild(2).GetComponent<Text>().text = "Tiros Fallados: " + tirosFallados);
        Debug.Log(transform.GetChild(3).GetComponent<Text>().text = "Tiros Totales: " + tirosTotales);
        Debug.Log(transform.GetChild(4).GetComponent<Text>().text = "Punteria: " + Math.Floor(((tirosAcertados + 0.0f) / tirosTotales) * 100) + "%");
        Debug.Log(transform.GetChild(5).GetComponent<Text>().text = "Enemigos Totales: " + 20);
    }

    public void setData(int tirosAcertados, int tirosFallados, int tirosTotales)
    {
        this.tirosAcertados = tirosAcertados;
        this.tirosFallados = tirosFallados;
        this.tirosTotales= tirosTotales;
        Debug.Log("Updating data");
    }

}

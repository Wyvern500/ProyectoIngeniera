using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaScript : MonoBehaviour
{

    public GameObject canvas;
    public GameObject estadisticas;

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Finalizar()
    {
        canvas.SetActive(false);
        gameObject.SetActive(false);
        estadisticas.SetActive(true);
        estadisticas.GetComponent<EstadisticasScript>().showStatistics();
    }
}

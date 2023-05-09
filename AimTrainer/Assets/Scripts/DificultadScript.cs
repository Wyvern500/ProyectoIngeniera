using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DificultadScript : MonoBehaviour
{

    public void Facil()
    {
        PlayerPrefs.SetInt("dificultad", 0);
        SceneManager.LoadScene(3);
    }

    public void Medio()
    {
        PlayerPrefs.SetInt("dificultad", 1);
        SceneManager.LoadScene(3);
    }

    public void Dificil()
    {
        PlayerPrefs.SetInt("dificultad", 2);
        SceneManager.LoadScene(3);
    }

    public void Atras()
    {
        SceneManager.LoadScene(0);
    }

}

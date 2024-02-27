using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassSelect : MonoBehaviour
{
    public void ChooseLight()
    {
        PlayerPrefs.SetString("HeroClass", "light");
        PlayerPrefs.SetInt("maxUnits", 28);
        PlayerPrefs.SetInt("unit1size", 1);
        PlayerPrefs.SetInt("unit2size", 1);
        PlayerPrefs.SetInt("unit3size", 2);
        PlayerPrefs.SetInt("unit4size", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void ChooseWater()
    {
        PlayerPrefs.SetString("HeroClass", "Water");
        PlayerPrefs.SetInt("maxUnits", 25);
        PlayerPrefs.SetInt("unit1size", 1);
        PlayerPrefs.SetInt("unit2size", 1);
        PlayerPrefs.SetInt("unit3size", 3);
        PlayerPrefs.SetInt("unit4size", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChooseNature()
    {
        PlayerPrefs.SetString("HeroClass", "Nature");
        PlayerPrefs.SetInt("maxUnits", 23);
        PlayerPrefs.SetInt("unit1size", 1);
        PlayerPrefs.SetInt("unit2size", 1);
        PlayerPrefs.SetInt("unit3size", 2);
        PlayerPrefs.SetInt("unit4size", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChooseBlood()
    {
        PlayerPrefs.SetString("HeroClass", "Blood");
        PlayerPrefs.SetInt("maxUnits", 21);
        PlayerPrefs.SetInt("unit1size", 2);
        PlayerPrefs.SetInt("unit2size", 2);
        PlayerPrefs.SetInt("unit3size", 4);
        PlayerPrefs.SetInt("unit4size", 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
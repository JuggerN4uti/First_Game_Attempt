using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject[] pages;
    public TMPro.TextMeshProUGUI page_number;
    private int current = 0;

    public void Next()
    {
        pages[current].SetActive(false);
        current++;
        if (current > 14)
            current = 0;
        pages[current].SetActive(true);
        Display_Page();
    }

    public void Previous()
    {
        pages[current].SetActive(false);
        current--;
        if (current < 0)
            current = 14;
        pages[current].SetActive(true);
        Display_Page();
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 7);
    }

    public void Display_Page()
    {
        page_number.text = (current + 1).ToString("0") + "/15";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Encyclopedia : MonoBehaviour
{
    public GameObject[] info, pages;
    public GameObject mad_scientist, monstrosity;
    public TMPro.TextMeshProUGUI page_button;
    public int current = 0, page = 0;

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 5);
    }

    public void NextPage()
    {
        pages[page].SetActive(false);
        page++;
        if (page == 3)
            page -= 3;
        pages[page].SetActive(true);
        page_button.text = (page + 1).ToString("0") + "/3";
    }

    public void UnitClicked(int id)
    {
        info[current].SetActive(false);
        current = id;
        info[current].SetActive(true);
    }

    public void Transform(int id)
    {
        if (current == 29)
        {
            UnitClicked(30);
            mad_scientist.SetActive(false);
            monstrosity.SetActive(true);
        }
        else if (current == 30)
        {
            UnitClicked(29);
            monstrosity.SetActive(false);
            mad_scientist.SetActive(true);
        }
        else
            UnitClicked(id);
    }
}
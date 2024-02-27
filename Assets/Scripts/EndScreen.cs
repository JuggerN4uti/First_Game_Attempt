using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title, level, defeated;
    public Image[] boss;
    public Sprite[] bosses;

    void Start()
    {
        title.text = PlayerPrefs.GetString("EndScreenTitle");
        level.text = "Your level: " + PlayerPrefs.GetString("PlayerLevel");
        defeated.text = "Total enemies vanquished: " + PlayerPrefs.GetString("EnemiesSlain");

        if (PlayerPrefs.GetInt("FirstBoss") == 0)
            boss[0].enabled = false;
        else
            boss[0].sprite = bosses[PlayerPrefs.GetInt("FirstBoss")];

        if (PlayerPrefs.GetInt("SecondBoss") == 0)
            boss[1].enabled = false;
        else
            boss[1].sprite = bosses[PlayerPrefs.GetInt("SecondBoss")];

        if (PlayerPrefs.GetInt("ThirdBoss") == 0)
            boss[2].enabled = false;
        else
            boss[2].sprite = bosses[PlayerPrefs.GetInt("ThirdBoss")];
    }

    public void Proceed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 6);
    }
}
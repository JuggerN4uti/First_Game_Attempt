using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPop : MonoBehaviour
{
    public float expire;
    public TMPro.TextMeshPro damage;

    void Start()
    {
        expire = 800;
    }

    // Update is called once per frame
    void Update()
    {
        expire -= 300 * Time.deltaTime;
        if (expire <= 0)
            Destroy(gameObject);
    }
}
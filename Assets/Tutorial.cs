using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SetPage(0);
    }

    int currentPage = 0;
    public Sprite[] Pages;

    void SetPage(int p)
    {
        currentPage = Mathf.Clamp(p, 0, Pages.Length - 1);
        GetComponent<Image>().sprite = Pages[currentPage];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (currentPage == Pages.Length - 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                return;
            }

            SetPage(currentPage + 1);
        }

        if (Input.GetMouseButtonUp(1))
        {
            SetPage(currentPage - 1);

        }
    }

}
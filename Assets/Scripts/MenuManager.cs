using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private static MenuManager m_MenuManager;

    void awake()
    {
        DontDestroyOnLoad(this);

        if (m_MenuManager == null)
        {
            m_MenuManager = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        Debug.Log("jeu lancé");
    }

    public void Tutorial()
    {
        Debug.Log("tuto");
    }

    public void Credits()
    {
        Debug.Log("credits");
    }

    public void Exit()
    {
        Debug.Log("adios");
        Application.Quit();
    }

}

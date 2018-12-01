using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    // Use this for initialization
    public Button m_YourFirstButton;
    public Button m_YourSecondButton;
    //public Button m_YourThirdButton;


    void Start()
    {
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        m_YourFirstButton.onClick.AddListener(TaskOnClick);
        //m_YourSecondButton.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
        //m_YourThirdButton.onClick.AddListener(() => ButtonClicked(42));
        m_YourSecondButton.onClick.AddListener(TaskOnClick2);
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (GameObject.Find("MusicManager") != null)
        {
            GameObject.Find("MusicManager").GetComponent<AudioSource>().Stop();
        }
    }

    void TaskOnClick2()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("Quitting");
        Application.Quit();
    }

    void TaskWithParameters(string message)
    {
        //Output this to console when the Button2 is clicked
        Debug.Log(message);
    }

    void ButtonClicked(int buttonNo)
    {
        //Output this to console when the Button3 is clicked
        Debug.Log("Button clicked = " + buttonNo);
    }
}

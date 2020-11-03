using UnityEngine;
using UnityEngine.UI;

public class Sas : MonoBehaviour
{
    public Controller Controller;
    private Controller controller;
    private Text text;

    private void Awake()
    {
        controller = Controller.GetComponent<Controller>();
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (controller.sas == true)
        {
            text.text = ("Sas on");
        }
        else if (controller.sas == false)
        {
            text.text = ("Sas off");
        }
    }
}
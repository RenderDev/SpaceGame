using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    public Controller Controller;
    private Controller controller;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        controller = Controller.GetComponent<Controller>();
    }

    private void Update()
    {
        text.text = string.Format("Fuel {0}", (controller.fuel).ToString("000.0"));
    }
}
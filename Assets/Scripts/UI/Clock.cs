using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private string t;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        t = System.DateTime.Now.ToString("HH:mm");
        text.text = string.Format(t);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class Speed : MonoBehaviour
{
    public new Rigidbody rigidbody;
    private Rigidbody rb;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        rb = rigidbody.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        text.text = string.Format("Speed {0:0.00}m/s", rb.velocity.magnitude);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    // add a list of guns
    public Gun Gun;

    private Gun gun;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        gun = Gun.GetComponent<Gun>();
    }

    private void Update()
    {
        text.text = string.Format("Ammo {0}", gun.AmmoCount.ToString());
    }
}
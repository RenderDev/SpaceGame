using GNB;
using UnityEngine;

public class GunTrigger : MonoBehaviour
{
    /*     private bool g = false;
        public bool gunOn; */
    public Gun[] guns;

    private void Update()
    {
        /*         if (gunOn == false && Input.GetKeyDown (KeyCode.I))
                {
                    g = true;
                    gunOn = true;
                }
                else if (gunOn == true && Input.GetKeyDown (KeyCode.I))
                {
                    g = false;
                    gunOn = false;
                } */
        if (Input.GetMouseButton(0) == true /* && g == true */ )
        {
            foreach (var gun in guns)
            {
                gun.Fire(Vector3.zero);
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public Rigidbody m_Shell;                   
    public Transform m_FireTransform;           
    
    public void Fire (float launchForce, float fireRate)
    {
        Rigidbody shellInstance = Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = launchForce * m_FireTransform.forward;
    }
}

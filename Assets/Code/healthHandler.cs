using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public Transform pfHealthBar;
    void Start()
    {
        HealthSystem healthSystem = new HealthSystem(100);

        Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        
        healthBar.Setup(healthSystem);
    }


}

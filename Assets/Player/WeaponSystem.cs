using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{

    private enum WeaponType
    {
        Pistol,
        Rifle,
        Bazooka

    }

    public Transform _shootingPoint;
    public GameObject _bullet;

    private bool bCanShoot = true;


    public void OnFire(InputValue value)
    {

        if(bCanShoot)
        {
            Instantiate(_bullet, _shootingPoint.position, transform.rotation);
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


}

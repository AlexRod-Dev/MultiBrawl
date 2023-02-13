using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    private float _currentHealth;
    public float _maxHealth;

    public GameObject _blood;


    // Start is called before the first frame update
    void Start() => _currentHealth = _maxHealth;


    public void TakeDamage(float _damage) 
    {
  
        _currentHealth -= _damage;
 
        if (_currentHealth <= 0)
        {

            RandomSpawner._spawnCount--;

            Instantiate(_blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
        else
        {
            Debug.Log(_currentHealth);
        }
     
    
    }

   
}

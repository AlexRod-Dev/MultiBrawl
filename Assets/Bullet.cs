using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float _speed;
    public float _damage;
    private Rigidbody2D _rb;
    public GameObject _blood;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * _speed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

      
        HealthSystem enemy = collision.GetComponent<HealthSystem>();
        if(enemy != null)
        {
            enemy.TakeDamage(_damage);
            Instantiate(_blood, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }
}

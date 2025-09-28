using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float thurstSpeed = 1.0f;
    public float turnSpeed = 1.0f;

    private Rigidbody2D _rigidBody;
    private bool _thrusting;
    private float _turnDirection;

    private GameManager gameManager;

    //Goi 1 lan trong suot cycle app, khoi tao ref
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>(); //ref toi rigidbody
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }
    void Update()
    {
        _thrusting = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _turnDirection = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        { _turnDirection = -1.0f;
        }
        else
        {
            _turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        ;
    }

    //Code lien quan toi physic update
    private void FixedUpdate() 
    {
        if (_thrusting)
        {
            _rigidBody.AddForce(this.transform.up * this.thurstSpeed); //equivalent of force in 3d
        }

        if (_turnDirection != 0.0f)
        {
            _rigidBody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
        // play sound
        if (gameManager != null)
        {
            gameManager.PlaySound(gameManager.shootSound);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     if(collision.gameObject.tag == "Asteroid")
        {
            _rigidBody.linearVelocity = Vector3.zero;
            _rigidBody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false); //stop entire gameobject, not rendering anything 

            if (gameManager != null)
            {
                gameManager.PlayerDie();
            }
            //Object.FindFirstObjectByType<GameManager>().PlayerDie(); //Need to optimize due to performance tax
        }
    }
}

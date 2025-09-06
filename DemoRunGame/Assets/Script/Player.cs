using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody rigidbody;
    private GameController gameController;
    private float moveX;
    private float moveZ;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            // 右
            moveX = 1;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // 左
            moveX = -1;
        }
        else
        {
            moveX = 0;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            // 前
            moveZ = 1;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            // 後
            moveZ = -1;
        }
        else
        {
            moveZ = 0;
        }
    }

    private void FixedUpdate()
    {
        Vector3 move = new (moveX, 0, moveZ);
        rigidbody.velocity = move * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Goal")
        {
            Debug.Log("Goal");
            gameController.GameClear();
        }
    }
}

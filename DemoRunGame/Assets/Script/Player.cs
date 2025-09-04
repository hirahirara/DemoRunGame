using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody rigidbody;
    private GameController gameController;
    private Vector3 moveX;
    private Vector3 moveZ;

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
            moveX = Vector3.right;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // 左
            moveX = Vector3.left;
        }
        else
        {
            moveX = Vector3.zero;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            // 前
            moveZ = Vector3.forward;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            // 後
            moveZ = Vector3.back;
        }
        else
        {
            moveZ = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = (moveX + moveZ) * speed;
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

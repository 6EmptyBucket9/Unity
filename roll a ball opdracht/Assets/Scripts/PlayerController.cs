using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public float speed = 0;

    public bool gameOver = false;

    public bool nextLevel = false;

    bool running = true;
    public float delay = 3;
    float timer;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);

    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Level2")
        {
            if ((count >= 9) && (gameOver == true))
            {
                winTextObject.SetActive(true);
                StartCoroutine(ExampleCoroutine());
            }

        }
        else
        {
            if ((count >= 5) && (gameOver == true))
            {
                winTextObject.SetActive(true);
                speed = 0;

            }
        }
        IEnumerator ExampleCoroutine()
        {
            yield return new WaitForSeconds(3);

            SetNextLevel();
        }
    }


    void SetNextLevel()
    {
        if (nextLevel == true)
        {
            SceneManager.LoadScene("Level2");
        }
    }
    private void FixedUpdate()
    {

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement);
        rb.AddForce(movement * speed);
    }
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);

        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

        }
        if (other.gameObject.CompareTag("PickUp GameOver"))
        {
            other.gameObject.SetActive(false);
            count++;
            nextLevel = true;
            gameOver = true;
            SetCountText();
        }

    }
}


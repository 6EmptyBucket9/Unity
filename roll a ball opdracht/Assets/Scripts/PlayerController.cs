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

    public float timeRemaining; 
    private float movementX;
    private float movementY;

    public float speed = 0;

    public bool gameOver = false;

    public bool nextLevel = false;

    bool timerIsRunning = false;
    public float delay = 3;

    public TextMeshProUGUI countText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI loseText;
    public GameObject winTextObject;

    public GameObject loseTextObject;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        timerIsRunning = true;
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        timeRemaining = 45;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Level2")
        {
            SetTimeText(timeRemaining);
        }

        // Debug log to check initial timeRemaining
        Debug.Log("Start: Initial timeRemaining set to: " + timeRemaining);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        Scene scene = SceneManager.GetActiveScene();
        countText.text = "Points: " + count.ToString();
        if (scene.name == "Level1")
        {
            if ((count >= 9) && (gameOver == true))
            {
                winTextObject.SetActive(true);
                StartCoroutine(ExampleCoroutine());
            }
        }
        else if (scene.name == "Level2")
        {
            if ((count >= 5) && (gameOver == true))
            {
                winTextObject.SetActive(true);
                StartCoroutine(ExampleCoroutine());
            }
        }
        else
        {
            if ((count >= 6) && (gameOver == true))
            {
                winTextObject.SetActive(true);
                timeRemaining = 0;
                speed = 0;
            }
        }
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(3);
        SetNextLevel();
    }

    void SetTimeText(float timeToDisplay)
    {
        if (timeText != null)
        {
            if (timeRemaining > 0)
            {
                timeToDisplay += 1;
            }

            //format the timer
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void SetNextLevel()
    {
        if (nextLevel)
        {
            if (SceneManager.GetActiveScene().name != "Level2")
            {
                SceneManager.LoadScene("Level2");
            }
            else
            {
                SceneManager.LoadScene("Level3");
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                SetTimeText(timeRemaining);
            }
            else if (timeRemaining > 0 && gameOver == true)
            {
                timeRemaining = 0;
                timerIsRunning = false;
                SetTimeText(timeRemaining);
                gameOver = true;
                speed = 0;
                if (loseTextObject != null)
                {
                    loseTextObject.SetActive(true);
                }
                else //check if object is assigned for fix
                {
                    Debug.LogError("loseTextObject is not assigned in the Inspector");
                }


            }
        }
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

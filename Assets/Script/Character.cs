using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{

    public static Character Instance;

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;
    private Vector3 startingPoint;

    private Rigidbody rb;
    private Animator animator;

    [SerializeField] GameObject hat, helmet, horn;
    [SerializeField] public GameObject[] checkPointsArray;
    [SerializeField] private GameObject checkPointsParent;
    [SerializeField] List<GameObject> hearts = new();
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject UI,taptostart;

    bool isOnBuildings = true, isJumping = false, isTapped = false;
    private int heart;

    public bool detectSwipeAfterRelease = false;

    public float jumpPower = 1f, rollPower = 1f, LeftRightSwapping = 1f;
    private float ForwardSpeed=2f;
    public float SWIPE_THRESHOLD = 20f;

    private const string SAVE_CHECKPOINT_INDEX = "Last_checkpoint_index";

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        //PlayerPrefs.DeleteAll();

    }
    private void Start()
    {
        heart = 2;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        UI.SetActive(true);
        gameOver.SetActive(false);

        LoadCheckPoint();

        animator.SetBool("isRunning", false);
        int savedCheckPointIndex = -1;
        savedCheckPointIndex = PlayerPrefs.GetInt(SAVE_CHECKPOINT_INDEX, -1);

        if (savedCheckPointIndex != -1){
            startingPoint = checkPointsArray[savedCheckPointIndex].transform.position;
            RespawnPlayer();
        }
        else{
            startingPoint = gameObject.transform.position;
        }
        
    }
    private void FixedUpdate()
    {
        
        if (PlayerPrefs.GetInt("item") == 1)
        {
            hat.SetActive(true);
            helmet.SetActive(false);
            horn.SetActive(false);
        }
        if (PlayerPrefs.GetInt("item") == 2)
        {
            helmet.SetActive(true);
            hat.SetActive(false);
            horn.SetActive(false);
        }
        if (PlayerPrefs.GetInt("item") == 3)
        {
            horn.SetActive(true);
            hat.SetActive(false);
            helmet.SetActive(false);
        }
    }
    void Update()
    {

        if (Input.touchCount>=1)
        {
            isTapped = true;
            taptostart.SetActive(false);
        }
        
        if (isTapped) {
            this.transform.Translate(0, 0, ForwardSpeed * Time.deltaTime);

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPos = touch.position;
                    fingerDownPos = touch.position;
                }
                //Detects Swipe while finger is still moving on screen
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeAfterRelease)
                    {
                        fingerDownPos = touch.position;
                        DetectSwipe();
                    }
                }
                //Detects swipe after finger is released from screen
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }
            
        }
    }
    void DetectSwipe()
    {
        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            //Debug.Log("Vertical Swipe Detected!");
            if (fingerDownPos.y - fingerUpPos.y > 0 && !isJumping)
            {
                OnSwipeUp();
                animator.SetBool("isRunning", true);
            }
            else if (fingerDownPos.y - fingerUpPos.y < 0 )
            {
                OnSwipeDown();
            }
            fingerUpPos = fingerDownPos;
        }
        else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            //Debug.Log("Horizontal Swipe Detected!");
            if (fingerDownPos.x - fingerUpPos.x > 0 )
            {
                OnSwipeRight();
                animator.SetBool("isRunning", true);
            }
            else if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                animator.SetBool("isRunning", true);
                OnSwipeLeft();
            }
            fingerUpPos = fingerDownPos;
        }
        else{
            //Debug.Log("No Swipe Detected!");
        }
    }
    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }
    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }
    void OnSwipeUp()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        animator.SetBool("isJumping", true);
    }
    void OnSwipeDown()
    {
        rb.AddForce(Vector3.down * rollPower, ForceMode.VelocityChange);
        animator.SetBool("isRolling", true);
        Invoke("PleaseRoll", 1);
    }
    void PleaseRoll(){ animator.SetBool("isRolling", false);}
    void OnSwipeLeft()
    {
        rb.AddForce(Vector3.left * LeftRightSwapping, ForceMode.Impulse);
    }
    void OnSwipeRight()
    {
        rb.AddForce(Vector3.right * LeftRightSwapping, ForceMode.Impulse);
    }
    private void LoadCheckPoint()
    {
        checkPointsArray = new GameObject[checkPointsParent.transform.childCount];
        int index = 0;
        foreach (Transform singleCheckPoint in checkPointsParent.transform)
        {
            checkPointsArray[index] = singleCheckPoint.gameObject;
            index++;
        }
    }
    
    void StandUp()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void RespawnPlayer()
    {
        
        isTapped = false;
        taptostart.SetActive(true);
        ForwardSpeed = 2f;
        gameObject.transform.position = startingPoint;
        animator.SetBool("isDead", false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            StartCoroutine(Dead());
        }
        if (collision.gameObject.tag == "Building")
        {
            isJumping = false;
            isOnBuildings = true;
            animator.SetBool("isJumping", false);
            ForwardSpeed = 2f;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
        {
            animator.SetBool("isRunning",true);
            isOnBuildings = false; 
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "SpeedBump")
        {
            ForwardSpeed = 3f;
        }
        if (other.gameObject.tag == "SpeedLower")
        {
            ForwardSpeed = 1f;
        }
        if (other.gameObject.tag == "Building")
        {
            ForwardSpeed = 2f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            StartCoroutine(PlayNewLevel());
        }
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            int checkPointIndex = -1;
            checkPointIndex = Array.FindIndex(checkPointsArray, match => match == other.gameObject);
            if (checkPointIndex != -1)
            {
                PlayerPrefs.SetInt(SAVE_CHECKPOINT_INDEX, checkPointIndex);
                startingPoint = other.gameObject.transform.position;
                other.gameObject.SetActive(false);
            }
        }
    }

    
    IEnumerator PlayNewLevel()
    {
        animator.SetBool("isWin", true);
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("StartUI");
    }
    IEnumerator Dead()
    {
        ForwardSpeed = 0f;
        rb.velocity = Vector3.zero;
        animator.SetBool("isDead", true);
        Destroy(hearts[heart]);
        heart--;
        yield return new WaitForSeconds(1.5f);
        if (heart == -1)
        {
            StartCoroutine(GameOver()); 
        }
        else{
            RespawnPlayer();
        }
        
    }
    IEnumerator GameOver()
    {
        animator.SetBool("isDead", false);
        yield return new WaitForSeconds(2f);
        gameOver.SetActive(true);
        heart = 2;

        //SceneManager.LoadScene("StartUI");
    }

}


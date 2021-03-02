using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2D;
    protected Animator playerAnim;
    
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    protected Vector2 startPos;
    protected string direction;
    private bool isPressed;
    protected GameManager gameManager;
    private SpriteRenderer[] spriteRenderer;
    
    public float speed = 2f;
    public float steps;
    public string codeName = "Hero";
    //public static bool needToGo;
    protected Vector2 targetReached;
    private Vector2 targetRight;
    private Vector2 targetLeft;
    private Vector2 targetUp;
    private Vector2 targetDown;

    

    private void Start()
    {
        
        playerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        targetReached = new Vector2(7, 7);
        direction = "Tapped";
        
    }

    private void OnEnable()
    {
        spriteRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>();
        GameManager.onClick += ChangeSortingLayer;
        Debug.Log("Subscribed");
    }

    private void OnDestroy()
    {
        Debug.Log("Unsubscribed");
        GameManager.onClick -= ChangeSortingLayer;
    }

    private void OnMouseDown()
    {
        
        if (gameManager.needToGo)
        {
            gameManager.InvokeOnClick();
            isPressed = true;
            startPos = new Vector2(transform.position.x, transform.position.y);
            targetRight = new Vector2(transform.position.x + steps, transform.position.y);
            targetLeft = new Vector2(transform.position.x - steps, transform.position.y);
            targetUp = new Vector2(transform.position.x, transform.position.y + steps);
            targetDown = new Vector2(transform.position.x, transform.position.y - steps);
            gameManager.counter = false;
            gameManager.heroName = gameObject.name;
            gameManager.adTools.SetActive(false);
            foreach (var s in spriteRenderer)
            {
                s.sortingLayerName = "player";
            }
            gameManager.movesleftBeforeAction = gameManager.movesLeft;

        }
    }

   
    private void Update()
    {


        if (Input.touchCount > 0 && isPressed)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }

            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    direction = "Tapped";

                    isPressed = false;
                }

                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    gameManager.movesLeft--;
                    isPressed = false;
                    if(x > 0)
                    {
                        direction = "Right";
                        targetReached = targetRight;
                        playerAnim.SetBool("goesRight", true);
                        
                    }
                    else
                    {
                        direction = "Left";
                        targetReached = targetLeft;
                        playerAnim.SetBool("goesLeft", true);
                        
                    }
                        
                }

                else
                {
                    //direction = y > 0 ? "Up" : "Down";
                    gameManager.movesLeft--;
                    isPressed = false;
                    if (y > 0)
                    {
                        direction = "Up";
                        targetReached = targetUp;
                        playerAnim.SetBool("goesUp", true);

                    }
                    else
                    {
                        direction = "Down";
                        targetReached = targetDown;
                        playerAnim.SetBool("goesDown", true);
                    }
                }

            }
        }
        if (direction != "Tapped")
        {
            gameManager.needToGo = false;

            rb2D.MovePosition(Vector2.MoveTowards(transform.position, targetReached, speed * Time.deltaTime));

        }


        if (new Vector2(transform.position.x, transform.position.y) == targetReached && gameManager.checkPoints > 0 && gameManager.heroName == gameObject.name)
        {
            if(!gameManager.menuOn)
            gameManager.needToGo = true;
            playerAnim.SetBool("goesRight", false);
            playerAnim.SetBool("goesLeft", false);
            playerAnim.SetBool("goesDown", false);
            playerAnim.SetBool("goesUp", false);
            if (codeName == "Hero3")
            {
                playerAnim.SetBool("onHeadsRight", false);
                playerAnim.SetBool("onHeadsLeft", false);
                playerAnim.SetBool("onHeadsUp", false);
                playerAnim.SetBool("onHeadsDown", false);
            }
            direction = "Tapped";

            if (gameManager.movesLeft == 0)
            {
                Debug.Log("Test");
                gameManager.GameOver();
            }
            
        }

    }

    public void ChangeSortingLayer()
    {
        foreach (var s in spriteRenderer)
        {
            s.sortingLayerName = "Default";
        }
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y) == targetReached && 
            collision.gameObject.CompareTag("Checkpoint") && new Vector2(transform.position.x, transform.position.y) == targetReached)
        {
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            if(!gameManager.gameOver)
                gameManager.needToGo = true;
            gameManager.checkPoints -= 1;
            gameManager.PlayStarParticle(targetReached);
            if (gameManager.checkPoints == 0)
            {
                Debug.Log("LevelChanged");
                gameManager.ChangeLevel();
            }
            if (gameManager.movesLeft == 0 && gameManager.checkPoints > 0)
                gameManager.GameOver();



        }
        else if (collision.gameObject.CompareTag("Bounds") && new Vector2(transform.position.x, transform.position.y) == targetReached)
        {
            gameManager.GameOver();
            gameObject.SetActive(false);
            Debug.Log("not active");
            
        }
    }


}

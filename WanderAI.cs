using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WanderAI : MonoBehaviour
{
    public NavMeshAgent agent;

    private AudioSource audioSource;
    public AudioClip roaring;

    private Animator playerAnim;

    public List<GameObject> destinationPoints = new List<GameObject>();

    public int currentState;
    public int randomPoint;
    public int _randomPoint;

    public string randomPointName;

    private bool isNotWalking = true;
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        StartCoroutine(State());
        audioSource = GetComponent<AudioSource>();
    }


    //void Update()
    //{
    //    //if (Input.GetMouseButton(0))
    //    //{
    //    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
    //    //    RaycastHit hit;

    //    //    if (Physics.Raycast(ray, out hit))
    //    //    {
    //    //        agent.SetDestination(hit.point);
    //    //        playerAnim.SetBool("isWalking", true);
    //    //        playerAnim.SetBool("isRoaring", false);

    //    //    }

    //    //}

    //    if (transform.position == agent.pathEndPosition)
    //    {
    //        playerAnim.SetBool("isWalking", false);

    //    }
    //}

    IEnumerator State()
    {
        while(true)
        {
            while (isNotWalking)
            {
                yield return new WaitForSeconds(5f);
                currentState = Random.Range(1, 5);
                SetState(currentState);
                print(currentState);
            }
            yield return new WaitForSeconds(1);
        }
        
    }

    //private void State()
    //{
    //    currentState = Random.Range(1, 5);
    //    SetState(currentState);
    //    print("beginCor");
    //    print(currentState);
    //}

    private void Roar()
    {
        audioSource.PlayOneShot(roaring, 0.5f);
        print("Roaring");
    }

    private void SetState(int currentState)
    {
        switch(currentState)
        {
            case 1://walking
                isNotWalking = false;
                playerAnim.SetBool("isRoaring", false);
                playerAnim.SetBool("isEating", false);
                playerAnim.SetBool("isWalking", true);
                while(randomPoint == _randomPoint)
                {
                    randomPoint = Random.Range(0, destinationPoints.Count);
                }
                _randomPoint = randomPoint;
                randomPointName = destinationPoints[randomPoint].name;
                print(randomPointName);
                agent.SetDestination(destinationPoints[randomPoint].transform.position);
                break;
            case 2://roaring
                playerAnim.SetBool("isEating", false);
                playerAnim.SetBool("isWalking", false);
                playerAnim.SetBool("isRoaring", true);
                Invoke("Roar", 2f);
                break;
            case 3://idle
                playerAnim.SetBool("isEating", false);
                playerAnim.SetBool("isWalking", false);
                playerAnim.SetBool("isRoaring", false);
                break;
            case 4://eating
                playerAnim.SetBool("isWalking", false);
                playerAnim.SetBool("isRoaring", false);
                playerAnim.SetBool("isEating", true);
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == randomPointName && isNotWalking == false)
        {
            print("Destination");
            isNotWalking = true;
            playerAnim.SetBool("isWalking", false);
            //StartCoroutine(State());
            //currentState = Random.Range(1, 5);
            //SetState(currentState);
            //print(currentState);
        }
        
    }
    
} 

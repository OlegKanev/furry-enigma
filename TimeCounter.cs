using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeCounter : MonoBehaviour
{
    public int currentStars;
    public int currentLives;
    

    private DateTime exitTime;
    private TimeSpan offlineTime;
    public  TimeSpan timeToIncrement = new TimeSpan(0, 10, 0);
    private TimeSpan _timeToIncrement = new TimeSpan(0, 10, 0);
    private TimeSpan subtractTime = new TimeSpan(0, 0, 1);
    private TimeSpan subtractTimeTest = new TimeSpan(0, 1, 0);

    private bool countDown = false;

    private void Awake()
    {

        if (PlayerPrefs.HasKey("currentStars"))
        {
            currentStars = PlayerPrefs.GetInt("currentStars");
            print(currentStars);
        }
        else
        {
            currentStars = 0; // удалить потом!!!!!!
            print("no");
        }
            

        if (PlayerPrefs.HasKey("currentLives"))
            currentLives = PlayerPrefs.GetInt("currentLives");
        else
            currentLives = 10;
        if (PlayerPrefs.HasKey("exitTime"))
            exitTime = DateTime.Parse(PlayerPrefs.GetString("exitTime"));
        if (PlayerPrefs.HasKey("timeToIncrement"))
            timeToIncrement = TimeSpan.Parse(PlayerPrefs.GetString("timeToIncrement"));
        
    }
    void Start()
    {
        if(exitTime != null)
         offlineTime = DateTime.Now - exitTime;
        else
         offlineTime = new TimeSpan(0, 0, 0);


        if (currentLives < 10)
        {
            for (; offlineTime >= _timeToIncrement && currentLives < 10; offlineTime -= _timeToIncrement)
            {
                currentLives++;

            }
            if(offlineTime < _timeToIncrement && currentLives < 10)
            {
                if (offlineTime < timeToIncrement)
                {
                    countDown = true;
                    timeToIncrement -= offlineTime;
                    StartCoroutine(Timer());
                }
                else if (offlineTime > timeToIncrement)
                {
                    countDown = true;
                    offlineTime -= timeToIncrement;
                    currentLives++;
                    timeToIncrement = _timeToIncrement - offlineTime;
                    StartCoroutine(Timer());
                }
                else if (offlineTime == timeToIncrement)
                {
                    currentLives++;
                    timeToIncrement = _timeToIncrement;
                    StartCoroutine(Timer());
                }
            }
            else if(currentLives >= 10)
                timeToIncrement = _timeToIncrement;

        }
        else
        {
            timeToIncrement = _timeToIncrement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && currentLives > 0)
        //{
        //    SubtractLife();
        //}

        //if (Input.GetKeyDown(KeyCode.Backspace) && currentLives < 10)
        //{
        //    currentLives++;
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    timeToIncrement -= subtractTimeTest;
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    PlayerPrefs.DeleteKey("currentStars");
        //    currentStars = 0;
        //}

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
                PlayerPrefs.SetString("timeToIncrement", timeToIncrement.ToString());
                PlayerPrefs.SetInt("currentLives", currentLives);
                PlayerPrefs.SetInt("currentStars", currentStars);
                Application.Quit();
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToIncrement.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }

    private void OnApplicationPause()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToIncrement.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToIncrement.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }
    IEnumerator Timer()
    {
        
            while (countDown)
            {
                yield return new WaitForSeconds(1f);
                timeToIncrement -= subtractTime;
                if (timeToIncrement < subtractTime)
                {
                    timeToIncrement = _timeToIncrement;
                    currentLives++;
                }

                if (currentLives >= 10)
                {
                    timeToIncrement = _timeToIncrement;
                    countDown = false;
                }

            }

        

    }

    public void SubtractLife()
    {
        currentLives--;
        PlayerPrefs.SetInt("currentLives", currentLives);
        if(countDown == false)
        {
            countDown = true;
            StartCoroutine(Timer());
        }
        
    }
}

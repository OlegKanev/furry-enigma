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
    public  TimeSpan timeToSubtract = new TimeSpan(0, 10, 0);
    private TimeSpan _timeToSubtract = new TimeSpan(0, 10, 0);
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
            timeToSubtract = TimeSpan.Parse(PlayerPrefs.GetString("timeToIncrement"));
        
    }
    void Start()
    {
        if(exitTime != null)
         offlineTime = DateTime.Now - exitTime;
        else
         offlineTime = new TimeSpan(0, 0, 0);


        if (currentLives < 10)
        {
            for (; offlineTime >= _timeToSubtract && currentLives < 10; offlineTime -= _timeToSubtract)
            {
                currentLives++;

            }
            if(offlineTime < _timeToSubtract && currentLives < 10)
            {
                if (offlineTime < timeToSubtract)
                {
                    countDown = true;
                    timeToSubtract -= offlineTime;
                    StartCoroutine(Timer());
                }
                else if (offlineTime > timeToIncrement)
                {
                    countDown = true;
                    offlineTime -= timeToSubtract;
                    currentLives++;
                    timeToSubtract = _timeToSubtract - offlineTime;
                    StartCoroutine(Timer());
                }
                else if (offlineTime == timeToSubtract)
                {
                    currentLives++;
                    timeToSubtract = _timeToSubtract;
                    StartCoroutine(Timer());
                }
            }
            else if(currentLives >= 10)
                timeToSubtract = _timeToSubtract;

        }
        else
        {
            timeToSubtract = _timeToSubtract;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
                PlayerPrefs.SetString("timeToIncrement", timeToSubtract.ToString());
                PlayerPrefs.SetInt("currentLives", currentLives);
                PlayerPrefs.SetInt("currentStars", currentStars);
                Application.Quit();
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToSubtract.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }

    private void OnApplicationPause()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToSubtract.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("exitTime", DateTime.Now.ToString());
        PlayerPrefs.SetString("timeToIncrement", timeToSubtract.ToString());
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetInt("currentStars", currentStars);
    }
    IEnumerator Timer()
    {
        
            while (countDown)
            {
                yield return new WaitForSeconds(1f);
                timeToSubtract -= subtractTime;
                if (timeToSubtract < subtractTime)
                {
                    timeToSubtract = _timeToSubtract;
                    currentLives++;
                }

                if (currentLives >= 10)
                {
                    timeToSubtract = _timeToSubtract;
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

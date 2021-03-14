using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class AdsManager : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void ShowFullscreenAd();

    [DllImport("__Internal")]
    private static extern void ShowRewardedAd();

    public Hint hint;

    void Start()
    {
        ShowFullscreenAd();
    }

    public void ShowRewardedVideo()
    {
        ShowRewardedAd();
    }

        
    public void OnRewardedAdsDidFinish(string value)
    {
        
        if (value == "Success")
        {
            ShowHint();
        }
        else if (value == "Skipped")
        {
            // Do not reward the user for skipping the ad.
        }
        else if (value == "Failed")
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }


    public void ShowHint()
    {
        hint = GameObject.FindGameObjectWithTag("MovesLeft").GetComponent<Hint>();
        hint.pointer.SetActive(true);
        //adTools.SetActive(false);
    }

    
}

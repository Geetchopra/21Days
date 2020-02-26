using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to control the Radial Loading Bar.
/// Can be customized to fill in any amount of seconds and 
/// display as either time or percentage.
/// </summary>
public class RadialLoader : MonoBehaviour
{
    //Loader Attributes
    private static string textType;
    private static float fillTime;
    private static float currentTime;
    private static KeyCode keyCode;
    private static bool hold;

    //UI Components
    private static Text text;
    private static Image loader;

    //To control activation
    private static bool activated;

    /// <summary>
    /// Initialize UI objects and set activated to false. 
    /// </summary>
    void Awake()
    {
        text = GetComponentInChildren<Text>();
        loader = GetComponentInChildren<Image>();

        activated = false;
        hold = false;

        //To get rid of errors for Completed()
        fillTime = 100.0f;
    }

    /// <summary>
    /// Static method that initiates a Radial Loading Bar and initalizes all loader attributes
    /// for the animation. Can be directly called from other objects in the scene.
    /// </summary>
    /// <param name="time"> The amount of seconds to fill the bar in. </param>
    /// <param name="textIndicator"> Type of UI text indication - % or time. </param>
    /// <param name="holdKey"> If there is a key to be held in order to load the bar. </param>
    /// <param name="key"> The key to be held. KeyCode.Equals is just a placeholder default value. </param>
    public static void Launch(float time, string textIndicator, bool holdKey = false, KeyCode key = KeyCode.Equals)
    {
        fillTime = time;
        textType = textIndicator;

        currentTime = 0f;
        loader.fillAmount = 0;
        activated = true;

        if (holdKey)
        {
            keyCode = key;
            hold = holdKey;
        }
    }

    /// <summary>
    /// Update - Called once every frame.
    /// Check for activation and update text and image fill amount accordingly.
    /// </summary>
    void Update()
    {
        //If a key is to be held then stop the loader when it is not held anymore.
        if (hold)
        {
            if (!Input.GetKey(keyCode))
            {
                activated = false;
            }
        }

        if (activated)
        {
            SetVisibility(true);

            currentTime += Time.deltaTime;

            SetText();

            loader.fillAmount = (currentTime / fillTime);

            //Animation has finished?
            Completed();
        }
        else
        {
            //To get rid of errors for Completed()
            fillTime = 100.0f;

            SetVisibility(false);
        }
    }

    /// <summary>
    /// Check if the radial loader finished loading.
    /// Can be called by other objects to do something after the bar loads.
    /// </summary>
    /// <returns></returns>
    public static bool Completed()
    {
        if (currentTime >= fillTime)
        {
            SetVisibility(false);
            activated = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Helper function to set visibility of the UI objects.
    /// </summary>
    /// <param name="value"> Boolean value indicating if objects need to be visible or not. </param>
    private static void SetVisibility(bool value)
    {
        text.gameObject.SetActive(value);
        loader.gameObject.SetActive(value);
    }

    /// <summary>
    /// Helper function to set the UI text as per the textType.
    /// If text type is % then gives an ##% text, otherwise #.# seconds.
    /// </summary>
    private static void SetText()
    {
        if (textType == "time")
            text.text = currentTime.ToString("0.0");
        else
            text.text = ((currentTime / fillTime) * 100).ToString("00") + "%";
    }
}

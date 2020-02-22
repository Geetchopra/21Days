using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the time currency in the game. 
/// Contains mostly static methods so that they can be called from other objects.
/// Also contains functions to manipulate the time.
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static float seconds;
    public static int minutes;
    public static int hours;
    public static int days;
    private static Text time;

    /// <summary>
    /// Called before the first frame update.
    /// </summary>
    void Start()
    {
        ResetTime();
        time = GetComponent<Text>();
    }

    /// <summary>
    /// Called once every frame.
    /// Counts down the time and speeds it up if the player is doing something.
    /// </summary>
    void Update()
    {
        //Remove the text when the inventory pops up.
        if (Input.GetKey(KeyCode.N))
        {
            time.enabled = false;
        }
        else
        {
            time.enabled = true;
        }

        //Speed up the time when the player is walking.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            //Speed it up even more if the player is sprinting.
            if (Input.GetKey(KeyCode.LeftShift))
            {
                seconds -= Time.deltaTime * 40.0f;
            }
            else
            {
                seconds -= Time.deltaTime * 20.0f;
            }
        }
        else
        {
            seconds -= Time.deltaTime;
        }
        
        //Update values as per a real clock.
        if (seconds < 0f)
        {
            seconds = 59.99999f;
            minutes--;
        }
        if (minutes < 0)
        {
            minutes = 59;
            hours--;
        }
        if (hours < 0)
        {
            hours = 23;
            days--;
        }

        //Update the time visually.
        SetTime();
    }

    /// <summary>
    /// Set the time text on the UI.
    /// </summary>
    public static void SetTime()
    {
        time.text = days.ToString("00") + "d:" + hours.ToString("00") + "h:" + minutes.ToString("00") + "m:" + (Mathf.Floor(seconds)).ToString("00") + "s";
    }

    /// <summary>
    /// Reset the time to 21 days.
    /// </summary>
    public void ResetTime()
    {
        seconds = 0.99999f;
        minutes = 0;
        hours = 0;
        days = 21;
    }

    /// <summary>
    /// Convert the clock time into seconds.
    /// </summary>
    /// <returns> The amount of seconds as calculated. </returns>
    public static int JoinTime()
    {
        return (days * 86400) + (hours * 3600) + (minutes * 60) + Convert.ToInt32(seconds);
    } 

    /// <summary>
    /// Split time from seconds to clock time, i.e. hours, minutes, days, and seconds.
    /// </summary>
    /// <param name="secondsToConvert"> The amount of seconds to split the time from. </param>
    /// <returns> A tuple of days, hours, minutes and seconds respectively. </returns>
    public static Tuple<int, int, int, float> SplitTime(int secondsToConvert)
    {
        //Quick maths
        int d = secondsToConvert / 86400;
        int h = (secondsToConvert % 86400) / 3600;
        int m = (secondsToConvert % 3600) / 60;
        float s = secondsToConvert % 60;

        return new Tuple<int, int, int, float>(d, h, m, s);
    }

    /// <summary>
    /// Convert seconds to clock days, minutes, hours or seconds.
    /// </summary>
    /// <param name="time"> The amount of seconds to convert from. </param>
    /// <param name="toType"> Type of conversion, d for days, h for hours and so on. </param>
    /// <returns> The converted time. </returns>
    public static int ConvertSeconds(int time, char toType)
    {
        if (toType == 'd')
        {
            return time * 86400;
        }
        else if (toType == 'h')
        {
            return time * 3600;
        }
        else if (toType == 'm')
        {
            return time * 60;
        }
        else
        {
            return time;
        }
    }

    /// <summary>
    /// Change the current time and update the UI.
    /// </summary>
    /// <param name="timeAmount"> Amount to be changed by. </param>
    /// <param name="type"> Type of amount, d for days, h for hours and so on. </param>
    /// <param name="operation"> To check if time is to be added or subtracted. </param>
    public static void ChangeTime(int timeAmount, char type, string operation)
    {
        int current_time = JoinTime();

        //Convert time to only seconds and add to current time.
        if (operation == "add")
        {
            current_time += ConvertSeconds(timeAmount, type);
        }
        else
        {
            current_time -= ConvertSeconds(timeAmount, type);
        }

        //Get new time and update it accordingly.
        Tuple<int, int, int, float> new_time = SplitTime(current_time);

        days = new_time.Item1;
        hours = new_time.Item2;
        minutes = new_time.Item3;
        seconds = new_time.Item4;
        
        SetTime();
    }

    /// <summary>
    /// Parse a time letter into the correct word, i.e. s is for Seconds etc.
    /// Used mainly for UI texts.
    /// </summary>
    /// <param name="amount"> The amount of time. </param>
    /// <param name="type"> Type of amount, d for days, h for hours and so on. </param>
    /// <returns></returns>
    public static string ParseTime(int amount, char type)
    {
        string parsed;

        switch (type)
        {
            case 's': parsed = "Second";
                break;
            case 'm': parsed = "Minute";
                break;
            case 'h': parsed = "Hour";
                break;
            case 'd': parsed = "Day";
                break;
            default: throw new Exception("Wrong time type for parsing entered!");
        }

        //Check for plurals.
        if (amount > 1)
            return parsed + "s";
        else
            return parsed;
    }
}

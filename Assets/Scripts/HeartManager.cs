using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public int health = 3;
    public Image[] hearts; // Array of heart images

    // This method will be called from the GameManager
    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLives)
            {
                // Enable the heart if its index is less than the current lives
                hearts[i].enabled = true;
            }
            else
            {
                // Disable the heart if its index is greater than or equal to the current lives
                hearts[i].enabled = false;
            }
        }
    }
}

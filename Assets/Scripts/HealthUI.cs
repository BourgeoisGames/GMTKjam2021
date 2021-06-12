using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    // The rectangle representing the player's health
    public RectTransform healthBar;
    // The width of this rectangle when the player is at full health
    public float maxHealthWidth;

    public void set_health(float fraction)
    {
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealthWidth * fraction);
    }
}

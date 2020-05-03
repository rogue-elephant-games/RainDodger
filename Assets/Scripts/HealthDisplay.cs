using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI healthText;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthText != null)
        {
            if(player != null)
                healthText.text = player.GetHealth().ToString();
            else
                player = FindObjectOfType<Player>();
        }
        else
            healthText = GetComponent<TextMeshProUGUI>();
            
    }

}

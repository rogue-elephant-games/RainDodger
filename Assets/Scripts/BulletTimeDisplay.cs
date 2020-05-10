using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletTimeDisplay : MonoBehaviour
{
    TextMeshProUGUI bulletTimeText;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        bulletTimeText = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletTimeText != null)
        {
            if(player != null)
                bulletTimeText.text = player.GetBulletTimeRemaining().ToString();
            else
                player = FindObjectOfType<Player>();
        }
        else
            bulletTimeText = GetComponent<TextMeshProUGUI>();
            
    }
}

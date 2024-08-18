using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp_text;

    public void UpdatePlayerHealth(int value)
    {

        hp_text.text = $"{value} ";
        
    }

}

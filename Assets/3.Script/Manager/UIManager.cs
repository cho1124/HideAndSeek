using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp_text;

    public void UpdatePlayerHealth(int value)
    {

        hp_text.text = $"{value} ";
        
    }

    public void LoobyBack()
    {
        Debug.Log("로비 이동!");

        SceneManager.LoadScene("Lobby");
    }

}

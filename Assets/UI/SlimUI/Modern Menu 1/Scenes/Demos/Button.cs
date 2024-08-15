using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject server_room_obj = null;
    [SerializeField] private GameObject room_obj = null;

    private void Start()
    {
        room_obj.SetActive(true);
    }

    public void ReadyLoading()
    {
        Debug.Log("게임 시작");

        server_room_obj.SetActive(false);
    }

    public void BackLoading()
    {
        Debug.Log("로비 이동");

        SceneManager.LoadScene("Lobby");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorAM : MonoBehaviour
{
    [SerializeField] private Animator mouse_cursor_ar = null;
    [SerializeField] private Animator camera_ar = null;
    [SerializeField] private GameObject mouse_cursor_obj = null;

    public void ExitGame()
    {
        camera_ar.Play("CameraExitAM", 0, 0);

        StartCoroutine(Timer_IE());
    }

    IEnumerator Timer_IE()
    {
        yield return new WaitForSeconds(2);

        mouse_cursor_obj.SetActive(true);

        yield return new WaitForSeconds(2);

        UnityEditor.EditorApplication.isPlaying = false;
    }
}
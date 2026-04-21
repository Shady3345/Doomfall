using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public static bool isShowing = true;

    public GameObject titlePanel;

    void Start()
    {
        isShowing = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        titlePanel.SetActive(true);
    }

    public void OnPlay()
    {
        isShowing = false;
        titlePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
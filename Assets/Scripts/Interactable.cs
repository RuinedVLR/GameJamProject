using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message;

    public bool isShovelPresent;
    public bool isFlashlightPresent;
    public bool isCrowbarPresent;
    public static bool isReading;

    public UnityEvent onInteraction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    public void RunStart()
    {
        SceneManager.LoadScene("3_GamePlayScene");
    }

    public void PickShovel(GameObject shovel)
    {
        isShovelPresent = true;
        shovel.SetActive(false);
    }

    public void PickFlashlight(GameObject flashlight)
    {
        isFlashlightPresent = true;
        flashlight.SetActive(false);
    }

    public void PickCrowbar(GameObject crowbar)
    {
        isCrowbarPresent = true;
        crowbar.SetActive(false);
    }

    public void ReadNote(GameObject noteText)
    {
        noteText.SetActive(true);
        isReading = true;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}

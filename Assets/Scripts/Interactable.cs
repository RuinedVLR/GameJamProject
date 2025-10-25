using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message;
    public bool isShovelPresent;

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

}

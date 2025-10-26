using UnityEngine;

public class NoteScript : MonoBehaviour
{
    public void CloseNote(GameObject noteText)
    {
        Interactable.isReading = false;
        noteText.SetActive(false);
        UnityEngine.Cursor.visible = false;
    }
}

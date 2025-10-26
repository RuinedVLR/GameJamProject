using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Image = UnityEngine.UI.Image;

public class RobbingTrigger : MonoBehaviour
{
    public float robTimer = 3f;
    public float stayTimer = 0;
    public TextMeshProUGUI moneyCount;
    public int money = 0;
    private System.Random rand = new();
    [SerializeField] public Image waitBarSprite;

    // Update is called once per frame
    void Update()
    {
        moneyCount.text = "Money: $" + money.ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grave"))
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= robTimer)
            {
                stayTimer = 0;
                money += rand.Next(50, 501);
                other.gameObject.SetActive(false);
            }
        }
    }
}

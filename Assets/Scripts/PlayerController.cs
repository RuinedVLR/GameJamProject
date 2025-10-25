using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.5f;
    public float sensitivity = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Stamina")]
    [SerializeField] private float runStaminaConsumption = 2;
    [SerializeField] private float jumpStaminaConsumption = 4;
    [SerializeField] private float staminaRechargeRate = 2;
    [SerializeField] private float maxStamina = 10;
    private float stamina;

    //input bools
    bool jump = false;
    bool run = false;

    private bool isGrounded;
    private Vector3 velocity;
    private CharacterController controller;
    //Health
    public float maxHealth = 100;
    public float currentHealth; //take damage: currentHealth -= 20;
    [SerializeField] public HealthBar healthBar;//use healthBar.UpdateHealthBar(maxHealth, currentHealth); anytime the player takes damage

    void Start()
    {
        stamina = maxStamina;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y == 0)
        {
            velocity.y = 0;
        }

        jump = Input.GetButtonDown("Jump") && isGrounded && stamina > jumpStaminaConsumption;
        run = Input.GetButton("Sprint") && stamina > 0f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        float movespeed = GetMoveSpeed();
        controller.Move(move * speed * Time.deltaTime);

        if (jump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);

        HandleStamina();
    }

    //function for determining the movespeed of the character
    private float GetMoveSpeed()
    {
        float moveSpeed;

        moveSpeed = run ? speed * sprintMultiplier : speed;

        return moveSpeed;
    }

    private void HandleStamina()
    {
        if (!run && !jump && stamina <= maxStamina)
        {
            stamina += staminaRechargeRate * Time.deltaTime;
        }
        else
        {
            if (run) stamina -= runStaminaConsumption * Time.deltaTime;
            if (jump) stamina -= jumpStaminaConsumption;
        }

        Debug.Log(stamina);
    }
}

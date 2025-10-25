using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sensitivity = 2f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public float crouchSpeed;
    public float crouchHeight;
    public float normalHeight;
    public Vector3 offset;
    public Transform player;
    bool crouching;

    private bool isGrounded;
    private Vector3 velocity;
    private CharacterController controller;
    //Health
    public float maxHealth = 100;
    public float currentHealth; //take damage: currentHealth -= 20;
    [SerializeField] public HealthBar healthBar;//use healthBar.UpdateHealthBar(maxHealth, currentHealth); anytime the player takes damage

    void Start()
    {
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

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (!crouching)
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        else if (crouching)
        {
            controller.Move(move * (speed / 2) * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = !crouching;
        }
        if (crouching == true)
        {
            controller.height = controller.height - crouchSpeed * Time.deltaTime;
            if (controller.height <= crouchHeight)
            {
                controller.height = crouchHeight;
            }
        }
        if (crouching == false)
        {
            controller.height = controller.height + crouchSpeed * Time.deltaTime;
            if (controller.height < normalHeight)
            {
                player.gameObject.SetActive(false);
                player.position = player.position + offset * Time.deltaTime;
                player.gameObject.SetActive(true);
            }
            
            if (controller.height >= normalHeight)
            {
                controller.height = normalHeight;
            }
        }

    }
}

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public MenuSystem menuSystem;
    private Rigidbody rb;
    //Health
    public float maxHealth = 100;
    public float currentHealth; //take damage: currentHealth -= 20;
    [SerializeField] public HealthBar healthBar;//use healthBar.UpdateHealthBar(maxHealth, currentHealth); anytime the player takes damage

    void Start()
    {
        menuSystem = FindObjectOfType<MenuSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHealth -= 20;
            healthBar.UpdateHealthBar(maxHealth, currentHealth);
            //soundmanager.playsound(SoundType.HITSOUND);

            Rigidbody rb = GetComponent<Rigidbody>();
            if( currentHealth <= 0)
            {
                SceneManager.LoadSceneAsync(1);
                Debug.Log("You Died!");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Catcher"))
        {
            SceneManager.LoadSceneAsync(2);
        }
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

        if(!Interactable.isReading)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            transform.Rotate(Vector3.up * mouseX);
            Camera.main.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
        }

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
        //pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuSystem.PauseGame();
        }
        else //unpause game
        {
            menuSystem.PauseEscape();
        }
    }
}

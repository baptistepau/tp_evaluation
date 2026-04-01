using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Souris / Cam�ra")]
    [SerializeField, Tooltip("Sensibilit� de la souris")]
    private float mouseSensitivity = 2f;
    [SerializeField, Tooltip("Limite du pitch (deg)")]
    private float pitchLimit = 80f;
    [SerializeField, Tooltip("R�f�rence � la cam�ra (laisser vide pour utiliser MainCamera)")]
    private Transform cameraTransform;

    private CharacterController controller;
    private float pitch = 0f;
    private Vector3 verticalVelocity;

    void Start()
    {
        //Ces deux lignes permettent de cacher le curseur et de le verrouiller au centre de l'�cran.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotation horizontale du joueur (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Rotation verticale de la caméra (Pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        // Mouvement relatif à la direction du joueur
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}

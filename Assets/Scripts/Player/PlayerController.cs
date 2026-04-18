using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float acceleration = 10f;
    public float deceleration = 8f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 0.1f;
    public Transform playerCamera;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentMoveVelocity;
    private float xRotation = 0f;
    private bool canMove = true;

    [Header("Map Boundaries")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        xRotation = 0f;
        playerCamera.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        Move();
        MouseLook();
    }

    void Move()
    {
        if (!canMove) return;

        float x = Keyboard.current.aKey.isPressed ? -1 :
                  Keyboard.current.dKey.isPressed ? 1 : 0;
        float z = Keyboard.current.sKey.isPressed ? -1 :
                  Keyboard.current.wKey.isPressed ? 1 : 0;

        Vector3 input = (transform.right * x + transform.forward * z).normalized;
        Vector3 targetVelocity = input * moveSpeed;

        if (input.magnitude > 0)
            currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetVelocity, acceleration * Time.deltaTime);
        else
            currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, Vector3.zero, deceleration * Time.deltaTime);

        controller.Move(currentMoveVelocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        EnforceBoundaries();
    }

    void EnforceBoundaries()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        if (transform.position.x != pos.x || transform.position.z != pos.z)
        {
            controller.enabled = false;
            transform.position = pos;
            controller.enabled = true;
        }
    }

    void MouseLook()
    {
        if (!canMove) return;

        Vector2 mouse = Mouse.current.delta.ReadValue();
        float mouseX = mouse.x * mouseSensitivity * Time.deltaTime * 100f;
        float mouseY = mouse.y * mouseSensitivity * Time.deltaTime * 100f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void SetCanMove(bool state)
    {
        canMove = state;
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !state;
    }
}
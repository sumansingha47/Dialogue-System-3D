using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public static MouseLook instance;

    [Header("Settings")]
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor = true;
    [Space]
    public Vector2 sensitivity = new Vector2(2, 2);
    [Space]
    public Vector2 smoothing = new Vector2(3, 3);

    [Header("First Person")]
    public GameObject characterBody;

    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;

    private Vector2 _mouseAbsolute;
    private Vector2 _smoothMouse;

    public Vector3 cameraLookAtOffset;

    [HideInInspector]
    public bool scoped;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        targetDirection = transform.localRotation.eulerAngles;

        if (characterBody)
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;

        if (lockCursor)
            UpdateCursorLockState();
    }

    void Update()
    {
        UpdateCursorLockState();

        if (DialogueManager.isActive || GameManager.instance.isPaused)
            return;

        HandleMouseLook();
    }

    private void UpdateCursorLockState()
    {
        if (DialogueManager.isActive || GameManager.instance.isPaused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void HandleMouseLook()
    {
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        _mouseAbsolute += _smoothMouse;

        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        if (characterBody)
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }
        else
        {
            var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= yRotation;
        }
    }
}

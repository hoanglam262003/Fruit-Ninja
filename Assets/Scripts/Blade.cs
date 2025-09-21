using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;
    private Collider bladeCollider;
    private TrailRenderer bladeTrail;
    private bool isSlicing;

    public Vector3 direction { get; private set; }
    public float sliceForce = 5f;
    public float minVelocity = 0.01f;

    private void Awake()
    {
        bladeCollider = GetComponent<Collider>();
        mainCamera = Camera.main;
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSlicing();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopSlicing();
        }
        else if (isSlicing)
        {
            ContinueSlicing();
        }
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), mainCamera.nearClipPlane));
        newPosition.z = 0f;
        transform.position = newPosition;
        isSlicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        isSlicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), mainCamera.nearClipPlane));
        newPosition.z = 0f;

        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minVelocity;

        transform.position = newPosition;
    }
}

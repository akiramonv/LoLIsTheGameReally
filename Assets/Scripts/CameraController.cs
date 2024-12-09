using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // ������ �� ������
    [SerializeField] private float speedCam = 3f; // �������� �������� ������
    [SerializeField] private float jumpZoomOut = 5f; // ��������� ������ ���������� ��� ������
    [SerializeField] private float zoomSpeed = 2f; // �������� ��������� �������� ������
    [SerializeField] private LayerMask groundLayer; // ���� ����� ��� �������� �������� � ������

    private Vector3 pos;
    private Camera mainCamera; // ������ �� ������
    private float defaultSize; // ����������� �������� �������� ������
    private Rigidbody2D playerRb; // Rigidbody ������ ��� ����������� ������

    private void Awake()
    {
        if (!player) player = FindObjectOfType<Hero>().transform;

        mainCamera = Camera.main; // �������� �������� ������
        if (mainCamera != null) defaultSize = mainCamera.orthographicSize; // ��������� ����������� ������ ������

        playerRb = player.GetComponent<Rigidbody2D>(); // �������� Rigidbody ������
    }

    private void Update()
    {
        // ����������� ������ �� �������
        pos = player.position;
        pos.z = -10f;
        pos.y += 3f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speedCam);

        // ���������, ��������� �� ����� � �������
        bool isInAir = !IsGrounded();
        float targetSize = isInAir ? defaultSize + jumpZoomOut : defaultSize;

        // ������ �������� ������� ������
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }

    // ��������, ��������� �� ����� �� �����
    private bool IsGrounded()
    {
        float extraHeight = 0.1f; // ���������� �����������
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null; // ���������� true, ���� ���� ������� � ������
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // ��������� ����� �� ��������
    }
}

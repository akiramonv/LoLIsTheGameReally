using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; // Ссылка на игрока
    [SerializeField] private float speedCam = 3f; // Скорость движения камеры
    [SerializeField] private float jumpZoomOut = 5f; // Насколько камера отдаляется при прыжке
    [SerializeField] private float zoomSpeed = 2f; // Скорость изменения масштаба камеры
    [SerializeField] private LayerMask groundLayer; // Слой земли для проверки контакта с землей

    private Vector3 pos;
    private Camera mainCamera; // Ссылка на камеру
    private float defaultSize; // Стандартное значение масштаба камеры
    private Rigidbody2D playerRb; // Rigidbody игрока для определения прыжка

    private void Awake()
    {
        if (!player) player = FindObjectOfType<Hero>().transform;

        mainCamera = Camera.main; // Получаем основную камеру
        if (mainCamera != null) defaultSize = mainCamera.orthographicSize; // Сохраняем изначальный размер камеры

        playerRb = player.GetComponent<Rigidbody2D>(); // Получаем Rigidbody игрока
    }

    private void Update()
    {
        // Перемещение камеры за игроком
        pos = player.position;
        pos.z = -10f;
        pos.y += 3f;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speedCam);

        // Проверяем, находится ли игрок в воздухе
        bool isInAir = !IsGrounded();
        float targetSize = isInAir ? defaultSize + jumpZoomOut : defaultSize;

        // Плавно изменяем масштаб камеры
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        }
    }

    // Проверка, находится ли игрок на земле
    private bool IsGrounded()
    {
        float extraHeight = 0.1f; // Допустимая погрешность
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, extraHeight, groundLayer);
        return hit.collider != null; // Возвращает true, если есть контакт с землей
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // Загружаем сцену по названию
    }
}

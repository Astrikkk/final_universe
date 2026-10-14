using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float nearSize = 5f;
    [SerializeField] private float farSize = 7f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float idleDelay = 2f;

    private Camera cam;
    private Vector3 lastTargetPos;
    private float idleTimer;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        lastTargetPos = target.position;

        // Мгновенно ставим камеру на игрока при старте
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
    }

    private void LateUpdate()
    {
        // Слежение за игроком
        Vector3 targetPos = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            followSpeed * Time.deltaTime
        );

        // Определяем, движется ли игрок
        float deltaX = Mathf.Abs(target.position.x - lastTargetPos.x);
        lastTargetPos = target.position;

        if (deltaX > 0.001f)
            idleTimer = 0f;
        else
            idleTimer += Time.deltaTime;

        // Определяем целевой размер камеры
        float targetSize = (idleTimer >= idleDelay) ? nearSize : farSize;

        // Плавный зум
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetSize,
            zoomSpeed * Time.deltaTime
        );
    }
}
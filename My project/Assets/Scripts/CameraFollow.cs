using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float nearSize = 4f;
    [SerializeField] private float farSize = 7f;
    [SerializeField] private float distanceForFarSize = 6f;
    [SerializeField] private float zoomSpeed = 2f;

    private Camera cam;
    private Transform player2;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        FindPlayer2();

        Vector3 targetPos = GetTargetPosition();
        transform.position = new Vector3(
            targetPos.x,
            targetPos.y,
            transform.position.z
        );
        cam.orthographicSize = nearSize;
    }

    private void LateUpdate()
    {
        if (player1 == null) return;

        FindPlayer2();

        Vector3 targetPos = GetTargetPosition();
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(targetPos.x, targetPos.y, transform.position.z),
            followSpeed * Time.deltaTime
        );

        float targetSize = GetTargetSize();
        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetSize,
            zoomSpeed * Time.deltaTime
        );
    }

    private void FindPlayer2()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        player2 = null;
        foreach (GameObject p in players)
        {
            if (p.transform != player1)
            {
                player2 = p.transform;
                break;
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        if (player2 == null)
            return player1.position;

        return (player1.position + player2.position) / 2f;
    }

    private float GetTargetSize()
    {
        if (player2 == null)
            return nearSize;

        float distance = Vector2.Distance(player1.position, player2.position);
        float t = Mathf.Clamp01(distance / distanceForFarSize);
        return Mathf.Lerp(nearSize, farSize, t);
    }
}
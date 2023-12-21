using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake() => instance = this;

    private Vector3 pivot = Vector3.zero;

    private const float panPercentage = 0.4f;
    private const float panSpeed = 45f; // deg/s

    public float angle = 0;
    private float viewPortSize = 5f;

    private Camera mainCamera;

    private void Start() => mainCamera = gameObject.GetComponent<Camera>();

    private void Update()
    {
        CameraPan();
        Zoom();
    }

    public void UpdatePivot(Vector2Int newPivot)
    {
        StopAllCoroutines();
        StartCoroutine(LerpPivot(newPivot));
    }

    private IEnumerator LerpPivot(Vector2Int newPivot)
    {
        Vector3 originalPivot = pivot;

        float timeScale = 0;
        while(timeScale < 1f)
        {
            timeScale += Time.deltaTime * 0.25f;
            TransformPivot(Vector3.Lerp(originalPivot, Helper.Vec2toVec3(newPivot), Mathf.SmoothStep(0, 1f, timeScale)));
            yield return null;
        }
    }
    

    private void TransformPivot(Vector3 newPivot)
    {
        transform.position += newPivot - pivot;
        pivot = newPivot;
    }

    private void CameraPan()
    {
        if (Mathf.Abs(Input.mousePosition.x / Screen.width - 0.5f) > panPercentage)
        {
            float angleStep = panSpeed * ((Input.mousePosition.x / Screen.width) > 0.5f ? -1 : 1) * Time.deltaTime;
            angle += angleStep;
            transform.RotateAround(pivot, Vector3.up, angleStep);
        }
    }

    private void Zoom()
    {
        if (Input.mouseScrollDelta.y == 0) return;
        viewPortSize = Mathf.Clamp(viewPortSize - Input.mouseScrollDelta.y * Time.deltaTime * 15f, 4f, 15f);
        mainCamera.orthographicSize = viewPortSize;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Only have things regarding the mouse, do not reference external classes in this class
/// </summary>
public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    private void Awake() => instance = this;

    public Camera mainCamera;
    public Vector2 GetMousePosition() => mousePosition;

    public Vector2Int GetMousePositionSnapped()
    {
        Vector2 position = GetMousePosition();
        return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }

    public bool HasMouseMoved() => !(Helper.RoundVec2(mousePosition) == Helper.RoundVec2(lastMousePosition));

    private Vector2 mousePosition, lastMousePosition;

    private void Update()
    {
        lastMousePosition = mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        float distance = 0;
        if (hPlane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            mousePosition =  new Vector2(point.x, point.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    public Camera mainCamera;

    public Transform snail;
    public Transform dragonFly;

    private void Start()
    {
        StartCoroutine(SpawnCritters());
    }

    private IEnumerator SpawnCritters()
    {
        if(!snail.gameObject.activeInHierarchy)
        {
            if (Random.Range(0, 1f) < 0.5f) StartCoroutine(SpawnSnail());
        }
        if (!dragonFly.gameObject.activeInHierarchy)
        {
            if (Random.Range(0, 1f) < 0.75f) StartCoroutine(SpawnDragonfly());
        }

        yield return new WaitForSeconds(Random.Range(10f, 30f));
        StartCoroutine(SpawnCritters());
    }

    private IEnumerator SpawnSnail()
    {
        snail.gameObject.SetActive(true);
        Renderer snailRenderer = snail.GetComponentInChildren<Renderer>();

        Vector3 startPosition = Helper.NormalVec2toVec3(GetPointFromScreen(new Vector2(1.1f, Random.Range(0, 1f))));
        Vector3 endPosition = Helper.NormalVec2toVec3(GetPointFromScreen(new Vector2(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f))));
        Vector3 direction = (endPosition - startPosition).normalized;

        snail.position = startPosition;
        snail.LookAt(endPosition,Vector3.up);

        while(Vector3.Distance(snail.position, endPosition) > 0.5f)
        {
            snail.position += 0.1f * Time.deltaTime * direction;
            yield return null;
        }
        while (snailRenderer.isVisible)
        {
            snail.position += 0.1f * Time.deltaTime * direction;
            yield return null;
        }

        snail.gameObject.SetActive(false);
    }

    private IEnumerator SpawnDragonfly()
    {
        dragonFly.gameObject.SetActive(true);
        Renderer dragonflyRenderer = dragonFly.GetComponentInChildren<Renderer>();

        Vector3 startPosition = Helper.NormalVec2toVec3(GetPointFromScreen(new Vector2(1.1f, Random.Range(0, 1f))));
        Vector3 endPosition = Helper.NormalVec2toVec3(GetPointFromScreen(new Vector2(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f))));
        Vector3 direction = (endPosition - startPosition).normalized;
        direction = new Vector3(direction.x, 0, direction.z);

        dragonFly.position = startPosition;
        dragonFly.LookAt(endPosition, Vector3.up);

        while (Vector3.Distance(dragonFly.position, endPosition) > 2f)
        {
            dragonFly.position += 4f * Time.deltaTime * direction;
            yield return null;
            dragonFly.position = new Vector3(dragonFly.position.x, startPosition.y + 1.5f * Mathf.Sin(Time.time), dragonFly.position.z);
        }
        while (dragonflyRenderer.isVisible)
        {
            dragonFly.position += 4f * Time.deltaTime * direction;
            yield return null;
            dragonFly.position = new Vector3(dragonFly.position.x, startPosition.y + 1.5f * Mathf.Sin(Time.time), dragonFly.position.z);
        }

        dragonFly.gameObject.SetActive(false);
    }


    private Vector2 GetPointFromScreen(Vector2 position)
    {
        Ray ray = mainCamera.ViewportPointToRay(position);
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        float distance = 0;
        if (hPlane.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            return new Vector2(point.x, point.z);
        }
        else return Vector2.zero;
    }
}

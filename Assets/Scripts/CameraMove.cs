using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float duration;
    public Transform target;
    public float targetFOV;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void Transition()
    {
        StartCoroutine(Move());
    }

    public IEnumerator Move()
    {
        float t = 0.0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float startFOV = cam.fieldOfView;
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / duration);

            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            yield return 0;
        }
    }
}

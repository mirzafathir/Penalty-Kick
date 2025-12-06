using System.Collections;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [Header("Cameras")]
    public Camera cutsceneCamera;
    public Camera gameplayCamera;

    [Header("Waypoints (urutan perjalanan kamera)")]
    public Transform[] waypoints;

    [Header("Timing")]
    public float moveDurationPerSegment = 2f; // lama perpindahan antar point
    public float waitAtEachPoint = 0.5f;      // pause sebentar di tiap titik

    [Header("Options")]
    public bool playOnStart = true;           // otomatis main saat Start

    private bool isPlaying = false;

    private void Start()
    {
        if (playOnStart)
        {
            PlayCutscene();
        }
        else
        {
            // default: matikan cutscene camera
            if (cutsceneCamera != null) cutsceneCamera.enabled = false;
        }
    }

    public void PlayCutscene()
    {
        if (isPlaying) return;
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("CameraCutscene: Waypoints belum diisi.");
            return;
        }

        StartCoroutine(CutsceneRoutine());
    }

    private IEnumerator CutsceneRoutine()
    {
        isPlaying = true;

        // matikan kamera gameplay
        if (gameplayCamera != null) gameplayCamera.enabled = false;

        // nyalakan kamera cutscene
        if (cutsceneCamera != null) cutsceneCamera.enabled = true;

        // letakkan kamera cutscene di waypoint pertama
        Transform camTransform = cutsceneCamera.transform;
        camTransform.position = waypoints[0].position;
        camTransform.rotation = waypoints[0].rotation;

        // jalan dari waypoint 0 -> 1 -> 2 -> ...
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Transform from = waypoints[i];
            Transform to = waypoints[i + 1];

            // mulai dari posisi/rotasi saat ini
            Vector3 startPos = from.position;
            Quaternion startRot = from.rotation;

            float t = 0f;
            while (t < moveDurationPerSegment)
            {
                t += Time.deltaTime;
                float lerp = Mathf.Clamp01(t / moveDurationPerSegment);

                camTransform.position = Vector3.Lerp(startPos, to.position, lerp);
                camTransform.rotation = Quaternion.Slerp(startRot, to.rotation, lerp);

                yield return null;
            }

            // pastikan sampai tepat di titik tujuan
            camTransform.position = to.position;
            camTransform.rotation = to.rotation;

            // tunggu sebentar di titik ini
            if (waitAtEachPoint > 0f)
                yield return new WaitForSeconds(waitAtEachPoint);
        }

        // selesai cutscene -> nyalakan kembali kamera gameplay
        if (cutsceneCamera != null) cutsceneCamera.enabled = false;
        if (gameplayCamera != null) gameplayCamera.enabled = true;

        isPlaying = false;
    }
}

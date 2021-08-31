using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class EditedPlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                // spawnedObject.transform.position = hitPose.position + hitPose.up*0.1f;
                spawnedObject.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);
                spawnedObject.transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
                spawnedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                StartCoroutine(ShowPortal());

                // Destroy prefabs to save computations
                Destroy(m_PlacedPrefab);

            }

        }
    }

    private IEnumerator ShowPortal()
    {
        yield return new WaitForSeconds(10.0f);
        // Get PortalCanvas object
        GameObject correctPortalCanvas = null;
        GameObject[] portalCanvasList = GameObject.FindGameObjectsWithTag("PortalCanvas");
        foreach (GameObject potentialPortalCanvas in portalCanvasList)
        {
            if (potentialPortalCanvas.transform.IsChildOf(spawnedObject.transform))
            {
                correctPortalCanvas = potentialPortalCanvas;
                break;
            }
        }
        // Turn on Mesh Renderer and play Audio 
        MeshRenderer meshRenderer = correctPortalCanvas.GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;
        AudioSource audioData = correctPortalCanvas.GetComponent<AudioSource>();
        audioData.Play(0);
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementSystem : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private GameObject[] previewPrefabs;
    [SerializeField] private float[] constructionTimes;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyPathLayer;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private KeyCode openMenuKey = KeyCode.X;
    [SerializeField] private float buildRange = 10f;
    [SerializeField] private float minDistanceFromPlayer = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject towerMenuUI;
    [SerializeField] private Button[] towerButtons;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minDistanceBetweenTowers = 3f;
    private float currentRotation = 0f;

    private GameObject currentPreview;
    private int selectedTowerIndex = -1;
    private bool isPlacingTower = false;

    #endregion

    #region Unity Callbacks

    void Start()
    {
        InitializeTowerMenu();
    }

    void Update()
    {
        HandleInput();
    }

    #endregion

    #region Menu Handling

    void InitializeTowerMenu()
    {
        towerMenuUI.SetActive(false);

        for (int i = 0; i < towerButtons.Length; i++)
        {
            int index = i;
            towerButtons[i].onClick.AddListener(() => SelectTower(index));
        }
    }

    void ToggleTowerMenu()
    {
        bool isActive = towerMenuUI.activeSelf;
        towerMenuUI.SetActive(!isActive);

        if (towerMenuUI.activeSelf)
        {
            isPlacingTower = false;
            if (currentPreview != null)
            {
                Destroy(currentPreview);
                currentPreview = null;
            }
        }
    }

    void SelectTower(int index)
    {
        selectedTowerIndex = index;
        isPlacingTower = true;

        towerMenuUI.SetActive(false);

        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        currentPreview = Instantiate(previewPrefabs[selectedTowerIndex]);
    }

    #endregion

    #region Tower Placement

    void HandleInput()
    {
        if (Input.GetKeyDown(openMenuKey))
        {
            ToggleTowerMenu();
        }

        if (isPlacingTower && selectedTowerIndex != -1)
        {
            PreviewTowerPlacement();

            if (Input.GetMouseButtonDown(0) && currentPreview != null)
            {
                PlaceTower();
            }

            if (Input.GetKeyDown(openMenuKey))
            {
                CancelPlacement();
            }
        }
    }

    void PreviewTowerPlacement()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer | enemyPathLayer))
        {
            Vector3 placementPosition = hit.point;

            float distanceFromPlayer = Vector3.Distance(player.position, placementPosition);
            if (distanceFromPlayer <= buildRange && distanceFromPlayer >= minDistanceFromPlayer)
            {
                if (!IsThereATowerNearby(placementPosition, minDistanceBetweenTowers))
                {
                    if (currentPreview != null)
                    {
                        currentPreview.SetActive(true);
                        currentPreview.transform.position = placementPosition;

                        RaycastHit groundHit;
                        if (Physics.Raycast(placementPosition + Vector3.up * 10, Vector3.down, out groundHit, Mathf.Infinity, groundLayer | enemyPathLayer))
                        {
                            placementPosition.y = groundHit.point.y;
                            float previewHeightOffset = 1.0f;
                            placementPosition.y += previewHeightOffset;
                            currentPreview.transform.position = placementPosition;
                        }

                        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
                        if (scrollInput != 0f)
                        {
                            currentRotation += scrollInput * rotationSpeed;
                            currentPreview.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
                        }
                    }
                }
                else
                {
                    if (currentPreview != null)
                        currentPreview.SetActive(false);
                }
            }
            else
            {
                if (currentPreview != null)
                    currentPreview.SetActive(false);
            }
        }
        else
        {
            if (currentPreview != null)
                currentPreview.SetActive(false);
        }
    }

    bool IsThereATowerNearby(Vector3 position, float minDistance)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, minDistance, towerLayer);
        return hitColliders.Length > 0;
    }

    void PlaceTower()
    {
        if (currentPreview != null && currentPreview.activeSelf)
        {
            RaycastHit hit;
            Vector3 towerPosition = currentPreview.transform.position;

            if (Physics.Raycast(towerPosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                towerPosition.y = hit.point.y;

                float towerHeightOffset = 1.0f;
                towerPosition.y += towerHeightOffset;

                Quaternion towerRotation = currentPreview.transform.rotation;

                StartCoroutine(BuildTowerWithDelay(selectedTowerIndex, towerPosition, towerRotation));

                Destroy(currentPreview);
                currentPreview = null;
                selectedTowerIndex = -1;
                isPlacingTower = false;
            }
        }
    }
    IEnumerator BuildTowerWithDelay(int towerIndex, Vector3 position, Quaternion rotation)
    {
        float constructionTime = constructionTimes[towerIndex];

        //nota para felipe, aqui pones la animacion, si sale mal eres estupido

        yield return new WaitForSeconds(constructionTime);

        Instantiate(towerPrefabs[towerIndex], position, rotation);
    }


    #endregion

    #region Utility

    void CancelPlacement()
    {
        if (currentPreview != null) Destroy(currentPreview);
        currentPreview = null;
        selectedTowerIndex = -1;
        isPlacingTower = false;
    }

    #endregion
}
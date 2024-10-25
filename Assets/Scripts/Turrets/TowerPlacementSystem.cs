using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementSystem : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] public GameObject[] towerPrefabs;
    [SerializeField] public GameObject[] previewPrefabs;
    [SerializeField] public float[] constructionTimes;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask enemyPathLayer;
    [SerializeField] public LayerMask towerLayer;
    [SerializeField] public KeyCode openMenuKey = KeyCode.X;
    [SerializeField] public float buildRange = 10f;
    [SerializeField] public float minDistanceFromPlayer = 2f;
    [SerializeField] public Transform player;
    [SerializeField] public GameObject towerMenuUI;
    [SerializeField] public Button[] towerButtons;
    [SerializeField] public float rotationSpeed = 10f;
    [SerializeField] public float minDistanceBetweenTowers = 3f;
    [SerializeField] public float currentRotation = 0f;

    private GameObject currentPreview;
    private int selectedTowerIndex = -1;
    private bool isPlacingTower = false;
    [SerializeField] private int[] towerCosts;

    #endregion

    #region Unity Callbacks

    public Transform centerPoint;
    public float radius = 200f;

    void Start()
    {
        InitializeTowerMenu();
        float angleStep = 360f / towerButtons.Length;
        for (int i = 0; i < towerButtons.Length; i++)
        {
            float angle = i * angleStep;
            float xPos = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float yPos = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            towerButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
        }
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

            TextMeshProUGUI buttonText = towerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Torre " + (i + 1) + " - " + towerCosts[i] + " monedas";
        }
    }

    void ToggleTowerMenu()
    {
        bool isActive = towerMenuUI.activeSelf;
        towerMenuUI.SetActive(!isActive);

        if (!isActive)
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
        if (GameManager.instance.HasEnoughCoins(towerCosts[index]))
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
        else
        {
            Debug.Log("No tienes dinero eres pobre");
        }
    }



    #endregion

    #region Tower Placement

    void HandleInput()
    {
        if (Input.GetKeyDown(openMenuKey))
        {
            ToggleTowerMenu();
        }
        if (towerMenuUI.activeSelf)
        {
            HandleWheelSelection();
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

    void HandleWheelSelection()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = mousePosition - center;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        if (angle >= 0 && angle < 90)
        {
            HighlightTowerButton(0);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(0);
            }
        }
        else if (angle >= 90 && angle < 180)
        {
            HighlightTowerButton(1);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(1);
            }
        }
        else if (angle >= 180 && angle < 270)
        {
            HighlightTowerButton(2);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(2);
            }
        }
        /*else if (angle >= 270 && angle < 360)
        {
            HighlightTowerButton(3);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(3);
            }
        }*/
    }

    void HighlightTowerButton(int index)
    {
        //Color o lo que sea
    }

    [SerializeField] private Image[] towerImages;
    [SerializeField] private TextMeshProUGUI[] costTexts;

    void UpdateTowerMenu()
    {
        for (int i = 0; i < towerButtons.Length; i++)
        {
            int playerCoins = GameManager.instance.playerCoins;

            if (playerCoins >= towerCosts[i])
            {
                towerButtons[i].interactable = true;
                towerButtons[i].GetComponent<CanvasGroup>().alpha = 1f;
            }
            else
            {
                towerButtons[i].interactable = false;
                towerButtons[i].GetComponent<CanvasGroup>().alpha = 0.5f;
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
            bool isValidPosition = distanceFromPlayer <= buildRange &&
                                   distanceFromPlayer >= minDistanceFromPlayer &&
                                   !IsThereATowerNearby(placementPosition, minDistanceBetweenTowers);
            if (Physics.Raycast(placementPosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                placementPosition.y = hit.point.y;
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
            SetPreviewColor(isValidPosition ? Color.green : Color.red);
            currentPreview.SetActive(true);
        }
        else
        {
            if (currentPreview != null)
                currentPreview.SetActive(false);
        }
    }

    void SetPreviewColor(Color color)
    {
        Renderer[] renderers = currentPreview.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
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
            Renderer previewRenderer = currentPreview.GetComponentInChildren<Renderer>();
            if (previewRenderer != null && previewRenderer.material.color == Color.green)
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

                    GameManager.instance.SpendCoins(towerCosts[selectedTowerIndex]);

                    Destroy(currentPreview);
                    currentPreview = null;
                    selectedTowerIndex = -1;
                    isPlacingTower = false;
                }
            }
            else
            {
                Debug.Log("No sirve, porque aquí no se puede");
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

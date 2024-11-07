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
    public float currentRotation = 0f;

    private GameObject currentPreview;
    private int selectedTowerIndex = -1;
    private bool isPlacingTower = false;
    [SerializeField] private int[] towerCosts;
    public Transform centerPoint;
    public float radius = 250f;

    #endregion

    #region Unity Callbacks


    void Start()
    {
        InitializeTowerMenu();
        if (towerButtons.Length >= 3)
        {
            towerButtons[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(1f, 280f);
            towerButtons[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-300f, -230f);
            towerButtons[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(300f, -230f);
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
            GameManager.instance.ChangeState(GameState.Paused);
        }
        else
        {
            GameManager.instance.ChangeState(GameState.Playing);
        }
    }

    void SelectTower(int index)
    {
        if (GameManager.instance.HasEnoughCoins(towerCosts[index]))
        {
            selectedTowerIndex = index;
            isPlacingTower = true;
            towerMenuUI.SetActive(false);

            CreateTowerPreview();
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
            if (currentPreview != null)
            {
                PreviewTowerPlacement();
            }

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

    void HighlightTowerImage(int index)
    {
        for (int i = 0; i < towerImages.Length; i++)
        {
            towerImages[i].rectTransform.localScale = Vector3.one;
            towerButtons[i].transform.localScale = Vector3.one;

            if (i == index)
            {
                towerImages[i].color = new Color(0.7f, 0f, 1f, 0.5f);
            }
            else
            {
                towerImages[i].color = Color.white;
            }
        }
    }

    void HandleWheelSelection()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 center = towerMenuUI.transform.position;
        Vector2 direction = mousePosition - center;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 45;
        if (angle < 0) angle += 360;

        if (angle >= 45 && angle < 135)
        {
            HighlightTowerImage(0);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(0);
            }
        }
        else if (angle >= 135 && angle < 225)
        {
            HighlightTowerImage(1);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(1);
            }
        }
        else if (angle >= 225 && angle < 315)
        {
            HighlightTowerImage(2);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(2);
            }
        }
        /*else if (angle >= 270 && angle < 360) // Cuarta torre
        {
            HighlightTowerButton(3);
            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(3);
            }
        }*/
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

    void CreateTowerPreview()
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }
        StartCoroutine(CreatePreviewCoroutine());
    }

    IEnumerator CreatePreviewCoroutine()
    {
        yield return null;

        currentPreview = Instantiate(previewPrefabs[selectedTowerIndex]);
        currentPreview.SetActive(true);
    }

    void PreviewTowerPlacement()
    {
        if (currentPreview == null) return;

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
                float previewHeightOffset = 0f;
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
                    float towerHeightOffset = 0f;
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
                Debug.Log("Esta rojo no se puede hombre");
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

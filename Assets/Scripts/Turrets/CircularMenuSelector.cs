using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuSelector : MonoBehaviour
{
    public GameObject[] towers;
    public Transform center;
    public float radius = 100f;
    public RectTransform menuWheel;
    public Transform player;
    public float maxRadius = 100f;

    private int currentSelectedTowerIndex = 0;
    private Vector2 wheelCenter;

    void Start()
    {
        wheelCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, menuWheel.position);
        HighlightSelectedTower(currentSelectedTowerIndex);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            menuWheel.gameObject.SetActive(true);
            Vector2 mousePos = Input.mousePosition;
            Vector2 direction = mousePos - wheelCenter;
            float distanceFromCenter = direction.magnitude;

            if (distanceFromCenter > maxRadius)
            {
                direction = direction.normalized * maxRadius;
                mousePos = wheelCenter + direction;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            if (angle >= 0 && angle < 90) currentSelectedTowerIndex = 0;
            else if (angle >= 90 && angle < 180) currentSelectedTowerIndex = 1;
            else if (angle >= 180 && angle < 270) currentSelectedTowerIndex = 2;
            else currentSelectedTowerIndex = 3;

            HighlightSelectedTower(currentSelectedTowerIndex);

            if (Input.GetMouseButtonDown(0))
            {
                SelectTower(currentSelectedTowerIndex);
                CloseWheelMenu();
            }
        }
        else
        {
            CloseWheelMenu();
        }
    }

    void SelectTower(int index)
    {
        Debug.Log("Torre seleccionada: " + towers[index].name);
    }

    void HighlightSelectedTower(int index)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            if (i == index)
            {
                towers[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
            else
            {
                towers[i].transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    void CloseWheelMenu()
    {
        menuWheel.gameObject.SetActive(false);
    }
}
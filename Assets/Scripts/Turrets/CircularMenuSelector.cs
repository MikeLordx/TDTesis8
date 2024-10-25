using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuSelector : MonoBehaviour
{
    public GameObject[] towers;  // Array con las torres
    public Transform center;     // Centro del c�rculo
    public float radius = 100f;  // Radio del c�rculo
    public RectTransform menuWheel;  // La UI del c�rculo
    public Transform player;     // Referencia al jugador
    public float maxRadius = 100f;  // Radio m�ximo permitido para mover el mouse

    private int currentSelectedTowerIndex = 0; // �ndice de la torre seleccionada
    private Vector2 wheelCenter;  // Centro del c�rculo en coordenadas de pantalla

    void Start()
    {
        // Calcula el centro del men� en coordenadas de pantalla
        wheelCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, menuWheel.position);

        // Inicializa el tama�o de las torres
        HighlightSelectedTower(currentSelectedTowerIndex);
    }

    void Update()
    {
        // Mostrar el men� de la rueda si se presiona la tecla
        if (Input.GetKey(KeyCode.X))
        {
            menuWheel.gameObject.SetActive(true);

            // Obtiene la posici�n del mouse
            Vector2 mousePos = Input.mousePosition;

            // Calcula la distancia desde el centro del men�
            Vector2 direction = mousePos - wheelCenter;
            float distanceFromCenter = direction.magnitude;

            // Limita el mouse a moverse en el borde del c�rculo
            if (distanceFromCenter > maxRadius)
            {
                direction = direction.normalized * maxRadius; // Restringe al borde
                mousePos = wheelCenter + direction; // Ajusta la posici�n del mouse
            }

            // Calcula el �ngulo del mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;  // Convierte a rango 0-360

            // Determina qu� torre est� seleccionada seg�n el �ngulo
            if (angle >= 0 && angle < 90) currentSelectedTowerIndex = 0;
            else if (angle >= 90 && angle < 180) currentSelectedTowerIndex = 1;
            else if (angle >= 180 && angle < 270) currentSelectedTowerIndex = 2;
            else currentSelectedTowerIndex = 3;

            // Resalta la torre seleccionada
            HighlightSelectedTower(currentSelectedTowerIndex);

            // Detecta clic izquierdo para confirmar la selecci�n de la torre
            if (Input.GetMouseButtonDown(0)) // Clic izquierdo
            {
                SelectTower(currentSelectedTowerIndex); // Selecciona la torre
                CloseWheelMenu(); // Cierra el men�
            }
        }
        else
        {
            CloseWheelMenu(); // Si sueltas la tecla, cierra el men�
        }
    }

    // M�todo para seleccionar la torre (puedes implementar la l�gica de colocaci�n aqu�)
    void SelectTower(int index)
    {
        Debug.Log("Torre seleccionada: " + towers[index].name);
        // Aqu� puedes agregar la l�gica de colocar la torre seleccionada
    }

    // Resalta la torre seleccionada aumentando su tama�o
    void HighlightSelectedTower(int index)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            if (i == index)
            {
                towers[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // Aumenta el tama�o de la torre seleccionada
            }
            else
            {
                towers[i].transform.localScale = new Vector3(1f, 1f, 1f); // Restaura el tama�o original
            }
        }
    }

    // M�todo para cerrar el men� de selecci�n
    void CloseWheelMenu()
    {
        menuWheel.gameObject.SetActive(false);
    }
}
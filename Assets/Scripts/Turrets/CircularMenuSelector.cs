using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuSelector : MonoBehaviour
{
    public GameObject[] towers;  // Array con las torres
    public Transform center;     // Centro del círculo
    public float radius = 100f;  // Radio del círculo
    public RectTransform menuWheel;  // La UI del círculo
    public Transform player;     // Referencia al jugador
    public float maxRadius = 100f;  // Radio máximo permitido para mover el mouse

    private int currentSelectedTowerIndex = 0; // Índice de la torre seleccionada
    private Vector2 wheelCenter;  // Centro del círculo en coordenadas de pantalla

    void Start()
    {
        // Calcula el centro del menú en coordenadas de pantalla
        wheelCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, menuWheel.position);

        // Inicializa el tamaño de las torres
        HighlightSelectedTower(currentSelectedTowerIndex);
    }

    void Update()
    {
        // Mostrar el menú de la rueda si se presiona la tecla
        if (Input.GetKey(KeyCode.X))
        {
            menuWheel.gameObject.SetActive(true);

            // Obtiene la posición del mouse
            Vector2 mousePos = Input.mousePosition;

            // Calcula la distancia desde el centro del menú
            Vector2 direction = mousePos - wheelCenter;
            float distanceFromCenter = direction.magnitude;

            // Limita el mouse a moverse en el borde del círculo
            if (distanceFromCenter > maxRadius)
            {
                direction = direction.normalized * maxRadius; // Restringe al borde
                mousePos = wheelCenter + direction; // Ajusta la posición del mouse
            }

            // Calcula el ángulo del mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;  // Convierte a rango 0-360

            // Determina qué torre está seleccionada según el ángulo
            if (angle >= 0 && angle < 90) currentSelectedTowerIndex = 0;
            else if (angle >= 90 && angle < 180) currentSelectedTowerIndex = 1;
            else if (angle >= 180 && angle < 270) currentSelectedTowerIndex = 2;
            else currentSelectedTowerIndex = 3;

            // Resalta la torre seleccionada
            HighlightSelectedTower(currentSelectedTowerIndex);

            // Detecta clic izquierdo para confirmar la selección de la torre
            if (Input.GetMouseButtonDown(0)) // Clic izquierdo
            {
                SelectTower(currentSelectedTowerIndex); // Selecciona la torre
                CloseWheelMenu(); // Cierra el menú
            }
        }
        else
        {
            CloseWheelMenu(); // Si sueltas la tecla, cierra el menú
        }
    }

    // Método para seleccionar la torre (puedes implementar la lógica de colocación aquí)
    void SelectTower(int index)
    {
        Debug.Log("Torre seleccionada: " + towers[index].name);
        // Aquí puedes agregar la lógica de colocar la torre seleccionada
    }

    // Resalta la torre seleccionada aumentando su tamaño
    void HighlightSelectedTower(int index)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            if (i == index)
            {
                towers[i].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // Aumenta el tamaño de la torre seleccionada
            }
            else
            {
                towers[i].transform.localScale = new Vector3(1f, 1f, 1f); // Restaura el tamaño original
            }
        }
    }

    // Método para cerrar el menú de selección
    void CloseWheelMenu()
    {
        menuWheel.gameObject.SetActive(false);
    }
}
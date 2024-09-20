using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTerrainPainter : MonoBehaviour
{
    public Texture2D[] textures; // Array de texturas para pintar
    public GameObject[] detailPrefabs; // Array de detalles como rocas, árboles
    public float brushSize = 1.0f; // Tamaño del pincel
    public float brushStrength = 1.0f; // Fuerza del pincel para pintar texturas

    private Renderer objectRenderer;
    private Texture2D objectTexture;
    private int textureWidth = 1024;
    private int textureHeight = 1024;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectTexture = new Texture2D(textureWidth, textureHeight);
        objectRenderer.material.mainTexture = objectTexture;

        // Inicializa la textura con un color base (por ejemplo, blanco)
        Color[] initialColors = new Color[textureWidth * textureHeight];
        for (int i = 0; i < initialColors.Length; i++)
        {
            initialColors[i] = Color.white;
        }
        objectTexture.SetPixels(initialColors);
        objectTexture.Apply();
    }

    private void Update()
    {
        // Detecta clic del ratón
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform) // Si el rayo golpea el objeto actual
                {
                    PaintOnModel(hit.textureCoord); // Pinta en el modelo usando las coordenadas UV
                }
            }
        }

        // Detecta clic derecho para colocar detalles
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    PlaceDetail(hit.point); // Coloca un detalle en la posición del impacto
                }
            }
        }
    }

    // Función para pintar en el modelo usando coordenadas UV
    private void PaintOnModel(Vector2 uv)
    {
        int x = (int)(uv.x * textureWidth);
        int y = (int)(uv.y * textureHeight);

        // Aplica la textura seleccionada
        for (int i = -Mathf.FloorToInt(brushSize / 2); i < Mathf.CeilToInt(brushSize / 2); i++)
        {
            for (int j = -Mathf.FloorToInt(brushSize / 2); j < Mathf.CeilToInt(brushSize / 2); j++)
            {
                int pixelX = Mathf.Clamp(x + i, 0, textureWidth - 1);
                int pixelY = Mathf.Clamp(y + j, 0, textureHeight - 1);

                objectTexture.SetPixel(pixelX, pixelY, textures[0].GetPixelBilinear(uv.x, uv.y) * brushStrength);
            }
        }

        objectTexture.Apply();
    }

    // Función para colocar detalles (ej. hierba, rocas) en la superficie del modelo
    private void PlaceDetail(Vector3 position)
    {
        // Selecciona un prefab aleatorio para colocar
        GameObject detailPrefab = detailPrefabs[Random.Range(0, detailPrefabs.Length)];

        // Instancia el objeto en la posición del impacto
        Instantiate(detailPrefab, position, Quaternion.identity);
    }
}

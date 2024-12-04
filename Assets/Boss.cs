using UnityEngine;

public class Boss : MonoBehaviour
{
    private GameObject victoryObject; // Referencia al objeto dentro del Canvas

    private void Start()
    {
        // Buscar el VictoryObject incluso si está inactivo
        VictoryObjectCheck();
    }

    private void OnDestroy()
    {
        // Activar el objeto de victoria al destruirse este objeto
        if (victoryObject != null)
        {
            victoryObject.SetActive(true);
            GameManager.instance.ChangeState(GameState.Victory);
            Time.timeScale = 0;
        }
    }

    private void VictoryObjectCheck()
    {
        // Buscar todos los objetos en la escena, incluyendo los inactivos
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name == "VictoryCanvas") // Asegúrate de que el nombre coincide
            {
                victoryObject = obj;
                break;
            }
        }

        if (victoryObject == null)
        {
            Debug.LogError("No se encontró un objeto llamado 'VictoryObject'. Asegúrate de que existe en la escena.");
        }
        else
        {
            Debug.Log("VictoryObject encontrado incluso estando inactivo.");
        }
    }
}

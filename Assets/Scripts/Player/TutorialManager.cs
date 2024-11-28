using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialText;
    public string[] tutorialSteps;
    private int currentStep = 0;

    public bool isTutorialComplete = false; 

    private void Start()
    {
        ShowStep();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            NextStep();
        }
    }

    void ShowStep()
    {
        if (currentStep < tutorialSteps.Length)
        {
            tutorialText.text = tutorialSteps[currentStep];
        }
        else
        {
            tutorialText.text = "Culo si pierdes";
            isTutorialComplete = true;
        }
    }

    void NextStep()
    {
        currentStep++;
        ShowStep();
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TMP_Text tutorialText;
    public Image tutorialImage;

    public TutorialStep[] tutorialSteps;
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
            TutorialStep step = tutorialSteps[currentStep];
            if (!string.IsNullOrEmpty(step.text))
            {
                tutorialText.text = step.text;
                tutorialText.gameObject.SetActive(true);
            }
            else
            {
                tutorialText.gameObject.SetActive(false);
            }
            if (step.image != null)
            {
                tutorialImage.sprite = step.image;
                tutorialImage.gameObject.SetActive(true);
            }
            else
            {
                tutorialImage.gameObject.SetActive(false);
            }
        }
        else
        {
            tutorialText.text = "";
            tutorialText.gameObject.SetActive(false);
            tutorialImage.gameObject.SetActive(false);
            isTutorialComplete = true;
        }
    }

    void NextStep()
    {
        currentStep++;
        ShowStep();
    }
}


[System.Serializable]
public class TutorialStep
{
    [TextArea]
    public string text;
    public Sprite image;
}

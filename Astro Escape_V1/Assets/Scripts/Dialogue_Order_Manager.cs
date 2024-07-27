using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueOrderManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI npcNameText;
    public Image npcImage;
    public GameObject dialoguePanel;
    public Image dialogueBackgroundImage; // Background image for the dialogue panel
    public Button continueButton; // Reference to the Continue button
    public string[] sentences;
    public string[] npcNames;
    public Sprite[] npcImages;
    public float dialogueSpeed;

    public GameObject instructionsPanel; // Reference to the Instructions panel
    public Button instructionsContinueButton; // Reference to the Continue button in Instructions panel
    public QuizManager quizManager; // Add a public reference to QuizManager

    private int index = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        // Initialize UI elements
        InitializeUI();
        
        // Start the dialogue sequence
        StartDialogueSequence();
    }

    private void InitializeUI()
    {
        dialoguePanel.SetActive(true);
        dialogueBackgroundImage.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);
        instructionsPanel.SetActive(false);
        instructionsContinueButton.gameObject.SetActive(false);

        continueButton.onClick.AddListener(ContinueDialogue);
        instructionsContinueButton.onClick.AddListener(StartQuiz);
    }

    void StartDialogueSequence()
    {
        if (sentences.Length > 0 && npcNames.Length > 0 && npcImages.Length > 0)
        {
            index = 0; // Reset index
            isDialogueActive = true;
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            EndDialogue();
        }
    }

    void ContinueDialogue()
    {
        if (isDialogueActive)
        {
            StopAllCoroutines(); // Stop the current dialogue coroutine
            StartCoroutine(DisplayDialogue()); // Continue displaying the next dialogue
        }
        else
        {
            ShowInstructionsPanel();
        }
    }

    void ShowInstructionsPanel()
    {
        dialoguePanel.SetActive(false);
        dialogueBackgroundImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        instructionsPanel.SetActive(true);
        instructionsContinueButton.gameObject.SetActive(true);
    }

    void StartQuiz()
    {
        if (quizManager != null)
        {
            quizManager.StartQuiz();
            instructionsPanel.SetActive(false); // Hide the instructions panel after starting the quiz
        }
    }

    IEnumerator DisplayDialogue()
    {
        if (index >= sentences.Length)
        {
            EndDialogue();
            yield break; // Exit if no more dialogues
        }

        // Display the current dialogue
        dialogueText.text = "";
        npcNameText.text = npcNames[index];
        npcImage.sprite = npcImages[index];

        foreach (char character in sentences[index].ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(dialogueSpeed);
        }

        index++; // Move to the next dialogue
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueBackgroundImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        isDialogueActive = false;

        ShowInstructionsPanel();
    }
}

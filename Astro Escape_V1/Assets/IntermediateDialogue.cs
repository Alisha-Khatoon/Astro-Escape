using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Add this namespace

public class IntermediateDialogue : MonoBehaviour
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
    public Image instructionsImage; // Reference to the instructions image

    private int index = 0;
    private bool isDialogueActive = false;

    void Start()
    {
        // Initially hide the dialogue panel, its background image, and the instructions image
        dialoguePanel.SetActive(false);
        dialogueBackgroundImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false); // Hide the continue button initially
        instructionsImage.gameObject.SetActive(false); // Hide the instructions image initially
        StartDialogueSequence();
        continueButton.onClick.AddListener(ContinueDialogue);
    }

    void StartDialogueSequence()
    {
        // Show the dialogue panel and its background image
        dialogueBackgroundImage.gameObject.SetActive(true);
        dialoguePanel.SetActive(true);
        continueButton.gameObject.SetActive(true); // Show the continue button

        // Start displaying the dialogue
        if (sentences.Length > 0 && npcNames.Length > 0 && npcImages.Length > 0)
        {
            index = 0; // Reset index
            isDialogueActive = true;
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            dialoguePanel.SetActive(false);
            dialogueBackgroundImage.gameObject.SetActive(false);
        }
    }

    void ContinueDialogue()
    {
        // Continue to the next dialogue
        if (isDialogueActive)
        {
            StopAllCoroutines(); // Stop the current dialogue coroutine
            StartCoroutine(DisplayDialogue()); // Start displaying the next dialogue
        }
    }

    IEnumerator DisplayDialogue()
    {
        // Ensure index is within bounds
        if (index >= sentences.Length)
        {
            dialoguePanel.SetActive(false);
            dialogueBackgroundImage.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false); // Hide the continue button
            isDialogueActive = false;
            ShowInstructions(); // Show the instructions image
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

    void ShowInstructions()
    {
        // Show the instructions image
        instructionsImage.gameObject.SetActive(true);
        continueButton.onClick.RemoveAllListeners(); // Remove previous listeners
        continueButton.onClick.AddListener(LoadSpaceDodgerScene); // Add new listener for loading the scene
        continueButton.gameObject.SetActive(true); // Show the continue button
    }

    void HideInstructions()
    {
        // Hide the instructions image
        instructionsImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false); // Hide the continue button
    }

    public void LoadSpaceDodgerScene()
    {
        // Load the Space_Dodger scene
        SceneManager.LoadScene("Space_Dodger");
    }
}

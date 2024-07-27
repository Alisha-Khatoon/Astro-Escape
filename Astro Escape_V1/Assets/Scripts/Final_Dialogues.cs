using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For scene management

public class FinalDialogues : MonoBehaviour
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
    public string nextSceneName; // Name of the scene to load when clicking "Play Again"

    private int index = 0;
    private bool isDialogueActive = false;
    private bool isLastDialogue = false; // Track if the last dialogue is being displayed

    void Start()
    {
        // Initially hide the dialogue panel and its background image
        dialoguePanel.SetActive(false);
        dialogueBackgroundImage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false); // Hide the continue button initially
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
        if (isDialogueActive)
        {
            if (isLastDialogue)
            {
                // Load the specified scene when the button is clicked
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                StopAllCoroutines(); // Stop the current dialogue coroutine
                StartCoroutine(DisplayDialogue()); // Start displaying the next dialogue
            }
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
            yield break; // Exit if no more dialogues
        }

        // Display the current dialogue
        string currentSentence = sentences[index];
        dialogueText.text = "";

        // Handle rich text for the last sentence
        if (index == sentences.Length - 1)
        {
            currentSentence = "<i>" + sentences[index] + "</i>";
            npcImage.gameObject.SetActive(false); // Hide NPC image for the last sentence
            isLastDialogue = true;
            // Change button text to "Play Again"
            continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play Again";
        }
        else
        {
            npcImage.gameObject.SetActive(true); // Ensure NPC image is visible
            isLastDialogue = false;
        }

        npcNameText.text = index < npcNames.Length ? npcNames[index] : ""; // Handle NPC name
        npcImage.sprite = index < npcImages.Length ? npcImages[index] : null; // Handle NPC image

        // Display the dialogue text with typing effect
        foreach (char character in currentSentence.ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(dialogueSpeed);
        }

        index++; // Move to the next dialogue
    }
}

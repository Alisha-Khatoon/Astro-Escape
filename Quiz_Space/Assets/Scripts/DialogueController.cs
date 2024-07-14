using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public string[] sentences;
    public float dialogueSpeed;
    private int index = 0;
    public QuizManager quizManager;

    public AudioClip dialogueStartClip; // Audio for dialogue start
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (sentences.Length > 0)
        {
            audioSource.PlayOneShot(dialogueStartClip); // Play dialogue start audio
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            dialoguePanel.SetActive(false);
            quizManager.StartQuiz();
        }
    }

    IEnumerator DisplayDialogue()
    {
        foreach (string sentence in sentences)
        {
            dialogueText.text = "";
            foreach (char character in sentence.ToCharArray())
            {
                dialogueText.text += character;
                yield return new WaitForSeconds(dialogueSpeed);
            }
            yield return new WaitForSeconds(1f);
            index++;
        }

        dialoguePanel.SetActive(false);
        quizManager.StartQuiz();
    }
}

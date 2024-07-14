using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Question
{
    public string questionText;
    public string correctAnswer;
    public string[] incorrectAnswers;
}

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public GameObject quizPanel;
    public GameObject dialoguePanel;
    public GameObject endGamePanel;
    public TextMeshProUGUI endGameMessage;
    public Button tryAgainButton;

    private List<Question> allQuestions = new List<Question>();
    private List<Question> selectedQuestions = new List<Question>();
    private Question currentQuestion;
    private int questionIndex = 0;
    private float timer = 20f;
    private int score = 0;

    void Start()
    {
        allQuestions.Add(new Question { questionText = "Which planet is covered by clouds of sulphuric acid?", correctAnswer = "Venus", incorrectAnswers = new string[] { "Earth", "Mars", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "Who was the first man in space?", correctAnswer = "Yuri Gagarin", incorrectAnswers = new string[] { "Neil Armstrong", "Buzz Aldrin", "Alan Shepard" } });
        allQuestions.Add(new Question { questionText = "Which is the largest moon in the solar system?", correctAnswer = "Ganymede", incorrectAnswers = new string[] { "Titan", "Europa", "Io" } });
        allQuestions.Add(new Question { questionText = "Which is the brightest comet in the solar system?", correctAnswer = "Halley's comet", incorrectAnswers = new string[] { "Hale-Bopp", "Encke's Comet", "Comet Hyakutake" } });
        allQuestions.Add(new Question { questionText = "Where is the chromosphere?", correctAnswer = "Sun", incorrectAnswers = new string[] { "Earth", "Mars", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "Where is geostationary satellite positioned at?", correctAnswer = "Equator", incorrectAnswers = new string[] { "Poles", "Mid-latitudes", "Tropics" } });
        allQuestions.Add(new Question { questionText = "What is the largest planet in our solar system?", correctAnswer = "Jupiter", incorrectAnswers = new string[] { "Saturn", "Earth", "Mars" } });
        allQuestions.Add(new Question { questionText = "What is the main component of the sun?", correctAnswer = "Hydrogen", incorrectAnswers = new string[] { "Helium", "Oxygen", "Carbon" } });
        allQuestions.Add(new Question { questionText = "Which planet is known as the Red Planet?", correctAnswer = "Mars", incorrectAnswers = new string[] { "Venus", "Saturn", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "What is the name of the galaxy we live in?", correctAnswer = "Milky Way", incorrectAnswers = new string[] { "Andromeda", "Whirlpool", "Sombrero" } });
        allQuestions.Add(new Question { questionText = "Which planet has the most moons?", correctAnswer = "Saturn", incorrectAnswers = new string[] { "Earth", "Mars", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "What is the closest star to Earth?", correctAnswer = "Proxima Centauri", incorrectAnswers = new string[] { "Sirius", "Alpha Centauri", "Betelgeuse" } });
        allQuestions.Add(new Question { questionText = "Which planet is famous for its rings?", correctAnswer = "Saturn", incorrectAnswers = new string[] { "Jupiter", "Uranus", "Neptune" } });
        allQuestions.Add(new Question { questionText = "What force keeps the planets in orbit around the sun?", correctAnswer = "Gravity", incorrectAnswers = new string[] { "Inertia", "Magnetism", "Friction" } });
        allQuestions.Add(new Question { questionText = "Which planet is known for having the largest volcano?", correctAnswer = "Mars", incorrectAnswers = new string[] { "Earth", "Venus", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "What is the most distant planet in our solar system?", correctAnswer = "Neptune", incorrectAnswers = new string[] { "Uranus", "Pluto", "Jupiter" } });
        allQuestions.Add(new Question { questionText = "What is the largest type of star in the universe?", correctAnswer = "Red supergiant", incorrectAnswers = new string[] { "White dwarf", "Neutron star", "Blue giant" } });
        allQuestions.Add(new Question { questionText = "What is the escape velocity from Earth?", correctAnswer = "11.2 km/s", incorrectAnswers = new string[] { "9.8 km/s", "7.9 km/s", "12.5 km/s" } });
        allQuestions.Add(new Question { questionText = "What is the name of the first artificial Earth satellite?", correctAnswer = "Sputnik 1", incorrectAnswers = new string[] { "Explorer 1", "Vanguard 1", "Luna 1" } });
        allQuestions.Add(new Question { questionText = "What is the largest asteroid in the asteroid belt?", correctAnswer = "Ceres", incorrectAnswers = new string[] { "Vesta", "Pallas", "Hygiea" } });
        allQuestions.Add(new Question { questionText = "What is the average distance from Earth to the Moon?", correctAnswer = "384,400 km", incorrectAnswers = new string[] { "300,000 km", "450,000 km", "500,000 km" } });
        allQuestions.Add(new Question { questionText = "What is the primary component of Saturn's rings?", correctAnswer = "Ice particles", incorrectAnswers = new string[] { "Dust", "Rock", "Gas" } });
        allQuestions.Add(new Question { questionText = "What is the rotational period of Jupiter?", correctAnswer = "10 hours", incorrectAnswers = new string[] { "12 hours", "24 hours", "30 hours" } });
        allQuestions.Add(new Question { questionText = "What is the name of the largest crater on the Moon?", correctAnswer = "South Pole-Aitken Basin", incorrectAnswers = new string[] { "Tycho", "Copernicus", "Mare Imbrium" } });
        allQuestions.Add(new Question { questionText = "What is the name of the first human-made object to leave the solar system?", correctAnswer = "Voyager 1", incorrectAnswers = new string[] { "Pioneer 10", "Voyager 2", "New Horizons" } });

        allQuestions = ShuffleList(allQuestions);
        selectedQuestions = allQuestions.GetRange(0, 15);
        quizPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        endGamePanel.SetActive(false);
        tryAgainButton.onClick.AddListener(StartQuiz);
    }

    void Update()
    {
        if (quizPanel.activeSelf)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                timerText.text = Mathf.Round(timer).ToString();
            }
            else
            {
                timer = 0;
                timerText.text = "0";
                feedbackText.text = "Game Over";
                feedbackText.color = Color.red;
                feedbackText.gameObject.SetActive(true);
                StartCoroutine(FlashFeedbackText());
            }
        }
    }

    public void StartQuiz()
    {
        dialoguePanel.SetActive(false);
        endGamePanel.SetActive(false);
        quizPanel.SetActive(true);
        score = 0;
        questionIndex = 0;
        UpdateScoreText();
        DisplayNextQuestion();
    }

    void DisplayNextQuestion()
    {
        feedbackText.gameObject.SetActive(false);
        if (questionIndex < selectedQuestions.Count)
        {
            currentQuestion = selectedQuestions[questionIndex];
            questionText.text = currentQuestion.questionText;
            List<string> answers = new List<string>(currentQuestion.incorrectAnswers);
            answers.Add(currentQuestion.correctAnswer);
            answers = ShuffleList(answers);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
                answerButtons[i].onClick.RemoveAllListeners();

                if (answers[i] == currentQuestion.correctAnswer)
                {
                    answerButtons[i].onClick.AddListener(CorrectAnswer);
                }
                else
                {
                    answerButtons[i].onClick.AddListener(IncorrectAnswer);
                }
            }

            timer = 20f;
            questionIndex++;
        }
        else
        {
            EndGame();
        }
    }

    void CorrectAnswer()
    {
        score += 25;
        UpdateScoreText(true);
        feedbackText.text = "Correct!";
        feedbackText.color = Color.green;
        feedbackText.gameObject.SetActive(true);
        Invoke("HideFeedbackAndDisplayNextQuestion", 1f);
    }

    void IncorrectAnswer()
    {
        score -= 25;
        UpdateScoreText(false);
        feedbackText.text = "Incorrect!";
        feedbackText.color = Color.red;
        feedbackText.gameObject.SetActive(true);
        Invoke("HideFeedbackAndDisplayNextQuestion", 1f);
    }

    void HideFeedbackAndDisplayNextQuestion()
    {
        feedbackText.gameObject.SetActive(false);
        DisplayNextQuestion();
    }

    void EndGame()
    {
        quizPanel.SetActive(false);
        endGamePanel.SetActive(true);

        if (questionIndex >= selectedQuestions.Count)
        {
            if (score >= 300)
            {
                endGameMessage.text = "Congrats! You possess an amazing knowledge about space!\n As a reward, you have collected the fuel!";
              
            }
            else
            {
                endGameMessage.text = "Sorry, your score is not sufficient to claim the fuel. Try again!";
            }
        }
        else
        {
            endGameMessage.text = "Time's up! Try again! ";
        }
    }

    void UpdateScoreText(bool isCorrect = true)
    {
        scoreText.text = "Score: " + score;
        if (isCorrect)
        {
            StartCoroutine(FlashScoreText(Color.green));
        }
        else
        {
            StartCoroutine(FlashScoreText(Color.red));
        }
    }

    IEnumerator FlashScoreText(Color color)
    {
        scoreText.color = color;
        yield return new WaitForSeconds(1f);
        scoreText.color = Color.white;
    }

    IEnumerator FlashFeedbackText()
    {
        for (int i = 0; i < 3; i++)
        {
            feedbackText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            feedbackText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        EndGame();
    }

    List<T> ShuffleList<T>(List<T> list)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = random.Next(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
}
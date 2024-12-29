using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text characterName;
    public Text messageText;
    public GameObject backgroundBox;

    Message[] currentMessages;
    Character[] currentCharacters;
    int activeMessage = 0;

    public bool isActive = false;

    // Add a typing speed variable.
    public float typingSpeed = 30.0f;

    private bool isTyping = false;

    private void Start()
    {
        backgroundBox.SetActive(false);
    }

    public void OpenDialoue(Message[] messages, Character[] characters)
    {
        currentMessages = messages;
        currentCharacters = characters;
        activeMessage = 0;
        isActive = true;
        backgroundBox.SetActive(true);
        DisplayMessage();
    }

    private void DisplayMessage()
    {
        if (isTyping)
        {
            // If typing, stop the coroutine to instantly show the entire message.
            StopAllCoroutines();
            isTyping = false;
            messageText.text = currentMessages[activeMessage].message;
        }
        else
        {
            // If not typing, start typing coroutine.
            StartCoroutine(TypeMessage(currentMessages[activeMessage].message));
        }

        characterName.text = currentCharacters[currentMessages[activeMessage].characterID].name;
    }

    // Coroutine to type the message letter by letter.
    private IEnumerator TypeMessage(string message)
    {
        messageText.text = "";

        foreach (char letter in message)
        {
            messageText.text += letter;
            yield return new WaitForSeconds(1 / typingSpeed);
        }

        isTyping = false; // Typing is complete.
    }

    public void NextMessage()
    {
        activeMessage++;

        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.SetActive(false);
        }
    }

    private void Update()
    {
        if (FindObjectOfType<Player>().inputHandler.interactInput && isActive == true && !isTyping)
        {
            FindObjectOfType<Player>().inputHandler.UseInteractInput();
            NextMessage();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{    
    public Message[] messages;
    public Character[] characters;

    private Player player;
    private DialogueManager dialogueManager;
    private bool isSpoken = false;
    private CapsuleCollider2D cc;
    

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        cc = GetComponent<CapsuleCollider2D>();
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        

        if (collision.gameObject.name == "Player")
        {
            
            if (player.inputHandler.interactInput && !isSpoken)
            {
                player.inputHandler.UseInteractInput();
                dialogueManager.OpenDialoue(messages, characters);                
                isSpoken=true;                
            }
        }     

    }

    private void Update()
    {
        if (isSpoken && !dialogueManager.isActive)
        {
            cc.isTrigger = true;
        }

    }

}


[System.Serializable]
public class Message
{
    public int characterID;
    public string message;
}

[System.Serializable]
public class Character
{
    public string name;
}
using UnityEngine;
using Yarn.Unity;

public class DialogueMover : MonoBehaviour
{
    private DialogueUI2 dialogueUI;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        // Retrieve references for the DialogueUI and Camera
        dialogueUI = FindObjectOfType<DialogueUI2>();
        cam = Camera.main;
    }

    private void Update()
    {
        // If the Camera changes view, this ensures that the Dialogue Bubble position
        // get recalculated based on the new Camera view, every frame
        SetDialogueOnTalkingCharacter();
    }

    // Retrieve the character who's talking from the text
    public void SetDialogueOnTalkingCharacter()
    {
        GameObject character;
        string line, name;

        // Get the dialogue line
        line = dialogueUI.getLineText();
        // Search for the character who's talking
        if (line.Contains(":"))
            name = line.Substring(0, line.IndexOf(":"));
        else
            //name = "Player";
            name = "Character Body";
        // Search the GameObject of the character in the Scene
        character = GameObject.Find(name);
        // Sets the dialogue position
        SetDialoguePosition(character);
    }

    private void SetDialoguePosition(GameObject character)
    {
        float x;
        float y;

        if (character.name.Equals("Merch"))
        {
            x = character.transform.position.x - 1;
            y = character.transform.position.y + 6.8f;

        }
        else if (character.name.Equals("Character Body"))
        {
            x = character.transform.position.x;
            y = character.transform.position.y + 5.7f;
        }
        else
        {
            x = character.transform.position.x - 1;
            y = character.transform.position.y + 6.8f;
        }

        // Retrieve the position where the top part of the sprite is in the world
        //float characterSpriteHeight = character.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        // Create position with the sprite top location
        Vector2 characterPosition = new Vector2(x,
                                                y);

        // Set the DialogueBubble position to the sprite top location in Screen Space
        this.transform.position = cam.WorldToScreenPoint(characterPosition);
    }
}
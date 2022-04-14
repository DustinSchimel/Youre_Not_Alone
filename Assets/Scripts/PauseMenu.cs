using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    private PlayerInputActions inputActions;
    public GameObject pauseMenuUI;

    public AudioMixer audioMixer;

    private int optionSelected;

    public Text resume;
    public Text volume;
    public Text quit;

    private void Start()
    {
        optionSelected = 0;
        //inputActions = new PlayerInputActions();
    }

    private void Update()
    {

    }

    public void Pause(InputAction.CallbackContext context, PlayerInputActions input)
    {
        if (context.performed)
        {
            inputActions = input;

            if (gameIsPaused)
            {
                inputActions.PauseMenu.Resume.performed += ResumePause;
                inputActions.PauseMenu.Resume.performed += MoveUpPause;
                inputActions.PauseMenu.Resume.performed += MoveDownPause;
                inputActions.PauseMenu.Resume.performed += VolumeUp;
                inputActions.PauseMenu.Resume.performed += VolumeDown;
                inputActions.PauseMenu.Resume.performed += SelectOption;

                Resume();
            }
            else
            {
                inputActions.PauseMenu.Resume.performed += ResumePause;
                inputActions.PauseMenu.Resume.performed += MoveUpPause;
                inputActions.PauseMenu.Resume.performed += MoveDownPause;
                inputActions.PauseMenu.Resume.performed += VolumeUp;
                inputActions.PauseMenu.Resume.performed += VolumeDown;
                inputActions.PauseMenu.Resume.performed += SelectOption;

                Pause();
            }
        }
    }

    public void ResumePause(InputAction.CallbackContext context)
    {
        inputActions.PauseMenu.Disable();
        inputActions.Player.Enable();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void MoveUpPause(InputAction.CallbackContext context)
    {
        Debug.Log("OptionUp");

        if (optionSelected == 0)    // Resume is selected
        {
            // Do nothing
        }
        else if (optionSelected == 1)   // Volume is selected
        {
            // disable volume bold

            // enable resume bold

            optionSelected = 0;
        }
        else if (optionSelected == 2)   // Quit is selected
        {
            // disable quit bold

            // enable volume bold

            optionSelected = 1;
        }
    }

    public void MoveDownPause(InputAction.CallbackContext context)
    {
        Debug.Log("OptionDown");

        if (optionSelected == 0)    // Resume is selected
        {
            // disable resume bold
            //resume.text = resume.text.Substring(3);

            // enable volume bold
            //volume.text = "<b>" + volume.text + "</b>";

            optionSelected = 1;
        }
        else if (optionSelected == 1)   // Volume is selected
        {
            // disable volume bold

            // enable quit bold

            optionSelected = 2;
        }
        else if (optionSelected == 2)   // Quit is selected
        {
            // Do nothing
        }
    }

    public void VolumeDown(InputAction.CallbackContext context)
    {
        if (optionSelected == 1)
        {

        }
    }

    public void VolumeUp(InputAction.CallbackContext context)
    {
        if (optionSelected == 1)
        {

        }
    }

    public void SelectOption(InputAction.CallbackContext context)
    {
        if (optionSelected == 0)    // Resume is selected
        {

        }
        else if (optionSelected == 1)   // Volume is selected
        {

        }
        else if (optionSelected == 2)   // Quit is selected
        {

        }
    }

    public void Resume()
    {
        inputActions.PauseMenu.Disable();
        inputActions.Player.Enable();

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    void Pause()
    {
        inputActions.Player.Disable();
        inputActions.PauseMenu.Enable();

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void Menu()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

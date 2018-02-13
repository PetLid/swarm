using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardCommands : MonoBehaviour {

    public Button optionsButton;
    public Button infoButton;
    public Button goalButton;
    public Button obstacleButton;
    public Button spawnButton;
    public Button startButton;

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }

        // Options
        else if(Input.GetKeyDown(KeyCode.O))
        {
            optionsButton.onClick.Invoke();
        }

        // Spawn point
        else if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            spawnButton.onClick.Invoke();
        }

        // Goal
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            goalButton.onClick.Invoke();
        }

        // Obstacle
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            obstacleButton.onClick.Invoke();
        }

        // Start
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            startButton.onClick.Invoke();
        }

        // Info
        else if (Input.GetKeyDown(KeyCode.I))
        {
            infoButton.onClick.Invoke();
        }
    }
}

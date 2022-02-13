using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Squiggle;
using Squiggle.Commands;

public class SquiggleCustomCommandSample : MonoBehaviour
{
    public Text SpeakerName;
    public Text SpeakerText;
    public Button RunSquiggle;
    string SquiggleCode;
    public Button NextCommandButton;
    void OnEnable()
    {
        SpeakerName.text = "";
        SpeakerText.text = "";
        NextCommandButton.gameObject.SetActive(false);

        SquiggleCode = Squiggle.Unity.Utils.ReadTextFileFromStreamingAssets("customCommandSample.txt");
        //VERY IMPORTANT - you need to replace line endings of loaded in files:
        SquiggleCode = SquiggleCode.Replace("\r\n", "\\r\\n"); //need to replace these due to serialization
        // {Text.Replace("\n", "D")}"); also works - may need to do this as well for OSX line endings

        RunSquiggle.onClick.AddListener(() => {
            EventSystem.current.SetSelectedGameObject(null);
            StartCoroutine(TryExecuteSquiggle());
        });

        Events.SetTextColor += OnSetTextColor;
    }
    void OnDisable()
    {
        RunSquiggle.onClick.RemoveAllListeners();
        NextCommandButton.onClick.RemoveAllListeners();
        Events.SetTextColor -= OnSetTextColor;
    }

    Squiggle.Runner currentRunner;
    bool currentlyRunning = false;
    Squiggle.Commands.Dialog activeDialogCommand;
    IEnumerator TryExecuteSquiggle()
    {
        if(currentlyRunning)
        {
            yield return null;
        }

        currentlyRunning = true;
        currentRunner = Squiggle.Core.Run(  squiggleText : SquiggleCode,
                                            runnerOptions : new Squiggle.Runner.Options(){
                                                AutoStart = false,
                                                Debug = true,
                                                LogHandler = (text) => Debug.Log(text),
                                                WaitOverride = (command) => StartCoroutine(WaitMs(command)),
                                                DialogHandler = (command) => {
                                                    SpeakerName.text = command.Speaker;
                                                    SpeakerText.text = command.Text;
                                                    NextCommandButton.onClick.AddListener(() => {
                                                        EventSystem.current.SetSelectedGameObject(null);
                                                        command.CommandExecutionComplete?.Invoke();
                                                    });
                                                    activeDialogCommand = command;
                                                },
                                        });
                                        
        currentRunner.CompletedExecution += OnRunnerComplete;

        currentRunner.CommandExecutionStarted += OnCommandExecutionStarted;

        currentRunner.Start();
        yield return null;
    }

    IEnumerator WaitMs(Squiggle.Commands.Wait waitCommand)
    {
        yield return new WaitForSeconds(waitCommand.WaitMS / 1000f);
        waitCommand.CommandExecutionComplete?.Invoke();
    }

    void OnSetTextColor(string color)
    {
        switch (color)
        {
            case "red":
                SpeakerText.color = Color.red;
                break;
            case "blue":
                SpeakerText.color = Color.blue;
                break;
            case "green":
                SpeakerText.color = Color.green;
                break;
            case "yellow":
                SpeakerText.color = Color.yellow;
                break;
            default:
                Debug.Log("unrecgonized color for text " + color);
                break;
        }
    }

    void OnCommandExecutionStarted(SquiggleCommand c)
    {
        NextCommandButton.gameObject.SetActive(c is Dialog);
    }

    void OnRunnerComplete()
    {
        currentRunner.CompletedExecution -= OnRunnerComplete;
        currentRunner.CommandExecutionStarted -= OnCommandExecutionStarted;
        
        currentRunner = null;
        currentlyRunning = false;
        SpeakerName.text = "";
        SpeakerText.text = "";
        NextCommandButton.gameObject.SetActive(false);
    }
}

public static class Events
{
    public static Action<string> SetTextColor;
}

[SquiggleCommand("setTextColor")]
public class SampleCustom : SquiggleCommand
{
    [Arg(1)] public string color;
    public override void Execute()
    {
        Events.SetTextColor?.Invoke(color);
        CommandExecutionComplete?.Invoke();
    }
}

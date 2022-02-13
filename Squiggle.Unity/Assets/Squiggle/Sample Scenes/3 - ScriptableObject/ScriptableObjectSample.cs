using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Squiggle;
using Squiggle.Unity;

public class ScriptableObjectSample : MonoBehaviour
{
    public Text SpeakerName;
    public Text SpeakerText;
    public Button RunSquiggle;
    public SquiggleScriptableObject ScriptableObject;
    void OnEnable()
    {
        SpeakerName.text = "";
        SpeakerText.text = "";

        RunSquiggle.onClick.AddListener(() => {
            EventSystem.current.SetSelectedGameObject(null);
            StartCoroutine(TryExecuteSquiggle());
        });
    }
    void OnDisable()
    {
        RunSquiggle.onClick.RemoveAllListeners();
    }

    Squiggle.Runner currentRunner;
    bool currentlyRunning = false;
    IEnumerator TryExecuteSquiggle()
    {
        if(currentlyRunning)
        {
            yield return null;
        }

        currentlyRunning = true;
        currentRunner = Squiggle.Core.Run(  squiggleText : ScriptableObject.SquiggleText,
                                            runnerOptions : new Squiggle.Runner.Options(){
                                                AutoStart = false,
                                                Debug = true,
                                                LogHandler = (text) => Debug.Log(text),
                                                WaitOverride = (command) => StartCoroutine(WaitMs(command)),
                                                DialogHandler = (command) => {
                                                    SpeakerName.text = command.Speaker;
                                                    SpeakerText.text = command.Text;
                                                    command.CommandExecutionComplete?.Invoke();
                                                },
                                        });
                                        
        currentRunner.CompletedExecution += OnRunnerComplete;
        currentRunner.Start();
        yield return null;
    }

    IEnumerator WaitMs(Squiggle.Commands.Wait waitCommand)
    {
        Debug.Log("inside wait ms");
        yield return new WaitForSeconds(waitCommand.WaitMS / 1000f);
        waitCommand.CommandExecutionComplete?.Invoke();
    }

    void OnRunnerComplete()
    {
        currentRunner.CompletedExecution -= OnRunnerComplete;
        currentRunner = null;
        currentlyRunning = false;
        SpeakerName.text = "";
        SpeakerText.text = "";
    }
}

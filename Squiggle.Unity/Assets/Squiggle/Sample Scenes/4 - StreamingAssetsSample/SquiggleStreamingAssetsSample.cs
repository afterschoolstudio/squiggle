using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Squiggle;

public class SquiggleStreamingAssetsSample : MonoBehaviour
{
    public Text SpeakerName;
    public Text SpeakerText;
    public Button RunSquiggle;
    string SquiggleCode;
    void OnEnable()
    {
        SpeakerName.text = "";
        SpeakerText.text = "";

        SquiggleCode = Squiggle.Unity.Utils.ReadTextFileFromStreamingAssets("sampleSquiggleScript.txt");
        //VERY IMPORTANT - you need to replace line endings of loaded in files:
        SquiggleCode = SquiggleCode.Replace("\r\n", "\\r\\n"); //need to replace these due to serialization
        // {Text.Replace("\n", "D")}"); also works - may need to do this as well for OSX line endings


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
        currentRunner = Squiggle.Core.Run(  squiggleText : SquiggleCode,
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

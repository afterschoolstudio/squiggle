using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Squiggle;

public class SquiggleTextBoxSample : MonoBehaviour
{
    public InputField InputField;
    public Button RunSquiggle;
    void OnEnable()
    {
        RunSquiggle.onClick.AddListener(() => {
            EventSystem.current.SetSelectedGameObject(null);
            TryExecuteSquiggle();
        });
    }
    void OnDisable()
    {
        RunSquiggle.onClick.RemoveAllListeners();
    }

    void TryExecuteSquiggle()
    {
        Squiggle.Core.Run(  squiggleText : InputField.text,
                            runnerOptions : new Squiggle.Runner.Options(){
                                Debug = true,
                                LogHandler = (text) => Debug.Log(text)
                            });
    }
}

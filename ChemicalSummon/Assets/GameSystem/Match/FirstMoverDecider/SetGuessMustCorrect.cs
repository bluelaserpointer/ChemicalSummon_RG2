using UnityEngine;

[DisallowMultipleComponent]
public class SetGuessMustCorrect : MonoBehaviour
{
    [SerializeField]
    bool activateOnAwake = true;
    private void Start()
    {
        if(activateOnAwake)
            SetNextGuessTrue();
    }
    public void SetNextGuessTrue()
    {
        MatchManager.FirstMoverDecider.mustCorrect = true;
    }
}

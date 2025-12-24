using System.Collections;
using UnityEngine;

public class BlackScreenFadeController : MonoBehaviour
{
    [SerializeField] private Animator blackScreenAnimator;
    [SerializeField] private StringVariable AnimationBoolName;

    [SerializeField] private EventIndex StartSceneChange;
    [SerializeField] private EventIndex SceneReadyEvent;

    private bool fadingOut;

    private void Awake()
    {
        blackScreenAnimator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        fadingOut = false;
        PlayAnimation(1);
        StartCoroutine(WaitForAnimation("FadeIn"));
    }

    public void FadeOut()
    {
        fadingOut = true;
        PlayAnimation(2);
        StartCoroutine(WaitForAnimation("FadeOut"));
    }

    private void PlayAnimation(int state)
    {
        blackScreenAnimator.SetInteger(AnimationBoolName.value, state);
    }

    private IEnumerator WaitForAnimation(string animName)
    {
        // Wait until Animator is in the correct state
        while (!blackScreenAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            yield return null;

        // Wait until animation finishes
        while (blackScreenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;

        // Handle what happens after the animation ends
        if (fadingOut && animName == "FadeOut")
        {
            Logger.Log("FadeOut");
            EventBus.Publish(StartSceneChange);
        }
        else if (!fadingOut && animName == "FadeIn")
        {
            Logger.Log("FadeIn");
            EventBus.Publish(SceneReadyEvent);
        }
    }
}
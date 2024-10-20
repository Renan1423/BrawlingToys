using UnityEngine;
using MoreMountains.Feedbacks;

public class FeedbacksPlayer : MonoBehaviour {
    [SerializeField] private MMF_Player _stepFeedback;
    [SerializeField] private MMF_Player _shootFeedback;
    [SerializeField] private MMF_Player _meleeFeedback;
    [SerializeField] private MMF_Player _dashFeedback;
    [SerializeField] private MMF_Player _getHitFeedback;
    [SerializeField] private MMF_Player _dieFeedback;

    public void StepFeedbacks() {
        _stepFeedback.PlayFeedbacks();
    }
}

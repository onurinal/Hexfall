using Hexfall.Manager;
using TMPro;
using UnityEngine;

namespace Hexfall.Hex
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform countdown;
        private TextMeshPro countdownText;
        private int currentCountdown;

        private static readonly int Countdown = Animator.StringToHash("Countdown");

        private void Start()
        {
            currentCountdown = Random.Range(5, 10);
            countdownText = countdown.GetComponentInChildren<TextMeshPro>();
            countdownText.text = currentCountdown.ToString();

            EventManager.OnMoveChanged += DecreaseCountdown;
        }

        private void OnDestroy()
        {
            EventManager.OnMoveChanged -= DecreaseCountdown;
        }

        private void DecreaseCountdown()
        {
            if (countdownText == null) return;
            currentCountdown--;
            countdownText.text = currentCountdown.ToString();

            animator.ResetTrigger(Countdown);
            animator.SetTrigger(Countdown);

            if (currentCountdown <= 0)
            {
                EventManager.StartOnGameOverEvent();
            }
        }
    }
}
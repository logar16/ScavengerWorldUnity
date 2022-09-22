using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScavengerWorld
{
    public class GameplayTestUI : MonoBehaviour
    {
        [SerializeField] private Testing player;
        [SerializeField] private Button attackEnemyButton;
        [SerializeField] private Button attackStorageButton;
        [SerializeField] private Button dropOffButton;
        [SerializeField] private Button gatherButton;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button idleButton;
        [SerializeField] private Button resetArenaButton;

        // Start is called before the first frame update
        void Start()
        {
            attackEnemyButton.onClick.AddListener(player.OnAttackEnemyButton);
            attackStorageButton.onClick.AddListener(player.OnAttackStorageButton);
            dropOffButton.onClick.AddListener(player.OnDropoffButton);
            gatherButton.onClick.AddListener(player.OnGatherButton);
            moveButton.onClick.AddListener(player.OnMoveButton);
            idleButton.onClick.AddListener(player.OnIdleButton);
            resetArenaButton.onClick.AddListener(player.OnResetArena);
        }

        private void OnDisable()
        {
            attackEnemyButton.onClick.RemoveAllListeners();
            attackStorageButton.onClick.RemoveAllListeners();
            dropOffButton.onClick.RemoveAllListeners();
            gatherButton.onClick.RemoveAllListeners();
            moveButton.onClick.RemoveAllListeners();
            idleButton.onClick.RemoveAllListeners();
            resetArenaButton.onClick.RemoveAllListeners();
        }
    }
}
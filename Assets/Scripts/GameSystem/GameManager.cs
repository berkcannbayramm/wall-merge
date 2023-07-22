using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBGameStudios.Managers
{
    [DefaultExecutionOrder(-9999)]
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameObject BBGameStudioSystems;
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private Panel panel;

        public bool CanClick { get; set; }
        public MoneyManager MoneyManager => moneyManager;
        public LevelManager LevelManager => levelManager;

        private void Awake()
        {
            DontDestroyOnLoad(BBGameStudioSystems);

            Application.targetFrameRate = 60;

            moneyManager.UpdateMoneyText();
        }

        public void WinState()
        {
            panel.AppearPanel(0);
        }

        public void LoseState()
        {
            panel.AppearPanel(1);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIStats : Singleton<UIStats>
    {
        private class Stats
        {
            public int Id;
            public ServerInventory Inventory;
            public UIPlayerStatsController PlayerStats;

            public Stats(int id, ServerInventory inventory, UIPlayerStatsController playerStats)
            {
                Id = id;
                Inventory = inventory;
                PlayerStats = playerStats;

                Inventory.Parts.OnValueChanged += (_, i) => PlayerStats.SetImage(i, false);
                Inventory.Medalions.OnValueChanged -= (_, i) => PlayerStats.SetImage(0, i > 0);
            }
        }

        [Header("References")]
        [SerializeField]
        private List<UIPlayerStatsController> playerStatsControllers;
        [SerializeField]
        private List<Sprite> playerSprites;

        private Dictionary<int, Stats> playerStats = new Dictionary<int, Stats>();

        public void RegisterServerInventory(ServerInventory serverInventory, int playerId)
        {
            UIPlayerStatsController playerStatsController = playerStatsControllers[0];
            playerStatsControllers.RemoveAt(0);
            playerStatsController.gameObject.SetActive(true);
            playerStatsController.Init(playerSprites[playerId - 1], serverInventory.Parts.Value);
            playerStats.Add(playerId, new Stats(playerId, serverInventory, playerStatsController));
        }
    }
}
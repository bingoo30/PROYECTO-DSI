using UnityEngine;
using System;


namespace UIToolkitDemo
{
    [RequireComponent(typeof(SaveManager))]
    public class GameDataManager : MonoBehaviour
    {

        [SerializeField] GameData m_GameData;
        public GameData GameData { set => m_GameData = value; get => m_GameData; }

        SaveManager m_SaveManager;
        bool m_IsGameDataInitialized;

        void OnEnable()
        {
            MainMenuUIEvents.HomeScreenShown += OnHomeScreenShown;

            CharEvents.CharacterShown += OnCharacterShown;
            CharEvents.LevelPotionUsed += OnLevelPotionUsed;

            SettingsEvents.SettingsUpdated += OnSettingsUpdated;
            SettingsEvents.PlayerFundsReset += OnResetFunds;

            ShopEvents.ShopItemPurchasing += OnPurchaseItem;

            MailEvents.RewardClaimed += OnRewardClaimed;
        }

        void OnDisable()
        {
            MainMenuUIEvents.HomeScreenShown -= OnHomeScreenShown;

            CharEvents.CharacterShown -= OnCharacterShown;
            CharEvents.LevelPotionUsed -= OnLevelPotionUsed;

            SettingsEvents.SettingsUpdated -= OnSettingsUpdated;
            SettingsEvents.PlayerFundsReset -= OnResetFunds;

            ShopEvents.ShopItemPurchasing -= OnPurchaseItem;

            MailEvents.RewardClaimed -= OnRewardClaimed;
        }

        void Awake()
        {
            m_SaveManager = GetComponent<SaveManager>();
        }

        void Start()
        {
            //if saved data exists, load saved data
            m_SaveManager.LoadGame();

            // flag that GameData is loaded the first time
            m_IsGameDataInitialized = true;

            ShowWelcomeMessage();

            UpdateFunds();
            UpdatePotions();
        }

        // transaction methods 
        void UpdateFunds()
        {
            if (m_GameData != null)
                ShopEvents.FundsUpdated?.Invoke(m_GameData);
        }

        void UpdatePotions()
        {
            if (m_GameData != null)
                ShopEvents.PotionsUpdated?.Invoke(m_GameData);
        }

        bool HasSufficientFunds(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return false;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    return m_GameData.gold >= discountedPrice;

                case CurrencyType.Gems:
                    return m_GameData.gems >= discountedPrice;

                case CurrencyType.USD:
                    return true;

                default:
                    return false;
            }
        }

        // do we have enough potions to level up?
        public bool CanLevelUp(CharacterData character)
        {
            if (m_GameData == null || character == null)
                return false;

            return (character.GetXPForNextLevel() <= m_GameData.levelUpPotions);
        }

        void PayTransaction(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return;

            CurrencyType currencyType = shopItem.CostInCurrencyType;

            float discountedPrice = (((100 - shopItem.discount) / 100f) * shopItem.cost);

            switch (currencyType)
            {
                case CurrencyType.Gold:
                    m_GameData.gold -= (uint)discountedPrice;
                    break;

                case CurrencyType.Gems:
                    m_GameData.gems -= (uint)discountedPrice;
                    break;

                // non-monetized placeholder - invoke in-app purchase logic/interface for a real application
                case CurrencyType.USD:
                    break;
            }
        }

        void PayLevelUpPotions(uint numberPotions)
        {
            if (m_GameData != null)
            {
                m_GameData.levelUpPotions -= numberPotions;

                ShopEvents.PotionsUpdated?.Invoke(m_GameData);
            }
        }

        void ReceivePurchasedGoods(ShopItemSO shopItem)
        {
            if (shopItem == null)
                return;

            ShopItemType contentType = shopItem.contentType;
            uint contentValue = shopItem.contentValue;

            ReceiveContent(contentType, contentValue);
        }

        // for gifts or purchases
        void ReceiveContent(ShopItemType contentType, uint contentValue)
        {
            switch (contentType)
            {
                case ShopItemType.Gold:
                    m_GameData.gold += contentValue;
                    UpdateFunds();
                    break;

                case ShopItemType.Gems:
                    m_GameData.gems += contentValue;
                    UpdateFunds();
                    break;

                case ShopItemType.HealthPotion:
                    m_GameData.healthPotions += contentValue;
                    UpdatePotions();
                    UpdateFunds();
                    break;

                case ShopItemType.LevelUpPotion:
                    m_GameData.levelUpPotions += contentValue;

                    UpdatePotions();
                    UpdateFunds();
                    break;
            }
        }

        void ShowWelcomeMessage()
        {
            string message = "Welcome " + "<color=#" + PopUpText.TextHighlight + ">" + GameData.username + "</color>!";

            HomeEvents.HomeMessageShown?.Invoke(message);
        }


        // event-handling methods

        // buying item from ShopScreen, pass button screen position 
        void OnPurchaseItem(ShopItemSO shopItem, Vector2 screenPos)
        {
            if (shopItem == null)
                return;

            // invoke transaction succeeded or failed
            if (HasSufficientFunds(shopItem))
            {
                PayTransaction(shopItem);
                ReceivePurchasedGoods(shopItem);
                ShopEvents.TransactionProcessed?.Invoke(shopItem, screenPos);

                AudioManager.PlayDefaultTransactionSound();
            }
            else
            {
                // notify listeners (PopUpText, sound, etc.)
                ShopEvents.TransactionFailed?.Invoke(shopItem);
                AudioManager.PlayDefaultWarningSound();
            }
        }

        // gift from a Mail Message
        void OnRewardClaimed(MailMessageSO msg, Vector2 screenPos)
        {
            if (msg == null)
                return;

            ShopItemType rewardType = msg.rewardType;

            uint rewardValue = msg.rewardValue;

            ReceiveContent(rewardType, rewardValue);

            ShopEvents.RewardProcessed?.Invoke(rewardType, rewardValue, screenPos);
            AudioManager.PlayDefaultTransactionSound();
        }

        // update values from SettingsScreen
        void OnSettingsUpdated(GameData gameData)
        {

            if (gameData == null)
                return;

            m_GameData.sfxVolume = gameData.sfxVolume;
            m_GameData.musicVolume = gameData.musicVolume;
            m_GameData.dropdownSelection = gameData.dropdownSelection;
            m_GameData.isSlideToggled = gameData.isSlideToggled;
            m_GameData.isToggled = gameData.isToggled;
            m_GameData.theme = gameData.theme;
            m_GameData.username = gameData.username;
            m_GameData.buttonSelection = gameData.buttonSelection;
        }

        // Attempt to level up the character using a potion
        void OnLevelPotionUsed(CharacterData charData)
        {
            if (charData == null)
                return;

            bool isLeveled = false;
            if (CanLevelUp(charData))
            {
                PayLevelUpPotions(charData.GetXPForNextLevel());
                isLeveled = true;
                AudioManager.PlayVictorySound();
            }
            else
            {
                AudioManager.PlayDefaultWarningSound();
            }

            // Notify other objects if level up succeeded or failed
            CharEvents.CharacterLeveledUp?.Invoke(isLeveled);
        }

        void OnResetFunds()
        {
            m_GameData.gold = 0;
            m_GameData.gems = 0;
            m_GameData.healthPotions = 0;
            m_GameData.levelUpPotions = 0;
            UpdateFunds();
            UpdatePotions();
        }

        void OnHomeScreenShown()
        {
            if (m_IsGameDataInitialized)
            {
                ShowWelcomeMessage();
            }
        }

        void OnCharacterShown(CharacterData charData)
        {
            // notify the CharScreen to enable or disable the LevelUpButton VFX
            CharEvents.LevelUpButtonEnabled?.Invoke(CanLevelUp(charData));
        }

    }
}

using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace UIToolkitDemo
{
    // represents one item in the shop
    public class ShopItemComponent
    {
        // Class selectors for item size (normal or wide)
        const string k_SizeNormalClass = "shop-item__size--normal";
        const string k_SizeWideClass = "shop-item__size--wide";

        // ScriptableObject pairing icons with currency/shop item types
        GameIconsSO m_GameIconsData;
        ShopItemSO m_ShopItemData;

        // visual elements
        Label m_Description;
        VisualElement m_ProductImage;
        VisualElement m_Banner;
        Label m_BannerLabel;
        VisualElement m_ContentCurrency;
        Label m_ContentValue;
        VisualElement m_CostIcon;
        Label m_Cost;
        VisualElement m_DiscountBadge;
        Label m_DiscountLabel;
        VisualElement m_DiscountSlash;
        VisualElement m_DiscountIcon;
        VisualElement m_DiscountGroup;
        VisualElement m_SizeContainer;
        Label m_DiscountCost;
        Button m_BuyButton;
        VisualElement m_CostIconGroup;
        VisualElement m_DiscountIconGroup;

        public ShopItemComponent(GameIconsSO gameIconsData, ShopItemSO shopItemData)
        {
            m_GameIconsData = gameIconsData;
            m_ShopItemData = shopItemData;
        }

        public void SetVisualElements(TemplateContainer shopItemElement)
        {
            // query the parts of the ShopItemElement
            m_SizeContainer = shopItemElement.Q("shop-item__container");
            m_Description = shopItemElement.Q<Label>("shop-item__description");
            m_ProductImage = shopItemElement.Q("shop-item__product-image");
            m_Banner = shopItemElement.Q("shop-item__banner");
            m_BannerLabel = shopItemElement.Q<Label>("shop-item__banner-label");
            m_DiscountBadge = shopItemElement.Q("shop-item__discount-badge");
            m_DiscountLabel = shopItemElement.Q<Label>("shop-item__badge-text");
            m_DiscountSlash = shopItemElement.Q("shop-item__discount-slash");
            m_ContentCurrency = shopItemElement.Q("shop-item__content-currency");
            m_ContentValue = shopItemElement.Q<Label>("shop-item__content-value");
            m_CostIcon = shopItemElement.Q("shop-item__cost-icon");
            m_Cost = shopItemElement.Q<Label>("shop-item__cost-price");
            m_DiscountIcon = shopItemElement.Q("shop-item__discount-icon");
            m_DiscountGroup = shopItemElement.Q("shop-item__discount-group");
            m_DiscountCost = shopItemElement.Q<Label>("shop-item__discount-price");
            m_BuyButton = shopItemElement.Q<Button>("shop-item__buy-button");

            m_CostIconGroup = shopItemElement.Q("shop-item__cost-icon-group");
            m_DiscountIconGroup = shopItemElement.Q("shop-item__discount-icon-group");
        }


        // Show the ScriptaboleObject data
        public void SetGameData(TemplateContainer shopItemElement)
        {
            if (m_GameIconsData == null)
            {
                Debug.LogWarning("ShopItemController SetGameData: missing GameIcons ScriptableObject data");
                return;
            }

            if (shopItemElement == null)
                return;

            // basic description and image
            m_Description.text = m_ShopItemData.itemName;
            m_ProductImage.style.backgroundImage = new StyleBackground(m_ShopItemData.sprite);

            // set up the promo banner
            m_Banner.style.display = (HasBanner(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_BannerLabel.style.display = (HasBanner(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_BannerLabel.text = m_ShopItemData.promoBannerText;

            // content value
            m_ContentCurrency.style.backgroundImage = new StyleBackground(m_GameIconsData.GetShopTypeIcon(m_ShopItemData.contentType));
            m_ContentValue.text = " " + m_ShopItemData.contentValue.ToString();

            FormatBuyButton();

            // Use the oversize style if discounted
            if (IsDiscounted(m_ShopItemData))
            {
                m_SizeContainer.AddToClassList(k_SizeWideClass);
                m_SizeContainer.RemoveFromClassList(k_SizeNormalClass);
            }
            else
            {
                m_SizeContainer.AddToClassList(k_SizeNormalClass);
                m_SizeContainer.RemoveFromClassList(k_SizeWideClass);
            }
        }

        // Format the cost and cost currency
        void FormatBuyButton()
        {
            string currencyPrefix = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "$" : string.Empty;
            string decimalPlaces = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? "0.00" : "0";

            if (m_ShopItemData.cost > 0.00001f)
            {
                m_Cost.text = currencyPrefix + m_ShopItemData.cost.ToString(decimalPlaces);
                Sprite currencySprite = m_GameIconsData.GetCurrencyIcon(m_ShopItemData.CostInCurrencyType);

                m_CostIcon.style.backgroundImage = new StyleBackground(currencySprite);
                m_DiscountIcon.style.backgroundImage = new StyleBackground(currencySprite);

                m_CostIconGroup.style.display = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? DisplayStyle.None : DisplayStyle.Flex;
                m_DiscountIconGroup.style.display = (m_ShopItemData.CostInCurrencyType == CurrencyType.USD) ? DisplayStyle.None : DisplayStyle.Flex;

            }
            // if the cost is 0, mark the ShopItem as free and hide the cost currency
            else
            {
                m_CostIconGroup.style.display = DisplayStyle.None;
                m_DiscountIconGroup.style.display = DisplayStyle.None;
                m_Cost.text = "Free";
            }

            // disable/enabled, depending whether the item is discounted
            m_DiscountBadge.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountLabel.text = m_ShopItemData.discount + "%";
            m_DiscountSlash.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountGroup.style.display = (IsDiscounted(m_ShopItemData)) ? DisplayStyle.Flex : DisplayStyle.None;
            m_DiscountCost.text = currencyPrefix + (((100 - m_ShopItemData.discount) / 100f) * m_ShopItemData.cost).ToString(decimalPlaces);
        }

        bool IsDiscounted(ShopItemSO shopItem)
        {
            return (shopItem.discount > 0);
        }

        bool HasBanner(ShopItemSO shopItem)
        {
            return !string.IsNullOrEmpty(shopItem.promoBannerText);
        }

        public void RegisterCallbacks()
        {
            if (m_BuyButton == null)
                return;

            // Store the cost/contents data in each button for later use
            m_BuyButton.userData = m_ShopItemData;
            m_BuyButton.RegisterCallback<ClickEvent>(BuyAction);

            // Prevent the button click from moving the ScrollView
            m_BuyButton.RegisterCallback<PointerMoveEvent>(MovePointerEventHandler);
        }

        // Prevents accidental left-right movement of the mouse from dragging the parent Scrollview
        void MovePointerEventHandler(PointerMoveEvent evt)
        {
            evt.StopImmediatePropagation();
        }

        // Clicking the Buy button on the ShopItem triggers a chain of events:

        //      ShopItemComponent (click the button) -->
        //      ShopController (buy an item) -->
        //      GameDataManager (verify funds)-->
        //      MagnetFXController (play effect on UI)

        void BuyAction(ClickEvent evt)
        {
            VisualElement clickedElement = evt.currentTarget as VisualElement;

            // Retrieve the Shop Item Data previously stored in the custom userData
            ShopItemSO shopItemData = clickedElement.userData as ShopItemSO;

            // Get the RootVisualElement 
            VisualElement rootVisualElement = m_SizeContainer.panel.visualTree;

            // Convert to screen position in pixels
            Vector2 clickPos = new Vector2(evt.position.x, evt.position.y);
            Vector2 screenPos = clickPos.GetScreenCoordinate(rootVisualElement);

            // Notify the ShopController (passes ShopItem data + screen position)
            ShopEvents.ShopItemClicked?.Invoke(shopItemData, screenPos);

            AudioManager.PlayDefaultButtonSound();
        }
    }
}


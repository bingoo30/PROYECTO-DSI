using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class HomeView : UIView
    {
        VisualElement m_PlayLevelButton;
        VisualElement m_LevelThumbnail;

        Label m_LevelNumber;
        Label m_LevelLabel;

        ChatView m_ChatView;
        public ChatView ChatView => m_ChatView;

        public HomeView(VisualElement topElement) : base(topElement)
        {
            m_ChatView = new ChatView(topElement);

            HomeEvents.LevelInfoShown += OnShowLevelInfo;
        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_PlayLevelButton = m_TopElement.Q("home-play__level-button");
            m_LevelLabel = m_TopElement.Q<Label>("home-play__level-name");
            m_LevelNumber = m_TopElement.Q<Label>("home-play__level-number");

            m_LevelThumbnail = m_TopElement.Q("home-play__background");
        }

        protected override void RegisterButtonCallbacks()
        {
            m_PlayLevelButton.RegisterCallback<ClickEvent>(ClickPlayButton);
        }

        // Optional: Unregistering the button callbacks is not strictly necessary
        // in most cases and depends on your application's lifecycle management.
        // You can choose to unregister them if needed for specific scenarios.
        protected void UnregisterButtonCallbacks()
        {
            m_PlayLevelButton.UnregisterCallback<ClickEvent>(ClickPlayButton);
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Dispose()
        {
            base.Dispose();
            HomeEvents.LevelInfoShown -= OnShowLevelInfo;

            UnregisterButtonCallbacks();
        }

        void ClickPlayButton(ClickEvent evt)
        {
            AudioManager.PlayDefaultButtonSound();
            HomeEvents.PlayButtonClicked?.Invoke();
        }

        // Event-handling Methods

        // shows the level information
        public void ShowLevelInfo(int levelNumber, string levelName, Sprite thumbnail)
        {
            m_LevelNumber.text = "Level " + levelNumber;
            m_LevelLabel.text = levelName;
            m_LevelThumbnail.style.backgroundImage = new StyleBackground(thumbnail);
        }

        // event-handling methods

        void OnShowLevelInfo(LevelSO levelData)
        {
            if (levelData == null)
                return;

            ShowLevelInfo(levelData.levelNumber, levelData.levelLabel, levelData.thumbnail);
        }
    }


}

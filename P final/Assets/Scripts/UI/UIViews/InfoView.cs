using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    public class InfoView : UIView
    {
        Button m_GetInfoButton;
        Button m_DocsButton;
        Button m_ForumButton;
        Button m_BlogButton;
        Button m_AssetStoreButton;
        
        string m_GetInfoURL = "https://resources.unity.com/games/user-interface-design-and-implementation-in-unity";
        string m_DocsURL = "https://docs.unity3d.com/Manual/UIElements.html";
        string m_ForumURL = "https://forum.unity.com/forums/ui-toolkit.178/";
        string m_BlogURL = "https://blog.unity.com/topic/user-interface";
        string m_AssetStoreURL = "https://assetstore.unity.com/2d/gui";

        // Constructor Initialize from base class
        public InfoView(VisualElement topElement) : base(topElement)
        {

        }

        protected override void SetVisualElements()
        {
            base.SetVisualElements();
            m_GetInfoButton = m_TopElement.Q<Button>("info-signup__button");
            m_DocsButton = m_TopElement.Q<Button>("info-content__docs-button");
            m_ForumButton = m_TopElement.Q<Button>("info-content__forum-button");
            m_BlogButton = m_TopElement.Q<Button>("info-content__blog-button");
            m_AssetStoreButton = m_TopElement.Q<Button>("info-content__asset-button");
        }

        // Note: unregistering the button callbacks is optional and omitted in this case. Use the
        // UnregisterCallback and UnregisterValueChangedCallback methods to unregister callbacks
        // when necessary.

        protected override void RegisterButtonCallbacks()
        {
            base.RegisterButtonCallbacks();
            m_GetInfoButton.RegisterCallback<ClickEvent>(evt => OpenURL(m_GetInfoURL));
            m_DocsButton.RegisterCallback<ClickEvent>(evt => OpenURL(m_DocsURL));
            m_ForumButton.RegisterCallback<ClickEvent>(evt => OpenURL(m_ForumURL));
            m_BlogButton.RegisterCallback<ClickEvent>(evt => OpenURL(m_BlogURL));
            m_AssetStoreButton.RegisterCallback<ClickEvent>(evt => OpenURL(m_AssetStoreURL));
        }

        static void OpenURL(string URL)
        {
            AudioManager.PlayDefaultButtonSound();
            Application.OpenURL(URL);
        }

    }
}

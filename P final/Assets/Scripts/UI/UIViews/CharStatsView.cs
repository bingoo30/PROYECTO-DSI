using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitDemo
{
    // Manages the Character Statistics UI to the right of the CharScreen
    public class CharStatsView: UIView
    {

        // pairs icons with specific data types
        GameIconsSO m_GameIconsData;

        // inventory, xp, and base data
        CharacterData m_CharacterData;

        // static base data from ScriptableObject
        CharacterBaseSO m_BaseStats;

        // stats tab
        Label m_LevelLabel;

        VisualElement m_CharacterClassIcon;
        Label m_CharacterClass;
        VisualElement m_RarityIcon;
        Label m_Rarity;
        VisualElement m_AttackTypeIcon;
        Label m_AttackType;

        Label m_BasePointsLife;
        Label m_BasePointsDefense;
        Label m_BasePointsAttack;
        Label m_BasePointsAttackSpeed;
        Label m_BasePointsSpecialAttack;
        Label m_BasePointsCriticalHit;

        // skills tab
        VisualElement m_ActiveFrame;
        int m_ActiveIndex;

        VisualElement[] m_SkillIcons = new VisualElement[3];
        SkillSO[] m_BaseSkills = new SkillSO[3];

        Label m_Skill;
        Label m_Category;
        Label m_Tier;
        Label m_Damage;
        Label m_NextTier;

        Button m_NextSkillButton;
        Button m_LastSkillButton;

        // bio tab
        Label m_BioTitle;
        Label m_BioText;

        float timeToNextClick = 0f;
        const float clickCooldown = 0.2f;


        // create a new controller with icon and character data
        public CharStatsView(VisualElement topElement): base(topElement)
        {
            m_GameIconsData = Resources.Load("GameData/GameIcons") as GameIconsSO;
        }

        public override void Dispose()
        {
            base.Dispose();
            UnregisterCallbacks();
        }

        // if the UI already exists, update the character data
        public void UpdateWindow(CharacterData charData)
        {
            m_CharacterData = charData;
            m_BaseStats = charData.CharacterBaseData;
            SetGameData();
            ShowSkill(0);
        }

        // query for Visual Elements
        protected override void SetVisualElements()
        {
            m_LevelLabel = m_TopElement.Q<Label>("char-stats__level");
            m_CharacterClassIcon = m_TopElement.Q("char-stats__class-icon");
            m_CharacterClass = m_TopElement.Q<Label>("char-stats__class-label");
            m_RarityIcon = m_TopElement.Q("char-stats__rarity-icon");
            m_Rarity = m_TopElement.Q<Label>("char-stats__rarity-label");
            m_AttackTypeIcon = m_TopElement.Q("char-stats__attack-type-icon");
            m_AttackType = m_TopElement.Q<Label>("char-stats__attack-type-label");

            m_BasePointsLife = m_TopElement.Q<Label>("char-stats__life-value");
            m_BasePointsDefense = m_TopElement.Q<Label>("char-stats__defense-value");
            m_BasePointsAttack = m_TopElement.Q<Label>("char-stats__attack-value");
            m_BasePointsAttackSpeed = m_TopElement.Q<Label>("char-stats__attack-speed-value");
            m_BasePointsSpecialAttack = m_TopElement.Q<Label>("char-stats__special-attack-value");
            m_BasePointsCriticalHit = m_TopElement.Q<Label>("char-stats__critical-hit-value");

            m_SkillIcons[0] = m_TopElement.Q("char-skills__icon1");
            m_SkillIcons[1] = m_TopElement.Q("char-skills__icon2");
            m_SkillIcons[2] = m_TopElement.Q("char-skills__icon3");

            m_Skill = m_TopElement.Q<Label>("char-skills__label");
            m_Category = m_TopElement.Q<Label>("char-skills__category-label");
            m_Tier = m_TopElement.Q<Label>("char-skills__tier-label");
            m_Damage = m_TopElement.Q<Label>("char-skills__tier-damage-label");
            m_NextTier = m_TopElement.Q<Label>("char-skills__next-tier-label");

            m_NextSkillButton = m_TopElement.Q<Button>("char-skills__next-button");
            m_LastSkillButton = m_TopElement.Q<Button>("char-skills__last-button");

            m_BioTitle = m_TopElement.Q<Label>("char-bio__title");
            m_BioText = m_TopElement.Q<Label>("char-bio__text");

            m_ActiveFrame = m_TopElement.Q("char-skills__active");

        }

        // assign data to each UI Element
        public void SetGameData()
        {
            m_BaseSkills[0] = m_BaseStats.skill1;
            m_BaseSkills[1] = m_BaseStats.skill2;
            m_BaseSkills[2] = m_BaseStats.skill3;

            // set level from CharacterData...
            uint levelNumber = (uint)m_CharacterData.CurrentLevel;
            m_LevelLabel.text = "Level " + levelNumber;

            // class/rarity/attackType
            m_CharacterClass.text = m_BaseStats.characterClass.ToString();
            m_Rarity.text = m_BaseStats.rarity.ToString();
            m_AttackType.text = m_BaseStats.attackType.ToString();

            // TO-DO: missing data validation
            Sprite charClassSprite = m_GameIconsData.GetCharacterClassIcon(m_BaseStats.characterClass);
            m_CharacterClassIcon.style.backgroundImage = new StyleBackground(charClassSprite);

            Sprite raritySprite = m_GameIconsData.GetRarityIcon(m_BaseStats.rarity);
            m_RarityIcon.style.backgroundImage = new StyleBackground(raritySprite);

            Sprite attackTypeSprite = m_GameIconsData.GetAttackTypeIcon(m_BaseStats.attackType);
            m_AttackTypeIcon.style.backgroundImage = new StyleBackground(attackTypeSprite);

            // base points
            m_BasePointsLife.text = m_BaseStats.basePointsLife.ToString();
            m_BasePointsDefense.text = m_BaseStats.basePointsDefense.ToString();
            m_BasePointsAttack.text = m_BaseStats.basePointsAttack.ToString();
            m_BasePointsAttackSpeed.text = m_BaseStats.basePointsAttackSpeed.ToString() + "/s";
            m_BasePointsSpecialAttack.text = m_BaseStats.basePointsSpecialAttack.ToString() + "/s";
            m_BasePointsCriticalHit.text = m_BaseStats.basePointsCriticalHit.ToString();

            // bio
            m_BioTitle.text = m_BaseStats.bioTitle;
            m_BioText.text = m_BaseStats.bio;


            if (m_BaseStats.skill1 != null && m_BaseStats.skill2 != null && m_BaseStats.skill3 != null)
            {
                m_SkillIcons[0].style.backgroundImage = new StyleBackground(m_BaseStats.skill1.sprite);
                m_SkillIcons[1].style.backgroundImage = new StyleBackground(m_BaseStats.skill2.sprite);
                m_SkillIcons[2].style.backgroundImage = new StyleBackground(m_BaseStats.skill3.sprite);
            }
            else
            {
                Debug.LogWarning("CharStatsWindow.SetGameData: " + m_CharacterData.CharacterBaseData.characterName +
                    " missing Skill ScriptableObject(s)");
                return;
            }
        }

        // set up interactive UI elements
        protected override void RegisterButtonCallbacks()
        {
            m_NextSkillButton.RegisterCallback<ClickEvent>(SelectNextSkill);
            m_LastSkillButton.RegisterCallback<ClickEvent>(SelectLastSkill);
            m_SkillIcons[0].RegisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[1].RegisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[2].RegisterCallback<ClickEvent>(SelectSkill);

            // initialize the active frame position after the layout builds
            m_SkillIcons[0].RegisterCallback<GeometryChangedEvent>(InitializeSkillMarker);

            // update the text
            ShowSkill(0);
        }

        // Optional: Unregistering the button callbacks is not strictly necessary
        // in most cases and depends on your application's lifecycle management.
        // You can choose to unregister them if needed for specific scenarios.
        void UnregisterCallbacks()
        {
            m_NextSkillButton.UnregisterCallback<ClickEvent>(SelectNextSkill);
            m_LastSkillButton.UnregisterCallback<ClickEvent>(SelectLastSkill);
            m_SkillIcons[0].UnregisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[1].UnregisterCallback<ClickEvent>(SelectSkill);
            m_SkillIcons[2].UnregisterCallback<ClickEvent>(SelectSkill);

            // initialize the active frame position after the layout builds
            m_SkillIcons[0].UnregisterCallback<GeometryChangedEvent>(InitializeSkillMarker);
        }

        void SelectLastSkill(ClickEvent evt)
        {
            if (Time.time < timeToNextClick)
                return;

            timeToNextClick = Time.time + clickCooldown;

            // only select when clicking directly on the visual element
            m_ActiveIndex--;
            if (m_ActiveIndex < 0)
            {
                m_ActiveIndex = 2;
            }
            ShowSkill(m_ActiveIndex);
            AudioManager.PlayDefaultButtonSound();
        }

        void SelectNextSkill(ClickEvent evt)
        {
            if (Time.time < timeToNextClick)
                return;

            timeToNextClick = Time.time + clickCooldown;
            m_ActiveIndex++;

            if (m_ActiveIndex > 2)
            {
                m_ActiveIndex = 0;
            }

            ShowSkill(m_ActiveIndex);
            AudioManager.PlayDefaultButtonSound();
        }

        void SelectSkill(ClickEvent evt)
        {
            VisualElement clickedElement = evt.target as VisualElement;

            int index = (int)Char.GetNumericValue(clickedElement.name[clickedElement.name.Length - 1]) - 1;
            index = Mathf.Clamp(index, 0, m_BaseSkills.Length - 1);

            ShowSkill(index);

            AudioManager.PlayAltButtonSound();
        }

        void ShowSkill(int index)
        {
            SkillSO skill = m_BaseSkills[index];
            if (skill != null)
            {
                SetSkillData(skill);
                MarkTargetElement(m_SkillIcons[index], 300);
                m_ActiveIndex = index;
            }
        }

        // show the description for a given skill
        void SetSkillData(SkillSO skill)
        {
            m_Skill.text = skill.skillName;
            m_Category.text = skill.GetCategoryText();
            m_Tier.text = skill.GetTierText((int)m_CharacterData.CurrentLevel);
            m_Damage.text = skill.GetDamageText();
            m_NextTier.text = skill.GetNextTierLevelText((int)m_CharacterData.CurrentLevel);
        }

        // set up the active frame after the layout builds
        void InitializeSkillMarker(GeometryChangedEvent evt)
        {
            // set its position over the first icon
            MarkTargetElement(m_SkillIcons[0], 0);
        }

        void MarkTargetElement(VisualElement targetElement, int duration = 200)
        {
            // target element, converted into the root space of the Active Frame
            Vector3 targetInRootSpace = ElementInRootSpace(targetElement, m_ActiveFrame);

            // padding offset
            Vector3 offset = new Vector3(10, 10, 0f);

            m_ActiveFrame.experimental.animation.Position(targetInRootSpace - offset, duration);
        }

        // convert target VisualElement into another element's parent space 
        Vector3 ElementInRootSpace(VisualElement targetElement, VisualElement newElement)
        {
            Vector2 targetInWorldSpace = targetElement.parent.LocalToWorld(targetElement.layout.position);
            VisualElement newRoot = newElement.parent;
            return newRoot.WorldToLocal(targetInWorldSpace);
        }
    }
}
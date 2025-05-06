using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SceneManager : MonoBehaviour
{
    private List<VisualElement> _all_visual_elements = new List<VisualElement>();
    VisualElement _initial_menu;

    VisualElement _startButton_initial_menu;
    VisualElement _exitButton_initial_menu;


    VisualElement _levels_menu;



    VisualElement _game_start_menu;
    VisualElement _startButton_game_start_menu;
    VisualElement _exitButton_game_start_menu;



    VisualElement _pause_menu;
    VisualElement _continueutton_pause_menu;
    VisualElement _exitButton_pause_menu;

    VisualElement _victory_menu;
    VisualElement _next_victory_menu;
    VisualElement _exitButton_victory_menu;


    VisualElement _game_over_menu;
    VisualElement _retry_game_over_menu;
    VisualElement _exitButton_game_over_menu;



    VisualElement _game_menu;



    private void NoContenido()
    {
        foreach (var item in _all_visual_elements)
        {
            item.style.display = DisplayStyle.None;
        }
    }

    private void NoContenidoSttings() {
        _contenidoYellow.style.display = DisplayStyle.None;
        _contenidoGreen.style.display = DisplayStyle.None;
        _contenidoBlue.style.display = DisplayStyle.None;
    }

    private void backToInitial() {
        _initial.style.display = DisplayStyle.Flex;
        //_startButton.style.display = DisplayStyle.Flex;
        //_settingsButton.style.display = DisplayStyle.Flex;
        //_exitButton.style.display = DisplayStyle.Flex;
    }

    private void OnEnable()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement root = uidoc.rootVisualElement;


        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("templates/MenuDerrota");

        _initial_menu = root.Q<VisualElement>("InitialMenu");
        _levels_menu = root.Q<VisualElement>("LevelsMenu");
        _game_start_menu = root.Q<VisualElement>("GameStartMenu");
        _pause_menu = root.Q<VisualElement>("PauseMenu");
        _victory_menu = root.Q<VisualElement>("VictoryMenu");
        _game_over_menu = root.Q<VisualElement>("GameOverMenu");
        _game_menu = root.Q<VisualElement>("GameMenu");


        _all_visual_elements.AddRange(new[]
        {     
            _initial_menu, _levels_menu, _game_start_menu,
            _pause_menu, _victory_menu, _game_over_menu, _game_menu
        });

     
        // Botones Menú Inicial
        _startButton_initial_menu = _initial_menu.Q("startButton");
        _exitButton_initial_menu = _initial_menu.Q("exitButton");

        // Game Start Menu
        _startButton_game_start_menu = _game_start_menu.Q("startButton");
        _exitButton_game_start_menu = _game_start_menu.Q("exitButton");

        // Pause Menu
        _continueutton_pause_menu = _pause_menu.Q("continueButton");
        _exitButton_pause_menu = _pause_menu.Q("exitButton");

        // Victory Menu
        _next_victory_menu = _victory_menu.Q("nextButton");
        _exitButton_victory_menu = _victory_menu.Q("exitButton");

        // Game Over Menu
        _retry_game_over_menu = _game_over_menu.Q("retryButton");
        _exitButton_game_over_menu = _game_over_menu.Q("exitButton");


        _startButton_initial_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Start -> Game Start Menu");
            ShowMenu(_game_start_menu);
        });

        _exitButton_initial_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Initial Menu");
            Application.Quit();
        });

        // GAME START MENU
        _startButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Game Start -> Game");
            ShowMenu(_game_menu);
        });

        _exitButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Game Start");
            ShowMenu(_initial_menu);
        });

        // PAUSE MENU
        _continueutton_pause_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Continue from Pause");
            ShowMenu(_game_menu);
        });

        _exitButton_pause_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Pause to Initial");
            ShowMenu(_initial_menu);
        });

        // VICTORY MENU
        _next_victory_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Next Level from Victory");
            ShowMenu(_levels_menu);
        });

        _exitButton_victory_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Victory");
            ShowMenu(_initial_menu);
        });

        // GAME OVER MENU
        _retry_game_over_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Retry Game");
            ShowMenu(_game_menu);
        });

        _exitButton_game_over_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Game Over");
            ShowMenu(_initial_menu);
        });
    }
}

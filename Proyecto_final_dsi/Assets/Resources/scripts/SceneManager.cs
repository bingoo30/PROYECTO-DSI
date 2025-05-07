using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SceneManager : MonoBehaviour
{
    private VisualElement _menuContainer;
    private List<VisualElement> _allMenus = new();


    VisualElement _initial_menu;

    VisualElement _startButton_initial_menu;
    VisualElement _exitButton_initial_menu;


    VisualElement _levels_menu;
    VisualElement _button_1_levels_menu;
    VisualElement _button_2_levels_menu;
    VisualElement _button_3_levels_menu;
    VisualElement _button_4_levels_menu;
    VisualElement _button_5_levels_menu;
    VisualElement _button_6_levels_menu;
    VisualElement _go_back_levels_menu;



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



    private void HideAllMenus()
    {
        foreach (var item in _allMenus)
        {
            item.style.display = DisplayStyle.None;
        }
    }
    private VisualElement LoadAndAddMenu(string resourceName) {
        var template = Resources.Load<VisualTreeAsset>($"templates/{resourceName}");
        var instance = template.CloneTree();
        _menuContainer.Add(instance);
        instance.style.display = DisplayStyle.None;
        _allMenus.Add(instance);
        Debug.Log("Menu: " + resourceName);
        instance.style.flexGrow = 1;
        return instance;
    }

    private void OnEnable()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement root = uidoc.rootVisualElement;
        _menuContainer = new VisualElement();
        _menuContainer.style.flexGrow = 1;
        _menuContainer.style.flexDirection = FlexDirection.Column;
        _menuContainer.style.width = Length.Percent(100);
        _menuContainer.style.height = Length.Percent(100);
        root.Add(_menuContainer);

       
        // Cargar e instanciar templates
        _initial_menu = LoadAndAddMenu("MenuInicial");
        _levels_menu = LoadAndAddMenu("MenuNiveles");
        //_game_start_menu = LoadAndAddMenu("GameStartMenu");
        _pause_menu = LoadAndAddMenu("MenuPausa");
        _victory_menu = LoadAndAddMenu("MenuVictoria");
        _game_over_menu = LoadAndAddMenu("MenuDerrota");
        //_game_menu = LoadAndAddMenu("GameMenu");


        // Botones Menú Inicial
        _startButton_initial_menu = _initial_menu.Q("startButton");
        _exitButton_initial_menu = _initial_menu.Q("exitButton");
        Debug.Log("Botones del menu inicial");
        // Game Start Menu
        //_startButton_game_start_menu = _game_start_menu.Q("startButton");
        //_exitButton_game_start_menu = _game_start_menu.Q("exitButton");

        _go_back_levels_menu = _levels_menu.Q("BotonVolver");
        _button_1_levels_menu = _levels_menu.Q("botonNivel1");
        _button_2_levels_menu = _levels_menu.Q("botonNivel2");
        _button_3_levels_menu = _levels_menu.Q("botonNivel3");
        _button_4_levels_menu = _levels_menu.Q("botonNivel4");
        _button_5_levels_menu = _levels_menu.Q("botonNivel5");
        _button_6_levels_menu = _levels_menu.Q("botonNivel6");


        // Pause Menu
        _continueutton_pause_menu = _pause_menu.Q("ContinuarBoton");
        _exitButton_pause_menu = _pause_menu.Q("SalirButton");
        Debug.Log("Botones del menu de pausa");

        // Victory Menu
        _next_victory_menu = _victory_menu.Q("SiguienteBoton");
        _exitButton_victory_menu = _victory_menu.Q("SalirButton");
        Debug.Log("Botones del menu de victoria");

        // Game Over Menu
        _retry_game_over_menu = _game_over_menu.Q("ReintentarBoton");
        _exitButton_game_over_menu = _game_over_menu.Q("SalirButton");
        Debug.Log("Botones del menu de derrota");


        _startButton_initial_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Start -> Game Start Menu");
            ShowMenu(_levels_menu);
        });

        _exitButton_initial_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Initial Menu");
            Application.Quit();
        });

        _go_back_levels_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Start -> Game Start Menu");
            ShowMenu(_initial_menu);
        });
        // GAME START MENU
        //_startButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Game Start -> Game");
        //    ShowMenu(_game_menu);
        //});

        //_exitButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Exit from Game Start");
        //    ShowMenu(_initial_menu);
        //});

        // PAUSE MENU
        //_continueutton_pause_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Continue from Pause");
        //    ShowMenu(_game_menu);
        //});

        //_exitButton_pause_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Exit from Pause to Initial");
        //    ShowMenu(_initial_menu);
        //});

        //// VICTORY MENU
        //_next_victory_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Next Level from Victory");
        //    ShowMenu(_levels_menu);
        //});

        //_exitButton_victory_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Exit from Victory");
        //    ShowMenu(_initial_menu);
        //});

        //// GAME OVER MENU
        //_retry_game_over_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Retry Game");
        //    ShowMenu(_game_menu);
        //});

        //_exitButton_game_over_menu.RegisterCallback<ClickEvent>(ev => {
        //    Debug.Log("Exit from Game Over");
        //    ShowMenu(_initial_menu);
        //});

        ShowMenu(_initial_menu);
    }

    private void ShowMenu(VisualElement menu) {
        HideAllMenus();
        menu.style.display = DisplayStyle.Flex;
    }
}

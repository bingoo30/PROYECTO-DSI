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
        VisualElement rootve = uidoc.rootVisualElement;


        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("templates/MenuDerrota");

        _initial = rootve.Q("InitialMenu");
        _settings = rootve.Q("Settings");
        _exit = rootve.Q("Exit");
        _game = rootve.Q("Game");

        Label text = rootve.Q<Label>("ContenidoAzul");
        Label text2 = rootve.Q<Label>("Explicacion");
        Debug.Log(text.name);

        text.text = @"<gradient=""colorGradient""> Difficulty </gradient>";
        text2.text = @"<rotate =""12""><gradient=""colorGradient2""> Puedes jugar con las tres dificultades seleccionadas, a cada minuto se cambia.</rotate></size>";


        _startButton = _initial.Q("startButton");
        _settingsButton = _initial.Q("settingsButton");
        _exitButton = _initial.Q("exitButton");

        _startButton.focusable = true;
        _settingsButton.focusable = true;
        _exitButton.focusable = true;

        _contenidoBlue = _settings.Q("contenidoBlue");
        _contenidoYellow = _settings.Q("contenidoYellow");
        _contenidoGreen = _settings.Q("contenidoGreen");

        _pestanaBlue = _settings.Q("pestanaBlue");
        _pestanaYellow = _settings.Q("pestanaYellow");
        _pestanaGreen = _settings.Q("pestanaGreen");

        _settingbackButton = _settings.Q("backButton");

        _gameBackButton = _game.Q("backButton");


        //UQueryBuilder<VisualElement> builder = new (rootve);


        //List<VisualElement> lista_ve = rootve.Query(className: "blue").ToList();


        //lista_ve.ForEach(element => { Debug.Log(element.name);
        //    element.AddToClassList("yellow");
        //});

        _gameBackButton.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("back");
            NoContenido();
            backToInitial();

        });
        _settingbackButton.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("back");
            NoContenido();
            backToInitial();
        });

        _startButton.RegisterCallback<ClickEvent>(ev =>
        {
            Debug.Log("Start");
            NoContenido();
            _game.style.display = DisplayStyle.Flex;
        });

        _exitButton.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("_exitButton");
            NoContenido();
        });

        _settingsButton.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("_settingsButton");
            NoContenido();
            _settings.style.display = DisplayStyle.Flex;
            _contenidoBlue.style.display = DisplayStyle.Flex;
        });

        _pestanaBlue.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("_pestanaBlue");
            NoContenidoSttings();
            _contenidoBlue.style.display = DisplayStyle.Flex;
        });

        _pestanaYellow.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("_pestanaYellow");
            NoContenidoSttings();
            _contenidoYellow.style.display = DisplayStyle.Flex;
        });

        _pestanaGreen.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("_pestanaGreen");
            NoContenidoSttings();
            _contenidoGreen.style.display = DisplayStyle.Flex;
        });
    }
}

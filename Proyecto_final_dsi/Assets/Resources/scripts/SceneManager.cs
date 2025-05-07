using stars_namespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class SceneManager : MonoBehaviour
{
    private VisualElement _menuContainer;
    private List<VisualElement> _allMenus = new();
    private List<VisualElement> _levelButtons = new();
    private VisualElement _blockerOverlay;

    private StarCustomControls _starSelector;
    private StarCustomControls _gameStartStars;
    private int _selectedStars = 0;
    private int _currentLevel = 0;

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
    VisualElement _continuebutton_pause_menu;
    VisualElement _exitButton_pause_menu;
    VisualElement _victory_pause_menu;

    VisualElement _victory_menu;
    VisualElement _next_victory_menu;
    VisualElement _exitButton_victory_menu;


    VisualElement _game_over_menu;
    VisualElement _retry_game_over_menu;
    VisualElement _exitButton_game_over_menu;



    VisualElement _game_menu;
    VisualElement _pause_game_menu;

    //menu de niveles
    private Label _estrellaTotal;
    private List<NivelData> _niveles;

    public class NivelData
    {
        public int nivel;
        public int estrellas;
    }


    private void HideAllMenus()
    {
        foreach (var item in _allMenus)
        {
            item.style.display = DisplayStyle.None;
        }

        _blockerOverlay.style.display = DisplayStyle.None;
    }
    private VisualElement LoadAndAddMenu(string resourceName) {
        var template = Resources.Load<VisualTreeAsset>($"templates/{resourceName}");
        var instance = template.CloneTree();

        if (resourceName == "MenuNivelPeque" || resourceName == "MenuPausa") {
            instance.style.position = Position.Absolute;
            instance.style.top = 0;
            instance.style.left = 0;
            instance.style.right = 0;
            instance.style.bottom = 0;
        }

        instance.style.display = DisplayStyle.None;
        instance.style.flexGrow = 1;
        _menuContainer.Add(instance);
        _allMenus.Add(instance);

        return instance;
    }

    private void OnEnable()
    {
        UIDocument uidoc = GetComponent<UIDocument>();
        VisualElement root = uidoc.rootVisualElement;
        _menuContainer = new VisualElement {
            style =
           {
                flexGrow = 1,
                flexDirection = FlexDirection.Column,
                width = Length.Percent(100),
                height = Length.Percent(100)
            }
        };
        root.Add(_menuContainer);


        //Bloqueador para que no se pueda interactuar con los botones de los otros menus
        _blockerOverlay = new VisualElement {
            style =
            {
                position = Position.Absolute,
                top = 0,
                left = 0,
                right = 0,
                bottom = 0,
                backgroundColor = new Color(0, 0, 0, 0),
                display = DisplayStyle.None
            },
            pickingMode = PickingMode.Position
        };
        _menuContainer.Add(_blockerOverlay);


        // Cargar e instanciar templates
        _initial_menu = LoadAndAddMenu("MenuInicial");
        _levels_menu = LoadAndAddMenu("MenuNiveles");
        _game_start_menu = LoadAndAddMenu("MenuNivelPeque");
        _victory_menu = LoadAndAddMenu("MenuVictoria");
        _game_over_menu = LoadAndAddMenu("MenuDerrota");
        _game_menu = LoadAndAddMenu("MenuJuego");

        _pause_menu = LoadAndAddMenu("MenuPausa");

        // Botones Men?Inicial
        _startButton_initial_menu = _initial_menu.Q("startButton");
        _exitButton_initial_menu = _initial_menu.Q("exitButton");
        Debug.Log("Botones del menu inicial");
        // Game Start Menu
        _startButton_game_start_menu = _game_start_menu.Q("botonJugar");
        _exitButton_game_start_menu = _game_start_menu.Q("botonVolver");
        _gameStartStars = _game_start_menu.Q<StarCustomControls>();

        _go_back_levels_menu = _levels_menu.Q("BotonVolver");
        _button_1_levels_menu = _levels_menu.Q("botonNivel1");
        _button_2_levels_menu = _levels_menu.Q("botonNivel2");
        _button_3_levels_menu = _levels_menu.Q("botonNivel3");
        _button_4_levels_menu = _levels_menu.Q("botonNivel4");
        _button_5_levels_menu = _levels_menu.Q("botonNivel5");
        _button_6_levels_menu = _levels_menu.Q("botonNivel6");

        _levelButtons.Add(_button_1_levels_menu);
        _levelButtons.Add(_button_2_levels_menu);
        _levelButtons.Add(_button_3_levels_menu);
        _levelButtons.Add(_button_4_levels_menu);
        _levelButtons.Add(_button_5_levels_menu);
        _levelButtons.Add(_button_6_levels_menu);
        _levelButtons.Add(_go_back_levels_menu);

        RegisterLevelButton(_button_1_levels_menu, 1);
        RegisterLevelButton(_button_2_levels_menu, 2);
        RegisterLevelButton(_button_3_levels_menu, 3);
        RegisterLevelButton(_button_4_levels_menu, 4);
        RegisterLevelButton(_button_5_levels_menu, 5);
        RegisterLevelButton(_button_6_levels_menu, 6);

        // Pause Menu
        _continuebutton_pause_menu = _pause_menu.Q("ContinuarBoton");
        _exitButton_pause_menu = _pause_menu.Q("SalirBoton");
        _victory_pause_menu = _pause_menu.Q("VictoriaBoton");
        Debug.Log("Botones del menu de pausa");

        // Victory Menu
        _next_victory_menu = _victory_menu.Q("SiguienteBoton");
        _exitButton_victory_menu = _victory_menu.Q("SalirBoton");
        Debug.Log("Botones del menu de victoria");

        // Game Over Menu
        _retry_game_over_menu = _game_over_menu.Q("ReintentarBoton");
        _exitButton_game_over_menu = _game_over_menu.Q("SalirBoton");
        Debug.Log("Botones del menu de derrota");

        _pause_game_menu = _game_menu.Q("botonPausa");

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
        _startButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Game Start -> Game");
            _blockerOverlay.style.display = DisplayStyle.Flex;
            ShowMenu(_game_menu);
        });

        _exitButton_game_start_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Game Start");
            _game_start_menu.style.display = DisplayStyle.None;
            _blockerOverlay.style.display = DisplayStyle.None;
        });

        _pause_game_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Going to pause menu");
            _blockerOverlay.style.display = DisplayStyle.Flex;
            _pause_menu.style.display = DisplayStyle.Flex;
        });


        // PAUSE MENU
        _continuebutton_pause_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Continue from Pause");
            _pause_menu.style.display = DisplayStyle.None;
            _blockerOverlay.style.display = DisplayStyle.None;
        });

        _exitButton_pause_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Pause to Initial");
            _pause_menu.style.display = DisplayStyle.None;
            _blockerOverlay.style.display = DisplayStyle.None;
            ShowMenu(_game_over_menu);
        });

        _victory_pause_menu.RegisterCallback<ClickEvent>(ev => {
            Debug.Log("Exit from Pause to Victory");
            _pause_menu.style.display = DisplayStyle.None;
            _blockerOverlay.style.display = DisplayStyle.None;
            ShowMenu(_victory_menu);
        });
        //// VICTORY MENU
        _starSelector = _victory_menu.Q<StarCustomControls>();
        if (_starSelector != null)
        {
            _starSelector.Interactivo = true;
            _starSelector.OnValueChanged += HandleStarSelection;
            _starSelector.Valor = 0;
        }

        _next_victory_menu.RegisterCallback<ClickEvent>(ev =>
        {
            Debug.Log("Next Level from Victory");
            SaveStars();
            ShowMenu(_levels_menu);
        });

        _exitButton_victory_menu.RegisterCallback<ClickEvent>(ev =>
        {
            Debug.Log("Exit from Victory");
            ResetStarSelector();
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

        //menu NIVELES
        _estrellaTotal = _levels_menu.Q<Label>("NumeroEstrellas");
        InitializeLevelMenu();

        ShowMenu(_initial_menu);
    }

    private void ShowMenu(VisualElement menu) {
        HideAllMenus();
        menu.style.display = DisplayStyle.Flex;
    }

    void RegisterLevelButton(VisualElement button, int levelNumber) {
        button.RegisterCallback<ClickEvent>(ev => {
            Debug.Log($"Seleccionado Nivel {levelNumber}");
            _currentLevel = levelNumber;

            _blockerOverlay.style.display = DisplayStyle.Flex;
            _game_start_menu.style.display = DisplayStyle.Flex;

            Label nivelPequeLabel = _game_start_menu.Q<Label>("NivelPeque");
            if (nivelPequeLabel != null) {
                nivelPequeLabel.text = $"Nivel {levelNumber}";
            }
            if (_gameStartStars != null)
            {
                var nivelData = _niveles.Find(n => n.nivel == levelNumber);
                if (nivelData != null)
                {
                    _gameStartStars.Valor = nivelData.estrellas;
                }
            }
        });
    }
    private void HandleStarSelection(int starCount)
    {
        _selectedStars = starCount;
        Debug.Log($"Estrellas seleccionadas: {_selectedStars}");
    }
    private void SaveStars()
    {
        if (_currentLevel > 0 && _selectedStars > 0)
        {
            // Actualizar datos locales
            int index = _currentLevel - 1;
            if (index >= 0 && index < _niveles.Count)
            {
                _niveles[index].estrellas = _selectedStars;
            }

            // Guardar en JSON
            JsonHelperEstrellas.SaveStars(_currentLevel, _selectedStars);

            // Actualizar UI
            updateEstrellasNivel(_currentLevel);
            UpdateTotalStars();
        }

        ResetStarSelector();
    }
    private void ResetStarSelector()
    {
        _selectedStars = 0;
        if (_starSelector != null)
        {
            _starSelector.Valor = 0;
            _starSelector.Interactivo = true;
        }
    }

    private void InitializeLevelMenu()
    {
        // Cargar datos existentes
        var datosJSON = JsonHelperEstrellas.LoadAllStars();
        if (_niveles == null)
        {
            _niveles = new List<NivelData>();
        }
        else
        {
            _niveles.Clear();
        }


        foreach (var estrellaNivel in datosJSON.niveles)
        {
            _niveles.Add(new NivelData
            {
                nivel = estrellaNivel.nivel,
                estrellas = estrellaNivel.estrellas
            });
        }

        //si no hay nada del json, inicializar por defecto
        if (_niveles.Count == 0)
        {
            for (int i = 1; i <= 6; i++)
            {
                _niveles.Add(new NivelData { nivel = i, estrellas = 0 });
                JsonHelperEstrellas.SaveStars(i, 0);
            }
        }

        ConfigurarEstrellas();
        UpdateTotalStars();
    }
    private void updateEstrellasNivel(int levelNumber)
    {
        string templateActual = $"botonNivel{levelNumber}";
        var template = _levels_menu.Q<TemplateContainer>(templateActual);

        if (template != null)
        {
            var estrellasControl = template.Q<StarCustomControls>();
            if (estrellasControl != null)
            {
                estrellasControl.Valor = _selectedStars;
            }
        }
    }
    private void ConfigurarEstrellas()
    {
        for (int i = 0; i < _niveles.Count; i++)
        {
            var nivel = _niveles[i];
            string nombreTemplate = $"botonNivel{nivel.nivel}";

            VisualElement template = _levels_menu.Q<VisualElement>(nombreTemplate);

            if (template != null)
            {
                var contenedor = template.Q<VisualElement>("botonNivel");
                // Estrellas
                StarCustomControls estrellas = contenedor.Q<StarCustomControls>();
                if (estrellas != null)
                {
                    estrellas.Valor = nivel.estrellas >= 0 ? nivel.estrellas : 0;
                    estrellas.Interactivo = false;
                }
            }
        }
    }
    private void UpdateTotalStars()
    {
        int total = 0;
        foreach (var nivel in _niveles)
        {
            total += nivel.estrellas;
        }
        _estrellaTotal.text = total.ToString();
    }

}

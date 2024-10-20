using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ToyBox.Components;
using Myra.Graphics2D.UI; // Required for Myra UI
using Myra;
using Apos.Camera;
using Apos.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace ToyBox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ComponentsRegistry registry;

        private Menu _menu;

       private Camera camera;

        SpritesManager manager;
        private GameState gameState;

        int selectedOut = 0;

        int selectedComponent = -1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            this.Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //register the component types
            registry = new ComponentsRegistry();
            registry.RegisterBuiltin(new BasicGateComponent(GateType.NOT), "not");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.AND), "and");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.OR), "or");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.XOR), "xor");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.NAND), "nand");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.NOR), "nor");
            registry.RegisterBuiltin(new BasicGateComponent(GateType.XNOR), "xnor");
            registry.RegisterBuiltin(new LEDComponent(1), "Yellow LED");
            registry.RegisterBuiltin(new LEDComponent(2), "Green LED");
            registry.RegisterBuiltin(new LEDComponent(3), "Blue LED");
            registry.RegisterBuiltin(new LEDComponent(4), "Red LED");
            registry.RegisterBuiltin(new ButtonComponent(), "button");

            //run unit tests
            UnitTests tests = new UnitTests();
            tests.RunTests(registry);

            gameState = new GameState();

            

            // Initialize Myra
            MyraEnvironment.Game = this;

            _menu = new Menu();
            _menu.InitializeMenu(); // Sets up the UI elements

            ResetButtons();

            IVirtualViewport defaultViewport = new DefaultViewport(GraphicsDevice, Window);

            camera = new Camera(defaultViewport);

            camera.SetViewport();
            

            camera.XY = new Vector2(0, 0);
            camera.Scale = new Vector2(1f, 1f);
            camera.Rotation = 0;//MathHelper.PiOver4

            // Make sure you're registering properly initialized components
            base.Initialize();
        }

        void ResetButtons()
        {
            List<string> buttons = new List<string>();
            foreach (var name in registry.GetNames())
            {
                buttons.Add(name);
            }

            _menu.RefreshButtons(buttons.ToArray());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            manager = new SpritesManager(Content);
        }

        bool lastPressed = false;

        int customCount = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                selectedOut = 0;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                selectedOut = 1;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                selectedOut = 2;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                selectedOut = 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                selectedOut = 4;
            }

            bool pressed = false;
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                pressed = true;
            }
            else
            {
                pressed = false;
            }

            if(pressed && !lastPressed && !_menu.IsOnUI())
            {
                float range = 20f;
                string selected = _menu.GetSelected();
                Vector2 pos = camera.ScreenToWorld(Mouse.GetState().Position.ToVector2());
                if (selected == "none")
                {
                    int component = gameState.GetComponent(pos, range);
                    if (component >= 0)
                    {
                        gameState.ToggleComponent(component);
                    }
                }
                else if (selected == "clear")
                {
                    gameState.Clear();
                    _menu.SetSelectedGateType("none");
                }
                else if (selected == "new")
                {
                    List<ComponentInstance> components = gameState.GetComponents();
                    ComponentType button = registry.Get("button");
                    ComponentType[] lights = [
                        registry.Get("Yellow LED"),
                        registry.Get("Green LED"),
                        registry.Get("Blue LED"),
                        registry.Get("Red LED"),
                        ];

                    List<int> inputs = new List<int>();
                    List<(int, int)> outputs = new List<(int, int)>();

                    int i = 0;
                    foreach (var component in components)
                    {
                        if (component.Type() == button)
                        {
                            inputs.Add(i);
                        }
                        else if (lights.Contains(component.Type()))
                        {
                            outputs.Add((i, 0));
                        }
                        i++;
                    }

                    CompoundComponent compound = new CompoundComponent(gameState.Clone(), inputs.ToArray(), outputs.ToArray(), registry);

                    registry.Register(compound, "custom_" + customCount);
                    customCount++;
                    _menu.SetSelectedGateType("none");
                    ResetButtons();
                    gameState.Clear();

                }
                else if (selected == "wire")
                {
                    int component = gameState.GetComponent(pos, range);
                    if (component >= 0)
                    {
                        if (selectedComponent == -1)
                        {
                            selectedComponent = component;
                        }
                        else
                        {
                            gameState.AddConnection(selectedComponent, selectedOut, component);
                            selectedComponent = -1;
                            _menu.SetSelectedGateType("none");
                        }
                    }
                }
                else
                {
                    gameState.AddComponent(registry.Get(selected), pos);
                    _menu.SetSelectedGateType("none");
                }
            }

            lastPressed = pressed;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.SetViewport();
            DrawBackground();

            _spriteBatch.Begin(transformMatrix: camera.View);

            gameState.Render(_spriteBatch, manager, registry);

            _spriteBatch.End();

            DrawForeground();
            camera.ResetViewport();

            _menu.Draw(selectedOut + 1);
        }

        
        private void DrawBackground() {
            // This gives you a matrix that is pushed under the ground plane.
            _spriteBatch.Begin(transformMatrix: camera.GetView(-1));

            // Draw the background.

            _spriteBatch.End();
        }

        private void DrawForeground() {
            _spriteBatch.Begin(transformMatrix: camera.View);

            // Draw the foreground.

            _spriteBatch.End();
        }

    }
}

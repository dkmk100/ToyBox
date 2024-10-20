using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ToyBox.Components;
using Myra.Graphics2D.UI; // Required for Myra UI
using Myra;
using Apos.Camera;
using Apos.Input;
using System.Collections.Generic;


namespace ToyBox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D gatePlaceholder;
        private ComponentsRegistry registry;

        private Menu _menu;

       private Camera camera;

        private GameState gameState;

        int selectedOut = 0;

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
            registry.RegisterBuiltin(new ButtonComponent(), "button");

            //run unit tests
            UnitTests tests = new UnitTests();
            tests.RunTests(registry);

            gameState = new GameState();

            

            // Initialize Myra
            MyraEnvironment.Game = this;

            _menu = new Menu();
            _menu.InitializeMenu(); // Sets up the UI elements

            List<string> buttons = new List<string>();
            buttons.Add("none");
            buttons.Add("wire");
            foreach (var name in registry.GetNames())
            {
                buttons.Add(name);
            }

            _menu.RefreshButtons(buttons.ToArray());

            IVirtualViewport defaultViewport = new DefaultViewport(GraphicsDevice, Window);

            camera = new Camera(defaultViewport);

            camera.SetViewport();
            

            camera.XY = new Vector2(100, 50);
            camera.Scale = new Vector2(2f, 2f);
            camera.Rotation = MathHelper.PiOver4;

                // Make sure you're registering properly initialized components
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gatePlaceholder = Content.Load<Texture2D>("gate");
            // TODO: use this.Content to load your game content here
        }

        bool lastPressed = false;

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

            bool pressed = false;
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                pressed = true;
            }
            else
            {
                pressed = false;
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

            // Your draw code.

            _spriteBatch.End();

            DrawForeground();
            camera.ResetViewport();

            _menu.Draw();

            camera.Z = 10f;
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

        public void DrawCamera() {
            camera.SetViewport();
            camera.ResetViewport();
        }

    }
}

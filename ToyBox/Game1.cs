﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ToyBox.Components;
using Myra.Graphics2D.UI; // Required for Myra UI
using Myra;
using Apos.Camera;
using Apos.Input;
using Track = Apos.Input.Track;


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
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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

            // Initialize Myra
            MyraEnvironment.Game = this;

            _menu = new Menu();
            _menu.InitializeMenu(); // Sets up the UI elements

            IVirtualViewport defaultViewport = new DefaultViewport(GraphicsDevice, Window);

            camera = new Camera(defaultViewport);

            camera.SetViewport();
            _spriteBatch.Begin(transformMatrix: camera.View);

            // Your draw code.

            _spriteBatch.End();
            camera.ResetViewport();

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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            camera.SetViewport();
            DrawBackground(camera);
            DrawForeground(camera);
            camera.SetViewport();

            camera.Z = 10f;

        }

        
        private void DrawBackground(Camera c) {
            // This gives you a matrix that is pushed under the ground plane.
            _spriteBatch.Begin(transformMatrix: c.GetView(-1));

            // Draw the background.

            _spriteBatch.End();
        }

        private void DrawForeground(Camera c) {
            _spriteBatch.Begin(transformMatrix: c.View);

            // Draw the foreground.

            _spriteBatch.End();
        }

        public void DrawCamera(Camera c) {
            c.SetViewport();
            c.ResetViewport();
        }

    }
}

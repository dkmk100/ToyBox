using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ToyBox.Components;
using Myra.Graphics2D.UI; // Required for Myra UI
using Myra;


namespace ToyBox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D gatePlaceholder;
        private ComponentsRegistry registry;

        private Menu _menu;

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

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(gatePlaceholder, new Vector2(0, 0), GraphicsDevice.Viewport.Bounds, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            _spriteBatch.End();
            _menu.Draw();
            base.Draw(gameTime);
        }
    }
}

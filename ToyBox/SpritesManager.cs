using Microsoft.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;

namespace ToyBox
{
    public class SpritesManager
    {
        Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>();
        ContentManager content;

        public SpritesManager(ContentManager content)
        {
            this.content = content;
        }
        public Texture2D getSprite(string name)
        {
            Texture2D texture;
            if (cache.TryGetValue(name, out texture)) {
                return texture;
            }
            else
            {
                texture = content.Load<Texture2D>("Assets/"+name);
                cache.Add(name, texture);
                return texture;
            }
        }

        public void Render(SpriteBatch batch, string name, Vector2 pos, float scale)
        {
            Texture2D sprite = getSprite(name);

            batch.Draw(sprite, pos, null, Color.White, 0f, new Vector2(sprite.Width / 2f, sprite.Height / 2f), scale, SpriteEffects.None, 0f);
        }
    }
}

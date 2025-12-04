using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace _103._Galactic_Sharp;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;
    private Starfield _starfield;
    private SoundManager _soundManager;
    private Arena _arena;
    private CollisionManager _collisionManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Configuration de la fenêtre
        Window.Title = "Galactic-Sharp";
        Window.AllowUserResizing = false;

        // Résolution 720p
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        // Centrage de la fenêtre
        int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        Window.Position = new Point(
            (screenWidth - _graphics.PreferredBackBufferWidth) / 2,
            (screenHeight - _graphics.PreferredBackBufferHeight) / 2
        );

        // Initialisation du gestionnaire de texte
        TextRenderer.Initialize(GraphicsDevice);

        // Initialisation du champ d'étoiles (200 étoiles)
        _starfield = new Starfield(GraphicsDevice, 200);

        // Initialisation du gestionnaire de son
        _soundManager = new SoundManager();

        // Initialisation du gestionnaire de jeu
        _gameManager = new GameManager(GraphicsDevice);

        // Initialisation de l'arène
        _arena = new Arena(GraphicsDevice);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Chargement des textures de vaisseaux depuis le dossier assets
        List<Texture2D> ships = new List<Texture2D>();

        string[] shipFiles = { "ships_0.png", "ships_1.png", "ships_2.png" };

        foreach (string file in shipFiles)
        {
            string path = Path.Combine("assets", file);
            if (File.Exists(path))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    ships.Add(Texture2D.FromStream(GraphicsDevice, stream));
                }
            }
        }

        _gameManager.LoadContent(ships);

        // Chargement du son moteur
        string soundPath = Path.Combine("assets", "propel_ships.wav");
        if (File.Exists(soundPath))
        {
            _soundManager.LoadSound("engine", soundPath);
            _gameManager.SetEngineSound(_soundManager.GetSound("engine"));
        }

        // Chargement du son de collision
        string collisionSoundPath = Path.Combine("assets", "colision_ships.wav");
        if (File.Exists(collisionSoundPath))
        {
            _soundManager.LoadSound("collision", collisionSoundPath);
        }

        // Chargement du son de tir
        string fireSoundPath = Path.Combine("assets", "fire_ships.wav");
        if (File.Exists(fireSoundPath))
        {
            _soundManager.LoadSound("fire", fireSoundPath);
            _gameManager.SetFireSound(_soundManager.GetSound("fire"));
        }

        // Chargement du son de texte
        string textSoundPath = Path.Combine("assets", "text_sound.wav");
        if (File.Exists(textSoundPath))
        {
            _soundManager.LoadSound("text", textSoundPath);
            _gameManager.SetTextSound(_soundManager.GetSound("text"));
        }

        // Initialisation du gestionnaire de collisions
        _collisionManager = new CollisionManager(_arena.Bounds, _soundManager.GetSound("collision"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _starfield.Update(gameTime);
        _gameManager.Update(gameTime);

        // Collisions seulement pendant la phase de jeu
        if (_gameManager.CurrentState == GameState.Playing)
        {
            _collisionManager.Update(_gameManager.Players, _gameManager.Projectiles);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _starfield.Draw(_spriteBatch);
        _arena.Draw(_spriteBatch);
        _gameManager.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

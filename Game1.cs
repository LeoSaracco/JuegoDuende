using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace JuegoDuende
{
	/// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Color color; 
		Texture2D sprite;
		Texture2D bomb;
		Texture2D coin;
		Texture2D fondo;
		Texture2D fondo2,fondo3,fondo4;
		Vector2 coordenadas;
		Vector2 posicion;
		Vector2 cartel;
		Vector2 inicio1;
		SpriteFont fuente1;
		SpriteFont fuente2;
		Rectangle rectanguloduende;
		Rectangle rectangulomoneda;
		Rectangle rectangulobomba;
		List<Vector2> posicionesmoneda = new List<Vector2> ();
		List<JuegoDuende> animacionmoneda = new List<JuegoDuende> ();
		List<Vector2> posicionesbomba = new List<Vector2> ();
		double probabilidadmoneda = 0.04;
		double probabilidadbomba = 0.03;
		int velocidadmoneda = 3;
		int velocidadbomba = 3;
		Random aleatorio = new Random ();
		Random aleatorio2 = new Random ();
		int velocidadsprite1 = 5;
		public SoundEffect Efectomoneda;
		public SoundEffect Efectobomba;
		Song Musica;
		JuegoDuende playerAnimation;
		int ind=0, vida=3, puntaje=0;
		bool vida1=true;
		bool inicio=false;
		bool pausa=false;
		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "../../Content";	            
			graphics.IsFullScreen = true;		

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
			posicion = Vector2.Zero;
			coordenadas = new Vector2 (50,550);
			cartel = new Vector2 (0, 100);
			inicio1 = new Vector2 (60, 200);
		}
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
          	spriteBatch = new SpriteBatch(GraphicsDevice);
			playerAnimation = new JuegoDuende (); 
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			sprite = Content.Load<Texture2D> ("Sprite/Duende1");
			fondo = Content.Load<Texture2D> ("Fondo/fondo");
			fondo2 = Content.Load<Texture2D> ("Fondo/fondo2");
			fondo3 = Content.Load<Texture2D> ("Fondo/Fondo3");
			fondo4 = Content.Load<Texture2D> ("Fondo/Fondo4");
			fuente1 = Content.Load<SpriteFont> ("Fuentes/fuente1"); 
			fuente2 = Content.Load<SpriteFont> ("Fuentes/fuente2"); 
			coin = Content.Load<Texture2D> ("Sprite/coin");
			bomb = Content.Load<Texture2D> ("Sprite/Bomba");
			Musica = Content.Load<Song>("Musica/Major_Lazer_LeanOn.wav");
			MediaPlayer.Play(Musica);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.3f;
			Efectobomba=Content.Load<SoundEffect> ("SonidoBomba/Bomb");
			Efectomoneda = Content.Load<SoundEffect> ("SonidoMoneda/Coin.wav");

            //TODO: use this.Content to load your game content here 
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
			if ((inicio == true)&&(pausa == false)) {
				color = Color.White;
				KeyboardState keyboard = Keyboard.GetState ();
				if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
					Exit ();
				}
				if (keyboard.IsKeyDown (Keys.P))
				 {
					pausa=true;
					spriteBatch.DrawString (fuente1, "Pausa", inicio1, Color.Yellow);
					if (keyboard.IsKeyDown (Keys.P)) {
						pausa = false;//Para que vuelva al juego de nuevo si el chabon quiere sacar la pausa

					}

				}


			
				if (keyboard.IsKeyDown (Keys.Escape))
					this.Exit ();
				if (keyboard.IsKeyUp (Keys.Up)) {
					sprite = Content.Load<Texture2D> ("Sprite/Duende1");
				}

				if (keyboard.IsKeyDown (Keys.Up))
					coordenadas.Y -= 0;
				else if (keyboard.IsKeyDown (Keys.Down))
					coordenadas.Y += 0;
				else if (keyboard.IsKeyDown (Keys.Right)) {
					sprite = Content.Load<Texture2D> ("Sprite/Duende2");
					coordenadas.X += velocidadsprite1;
				} else if (keyboard.IsKeyDown (Keys.Left))
					coordenadas.X -= velocidadsprite1;


				//calcula el rectangulo del duende.
				rectanguloduende =
				new Rectangle ((int)coordenadas.X, (int)coordenadas.Y,
				               sprite.Width, sprite.Height);

				//Calcula que el personaje no se vaya de la pantalla.
				coordenadas.X = MathHelper.Clamp (coordenadas.X, 
				                                 0, Window.ClientBounds.Width - sprite.Width);
				coordenadas.Y = MathHelper.Clamp (coordenadas.Y, 
				                                 0, Window.ClientBounds.Height - sprite.Height);

				//Calcula la probabilidad de monedas.
				if ((aleatorio.NextDouble () < probabilidadmoneda) && (aleatorio.NextDouble () != aleatorio2.NextDouble ())) { 
					ind++;
					float x = (float)aleatorio.NextDouble () * 
						Window.ClientBounds.Width; 
					posicionesmoneda.Add (new Vector2 (x, 0));
					animacionmoneda.Add (new JuegoDuende ());

				}	
				//Cuenta la animacion de la moneda.
				for (int i = 0; i < posicionesmoneda.Count; i++) { 
					// asigno las posiciones de las monedas
					animacionmoneda [i].Initialize (coin, posicionesmoneda [i], 44, 40, 10, 2200, Color.White, 0.8f, true);
				}

				//Cuenta la posicion de la moneda.
				for (int i = 0; i < posicionesmoneda.Count; i++) { 
					// actualizo las posiciones de las monedas
					posicionesmoneda [i] = new Vector2 (posicionesmoneda [i].X, 
					                                   posicionesmoneda [i].Y + velocidadmoneda);
					animacionmoneda [i].Update (gameTime);

					//Calcula el rectangulo de la moneda.
					rectangulomoneda =
					new Rectangle ((int)posicionesmoneda [i].X, (int)posicionesmoneda [i].Y,
					               coin.Width / 10, coin.Height); 

					// eliminar las monedas cuando salen de la pantalla 
					if (posicionesmoneda [i].Y > Window.ClientBounds.Height) { 
						posicionesmoneda.RemoveAt (i);
						// decrecemos i, por que hay una moneda menos 
						i--;
					}
					if (rectanguloduende.Intersects (rectangulomoneda)) {
						posicionesmoneda.RemoveAt (i);
						Efectomoneda.Play ();
						puntaje = puntaje + 10;
					}

				}
				// aparecen nuevos enemigos según la probabilidad 
				if (aleatorio2.NextDouble () < probabilidadbomba) { 
					float x = (float)aleatorio2.NextDouble () * 
						Window.ClientBounds.Width; 
					posicionesbomba.Add (new Vector2 (x, 0)); 
				}
				//Cuenta la posicion de la bomba.
				for (int i = 0; i < posicionesbomba.Count; i++) { 
					// actualizo las posiciones de las bombas
					posicionesbomba [i] = new Vector2 (posicionesbomba [i].X, 
					                                  posicionesbomba [i].Y + velocidadbomba);
					//Calcula el rectangulo de la bomba.
					rectangulobomba =
					new Rectangle ((int)posicionesbomba [i].X, (int)posicionesbomba [i].Y,
					               bomb.Width, bomb.Height); 
					// eliminar las bombas cuando salen de la pantalla.
					if (posicionesbomba [i].Y > Window.ClientBounds.Height) { 
						posicionesbomba.RemoveAt (i);
						// decrecemos i, por que hay una bomba menos.
						i--;
					}

					if (rectanguloduende.Intersects (rectangulobomba)) {
						posicionesbomba.RemoveAt (i);
						vida = vida - 1;
						Efectobomba.Play ();
						color = Color.Red;
					}
				}
				if (puntaje == 150) {
					fondo = fondo2;
				}
				if (puntaje == 200) {
					fondo = fondo3;
				}
				if (puntaje == 250) {
					fondo = fondo4;
				}
				if ((puntaje == 100) && (vida1 == true)) {
					vida++;
					vida1 = false;
				}
				if (vida == 0) {
					spriteBatch.DrawString (fuente1, "Perdiste!!!", inicio1, Color.Yellow);
					pausa = true;
				}

				// TODO: Add your update logic here			
				base.Update (gameTime);

        	}
		}
	
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin (); 
			spriteBatch.Draw (fondo, posicion, Color.White); 
			spriteBatch.DrawString (fuente2, "Vidas: " + vida, cartel, Color.Red);
			spriteBatch.DrawString (fuente1, "Puntos: " + puntaje, posicion, Color.Orange);
			spriteBatch.Draw (sprite, rectanguloduende,color);
			KeyboardState keyboard = Keyboard.GetState ();
			if (keyboard.IsKeyDown (Keys.Enter)) {
				inicio = true;
				}
			
				if (inicio == false) {
					spriteBatch.DrawString (fuente1, "Presione enter para comenzar!", inicio1, Color.Yellow);
				
			}

			for (int i = 0; i < posicionesmoneda.Count; i++) 
			{ 
				animacionmoneda [i].Draw (spriteBatch);
			}
			playerAnimation.Draw(spriteBatch);

			foreach (Vector2 posicionbomba in posicionesbomba) { 
				spriteBatch.Draw (bomb, posicionbomba, Color.White);
			}

			spriteBatch.End ();
			//TODO: Add your drawing code here
            
            base.Draw(gameTime);
        }
    }
}
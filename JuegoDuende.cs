using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JuegoDuende
{
	class JuegoDuende
	{
		// La imagen animada representada por un grupo de imagenes
		Texture2D spriteStrip;
		// Valor para escalar el sprite
		float scale;
		// Tiempo desde la ultima vez que se actualizo la imagen
		int elapsedTime;
		// Tiempo de despliegue por imagen
		int frameTime;
		// Numero de imagenes que conforman la animacion
		int frameCount;
		// Indice de la imagen actual
		int currentFrame;
		// Color de la imagen a desplegar
		Color color;
		// El area de la imagen que vamos a desplegar
		Rectangle sourceRect = new Rectangle();
		// El area donde queremos desplegar la imagen
		Rectangle destinationRect = new Rectangle();
		// Ancho de la una imagen
		public int FrameWidth;
		// Alto de una imagen
		public int FrameHeight;
		// Estado de la animacion
		public bool Active;
		// Repetir animacion
		public bool Looping;
		// Posicion del sprite
		public Vector2 Position;

		public void Initialize(Texture2D texture, Vector2 position,
		                       int frameWidth, int frameHeight, int frameCount,
		                       int frametime, Color color, float scale, bool looping)
		{
			// Mantener copias locales de los valores pasados
			this.color = color;
			this.FrameWidth = frameWidth;
			this.FrameHeight = frameHeight;
			this.frameCount = frameCount;
			this.frameTime = frametime;
			this.scale = scale;

			Looping = looping;
			Position = position;
			spriteStrip = texture;

			// Hacer reset a los tiempos
			elapsedTime = 0;

			currentFrame++;
			if (currentFrame > 10)
				currentFrame = 0;
			// Activar la animacion por defecto
			Active = true;
		}

		public void Update(GameTime gameTime)
		{


			// No actualizar si la imagen esta desactivada
			if (Active == false) 
				return;

			// Actualizar tiempo transcurrido
			elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			// Si elapsedTime es mayor que frame time debemos cambiar de imagen
			if (elapsedTime > frameTime)
			{
				// Movemos a la siguiente imagen
				currentFrame++;
				// Si currentFrame es igual al frameCount 
				// hacemos reset currentFrame a cero
				if (currentFrame == frameCount)
				{
					currentFrame = 0;
					// Si no queremos repetir la animacion
					// asignamos Active a falso
					if (Looping == false)
						Active = false;
				}
				// Reiniciamos elapsedTime a cero
				elapsedTime = 0;
			}
			// Tomamos la imagen correcta multiplicando el currentFrame 
			// por el ancho de la imagen
			sourceRect = new Rectangle(
				currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

			// Actualizamos la posicion de la imagen en escala
			destinationRect = new Rectangle(
				(int)Position.X - (int)(FrameWidth * scale) / 2,
				(int)Position.Y - (int)(FrameHeight * scale) / 2,
				(int)(FrameWidth * scale),
				(int)(FrameHeight * scale));
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Active)
			{
				spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
			}
		}
	}
}

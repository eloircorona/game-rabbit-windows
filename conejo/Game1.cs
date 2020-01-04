using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace conejo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Banderas
        int escenario;

        //Conejo
        Texture2D conejo_izq, conejo_der, vida1, vida2, vida3;
        Vector2 pconejo, pvida1, pvida2, pvida3;
        BoundingBox bconejo;
        int direccion;
        bool cae, sobreplataforma;
        float masa, tiemposuelto, timeup;
        int vida;

        //Menu (escenario 0)
        Texture2D fondo_menu, boton_play, boton_instrucciones, zanahoria;
        Vector2 pboton_play, pboton_instrucciones, pzanahoria;
        BoundingBox bboton_play, bboton_instrucciones;
        int opt;

        //Instrucciones
        Texture2D instrucciones;

        //Game Over
        Texture2D game_over;

        //Escenario 1
        Texture2D fondo_escenario, end;
        Vector2 pfondo_escenario, pend;
        Texture2D plataforma1, plataforma2, plataforma3, plataforma4;
        Vector2 pplataforma1, pplataforma2, pplataforma3, pplataforma4;
        BoundingBox bplataforma1_top, bplataforma2_top, bplataforma3_top, bplataforma4_top;
        BoundingBox bplataforma1_left, bplataforma2_left, bplataforma3_left, bplataforma4_left;
        BoundingBox bplataforma1_right, bplataforma2_right, bplataforma3_right, bplataforma4_right;
        bool escenario_pinicial, movimiento_escenario;
        int next_level;

        Texture2D enemigo1_der, enemigo1_izq, enemigo2_der, enemigo2_izq, enemigo3_der, enemigo3_izq;
        Vector2 penemigo1, penemigo2, penemigo3;
        BoundingBox benemigo1, benemigo2, benemigo3;
        bool mov1, mov2, mov3;
        bool vivo1, vivo2, vivo3;
        int dir1, dir2, dir3;

        //General
        KeyboardState kb;
        Matrix camara;
        int posCamX, posCamY;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
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
            escenario = 0;
            escenario_pinicial = true;
            next_level = 0;

            //Instrucciones
            instrucciones = Content.Load<Texture2D>("img/instrucciones");

            //game_over
            game_over = Content.Load<Texture2D>("img/game_over");

            //Menu
            fondo_menu = Content.Load<Texture2D>("img/menu");
            boton_play = Content.Load<Texture2D>("img/boton_play");
            boton_instrucciones = Content.Load<Texture2D>("img/boton_instrucciones");
            zanahoria = Content.Load<Texture2D>("img/zanahoria");
            
            pboton_play = new Vector2(200, 250);
            pboton_instrucciones = new Vector2(700, 250);
            pzanahoria = new Vector2(100, 320);

            bboton_play = new BoundingBox(new Vector3(pboton_play.X, pboton_play.Y, 0), new Vector3(pboton_play.X + boton_play.Width, pboton_play.Y + boton_play.Height, 0));
            bboton_instrucciones = new BoundingBox(new Vector3(pboton_instrucciones.X, pboton_instrucciones.Y, 0), new Vector3(pboton_instrucciones.X + boton_instrucciones.Width, pboton_instrucciones.Y + boton_instrucciones.Height, 0));

            opt = 1;

            //Conejo
            conejo_izq = Content.Load<Texture2D>("img/conejo_izquierda");
            conejo_der = Content.Load<Texture2D>("img/conejo_derecha");
            vida1 = Content.Load<Texture2D>("img/vida");
            vida2 = Content.Load<Texture2D>("img/vida");
            vida3 = Content.Load<Texture2D>("img/vida");

            pconejo = new Vector2(20, 60);
            pvida1 = new Vector2(10, 20);
            pvida2 = new Vector2(75, 20);
            pvida3 = new Vector2(140, 20);

            bconejo = new BoundingBox(new Vector3(pconejo.X, pconejo.Y, 0), new Vector3(pconejo.X + conejo_der.Width, pconejo.Y + conejo_der.Height, 0));
            
            direccion = 1;
            cae = false;
            sobreplataforma = false;
            masa = 5f;
            tiemposuelto = 0;
            timeup = 0.05f;

            vida = 3;

            //Escenario 1
            fondo_escenario = Content.Load<Texture2D>("img/escenario");
            plataforma1 = Content.Load<Texture2D>("img/plataforma1");
            plataforma2 = Content.Load<Texture2D>("img/plataforma2");
            plataforma3 = Content.Load<Texture2D>("img/plataforma3");
            plataforma4 = Content.Load<Texture2D>("img/plataforma4");
            end = Content.Load<Texture2D>("img/entrada");

            enemigo1_der = Content.Load<Texture2D>("img/serpiente_derecha");
            enemigo1_izq = Content.Load<Texture2D>("img/serpiente_izquierda");
            enemigo2_der = Content.Load<Texture2D>("img/serpiente_derecha");
            enemigo2_izq = Content.Load<Texture2D>("img/serpiente_izquierda");
            enemigo3_der = Content.Load<Texture2D>("img/serpiente_derecha");
            enemigo3_izq = Content.Load<Texture2D>("img/serpiente_izquierda");

            pfondo_escenario = new Vector2(0, 0);
            pplataforma1 = new Vector2(0, 550); //50 x 190
            pplataforma2 = new Vector2(400, 450);
            pplataforma3 = new Vector2(1000, 550);
            pplataforma4 = new Vector2(2300, 450);
            pend = new Vector2(pplataforma4.X + plataforma4.Width - end.Width, pplataforma4.Y - end.Height);

            penemigo1 = new Vector2(pplataforma2.X, pplataforma2.Y - enemigo1_der.Height);
            penemigo2 = new Vector2(pplataforma3.X + plataforma2.Width - enemigo2_der.Width, pplataforma3.Y - enemigo1_der.Height);
            penemigo3 = new Vector2(pplataforma4.X, pplataforma4.Y - enemigo1_der.Height);

            mov1 = true; mov2 = true; mov3 = true;
            dir1 = 1; dir2 = 2; dir3 = 1;
            vivo1 = true; vivo2 = true; vivo3 = true;

            benemigo1 = new BoundingBox(new Vector3(penemigo1.X, penemigo1.Y, 0), new Vector3(penemigo1.X + enemigo1_der.Width, penemigo1.Y + enemigo1_der.Height, 0));
            benemigo2 = new BoundingBox(new Vector3(penemigo2.X, penemigo2.Y, 0), new Vector3(penemigo2.X + enemigo1_der.Width, penemigo2.Y + enemigo1_der.Height, 0));
            benemigo3 = new BoundingBox(new Vector3(penemigo3.X, penemigo3.Y, 0), new Vector3(penemigo3.X + enemigo1_der.Width, penemigo3.Y + enemigo1_der.Height, 0));

            bplataforma1_top = new BoundingBox(new Vector3(pplataforma1.X + 5, pplataforma1.Y, 0), new Vector3(pplataforma1.X + plataforma1.Width - 5, pplataforma1.Y + 5, 0));
            bplataforma1_left = new BoundingBox(new Vector3(pplataforma1.X, pplataforma1.Y + 5, 0), new Vector3(pplataforma1.X + 5, pplataforma1.Y + plataforma1.Height, 0));
            bplataforma1_right = new BoundingBox(new Vector3(pplataforma1.X + plataforma1.Width - 5, pplataforma1.Y + 5, 0), new Vector3(pplataforma1.X + plataforma1.Width, pplataforma1.Y + plataforma1.Height, 0));

            bplataforma2_top = new BoundingBox(new Vector3(pplataforma2.X + 5, pplataforma2.Y, 0), new Vector3(pplataforma2.X + plataforma2.Width - 5, pplataforma2.Y + 5, 0));
            bplataforma2_left = new BoundingBox(new Vector3(pplataforma2.X, pplataforma2.Y + 5, 0), new Vector3(pplataforma2.X + 5, pplataforma2.Y + plataforma2.Height, 0));
            bplataforma2_right = new BoundingBox(new Vector3(pplataforma2.X + plataforma2.Width - 5, pplataforma2.Y + 5, 0), new Vector3(pplataforma2.X + plataforma2.Width, pplataforma2.Y + plataforma2.Height, 0));

            bplataforma3_top = new BoundingBox(new Vector3(pplataforma3.X + 5, pplataforma3.Y, 0), new Vector3(pplataforma3.X + plataforma3.Width - 5, pplataforma3.Y + 5, 0));
            bplataforma3_left = new BoundingBox(new Vector3(pplataforma3.X, pplataforma3.Y + 5, 0), new Vector3(pplataforma3.X + 5, pplataforma3.Y + plataforma3.Height, 0));
            bplataforma3_right = new BoundingBox(new Vector3(pplataforma3.X + plataforma3.Width - 5, pplataforma3.Y + 5, 0), new Vector3(pplataforma3.X + plataforma3.Width, pplataforma3.Y + plataforma3.Height, 0));

            bplataforma4_top = new BoundingBox(new Vector3(pplataforma4.X + 5, pplataforma4.Y, 0), new Vector3(pplataforma4.X + plataforma4.Width - 5, pplataforma4.Y + 5, 0));
            bplataforma4_left = new BoundingBox(new Vector3(pplataforma4.X, pplataforma4.Y + 5, 0), new Vector3(pplataforma4.X + 5, pplataforma4.Y + plataforma4.Height, 0));
            bplataforma4_right = new BoundingBox(new Vector3(pplataforma4.X + plataforma4.Width - 5, pplataforma4.Y + 5, 0), new Vector3(pplataforma4.X + plataforma4.Width, pplataforma4.Y + plataforma4.Height, 0));

            //General
            posCamX = 600;
            posCamY = 300;
            camara = Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, 0)) * Matrix.CreateTranslation(new Vector3(-pconejo, 0));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Menu
            if (opt == 1) pzanahoria.X = 100;
            else if (opt == 2) pzanahoria.X = 600;
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && (!kb.IsKeyDown(Keys.Enter)))
            {
                if (escenario == 0)
                {
                    if (opt == 1) escenario = 1;
                    else if (opt == 2) escenario = 3;
                }
                else if (escenario == 3) escenario = 0;
                else if (escenario == 4) { escenario = 0; opt = 0; }
            }

            //Cargar imagenes escenario 2
            if (escenario == 2)
            {
                fondo_escenario = Content.Load<Texture2D>("img/escenario1");
                plataforma1 = Content.Load<Texture2D>("img/plataforma5");
                plataforma2 = Content.Load<Texture2D>("img/plataforma6");
                plataforma3 = Content.Load<Texture2D>("img/plataforma7");
                plataforma4 = Content.Load<Texture2D>("img/plataforma8");
                end = Content.Load<Texture2D>("img/piramide");

                if (!movimiento_escenario) pfondo_escenario = new Vector2(0, 0);
                if (escenario_pinicial)
                {
                    pconejo = new Vector2(20, 60);
                    posCamX = 600;
                    posCamY = 300;
                    movimiento_escenario = true;
                }
                pplataforma1 = new Vector2(-10, 550); //50 x 190
                pplataforma2 = new Vector2(600, 450);
                pplataforma3 = new Vector2(2000, 550);
                pplataforma4 = new Vector2(3600, 450);
                pend = new Vector2(pplataforma4.X + plataforma4.Width - end.Width, pplataforma4.Y - end.Height);

                vivo1 = true; vivo2 = true; vivo3 = true;

                escenario_pinicial = false;
            }

            //Escenario 4
            if (escenario == 4) vida = 3;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !(kb.IsKeyDown(Keys.Right)))
            {
                if (escenario != 0)
                {
                    if (
                    (!bconejo.Intersects(bplataforma1_left)) &&
                    (!bconejo.Intersects(bplataforma2_left)) &&
                    (!bconejo.Intersects(bplataforma3_left)) &&
                    (!bconejo.Intersects(bplataforma4_left))
                    )
                    { direccion = 1; pconejo.X += 10; }
                    if (pconejo.X <= 3499 && pconejo.X >= 600) { posCamX += 10; pfondo_escenario.X += 10; }
                }
                else
                {
                    opt = 2;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && !(kb.IsKeyDown(Keys.Left)))
            {
                if(escenario != 0)
                {
                    if (
                        (!bconejo.Intersects(bplataforma1_right)) &&
                        (!bconejo.Intersects(bplataforma2_right)) &&
                        (!bconejo.Intersects(bplataforma3_right)) &&
                        (!bconejo.Intersects(bplataforma4_right)) &&
                        pconejo.X >= 0
                        )
                    { direccion = 2; pconejo.X -= 10; }
                    if (pconejo.X <= 3499 && pconejo.X >= 600) { posCamX -= 10; pfondo_escenario.X -= 10; }
                }
                else
                {
                    opt = 1;
                }
            } 
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !(kb.IsKeyDown(Keys.Up)))
                pconejo.Y -= 10;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !(kb.IsKeyDown(Keys.Down)))
                if(sobreplataforma == false)
                    pconejo.Y += 10;

            //BoundingBox Update
            bconejo = new BoundingBox(new Vector3(pconejo.X, pconejo.Y, 0), new Vector3(pconejo.X + conejo_der.Width, pconejo.Y + conejo_der.Height, 0));

            benemigo1 = new BoundingBox(new Vector3(penemigo1.X, penemigo1.Y, 0), new Vector3(penemigo1.X + enemigo1_der.Width, penemigo1.Y + enemigo1_der.Height, 0));
            benemigo2 = new BoundingBox(new Vector3(penemigo2.X, penemigo2.Y, 0), new Vector3(penemigo2.X + enemigo1_der.Width, penemigo2.Y + enemigo1_der.Height, 0));
            benemigo3 = new BoundingBox(new Vector3(penemigo3.X, penemigo3.Y, 0), new Vector3(penemigo3.X + enemigo1_der.Width, penemigo3.Y + enemigo1_der.Height, 0));

            bplataforma1_top = new BoundingBox(new Vector3(pplataforma1.X + 5, pplataforma1.Y, 0), new Vector3(pplataforma1.X + plataforma1.Width - 5, pplataforma1.Y + 5, 0));
            bplataforma1_left = new BoundingBox(new Vector3(pplataforma1.X, pplataforma1.Y + 5, 0), new Vector3(pplataforma1.X + 5, pplataforma1.Y + plataforma1.Height, 0));
            bplataforma1_right = new BoundingBox(new Vector3(pplataforma1.X + plataforma1.Width - 5, pplataforma1.Y + 5, 0), new Vector3(pplataforma1.X + plataforma1.Width, pplataforma1.Y + plataforma1.Height, 0));

            bplataforma2_top = new BoundingBox(new Vector3(pplataforma2.X + 5, pplataforma2.Y, 0), new Vector3(pplataforma2.X + plataforma2.Width - 5, pplataforma2.Y + 5, 0));
            bplataforma2_left = new BoundingBox(new Vector3(pplataforma2.X, pplataforma2.Y + 5, 0), new Vector3(pplataforma2.X + 5, pplataforma2.Y + plataforma2.Height, 0));
            bplataforma2_right = new BoundingBox(new Vector3(pplataforma2.X + plataforma2.Width - 5, pplataforma2.Y + 5, 0), new Vector3(pplataforma2.X + plataforma2.Width, pplataforma2.Y + plataforma2.Height, 0));

            bplataforma3_top = new BoundingBox(new Vector3(pplataforma3.X + 5, pplataforma3.Y, 0), new Vector3(pplataforma3.X + plataforma3.Width - 5, pplataforma3.Y + 5, 0));
            bplataforma3_left = new BoundingBox(new Vector3(pplataforma3.X, pplataforma3.Y + 5, 0), new Vector3(pplataforma3.X + 5, pplataforma3.Y + plataforma3.Height, 0));
            bplataforma3_right = new BoundingBox(new Vector3(pplataforma3.X + plataforma3.Width - 5, pplataforma3.Y + 5, 0), new Vector3(pplataforma3.X + plataforma3.Width, pplataforma3.Y + plataforma3.Height, 0));

            bplataforma4_top = new BoundingBox(new Vector3(pplataforma4.X + 5, pplataforma4.Y, 0), new Vector3(pplataforma4.X + plataforma4.Width - 5, pplataforma4.Y + 5, 0));
            bplataforma4_left = new BoundingBox(new Vector3(pplataforma4.X, pplataforma4.Y + 5, 0), new Vector3(pplataforma4.X + 5, pplataforma4.Y + plataforma4.Height, 0));
            bplataforma4_right = new BoundingBox(new Vector3(pplataforma4.X + plataforma4.Width - 5, pplataforma4.Y + 5, 0), new Vector3(pplataforma4.X + plataforma4.Width, pplataforma4.Y + plataforma4.Height, 0));

            //Siguiente nivel
            if (pconejo.X <= pend.X + end.Width && pconejo.X >= pend.X) next_level++ ;
            if (next_level == 1) escenario = 2;

            //Sobre plataforma
            if (
                bconejo.Intersects(bplataforma1_top) && !bconejo.Intersects(bplataforma1_right) && !bconejo.Intersects(bplataforma1_left) ||
                bconejo.Intersects(bplataforma2_top) && !bconejo.Intersects(bplataforma2_right) && !bconejo.Intersects(bplataforma2_left) ||
                bconejo.Intersects(bplataforma3_top) && !bconejo.Intersects(bplataforma3_right) && !bconejo.Intersects(bplataforma3_left) ||
                bconejo.Intersects(bplataforma4_top) && !bconejo.Intersects(bplataforma4_right) && !bconejo.Intersects(bplataforma4_left)
              )
            {
                sobreplataforma = true;
            }
            else
                sobreplataforma = false;

            if (vida == 0) escenario = 4;

            //Cae al vacio
            if (pconejo.Y + conejo_der.Height / 2 >= fondo_escenario.Height) { cae = true; pconejo = new Vector2(20, 60); posCamX = 600; vida -= 1; }

            //Gravedad
            if (sobreplataforma == false) { tiemposuelto += timeup; pconejo.Y = pconejo.Y + (masa * tiemposuelto); }
            else { cae = false; masa = 3f; tiemposuelto = 0; timeup = 0.05f; }

            //Enemigos
            if (bconejo.Intersects(benemigo1) && vivo1) { vida -= 1; vivo1 = false; pconejo = new Vector2(20, 60); }
            if (bconejo.Intersects(benemigo1) && vivo2) { vida -= 1; vivo2 = false; pconejo = new Vector2(20, 60); }
            if (bconejo.Intersects(benemigo1) && vivo3) { vida -= 1; vivo3 = false; pconejo = new Vector2(20, 60); }

            if (vivo1)
            {
                if (mov1)
                {
                    if (dir1 == 1) penemigo1.X += 1;
                    if (dir1 == 2) penemigo1.X -= 1;
                }
                if (penemigo1.X <= pplataforma2.X) dir1 = 1;
                if (penemigo1.X + enemigo1_der.Width >= pplataforma2.X + plataforma2.Width) dir1 = 2;
            }
            if (vivo2)
            {
                if (mov2)
                {
                    if (dir2 == 1) penemigo2.X += 1;
                    if (dir2 == 2) penemigo2.X -= 1;
                }
                if (penemigo2.X <= pplataforma3.X) dir2 = 1;
                if (penemigo2.X + enemigo2_der.Width >= pplataforma3.X + plataforma3.Width) dir2 = 2;
            }
            if (vivo3)
            {
                if (mov3)
                {
                    if (dir3 == 1) penemigo3.X += 1;
                    if (dir3 == 2) penemigo3.X -= 1;
                }
                if (penemigo3.X <= pplataforma4.X) dir3 = 1;
                if (penemigo3.X + enemigo3_der.Width >= pplataforma4.X + plataforma4.Width) dir3 = 2;
            }


            //Camara
            camara = Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, 0)) * Matrix.CreateTranslation(new Vector3(-posCamX, -posCamY, 0));

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState, camara);

            switch (escenario)
            {
                case 0: //Menu
                    spriteBatch.Draw(fondo_menu, Vector2.Zero, Color.White);
                    spriteBatch.Draw(boton_play, pboton_play, Color.White);
                    spriteBatch.Draw(boton_instrucciones, pboton_instrucciones, Color.White);
                    spriteBatch.Draw(zanahoria, pzanahoria, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(fondo_escenario, pfondo_escenario, Color.White);
                    spriteBatch.Draw(end, pend, Color.White);
                    spriteBatch.Draw(plataforma1, pplataforma1, Color.White);
                    spriteBatch.Draw(plataforma2, pplataforma2, Color.White);
                    spriteBatch.Draw(plataforma3, pplataforma3, Color.White);
                    spriteBatch.Draw(plataforma4, pplataforma4, Color.White);
                    if (direccion == 1) spriteBatch.Draw(conejo_der, pconejo, Color.White);
                    else if (direccion == 2) spriteBatch.Draw(conejo_izq, pconejo, Color.White);
                    switch (vida)
                    {
                        case 3:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            spriteBatch.Draw(vida2, pvida2, Color.White);
                            spriteBatch.Draw(vida3, pvida3, Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            spriteBatch.Draw(vida2, pvida2, Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            break;
                    }
                    if (vivo1)
                    {
                        if (dir1 == 1) spriteBatch.Draw(enemigo1_der, penemigo1, Color.White);
                        if (dir1 == 2) spriteBatch.Draw(enemigo1_izq, penemigo1, Color.White);
                    }
                    if (vivo2)
                    {
                        if (dir2 == 1) spriteBatch.Draw(enemigo2_der, penemigo2, Color.White);
                        if (dir2 == 2) spriteBatch.Draw(enemigo2_izq, penemigo2, Color.White);
                    }
                    if (vivo3)
                    {
                        if (dir3 == 1) spriteBatch.Draw(enemigo3_der, penemigo3, Color.White);
                        if (dir3 == 2) spriteBatch.Draw(enemigo3_izq, penemigo3, Color.White);
                    }
                    break;
                case 2:
                    spriteBatch.Draw(fondo_escenario, pfondo_escenario, Color.White);
                    spriteBatch.Draw(end, pend, Color.White);
                    spriteBatch.Draw(plataforma1, pplataforma1, Color.White);
                    spriteBatch.Draw(plataforma2, pplataforma2, Color.White);
                    spriteBatch.Draw(plataforma3, pplataforma3, Color.White);
                    spriteBatch.Draw(plataforma4, pplataforma4, Color.White);
                    if (direccion == 1) spriteBatch.Draw(conejo_der, pconejo, Color.White);
                    else if (direccion == 2) spriteBatch.Draw(conejo_izq, pconejo, Color.White);
                    switch (vida)
                    {
                        case 3:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            spriteBatch.Draw(vida2, pvida2, Color.White);
                            spriteBatch.Draw(vida3, pvida3, Color.White);
                            break;
                        case 2:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            spriteBatch.Draw(vida2, pvida2, Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(vida1, pvida1, Color.White);
                            break;
                    }
                    if (vivo1)
                    {
                        if (dir1 == 1) spriteBatch.Draw(enemigo1_der, penemigo1, Color.White);
                        if (dir1 == 2) spriteBatch.Draw(enemigo1_izq, penemigo1, Color.White);
                    }
                    if (vivo2)
                    {
                        if (dir2 == 1) spriteBatch.Draw(enemigo2_der, penemigo2, Color.White);
                        if (dir2 == 2) spriteBatch.Draw(enemigo2_izq, penemigo2, Color.White);
                    }
                    if (vivo3)
                    {
                        if (dir3 == 1) spriteBatch.Draw(enemigo3_der, penemigo3, Color.White);
                        if (dir3 == 2) spriteBatch.Draw(enemigo3_izq, penemigo3, Color.White);
                    }
                    break;
                case 3: //Instrucciones
                    spriteBatch.Draw(instrucciones, Vector2.Zero, Color.White);
                    break;
                case 4: //Instrucciones
                    spriteBatch.Draw(game_over, Vector2.Zero, Color.White);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

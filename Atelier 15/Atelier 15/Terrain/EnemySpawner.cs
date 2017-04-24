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


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemySpawner : Microsoft.Xna.Framework.GameComponent
    {
        int NiveauEnemy { get; set; }
        const int NOMBRE_SECONDES_DANS_MINUTES = 60;
        float TempsSpawnInitial { get; set; }
        float[] TempsSpawnUpgrade { get; set; }
        float TempsÉcoulerDepuisDernierSpawn { get; set; }
        float IntervalleSpawn { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Random GenerateurAleatoire { get; set; }
        public EnemySpawner(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            GenerateurAleatoire = Game.Services.GetService(typeof(Random)) as Random;
            TempsSpawnUpgrade = new float[(12)];
            TempsSpawnInitial = 3 * NOMBRE_SECONDES_DANS_MINUTES; //spawn initial
            TempsSpawnUpgrade[0] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[1] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 2 a 3
            TempsSpawnUpgrade[2] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 3 a 4
            TempsSpawnUpgrade[3] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 4 a 5
            TempsSpawnUpgrade[4] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 5 a 6
            TempsSpawnUpgrade[5] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[6] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[7] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[8] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[9] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[10] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            TempsSpawnUpgrade[11] = (5 * NOMBRE_SECONDES_DANS_MINUTES); // niv 1 a 2
            NiveauEnemy = 0;
            IntervalleMAJ = TempsSpawnInitial;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                if (IntervalleMAJ == TempsSpawnInitial)
                {
                    IntervalleMAJ = TempsSpawnUpgrade[NiveauEnemy];
                }
                else
                {
                    ++NiveauEnemy;
                    IntervalleMAJ = TempsSpawnUpgrade[NiveauEnemy];
                }
                TempsÉcouléDepuisMAJ = 0;
            }
            if (IntervalleMAJ != TempsSpawnInitial)
            {
                TempsÉcoulerDepuisDernierSpawn += tempsÉcoulé;
                if (TempsÉcoulerDepuisDernierSpawn >= IntervalleSpawn)
                {
                    CréerEnemyAleatoire();
                    IntervalleSpawn = TrouverNouvelInterval();
                    TempsÉcoulerDepuisDernierSpawn = 0;
                }
            }
        }
            
        private void CréerEnemyAleatoire()
        {
            int nbEnemy = GenerateurAleatoire.Next(0, 10);
            for (int i = 0; i < nbEnemy; i++)
            {
                Ennemis e = new Ennemis(Game, "player", 0.01f, new Vector3(256 / 2f + 2, 0, 256 / 2f + 2), Vector3.Zero, NiveauEnemy + 1);
                Game.Components.Add(e);
            }
        }

        public void CréerEnemy(int niveauEnemy)
        {
            Ennemis e = new Ennemis(Game, "player", 0.01f, new Vector3(256 / 2f + 2, 0, 256 / 2f + 2), Vector3.Zero, niveauEnemy);
            Game.Components.Add(e);
        }

        private float TrouverNouvelInterval()
        {
            return GenerateurAleatoire.Next(30, 45);
        }
    }
}

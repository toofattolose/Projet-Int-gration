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
    public class Arbre : Model3D, IClicDroitRessource
    {
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float TempsÉcouléDepuisDernierSon { get; set; }
        string État { get; set; }
        float TempsDeCollection { get; set; }
        int NombreDeCollection { get; set; }
        Joueur JoueurPrésent { get; set; }
        SoundEffect SoundChop { get; set; }

        public Arbre(Game game, string nomModele, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, échelle, position, rotationInitiale)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            SoundChop = Game.Content.Load<SoundEffect>("SoundEffects/wood");
        }

        public override void Update(GameTime gameTime)
        {
            switch(État)
            {
                case "collection":
                    IntervalleMAJ = JoueurPrésent.TempsCollectionRessource;
                    NombreDeCollection = JoueurPrésent.NombreCollectionRessource;
                    CollectionDeRessource(gameTime);
                    break;
                case "null":
                    break;
            }
        }

        //méthode qui va collectionner le bois
        public void EstCliquéDroit(Joueur joueurPrésent)
        {
            if (!joueurPrésent.EstEnCollectionRessource)
            {
                IntervalleMAJ = joueurPrésent.TempsCollectionRessource;
                NombreDeCollection = joueurPrésent.NombreCollectionRessource;
                JoueurPrésent = joueurPrésent;
                JoueurPrésent.EstEnCollectionRessource = true;
                État = "collection";
            }
        }

        //Collection du bois pour le joueur
        private void CollectionDeRessource(GameTime gameTime)
        {
            int temps1Seconde = 1;
            float distanceJoueur = (float)(Math.Abs(Math.Sqrt(Math.Pow(JoueurPrésent.Position.X - Position.X, 2) + Math.Pow(JoueurPrésent.Position.Y - Position.Y, 2) + Math.Pow(JoueurPrésent.Position.Z - Position.Z, 2))));
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisDernierSon += tempsÉcoulé;
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                JoueurPrésent.NombreDeBois += NombreDeCollection;
                AfficheurCollectionRessource afficheur = new AfficheurCollectionRessource(Game, Position, Color.Red, NombreDeCollection);
                Game.Components.Add(afficheur);
                TempsÉcouléDepuisMAJ = 0;
            }
            if (TempsÉcouléDepuisDernierSon >= temps1Seconde)
            {
                SoundChop.Play();
                TempsÉcouléDepuisDernierSon = 0;
            }
            if (distanceJoueur >= 5)
            {
                JoueurPrésent.EstEnCollectionRessource = false;
                État = "null";
            }
        }
    }
}

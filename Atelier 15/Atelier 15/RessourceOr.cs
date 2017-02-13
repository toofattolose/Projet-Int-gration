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
    public class RessourceOr : Model3D
    {
        float IntervalleMAJ { get; set; }
        float Temps�coul�DepuisMAJ { get; set; }
        string �tat { get; set; }
        float TempsDeCollection { get; set; }
        int NombreDeCollection { get; set; }
        Joueur JoueurPr�sent { get; set; }

        public RessourceOr(Game game, string nomModele, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModele, �chelle, position, rotationInitiale)
        {

        }


        public override void Update(GameTime gameTime)
        {
            switch (�tat)
            {
                case "collection":
                    CollectionDeRessource(gameTime);
                    break;
                case "null":
                    break;
            }
        }

        //m�thode qui va collectionner le bois
        public void EstCliqu�Droit(float tempsDeCollection, int nombreDeCollection, Joueur joueurPr�sent)
        {
            IntervalleMAJ = tempsDeCollection;
            NombreDeCollection = nombreDeCollection;
            JoueurPr�sent = joueurPr�sent;
            �tat = "collection";
        }

        //Collection du bois pour le joueur
        private void CollectionDeRessource(GameTime gameTime)
        {
            float distanceJoueur = (float)(Math.Abs(Math.Sqrt(Math.Pow(JoueurPr�sent.Position.X - Position.X, 2) + Math.Pow(JoueurPr�sent.Position.Y - Position.Y, 2) + Math.Pow(JoueurPr�sent.Position.Z - Position.Z, 2))));
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Temps�coul�DepuisMAJ += temps�coul�;
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                JoueurPr�sent.NombreDOR += NombreDeCollection;
                Temps�coul�DepuisMAJ = 0;
            }
            if (distanceJoueur >= 10)
            {
                �tat = "null";
            }
        }
    }
}

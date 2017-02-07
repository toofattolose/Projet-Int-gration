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
    public class GenerateurProcedural : Microsoft.Xna.Framework.GameComponent
    {
        Vector3 PositionInitiale { get; set; }
        Vector3 �tendue { get; set; }
        Vector2 Charpente { get; set; }
        Vector2 Delta { get; set; }
        Vector3 Origine { get; set; }
        Vector3 OrigineJoueur { get; set; }
        Vector2 PositionDansCase { get; set; }
        Random g�n�rateur = new Random();
        DepthStencilState �tatDeProfondeur { get; set; }
        GraphicsDeviceManager PeripheriqueGraphique { get; set; }
        GridDeJeu Grid { get; set; }

        public GenerateurProcedural(Game game, Vector3 positionInitiale, Vector3 �tendue, Vector2 charpente)
            : base(game)
        {
            PositionInitiale = positionInitiale;
            �tendue = �tendue;
            Charpente = charpente;
        }

        public override void Initialize()
        {
            Origine = new Vector3(-�tendue.X / 2, 0, 0);
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            OrigineJoueur = new Vector3(0, 0, �tendue.Z / 2f);
            Delta = new Vector2(�tendue.X / Charpente.X, �tendue.Z / Charpente.Y);
            PositionDansCase = Delta / 2;
            Cr�erLesComposants();
        }

        private void Cr�erLesComposants()
        {
            Game.Components.Add(new Joueur(Game, "tree1", 0.01f, OrigineJoueur, Vector3.Zero, 1f / 60f));
            for (int i = 0; i < Charpente.X - 1; i++)
            {
                for (int j = 0; j < Charpente.Y - 1; j++)
                {
                    Vector3 pos = new Vector3(Origine.X + (i * Delta.X) + PositionDansCase.X, Origine.Y, Origine.Z + (j * Delta.Y) + PositionDansCase.Y);
                    Vector3 vecteurPosJoueur = new Vector3(pos.X - OrigineJoueur.X, 0, pos.Z - OrigineJoueur.Z);
                    float distanceJoueur = (float)Math.Sqrt(Math.Pow(vecteurPosJoueur.X, 2) + Math.Pow(vecteurPosJoueur.Z, 2));
                    if (distanceJoueur > 20)
                    {
                        float valeur = TrouverValeurAl�atoire(0, 100);
                        if (valeur >= 90)
                        {
                            Game.Components.Add(new Arbre(Game, "tree1", 0.02f, pos, new Vector3(0, 0, 0)));
                            Grid.TableauGrid[i, j] = false;
                        }
                        else
                        {
                            if (valeur >= 80)
                            {
                                Game.Components.Add(new RessourceOr(Game, "gold1", 0.02f, pos, new Vector3(0, 0, 0)));
                                Grid.TableauGrid[i, j] = false;
                            }
                            else
                            {
                                if (valeur >= 40)
                                {
                                    Game.Components.Add(new Roche(Game, "rock1", 0.02f, pos, new Vector3(0, 0, 0)));
                                    Grid.TableauGrid[i, j] = false;
                                }
                            }
                        }
                    }  
                }
            }
        }
        private float TrouverValeurAl�atoire(int valeurMin, int valeurMax)
        {
            return g�n�rateur.Next(valeurMin, valeurMax + 1);
        }
    }
}

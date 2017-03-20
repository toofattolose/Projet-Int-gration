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
        Vector3 Étendue { get; set; }
        Vector2 Charpente { get; set; }
        Vector2 Delta { get; set; }
        Vector3 Origine { get; set; }
        Vector3 OrigineJoueur { get; set; }
        Vector2 PositionDansCase { get; set; }
        Random générateur = new Random();
        DepthStencilState ÉtatDeProfondeur { get; set; }
        GraphicsDeviceManager PeripheriqueGraphique { get; set; }
        GridDeJeu Grid { get; set; }

        public GenerateurProcedural(Game game, Vector3 positionInitiale, Vector3 étendue, Vector2 charpente)
            : base(game)
        {
            PositionInitiale = positionInitiale;
            Étendue = étendue;
            Charpente = charpente;
        }

        public override void Initialize()
        {
            //Origine = new Vector3(-Étendue.X / 2, 0, 0);
            Origine = Vector3.Zero;
            
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            OrigineJoueur = new Vector3(Étendue.X /2f + 2, 0, Étendue.Z / 2f + 2);
            Delta = new Vector2(Étendue.X / Charpente.X, Étendue.Z / Charpente.Y);
            PositionDansCase = Delta / 2;
            CréerLesComposants();
        }

        private void CréerLesComposants()
        {
            Game.Components.Add(new Joueur(Game, "player", 0.01f, OrigineJoueur, Vector3.Zero, 1f / 60f));
            //Game.Components.Add(new Model3D(Game, "startingrock", 0.02f, OrigineJoueur, Vector3.Zero));
            for (int i = 0; i < Charpente.X - 1; i++)
            {
                for (int j = 0; j < Charpente.Y - 1; j++)
                {
                    Vector3 pos = new Vector3(Origine.X + (i * Delta.X) + PositionDansCase.X, Origine.Y, Origine.Z + (j * Delta.Y) + PositionDansCase.Y);
                    Vector3 positionCaseDeConstruction = new Vector3(Origine.X + (i * Delta.X), Origine.Y, Origine.Z + (j * Delta.Y));
                    Vector3 vecteurPosJoueur = new Vector3(pos.X - OrigineJoueur.X, 0, pos.Z - OrigineJoueur.Z);
                    Game.Components.Add(new CaseDeConstruction(Game, positionCaseDeConstruction + new Vector3(0,5,0)));
                    float distanceJoueur = (float)Math.Sqrt(Math.Pow(vecteurPosJoueur.X, 2) + Math.Pow(vecteurPosJoueur.Z, 2));
                    float rotation = MathHelper.ToRadians(TrouverValeurAléatoire(0, 360));
                    if (distanceJoueur > 20)
                    {
                        float valeur = TrouverValeurAléatoire(0, 100);
                        if (valeur >= 99)
                        {
                            float typeArbre = TrouverValeurAléatoire(0, 1);
                            if (typeArbre == 0)
                            {
                                Game.Components.Add(new Arbre(Game, "tree1", 0.015f, pos, new Vector3(0, rotation, 0)));
                            }
                            else
                            {
                                Game.Components.Add(new Arbre(Game, "tree2", 0.015f, pos, new Vector3(0, rotation, 0)));
                            }
                            Grid.TableauGrid[i, j] = false;
                            Grid.GridCase[i, j].Accessible = false;
                        }
                        else
                        {
                            if (valeur >= 98)
                            {
                                Game.Components.Add(new RessourceOr(Game, "gold1", 0.02f, pos, new Vector3(0, rotation, 0)));
                                Grid.TableauGrid[i, j] = false;
                                Grid.GridCase[i, j].Accessible = false;
                            }
                            else
                            {
                                if (valeur >= 40)
                                {
                                    Game.Components.Add(new Roche(Game, "rock1", 0.02f, pos, new Vector3(0, rotation, 0)));
                                    Grid.TableauGrid[i, j] = false;
                                    Grid.GridCase[i, j].Accessible = false;
                                }
                                else
                                {
                                    if (valeur >= 35)
                                    {
                                        Game.Components.Add(new Plante(Game, "plant1", 0.02f, pos, new Vector3(0, rotation, 0)));
                                    }
                                }  
                            }
                        }
                    }  
                }
            }
            Game.Components.Add(new AfficheurRessource(Game));
        }
        private float TrouverValeurAléatoire(int valeurMin, int valeurMax)
        {
            return générateur.Next(valeurMin, valeurMax + 1);
        }

    }
}

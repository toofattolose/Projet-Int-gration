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
    public class PlacementBuilding : Model3D
    {
        InputManager GestionInput { get; set; }
        int TypeBuilding { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Caméra CaméraJeu { get; set; }
        GridDeJeu Grid { get; set; }
        RessourcesManager<Model> GestionnaireDeModèles { get; set; }

        public PlacementBuilding(Game game, string nomModel, float échelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModel, échelle, position, rotationInitiale)
        {
            // TODO: Construct any child components here
        }

        public override void Initialize()
        {
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            CaméraJeu = Game.Services.GetService(typeof(Caméra)) as Caméra;
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            GestionnaireDeModèles = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            IntervalleMAJ = 1 / 60f;
        }


        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;   
            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            GérerClavier();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                GérerModel();
                GérerDéplacement();
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        //GestionDesTouches
        private void GérerClavier()
        {
            if (GestionInput.EstNouvelleTouche(Keys.Left))
            {
                --TypeBuilding;
                if (TypeBuilding < 0)
                {
                    TypeBuilding = 3;
                }
            }
            if (GestionInput.EstNouvelleTouche(Keys.Right))
            {
                ++TypeBuilding;
                if (TypeBuilding > 3)
                {
                    TypeBuilding = 0;
                }
            }
        }

        //Gestion du model
        private void GérerModel()
        {
            if (TypeBuilding == 0)
            {
                NomModele = "tree2";
            }
            else
            {
                if (TypeBuilding == 1)
                {
                    NomModele = "tree1";
                }
                else
                {
                    if (TypeBuilding == 2)
                    {
                        NomModele = "rock1";
                    }
                    else
                    {
                        NomModele = "player";
                    }
                }
            }
            Modèle = GestionnaireDeModèles.Find(NomModele);
        }

        //Gestion des déplacement
        private void GérerDéplacement()
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector3 positionDansGrid = new Vector3((float)Math.Floor(positionSouris.X / Grid.Delta.X),0, (float)Math.Floor(positionSouris.Z / Grid.Delta.Y));
            Vector3 vecteurDelta = new Vector3(Grid.DeltaDiviséParDeux, 0, Grid.DeltaDiviséParDeux);
            Position = (positionDansGrid * Grid.Delta.X) + vecteurDelta;
            CalculerMonde();
        }

        //Utilisé pour trouver la position de la souris dans un environnement 3D
        private Vector3 TrouverPositionSouris(Point ms)
        {
            Vector2 positionSouris = new Vector2(ms.X, ms.Y);
            Vector3 nearScreenPoint = new Vector3(positionSouris, 0);
            Vector3 farScreenPoint = new Vector3(positionSouris, 1.01f);
            Vector3 nearWorldPoint = Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, CaméraJeu.Projection, CaméraJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }

        public override void Draw(GameTime gameTime)
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector3 positionDansGrid = new Vector3((float)Math.Floor(Position.X / Grid.Delta.X), 0, (float)Math.Floor(Position.Z / Grid.Delta.Y));
            if (Grid.TableauGrid[(int)positionDansGrid.X,(int)positionDansGrid.Z])
            {
                base.Draw(gameTime);
            }     
        }

        //Vérifie si la souris sort de la zone de jeu
        private bool VérifierPositionSouris(Vector3 positionSouris)
        {
            if (positionSouris.X < 0)
            {
                return false;
            }
            else
            {
                if (positionSouris.X > (Grid.Charpente.X * Grid.Delta.X) - 2)
                {
                    return false;
                }
            }
            if (positionSouris.Y < 0)
            {
                return false;
            }
            else
            {
                if (positionSouris.Y > (Grid.Charpente.Y * Grid.Delta.Y) - 2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

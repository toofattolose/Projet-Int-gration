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
        float Temps�coul�DepuisMAJ { get; set; }
        float IntervalleMAJ { get; set; }
        Cam�ra Cam�raJeu { get; set; }
        GridDeJeu Grid { get; set; }
        RessourcesManager<Model> GestionnaireDeMod�les { get; set; }
        Joueur JoueurPr�sent { get; set; }
        int[,] TableauDesPrix { get; set; }
        int NombreWall { get; set; }

        public PlacementBuilding(Game game, string nomModel, float �chelle, Vector3 position, Vector3 rotationInitiale)
            : base(game, nomModel, �chelle, position, rotationInitiale)
        {
            // TODO: Construct any child components here
            TableauDesPrix = new int[4, 2];

            //Mur
            TableauDesPrix[0, 0] = 50; //Prix bois
            TableauDesPrix[0, 1] = 0; // Prix Or

            //Generatrice
            TableauDesPrix[1, 0] = 50; //75; //Prix bois
            TableauDesPrix[1, 1] = 25;// 25; //Prix or

            //R�parateur
            TableauDesPrix[2, 0] = 50;//50; //Prix bois
            TableauDesPrix[2, 1] = 25;//50; //Prix or

            //Turret
            TableauDesPrix[3, 0] = 25;//75; //Prix bois
            TableauDesPrix[3, 1] = 50;//50; //Prix or

        }

        public override void Initialize()
        {
            base.Initialize();
            foreach (Mur m in Game.Components.OfType<Mur>())
            {
                ++NombreWall;
            }

            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            Cam�raJeu = Game.Services.GetService(typeof(Cam�ra)) as Cam�ra;
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            GestionnaireDeMod�les = Game.Services.GetService(typeof(RessourcesManager<Model>)) as RessourcesManager<Model>;
            IntervalleMAJ = 1 / 60f;
        }


        public override void Update(GameTime gameTime)
        {
            float temps�coul� = (float)gameTime.ElapsedGameTime.TotalSeconds;   
            Temps�coul�DepuisMAJ += temps�coul�;
            G�rerClavier();
            G�rerPlacement();
            if (Temps�coul�DepuisMAJ >= IntervalleMAJ)
            {
                foreach (Joueur j in Game.Components.OfType<Joueur>())
                {
                    JoueurPr�sent = j;
                }
                G�rerModel();
                G�rerD�placement();
                base.CalculerMonde();
                Temps�coul�DepuisMAJ = 0;
            }
        }

        private void G�rerPlacement()
        {
            if (GestionInput.EstNouveauClicGauche())
            {
                Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
                Vector3 vecteurDelta = new Vector3(Grid.DeltaDivis�ParDeux, 0, Grid.DeltaDivis�ParDeux);
                Vector3 positionDansGrid = new Vector3((float)Math.Floor(positionSouris.X / Grid.Delta.X), 0, (float)Math.Floor(positionSouris.Z / Grid.Delta.Y));
                Vector3 positionBuilding = new Vector3((positionDansGrid.X * Grid.Delta.X) + vecteurDelta.X, (positionDansGrid.Y * Grid.Delta.Y) + vecteurDelta.Y, (positionDansGrid.Z * Grid.Delta.Y) + vecteurDelta.Z);
                if (Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z])
                {
                    if (TypeBuilding == 3)
                    {
                        if (JoueurPr�sent.NombreDeBois >= TableauDesPrix[0, 0] && JoueurPr�sent.NombreDOR >= TableauDesPrix[0, 1] && NombreWall < 1)
                        {
                            Mur buildingMur = new Mur(Game, "wall", 0.02f, positionBuilding, Vector3.Zero);
                            Game.Components.Add(buildingMur);
                            Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z] = false;
                            Grid.GridCase[(int)positionDansGrid.X, (int)positionDansGrid.Z].Accessible = false;
                            JoueurPr�sent.NombreDeBois -= TableauDesPrix[0, 0];
                            JoueurPr�sent.NombreDOR -= TableauDesPrix[0, 1];
                        }
                    }
                    else
                    {
                        if (TypeBuilding == 2)
                        {
                            if (JoueurPr�sent.NombreDeBois >= TableauDesPrix[1, 0] && JoueurPr�sent.NombreDOR >= TableauDesPrix[1, 1])
                            {
                                Generatrice buildingGeneratrice = new Generatrice(Game, "generator", 0.02f, positionBuilding, Vector3.Zero);
                                Game.Components.Add(buildingGeneratrice);
                                Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z] = false;
                                Grid.GridCase[(int)positionDansGrid.X, (int)positionDansGrid.Z].Accessible = false;
                                JoueurPr�sent.NombreDeBois -= TableauDesPrix[1, 0];
                                JoueurPr�sent.NombreDOR -= TableauDesPrix[1, 1];
                            }
                        }
                        else
                        {
                            if (TypeBuilding == 1)
                            {
                                if (JoueurPr�sent.NombreDeBois >= TableauDesPrix[2, 0] && JoueurPr�sent.NombreDOR >= TableauDesPrix[2, 1])
                                {
                                    Reparateur buildingReparateur = new Reparateur(Game, "reparator", 0.02f, positionBuilding, Vector3.Zero);
                                    Game.Components.Add(buildingReparateur);
                                    Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z] = false;
                                    Grid.GridCase[(int)positionDansGrid.X, (int)positionDansGrid.Z].Accessible = false;
                                    JoueurPr�sent.NombreDeBois -= TableauDesPrix[2, 0];
                                    JoueurPr�sent.NombreDOR -= TableauDesPrix[2, 1];
                                }

                            }
                            else
                            {
                                if (JoueurPr�sent.NombreDeBois >= TableauDesPrix[3, 0] && JoueurPr�sent.NombreDOR >= TableauDesPrix[3, 1])
                                {
                                    Turret buildingTurret = new Turret(Game, "turret", 0.075f, positionBuilding, Vector3.Zero);
                                    Game.Components.Add(buildingTurret);
                                    Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z] = false;
                                    Grid.GridCase[(int)positionDansGrid.X, (int)positionDansGrid.Z].Accessible = false;
                                    JoueurPr�sent.NombreDeBois -= TableauDesPrix[3, 0];
                                    JoueurPr�sent.NombreDOR -= TableauDesPrix[3, 1];
                                }

                            }
                        }
                    }
                }
            }
        }

        //GestionDesTouches
        private void G�rerClavier()
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
        private void G�rerModel()
        {
            if (TypeBuilding == 0)
            {
                NomModele = "turret";
            }
            else
            {
                if (TypeBuilding == 1)
                {
                    NomModele = "reparator";
                }
                else
                {
                    if (TypeBuilding == 2)
                    {
                        NomModele = "generator";
                    }
                    else
                    {
                        NomModele = "wall";
                    }
                }
            }
            Mod�le = GestionnaireDeMod�les.Find(NomModele);
        }

        //Gestion des d�placement
        private void G�rerD�placement()
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector3 positionDansGrid = new Vector3((float)Math.Floor(positionSouris.X / Grid.Delta.X),0, (float)Math.Floor(positionSouris.Z / Grid.Delta.Y));
            Vector3 vecteurDelta = new Vector3(Grid.DeltaDivis�ParDeux, 0, Grid.DeltaDivis�ParDeux);
            Position = (positionDansGrid * Grid.Delta.X) + vecteurDelta;
            CalculerMonde();
        }

        //Utilis� pour trouver la position de la souris dans un environnement 3D
        private Vector3 TrouverPositionSouris(Point ms)
        {
            Vector2 positionSouris = new Vector2(ms.X, ms.Y);
            Vector3 nearScreenPoint = new Vector3(positionSouris, 0);
            Vector3 farScreenPoint = new Vector3(positionSouris, 1.01f);
            Vector3 nearWorldPoint = Game.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);
            Vector3 farWorldPoint = Game.GraphicsDevice.Viewport.Unproject(farScreenPoint, Cam�raJeu.Projection, Cam�raJeu.Vue, Matrix.Identity);

            Vector3 direction = farWorldPoint - nearWorldPoint;

            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;

            return zeroWorldPoint;
        }

        public override void Draw(GameTime gameTime)
        {
            Vector3 positionSouris = TrouverPositionSouris(GestionInput.GetPositionSouris());
            Vector3 positionDansGrid = new Vector3((float)Math.Floor(Position.X / Grid.Delta.X), 0, (float)Math.Floor(Position.Z / Grid.Delta.Y));
            try
            {
                if (Grid.TableauGrid[(int)positionDansGrid.X, (int)positionDansGrid.Z])
                {
                    base.Draw(gameTime);
                }
            }
            catch (Exception) { }
        }
    }
}

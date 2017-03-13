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
    public class PathFindingAStar : Microsoft.Xna.Framework.GameComponent
    {
        public Case CaseDépart { get; set; }
        public Case CaseCible { get; set; }
        List<Case> ListCasesOuvertes { get; set; }
        List<Case> ListCasesFermées { get; set; }
        List<Case> ListVoisins { get; set; }
        Case CaseActuelle { get; set; }
        GridDeJeu Grid { get; set; }
        public List<Case> Path { get; set; }

        public PathFindingAStar(Game game):base(game)
        {
        }

        public override void Initialize()
        {
            Grid = Game.Services.GetService(typeof(GridDeJeu)) as GridDeJeu;
            ListCasesFermées = new List<Case>();
            ListCasesOuvertes = new List<Case>();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public void TrouverPath(Vector2 posDépart, Vector2 posCible)
        {
            int VariableExit = 1;
            CaseDépart = Grid.GridCase[(int)Math.Floor(posDépart.X), (int)Math.Floor(posDépart.Y)];
            CaseCible = Grid.GridCase[(int)Math.Floor(posCible.X), (int)Math.Floor(posCible.Y)];

            ListCasesOuvertes.Add(CaseDépart);
            
            while (ListCasesOuvertes.Count() != 0 && VariableExit != 0)
            {
                CaseActuelle = ListCasesOuvertes[0];
                for(int i = 0; i < ListCasesOuvertes.Count(); ++i)
                {
                    if(ListCasesOuvertes[i].F < CaseActuelle.F || ListCasesOuvertes[i].F == CaseActuelle.F && ListCasesOuvertes[i].H < CaseActuelle.H)
                    {
                        CaseActuelle = ListCasesOuvertes[i];
                    }
                }
                ListCasesOuvertes.Remove(CaseActuelle);
                ListCasesFermées.Add(CaseActuelle);

                if(CaseActuelle == CaseCible)
                {
                    RetracerPath();
                    VariableExit = 0;
                }
                else
                {
                    int compteurVoisins = 0;
                    ListVoisins = Grid.GetVoisins(CaseActuelle);
                    foreach (Case c in ListVoisins)
                    {
                        if(compteurVoisins == 0)
                        {
                            if (!c.Accessible || ListCasesFermées.Contains(c) || (!ListVoisins[1].Accessible && !ListVoisins[3].Accessible))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 2)
                        {
                            if (!c.Accessible || ListCasesFermées.Contains(c) || (!ListVoisins[1].Accessible && !ListVoisins[4].Accessible))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 5)
                        {
                            if (!c.Accessible || ListCasesFermées.Contains(c) || (!ListVoisins[3].Accessible && !ListVoisins[6].Accessible))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 7)
                        {
                            if (!c.Accessible || ListCasesFermées.Contains(c) || (!ListVoisins[6].Accessible && !ListVoisins[4].Accessible))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 1 || compteurVoisins == 3 || compteurVoisins == 4 || compteurVoisins == 6)
                        {
                            if (!c.Accessible || ListCasesFermées.Contains(c))
                            {
                                continue;
                            }
                        }

                        float nouvelleDistanceVoisin = CaseActuelle.G + DistanceCase_Case(CaseActuelle, c);
                        if(nouvelleDistanceVoisin < c.G || !ListCasesOuvertes.Contains(c))
                        {
                            c.G = nouvelleDistanceVoisin;
                            c.H = DistanceCase_Case(c, CaseCible);
                            c.CaseParent = CaseActuelle;

                            if(!ListCasesOuvertes.Contains(c))
                            {
                                ListCasesOuvertes.Add(c);
                            }
                        }
                    }
                }
            }
        }

        float DistanceCase_Case(Case case1, Case case2)
        {
            float DistanceX = Math.Abs(case2.Position.X - case1.Position.X);
            float DistanceY = Math.Abs(case2.Position.Y - case1.Position.Y);

            if(DistanceX > DistanceY)
            {
                return 1.4f * DistanceY + (DistanceX - DistanceY);
            }
            else
            {
                return 1.4f * DistanceX + (DistanceY - DistanceX);
            }
        }

        void RetracerPath() // retrace le path de la dernière case à la première et l'inverse
        {
            Path = new List<Case>();
            Case caseActuelle = CaseCible;

            while(caseActuelle != CaseDépart)
            {
                Path.Add(caseActuelle);
                caseActuelle = caseActuelle.CaseParent;
            }

            Path.Reverse();
        }

    }
}

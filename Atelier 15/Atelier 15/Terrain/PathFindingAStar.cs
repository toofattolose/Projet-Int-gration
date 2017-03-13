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

        void TrouverPath(Vector2 posDépart, Vector2 posCible)
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
                            if (c.Accessible == false || ListCasesFermées.Contains(c) || (ListVoisins[1].Accessible == false && ListVoisins[3].Accessible == false))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 2)
                        {
                            if (c.Accessible == false || ListCasesFermées.Contains(c) || (ListVoisins[1].Accessible == false && ListVoisins[4].Accessible == false))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 5)
                        {
                            if (c.Accessible == false || ListCasesFermées.Contains(c) || (ListVoisins[3].Accessible == false && ListVoisins[6].Accessible == false))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 7)
                        {
                            if (c.Accessible == false || ListCasesFermées.Contains(c) || (ListVoisins[6].Accessible == false && ListVoisins[4].Accessible == false))
                            {
                                continue;
                            }
                        }
                        if(compteurVoisins == 1 || compteurVoisins == 3 || compteurVoisins == 4 || compteurVoisins == 6)
                        {
                            if (c.Accessible == false || ListCasesFermées.Contains(c))
                            {
                                continue;
                            }
                        }
                    }
                }
            }
        }

        //float DistanceCase_Case(Case case1, Case case2)
        //{

        //}

    }
}

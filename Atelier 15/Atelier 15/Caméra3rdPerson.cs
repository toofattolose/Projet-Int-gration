using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AtelierXNA
{
    public class Caméra3rdPerson : Caméra
    {
        const float ANGLE_ROTATION = MathHelper.Pi / 90f;
        const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
        const float ACCÉLÉRATION = 0.001f;
        const float VITESSE_INITIALE_TRANSLATION = 0.5f;
        const float RAYON_COLLISION = 1f;

        Vector3 Direction { get; set; }
        Vector3 Latéral { get; set; }
        float VitesseTranslation { get; set; }
        float VitesseRotation { get; set; }

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        InputManager GestionInput { get; set; }

        public Vector3 PositionCameraInitiale { get; set; }

        bool estEnZoom;
        bool EstEnZoom
        {
            get { return estEnZoom; }
            set
            {
                float ratioAffichage = Game.GraphicsDevice.Viewport.AspectRatio;
                estEnZoom = value;
                if (estEnZoom)
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF / 2, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
                else
                {
                    CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
                }
            }
        }

        public Caméra3rdPerson(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
           : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            CréerPointDeVue(positionCaméra, cible, orientation);
            Direction = cible - Position;
            Direction.Normalize();
            EstEnZoom = false;
        }

        public override void Initialize()
        {
            VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
            PositionCameraInitiale = new Vector3(0, 10, 2);
            TempsÉcouléDepuisMAJ = 0;
            base.Initialize();
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;       
        }

        protected override void CréerPointDeVue()
        {
            // Méthode appelée s'il est nécessaire de recalculer la matrice de vue.
            // Calcul et normalisation de certains vecteurs
            // (à compléter)

            Direction = Vector3.Normalize(Direction);
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            OrientationVerticale = Vector3.Cross(Direction, Latéral);

            Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
            GénérerFrustum();
        }

        protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
        {
            // À la construction, initialisation des propriétés Position, Cible et OrientationVerticale,
            // ainsi que le calcul des vecteur Direction, Latéral et le recalcul du vecteur OrientationVerticale
            // permettant de calculer la matrice de vue de la caméra subjective
            // (à compléter)

            Position = position;
            Cible = cible;
            OrientationVerticale = orientation;
            Direction = Vector3.Normalize(Cible - Position);
            Latéral = Vector3.Cross(Direction, OrientationVerticale);
            OrientationVerticale = Vector3.Cross(Direction, Latéral);
            CréerPointDeVue();
        }

        public override void Update(GameTime gameTime)
        {
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;
            GestionClavier();
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {
                if (GestionInput.EstEnfoncée(Keys.LeftShift) || GestionInput.EstEnfoncée(Keys.RightShift))
                {
                     GérerDéplacement();
                    CréerPointDeVue(Position, Cible, OrientationVerticale);
                }
                TempsÉcouléDepuisMAJ = 0;
            }
            base.Update(gameTime);
        }

        private int GérerTouche(Keys touche)
        {
            return GestionInput.EstEnfoncée(touche) ? 1 : 0;
        }

        private void GérerDéplacement()
        {
            Vector3 nouvellePosition = Position;
            float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
            float déplacementLatéral = (GérerTouche(Keys.A) - GérerTouche(Keys.D)) * VitesseTranslation;

            CréerPointDeVue();

            Position += Direction * déplacementDirection;
            Position -= Latéral * déplacementLatéral;

        }

        private void GestionClavier()
        {
            if (GestionInput.EstNouvelleTouche(Keys.Z))
            {
                EstEnZoom = !EstEnZoom;
            }
        }
    }
}

using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Sound;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Terrain;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Group.Model.Parcelas;
using TGC.Group.Model.Camera;
using TGC.Core.Shaders;
using Microsoft.DirectX.Direct3D;
using TGC.Examples.Camara;
using TGC.Group.Model.Enviroment_Objects;
using TGC.Core.Collision;
using TGC.Core.Text;
using TGC.Examples.Engine2D.Spaceship.Core;
using TGC.Core.Particle;
using TGC.Core.Mathematica;
using TGC.Core.Fog;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    /// 

    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }
        #region HUD
        //HUD
        private Drawer2D drawer2D;

        //Head        
        private CustomSprite headHUD;

        //Box
        private CustomSprite boxHUD;

        //Text
        private TgcText2D livesText;
        private TgcText2D boxesText;

        #endregion

        //Sonidos
        private TgcStaticSound jumpSound;
        private Tgc3dSound jungleAmbience;
        private TgcStaticSound walkSound;
        private TgcStaticSound takeBox;
        private Tgc3dSound cricketsSound;
        private TgcStaticSound getalife;
        private TgcStaticSound gameOverSound;
        private TgcStaticSound gameWinSound;
        private bool playedMusic = false;

        //Optimización por Frustum Culling
        List<TgcMesh> candidatos = new List<TgcMesh>();

        //Camara 3ra persona no pudo ser el efecto de resorte
        private TgcThirdPersonCamera camara3rdPerson;
        private List<TgcMesh> objectsFront = new List<TgcMesh>();
        private List<TgcMesh> objectsBack = new List<TgcMesh>();

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        //Paths
        private List<Parcela> FullLevel = new List<Parcela>();
        private List<Parcela> Pits = new List<Parcela>();

        //Player
        private TgcSkeletalMesh character;
        private TgcBoundingSphere characterSphere;
        private TgcBoundingAxisAlignBox characterBox;
        private bool jumping = false;
        private int boxesTaked = 0;
        private float positionY = 0;
        private int lives = 5;
        private bool winGame = false;
        private bool gameOver = false;
        private ParticleEmitter walkEmitter;
        private bool walk = false;

        //Vida
        private Vida live;
        private List<Vida> Lives = new List<Vida>();

        //Caja
        private Caja BoxClass;

        //Cajas a Juntar
        private List<Caja> Boxes = new List<Caja>();

        //Parametros varios
        private float velocity = 15f;
        private float rotationVelocity = 2f;
        private float acumTime;

        //Skybox
        private TgcSkyBox SkyBox { get; set; }

        //Ambiente
        private List<Tower> torres = new List<Tower>();
        private List<Temple> templos = new List<Temple>();
        private List<TgcMesh> plantas = new List<TgcMesh>();
        private ParticleEmitter fireEmitter;
        private ParticleEmitter leafEmitter;
        private TgcMesh fogata;

        //Niebla
        //private TgcFog fog;
        //private Microsoft.DirectX.Direct3D.Effect effectFog;

        //Shader
        private Microsoft.DirectX.Direct3D.Effect Shader { get; set; }
        private List<TgcMesh> meshToShade = new List<TgcMesh>();
        private Microsoft.DirectX.Direct3D.Effect ShaderQuad { get; set; }
        public bool Walk { get => walk; set => walk = value; }
        public bool Walk1 { get => walk; set => walk = value; }

        //Full Quad 
        CustomVertex.PositionTextured[] screenQuadVertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
        //vertex buffer de los triangulos
        VertexBuffer ScreenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                4, D3DDevice.Instance.Device, Usage.Dynamic | Usage.WriteOnly,
            CustomVertex.PositionTextured.Format, Pool.Default);

        private Surface g_pDepthStencil; // Depth-stencil buffer
        private Texture g_pRenderTarget;

        //Messages
        private System.Drawing.Text.PrivateFontCollection Fonts;
        private TgcText2D gameOverMessage;
        private TgcText2D winGameMessage;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;

            //Cargar personaje con animaciones
            var skeletalLoader = new TgcSkeletalLoader();
            //RobotTGC ok; Hunter ok; BasicHuman ok; Samus nope; Trooper nope;
            character =
                skeletalLoader.loadMeshAndAnimationsFromFile(
                    MediaDir + "SkeletalAnimations\\Robot\\Robot-TgcSkeletalMesh.xml",
                    MediaDir + "SkeletalAnimations\\Robot\\",
                    new[]
                    {
                        MediaDir + "SkeletalAnimations\\Robot\\Caminando-TgcSkeletalAnim.xml",
                        MediaDir + "SkeletalAnimations\\Robot\\Parado-TgcSkeletalAnim.xml"
                    });

            //Configurar animacion inicial
            character.playAnimation("Parado", true);

            //Se utiliza autotransform, aunque este es un claro ejemplo de que no se debe usar autotransform,
            //hay muchas operaciones y la mayoria las maneja el manager de colisiones, con lo cual se esta
            //perdiendo el control de las transformaciones del personaje.
            //Escalarlo porque es muy grande
            character.Scale = new TGCVector3(0.1f, 0.1f, 0.1f);
            //Lo ubicamos 
            character.Position = new TGCVector3(10, -0.5f, 10);
            character.UpdateMeshTransform();
            
            //BoundingSphere que va a usar el personaje
            character.AutoUpdateBoundingBox = true;
            characterSphere = new TgcBoundingSphere(character.BoundingBox.calculateBoxCenter(), character.BoundingBox.calculateBoxRadius());
            characterBox = character.BoundingBox.clone();  

            //Configuro donde esta la posicion de la camara y hacia donde mira.
            camara3rdPerson = new TgcThirdPersonCamera(character.Position,40,80);
            //var fpsCamara = new TGC.Group.Camera.TgcFpsCamera(cameraPosition,100,100,Input);
            Camara = camara3rdPerson;

            #region Cajas
            //Cajas| objetivo es juntar una serie de cajas
            var boxTexture = MediaDir + "cajaMadera2.jpg";
            var posYBox = 5;

            BoxClass = new Caja(new Vector3(30,posYBox,30),boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(20, posYBox, 80), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(60, posYBox, 70), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(80, posYBox, 70), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(30, 25, 120), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(25, 25, 145), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(35, 25, 165), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(68, 25, 165), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(78, 25, 185), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(110, posYBox, 65), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(120, posYBox, 85), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(130, posYBox, 45), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(225, posYBox, 215), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(75, posYBox, 225), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(130, posYBox, 235), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(165, posYBox, 265), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(180, posYBox, 35), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(170, posYBox, 70), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(150, posYBox, 30), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(120, posYBox, 280), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(220, posYBox, 325), boxTexture);
            Boxes.Add(BoxClass);
            
            BoxClass = new Caja(new Vector3(130, posYBox, 330), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(80, posYBox, 365), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(120, posYBox, 380), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(180, posYBox, 365), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(170, posYBox, 420), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(130, 25, 480), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(80, 25, 420), boxTexture);
            Boxes.Add(BoxClass);
            #endregion

            //Crear SkyBox
            SkyBox = new TgcSkyBox
            {
                Center = new TGCVector3(0, 500, 0),
                Size = new TGCVector3(10000, 10000, 10000)
            };
            var texturesPath = MediaDir + "SkyBox1\\";
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            SkyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");
            SkyBox.Init();

            //Shaders
            Shader = TGC.Core.Shaders.TgcShaders.loadEffect(ShadersDir + "TgcMeshShader.fx");

            #region Caminos
            /*
            SIz = Superior Izquierda 
            SDe = Superior Derecha
            IIZ = Inferior Izquierda
            IDe = Inferior Izquierda
            Hor = Horizontal
            Ver = Vertical

             *---*---*---*
             |SIz|Hor|SDe|
             *---*---*---*
             |Ver|   |Ver|
             *---*---*---*
             |IIz|Hor|IDe|
             *---*---*---*
             */

            //caminos
            Horizontal pathHorizontal;
            Vertical pathVertical;
            SuperiorLeft pathSuperiorLeft;
            SuperiorRight pathSuperiorRight;
            InferiorLeft pathInferiorLeft;
            InferiorRight pathInferiorRight;
            Pit pathPit;
            Inicio pathInicio;
            Fin pathFin;
            SideLeft pathSideLeft;
            PitH pathPitH;

            //Modelo de planta
            var plantModel = new TgcSceneLoader().loadSceneFromFile(MediaDir + "Planta\\Planta-TgcScene.xml").Meshes[0];

            //Texturas 
            var wallTexture = TgcTexture.createTexture(MediaDir + "azwallAmoss.jpg");
            var grassTexture = TgcTexture.createTexture(MediaDir + "azgrss.jpg");
            var poleTexture = TgcTexture.createTexture(MediaDir + "az_pole01.jpg");
            var topTexture = TgcTexture.createTexture(MediaDir + "AzStatB.jpg");

            //Paths horizontales
            pathHorizontal = new Horizontal(new TGCVector3(50,0,50), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathHorizontal);

            pathHorizontal = new Horizontal(new TGCVector3(100, 0, 200), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathHorizontal);

            pathHorizontal = new Horizontal(new TGCVector3(100, 0, 300), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathHorizontal);

            pathHorizontal = new Horizontal(new TGCVector3(100, 0, 350), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathHorizontal);

            pathHorizontal = new Horizontal(new TGCVector3(100, 20, 450), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathHorizontal);

            //Paths verticales            

            pathVertical = new Vertical(new TGCVector3(0, 0, 0), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new TGCVector3(150, 0, 50), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new TGCVector3(150, 0, 150), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new TGCVector3(0, 20, 100), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new TGCVector3(150, 0, 400), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new TGCVector3(50, 20, 400), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathVertical);
            
            //Paths superior izquierdo
            pathSuperiorLeft = new SuperiorLeft(new TGCVector3(0, 0, 50), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorLeft);

            pathSuperiorLeft = new SuperiorLeft(new TGCVector3(0, 20, 150), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorLeft);

            pathSuperiorLeft = new SuperiorLeft(new TGCVector3(50, 0, 350), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorLeft);

            pathSuperiorLeft = new SuperiorLeft(new TGCVector3(50, 20, 450), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorLeft);

            //Paths superior derecho
            pathSuperiorRight = new SuperiorRight(new TGCVector3(100, 0, 50), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorRight);

            pathSuperiorRight = new SuperiorRight(new TGCVector3(200, 0, 300), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSuperiorRight);

            //Paths inferior izquierdo
            pathInferiorLeft = new InferiorLeft(new TGCVector3(100, 0, 0), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorLeft);

            pathInferiorLeft = new InferiorLeft(new TGCVector3(50, 0, 300), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorLeft);

            //Paths inferior derecho
            pathInferiorRight = new InferiorRight(new TGCVector3(150, 0, 0), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorRight);

            pathInferiorRight = new InferiorRight(new TGCVector3(50, 20, 150), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorRight);

            pathInferiorRight = new InferiorRight(new TGCVector3(200, 0, 200), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorRight);

            pathInferiorRight = new InferiorRight(new TGCVector3(150, 0, 250), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorRight);

            pathInferiorRight = new InferiorRight(new TGCVector3(150, 0, 350), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathInferiorRight);

            //Path inicio
            pathInicio = new Inicio(new TGCVector3(0, 0, -50), grassTexture, wallTexture, poleTexture, topTexture);
            FullLevel.Add(pathInicio);

            //Path fin
            pathFin = new Fin(new TGCVector3(150, 0, 500), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathFin);

            //Paths que son fozas
            pathPit = new Pit(new TGCVector3(150, 0, 100), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPit);
            Pits.Add(pathPit);

            pathPit = new Pit(new TGCVector3(200, 0, 250), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPit);
            Pits.Add(pathPit);

            pathPit = new Pit(new TGCVector3(150, 0, 450), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPit);
            Pits.Add(pathPit);

            pathPitH = new PitH(new TGCVector3(150, 0, 200), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPitH);
            Pits.Add(pathPitH);

            pathPitH = new PitH(new TGCVector3(150, 0, 300), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPitH);
            Pits.Add(pathPitH);

            //Path lado izquierdo
            pathSideLeft = new SideLeft(new TGCVector3(50, 0, 200), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSideLeft);

            pathSideLeft = new SideLeft(new TGCVector3(100, 0, 250), grassTexture, wallTexture, poleTexture, topTexture, plantModel);
            FullLevel.Add(pathSideLeft);
#endregion

            foreach (var path in FullLevel)
            {
                foreach(var wall in path.GetAllMeshes())
                {
                    meshToShade.Add(wall);
                }
            }

            #region Plantas
            var basePlant = new TgcSceneLoader().loadSceneFromFile(MediaDir + "\\ArbolSelvatico\\ArbolSelvatico-TgcScene.xml").Meshes[0];
            basePlant.Position = new TGCVector3(0, 0,-60);
            basePlant.Scale = new TGCVector3(0.25f, 0.25f, 0.25f);
            var random = new Random();
            var ran = random.Next(1, 100);
            basePlant.RotateY(ran);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(50,0,-60);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f) ;
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(100, 0, -30);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(-40, 0, 120);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(-40, 0, 150);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(-40, 0, -20);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(170, 0, -25);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(235, 0, 5);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(230, 0, 50);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(275, 0, 225);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(275, 0, 275);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(270, 0, 335);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(20, 0, 320);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(25, 0, 380);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(240, 0, 480);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(-30, 0, 200);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(130, 0, -55);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(220, 0, -40);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(300, 0, 300);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(20, 0, 440);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(35, 0, 480);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(175, 0, 575);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(20, 0, 495);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(40, 0, 535);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(80, 0, 515);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(125, 0, 525);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);

            basePlant = basePlant.clone("a");
            basePlant.Position = new TGCVector3(235, 0, 535);
            ran = random.Next(20, 100);
            basePlant.RotateY(ran);
            basePlant.Scale = new TGCVector3(0.25f, 0.008f * ran, 0.25f);
            basePlant.Enabled = true;
            basePlant.UpdateMeshTransform();
            basePlant.updateBoundingBox();

            plantas.Add(basePlant);
            #endregion

            //Torres
            Tower torre;

            torre = new Tower(new Vector3(50,0,0),MediaDir + "azwallb2.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            torre = new Tower(new Vector3(50, 0, 100), MediaDir + "azwallad2b.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            torre = new Tower(new Vector3(100, 0, 100), MediaDir + "azwallb2.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            torre = new Tower(new Vector3(100, 0, 150), MediaDir + "azwallad2b.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            torre = new Tower(new Vector3(50, 0, 250), MediaDir + "azwallb2.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            torre = new Tower(new Vector3(100, 0, 400), MediaDir + "azwallad2b.jpg", MediaDir + "az_pole01.jpg");
            torres.Add(torre);

            //Templos
            Temple templo;

            templo = new Temple(new Vector3(-100, 0, 0), MediaDir + "azwallAc.jpg", MediaDir + "az_pole01.jpg", MediaDir + "azwalltrim2.jpg", MediaDir + "AzStatB.jpg", MediaDir + "azgroundB.jpg");
            templos.Add(templo);

            templo = new Temple(new Vector3(200, 0, 100), MediaDir + "azwallAc.jpg", MediaDir + "az_pole01.jpg", MediaDir + "azwalltrim2.jpg", MediaDir + "AzStatB.jpg", MediaDir + "azgroundB.jpg");
            templos.Add(templo);

            templo = new Temple(new Vector3(-50, 0, 200), MediaDir + "azwallAc.jpg", MediaDir + "az_pole01.jpg", MediaDir + "azwalltrim2.jpg", MediaDir + "AzStatB.jpg", MediaDir + "azgroundB.jpg");
            templos.Add(templo);

            templo = new Temple(new Vector3(200, 0, 350), MediaDir + "azwallAc.jpg", MediaDir + "az_pole01.jpg", MediaDir + "azwalltrim2.jpg", MediaDir + "AzStatB.jpg", MediaDir + "azgroundB.jpg");
            templos.Add(templo);

            //Sonidos Init
            jumpSound = new TgcStaticSound();
            jumpSound.dispose();
            jumpSound.loadSound(MediaDir + "Mario_Jumping.wav", DirectSound.DsDevice);

            jungleAmbience = new Tgc3dSound(MediaDir + "jungle.wav", new TGCVector3(100, 100, 100), DirectSound.DsDevice)
            {
                MinDistance = 120
            };

            walkSound = new TgcStaticSound();
            walkSound.dispose();
            walkSound.loadSound(MediaDir + "pl_grass1.wav", DirectSound.DsDevice);

            takeBox = new TgcStaticSound();
            takeBox.dispose();
            takeBox.loadSound(MediaDir + "door_wood_shake1.wav", DirectSound.DsDevice);//TODO: cambiar este sonido por uno mejor

            cricketsSound = new Tgc3dSound(MediaDir + "crickets.wav", new TGCVector3(200, 0, 100), DirectSound.DsDevice)
            {
                MinDistance = 20
            };

            getalife = new TgcStaticSound();
            getalife.dispose();
            getalife.loadSound(MediaDir + "getalife.wav", DirectSound.DsDevice);

            gameOverSound = new TgcStaticSound();
            gameOverSound.dispose();
            gameOverSound.loadSound(MediaDir + "gameover.wav", DirectSound.DsDevice);

            gameWinSound = new TgcStaticSound();
            gameWinSound.dispose();
            gameWinSound.loadSound(MediaDir + "gamewin.wav", DirectSound.DsDevice);

            //Mensajes
            //Fuentes
            Fonts = new System.Drawing.Text.PrivateFontCollection();
            Fonts.AddFontFile(MediaDir + "\\Fonts\\The tropical jungle.ttf");
            Fonts.AddFontFile(MediaDir + "\\Fonts\\TroglodyteNF.ttf");

            //Game Over
            gameOverMessage = new TgcText2D();
            gameOverMessage.changeFont(new System.Drawing.Font(Fonts.Families[0], 55));
            gameOverMessage.Color = Color.Red;
            gameOverMessage.Text = "Game Over!";
            gameOverMessage.Position = new Point(D3DDevice.Instance.Width / 16, D3DDevice.Instance.Height / 2);

            //Win Game
            winGameMessage = new TgcText2D();
            winGameMessage.changeFont(new System.Drawing.Font(Fonts.Families[0], 55));
            winGameMessage.Color = Color.Green;
            winGameMessage.Text = "You Win!";
            winGameMessage.Position = new Point(D3DDevice.Instance.Width / 16, D3DDevice.Instance.Height / 2);

            //Full Quad init
            ScreenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);
            ShaderQuad = TgcShaders.loadEffect(ShadersDir + "FullQuad.fx");

            g_pDepthStencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth,
                d3dDevice.PresentationParameters.BackBufferHeight,
                DepthFormat.D24S8, MultiSampleType.None, 0, true);

            g_pRenderTarget = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget, Format.X8R8G8B8,
                Pool.Default);

            ShaderQuad.SetValue("g_RenderTarget", g_pRenderTarget);

            ShaderQuad.SetValue("screen_dx", d3dDevice.PresentationParameters.BackBufferWidth);
            ShaderQuad.SetValue("screen_dy", d3dDevice.PresentationParameters.BackBufferHeight);

            #region HUD init
            //HUD
            drawer2D = new Drawer2D();

            //Head
            headHUD = new CustomSprite
            {
                Bitmap = new CustomBitmap(MediaDir + "\\head.png", D3DDevice.Instance.Device)
            };

            livesText = new TgcText2D();
            livesText.changeFont(new System.Drawing.Font(Fonts.Families[1], 55));
            livesText.Color = Color.Red;
            livesText.Text = "x" + lives;
            livesText.Position = new Point((int) (D3DDevice.Instance.Width * 0.35f) , (int)(D3DDevice.Instance.Height * 0.75f) );

            //Box
            boxHUD = new CustomSprite
            {
                Bitmap = new CustomBitmap(MediaDir + "\\box.png", D3DDevice.Instance.Device)
            };

            boxesText = new TgcText2D();
            boxesText.changeFont(new System.Drawing.Font(Fonts.Families[1], 55));
            boxesText.Color = Color.Red;
            boxesText.Text = "x" + boxesTaked;
            boxesText.Position = new Point(-(int)(D3DDevice.Instance.Width / 2 * 0.6f), (int)(D3DDevice.Instance.Height * 0.1f));
            #endregion

            //Logos de vida
            live = new Vida(MediaDir + "\\LogoTGC\\LogoTGC-TgcScene.xml", new TGCVector3(175,10,170));
            Lives.Add(live);

            live = new Vida(MediaDir + "\\LogoTGC\\LogoTGC-TgcScene.xml", new TGCVector3(80, 10, 335));
            Lives.Add(live);

            live = new Vida(MediaDir + "\\LogoTGC\\LogoTGC-TgcScene.xml", new TGCVector3(75, 30, 475));
            Lives.Add(live);

            //Particulas del personaje
            walkEmitter = new ParticleEmitter(MediaDir + "pisada.png", 40)
            {
                CreationFrecuency = 1f,
                MinSizeParticle = 46f,
                MaxSizeParticle = 56f,
                Position = character.Position,
                Speed = new TGCVector3(0, 20, 0),
                ParticleTimeToLive = 4f,
                Dispersion = 1000,
                Enabled = true,
                Playing = true
            };

            fireEmitter = new ParticleEmitter(MediaDir + "fuego.png", 40)
            {
                CreationFrecuency = 0.2f,
                MinSizeParticle = 0.6f,
                MaxSizeParticle = 1f,
                Position = new TGCVector3(25, 0, -20),
                Speed = new TGCVector3(0, 20, 0),
                ParticleTimeToLive = 4f
            };
            fireEmitter.ParticleTimeToLive = 1f;
            fireEmitter.Dispersion = 350;
            fireEmitter.Enabled = true;
            fireEmitter.Playing = true;

            leafEmitter = new ParticleEmitter(MediaDir + "hoja.png", 140)
            {
                CreationFrecuency = 1f,
                MinSizeParticle = 46f,
                MaxSizeParticle = 56f,
                Position = new TGCVector3(25, 200, -30),
                Speed = new TGCVector3(0, -20, 0),
                ParticleTimeToLive = 4f,
                Dispersion = 1000,
                Enabled = true,
                Playing = true
            };

            fogata = new TgcSceneLoader().loadSceneFromFile(MediaDir + "\\Tripode\\Tripode-TgcScene.xml").Meshes[0];
            fogata.Position = new TGCVector3(22, 0, -20);
            fogata.Scale = new TGCVector3(0.04f, 0.04f, 0.04f);
            fogata.Enabled = true;
            fogata.UpdateMeshTransform();

            //Init de niebla
            //effectFog = TgcShaders.loadEffect(ShadersDir + "TgcFogShader.fx");

            acumTime = 0;
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            headHUD.Position = new Vector2(D3DDevice.Instance.Width * 0.7f , D3DDevice.Instance.Height * 0.7f);
            headHUD.Scaling = new Vector2(0.25f, 0.15f);

            boxHUD.Position = new Vector2(D3DDevice.Instance.Width * 0.05f, D3DDevice.Instance.Height * 0.1f);
            boxHUD.Scaling = new Vector2(0.25f, 0.25f);

            livesText.Text = "x" + lives;
            boxesText.Text = "x" + boxesTaked;

            walkEmitter.Position = character.Position;

            //Calcular proxima posicion de personaje segun Input
            var moveForward = 0f;
            float rotate = 0;
            bool moving = false;
            bool rotating = false;
            Walk = false;
            
            float jump = 0;

            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }
            
            //Move forward
            if (Input.keyDown(Key.W))
            {
                moveForward = -velocity;
                Walk = true;
                moving = true;
            }

            //Move backwards
            if (Input.keyDown(Key.S))
            {
                moveForward = velocity;
                Walk = true;
                moving = true;
            }

            //Rotate left
            if (Input.keyDown(Key.A))
            {
                rotate = -rotationVelocity;
                rotating = true;
            }

            //Rotate right
            if (Input.keyDown(Key.D))
            {
                rotate = rotationVelocity;
                rotating = true;
            }

            Parcela parcelaCol = new Parcela();
            //Jump
            foreach(var path in FullLevel)
            {
                if (path.IsInParcela(character.Position))
                {
                    parcelaCol = path;
                }
            }

            positionY = parcelaCol.GetPosition().Y;

            if (Input.keyPressed(Key.Space) && jump == 0)
            {
                jumpSound.play(false);
                jumping = true;
                moving = true;
            }

            if (jumping)
            {
                jump += ElapsedTime * 80f;
            }

            if (jump > 30 || character.Position.Y > 40)
            {
                jumping = false;
            }

            if (character.Position.Y > positionY  && !jumping)
            {
                moving = true;
                jump -= 80 * ElapsedTime;
            }
                
            if (character.Position.Y < positionY - 20)
            {
                character.Move(new TGCVector3(character.Position.X, 0, character.Position.Z) - character.Position);
            }            
            
            if (rotating)
            {
                character.playAnimation("Caminando", true);
                var rotAngle = rotate * ElapsedTime;
                camara3rdPerson.rotateY(rotAngle);
                Camara = camara3rdPerson;
                character.RotateY(rotAngle);
            }
            
            //Condiciones de derrota y victoria
            if (lives == 0 && !playedMusic)
            {
                gameOverSound.play(false);
                playedMusic = true;
                gameOver = true;
            }
            if (boxesTaked == 28 && !playedMusic)
            { 
                winGame = true;
                playedMusic = true;
                gameWinSound.play(false);
            } 

            //Vector de movimiento
            var movementVector = Vector3.Empty;
            if ((moving || rotating) && !jumping) walkSound.play(false);
           
            if ((moving || rotating || jumping) && (!gameOver) && (!winGame))
            {
                character.playAnimation("Caminando", true);
                //Colision mocha TODO: arregla esto hermano, ahora solo deja caminar en un rango hay que analizar dentro de cada tipo de parcela.
                movementVector = new Vector3(FastMath.Sin(character.Rotation.Y) * moveForward * 0.1f, jump, FastMath.Cos(character.Rotation.Y) * moveForward * 0.1f);
            }
            else
            {
                character.playAnimation("Parado", true);
            }

            //Acumuolamos tiempo para distintas tareas
            acumTime += ElapsedTime;

            character.Move(new TGCVector3(movementVector));
            character.UpdateMeshTransform();
            
            //Vemos si cae en algun pozo
            foreach (var pit in Pits)
            {
                if (pit.IsInPit(character.Position))
                {
                    lives -= 1;
                    character.Move(0, 0, -50);
                    break;
                }
            }

            //Vemos que se interpone a la camara
            var walls = new List<TgcMesh>();

            foreach(var path in FullLevel)
            {
                var dif = path.GetPosition() - character.Position;
                var dist = FastMath.Pow2(FastMath.Abs(dif.X)) + FastMath.Pow2(FastMath.Abs(dif.Y)) + FastMath.Pow2(FastMath.Abs(dif.Z));
                if(dist <= 22500)
                {
                    foreach (var wall in path.GetAllMeshes())
                    {
                        walls.Add(wall);
                    }
                }
            }

            foreach (var templo in templos)
            {
                foreach (var wall in templo.getWalls())
                {
                    walls.Add(wall);
                }
            }

            foreach (var torre in torres)
            {
                foreach (var wall in torre.getWalls())
                {
                    walls.Add(wall);
                }
            }

            foreach (var plant in plantas)
            {
                walls.Add(plant);
            }

            objectsFront.Clear();
            objectsBack.Clear();
            foreach(var mesh in walls)
            {
                if (TgcCollisionUtils.intersectSegmentAABB(Camara.Position, new TGCVector3(character.Position.X, character.Position.Y + 5, character.Position.Z), mesh.BoundingBox, out TGCVector3 q) ||
                    TgcCollisionUtils.intersectSegmentAABB(Camara.Position, new TGCVector3(character.Position.X, character.Position.Y + 50, character.Position.Z), mesh.BoundingBox, out q) ||
                    TgcCollisionUtils.intersectSegmentAABB(Camara.Position, new TGCVector3(character.Position.X + 5, character.Position.Y + 25, character.Position.Z + 5), mesh.BoundingBox, out q) ||
                    TgcCollisionUtils.intersectSegmentAABB(Camara.Position, new TGCVector3(character.Position.X - 5, character.Position.Y + 25, character.Position.Z - 5), mesh.BoundingBox, out q))
                {
                    objectsBack.Add(mesh);
                }
                else
                {
                    objectsFront.Add(mesh);
                }
            }

            jungleAmbience.play(true);

            camara3rdPerson.Target = character.Position;

            PostUpdate();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            //PreRender();

            ClearTextures();
            var device = D3DDevice.Instance.Device;
            var pOldRT = device.GetRenderTarget(0);
            var pSurf = g_pRenderTarget.GetSurfaceLevel(0);
            device.SetRenderTarget(0, pSurf);
            var pOldDS = device.DepthStencilSurface;
            device.DepthStencilSurface = g_pDepthStencil;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

            device.BeginScene();
            //Aplicar a cada mesh el shader actual
            foreach (var mesh in meshToShade)
            {
                mesh.Effect = Shader;
                //El Technique depende del tipo RenderType del mesh
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }

            foreach(var live in Lives)
            {
                live.applyShader(Shader);
            }

            foreach(var box in Boxes)
            {
                box.applyEffect(Shader);
            }
            
            if (gameOver)
            {
                gameOverMessage.render();
            }

            if (winGame)
            {
                winGameMessage.render();
            }

            livesText.render();
            boxesText.render();

            //Renderizo cielo
            SkyBox.Render();

            //Cajas renderizadas
            foreach (var box in Boxes)
            {
                if (box.isColliding(character.BoundingBox) && box.boxQuantity == 0)
                {
                    takeBox.play(false);
                    boxesTaked += 1;
                }
                box.takeBox(character.BoundingBox);
                box.render();
            }

            foreach (var live in Lives)
            {
                if (live.isColliding(character.BoundingBox) && live.liveQuantity == 0)
                {
                    getalife.play(false);
                    lives += 1;
                } 
                live.takeLive(character.BoundingBox);
                live.render();
            }
            
            foreach(var wall in meshToShade)
            {
                wall.Effect.SetValue("color", ColorValue.FromColor(Color.PeachPuff));
                wall.Effect.SetValue("time", acumTime );
                wall.Effect.SetValue("timeFrame", ElapsedTime);
                wall.Effect.SetValue("playerPos", new Vector4( character.Position.X, character.Position.Y, character.Position.Z, 1));
            }

            //renderizo y animo el personaje
            character.animateAndRender(ElapsedTime);

            candidatos.Clear();

            //Renderizo el camino
            //TODO: Para que quede copado debo sumar un fog sino se ve feo
            foreach (var path in objectsFront)
            {
                //Renderizar modelo con FrustumCulling
                var r = TgcCollisionUtils.classifyFrustumAABB(Frustum, path.BoundingBox);
                if (r != TgcCollisionUtils.FrustumResult.OUTSIDE)
                {
                    candidatos.Add(path);
                }
            }

            foreach (var path in candidatos)
            {
                path.Render();
            }

            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(headHUD);
            drawer2D.DrawSprite(boxHUD);
            drawer2D.EndDrawSprite();

            //Sistema de particulas
            D3DDevice.Instance.ParticlesEnabled = true;
            D3DDevice.Instance.EnableParticles();
            /*
            if (walk)
            {
                walkEmitter.Enabled = true;
                walkEmitter.CreationFrecuency = 0.2f;
                walkEmitter.MinSizeParticle = 0.6f;
                walkEmitter.MaxSizeParticle = 1f;
                walkEmitter.Speed = new TGCVector3(0, 5, 0);
                walkEmitter.ParticleTimeToLive = 1f;
                walkEmitter.Dispersion = 350;
                walkEmitter.Position = character.Position;
                walkEmitter.Playing = true;
                walkEmitter.render(ElapsedTime);
            }
            if (!walk) walkEmitter.Playing = false;

            walkEmitter.render(ElapsedTime);*/
            
            fireEmitter.render(ElapsedTime);
            /*
            leafEmitter.Enabled = true;
            leafEmitter.CreationFrecuency = 0.2f;
            leafEmitter.MinSizeParticle = 0.3f;
            leafEmitter.MaxSizeParticle = 0.8f;
            leafEmitter.Speed = new TGCVector3(2, -20, 4);
            leafEmitter.ParticleTimeToLive = 1f;
            leafEmitter.Position = new TGCVector3( character.Position.X, character.Position.Y + 40, character.Position.Z);
            leafEmitter.Dispersion = 1050;
            leafEmitter.Playing = true;
            leafEmitter.render(ElapsedTime);*/

            fogata.Render();

            device.EndScene();

            //Arranque del full quad
            pSurf.Dispose();

            // restuaro el render target y el stencil
            device.DepthStencilSurface = pOldDS;
            device.SetRenderTarget(0, pOldRT);

            // dibujo el quad pp dicho :
            device.BeginScene();

            ShaderQuad.Technique = "PostProcess";
            // Condicion para una tecnica diferente --> if ((glowstick.getEnergia() == 0 && glowstick.getSelect()) || (System.Math.Truncate(lighter.getEnergia()) == 0 && lighter.getSelect()) || (System.Math.Truncate(flashlight.getEnergia()) == 0 && flashlight.getSelect())) ShaderQuad.Technique = "Waves";
            ShaderQuad.SetValue("time", ElapsedTime);
            device.VertexFormat = CustomVertex.PositionTextured.Format;
            device.SetStreamSource(0, ScreenQuadVB, 0);
            ShaderQuad.SetValue("g_RenderTarget", g_pRenderTarget);

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            ShaderQuad.Begin(FX.None);
            ShaderQuad.BeginPass(0);
            device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            ShaderQuad.EndPass();
            ShaderQuad.End();

            device.EndScene();

            device.BeginScene();

            //Dibuja un texto por pantalla
            DrawText.drawText("Tiempo Acumulado: " + acumTime, 0, 40, Color.OrangeRed);
            DrawText.drawText("Ubicacion Personaje: \n" + character.Position, 0, 50, Color.OrangeRed);
            DrawText.drawText("Cajas obtenidas: " + boxesTaked, 0, 110, Color.OrangeRed);
            DrawText.drawText("Vidas: " + lives, 0, 130, Color.OrangeRed);
            DrawText.drawText("posicion texto cajas: " + boxesText.Position, 0, 150, Color.OrangeRed);

            RenderAxis();
            RenderFPS();
            device.EndScene();
            device.Present();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            //PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            character.Dispose();
            SkyBox.Dispose();

            //Dispose de los caminos
            foreach(var path in FullLevel)
            {
                path.Dispose();
            }

            //Dispose de las torres
            foreach(var tower in torres)
            {
                tower.dispose();
            }

            //Dispose de las Cajas
            foreach (var box in Boxes)
            {
                box.dispose();
            }

            //Dispose de los Templos
            foreach (var templo in templos)
            {
                templo.dispose();
            }

            jumpSound.dispose();
        }
    }
}
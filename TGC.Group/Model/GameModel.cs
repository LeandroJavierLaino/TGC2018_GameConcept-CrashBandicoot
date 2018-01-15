using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;
using System.Drawing;
using TGC.Core.BoundingVolumes;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Terrain;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Group.Model.Parcelas;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer m�s ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
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
        //Camara 3ra persona con efecto de resorte
        private TGC.Examples.Camara.TgcSpringThirdPersonCamera camaraSpring;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        //Path a partir de un plano
        private TgcPlane Path { get; set; }
        private TgcPlane PathB { get; set; }
        private List<TgcPlane> FullCourse = new List<TgcPlane>();
        private Horizontal pathHorizontal;


        //Player
        private TgcSkeletalMesh character;
        private TgcBoundingSphere characterSphere;
        private TgcBoundingAxisAlignBox characterBox;
        private bool jumping = false;

        //Caja
        private Caja BoxClass;

        //Cajas a Juntar
        private List<Caja> Boxes = new List<Caja>();

        //Vegetacion
        private TgcMesh Planta;
        private List<TgcMesh> Plantas = new List<TgcMesh>();

        //Parametros varios
        private float velocity = 5f;
        private float rotationVelocity = 10;
        private float acumTime;

        private TgcSkyBox skyBox { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqu� todo el c�digo de inicializaci�n: cargar modelos, texturas, estructuras de optimizaci�n, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;
            
            //Textura de la carperta Media. Game.Default es un archivo de configuracion (Game.settings) util para poner cosas.
            //Pueden abrir el Game.settings que se ubica dentro de nuestro proyecto para configurar.
            var pathTexturaCaja = MediaDir + Game.Default.TexturaCaja;

            //Cargamos una textura, tener en cuenta que cargar una textura significa crear una copia en memoria.
            //Es importante cargar texturas en Init, si se hace en el render loop podemos tener grandes problemas si instanciamos muchas.
            var texture = TgcTexture.createTexture(pathTexturaCaja);

            //Internamente el framework construye la matriz de view con estos dos vectores.
            //Luego en nuestro juego tendremos que crear una c�mara que cambie la matriz de view con variables como movimientos o animaciones de escenas.

            //Path basico
            var texturaPasto = MediaDir + "grass.jpg";
            Path = new TgcPlane(Vector3.Empty, new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(texturaPasto), 4, 4);
            PathB = new TgcPlane(new Vector3(50,0,0), new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(texturaPasto), 4, 4);

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
            character.Scale = new Vector3(0.1f, 0.1f, 0.1f);
            //Lo ubicamos 
            character.Position = new Vector3(10, 0, 10);
            character.UpdateMeshTransform();
            //BoundingSphere que va a usar el personaje
            character.AutoUpdateBoundingBox = true;
            characterSphere = new TgcBoundingSphere(character.BoundingBox.calculateBoxCenter(), character.BoundingBox.calculateBoxRadius());
            characterBox = character.BoundingBox.clone();            

            //Suelen utilizarse objetos que manejan el comportamiento de la camara.
            //Lo que en realidad necesitamos gr�ficamente es una matriz de View.
            //El framework maneja una c�mara est�tica, pero debe ser inicializada.
            //Posici�n de la camara.
            var cameraPosition = new Vector3(0, 0, 125);
            //Quiero que la camara mire hacia el origen (0,0,0).
            var lookAt = Vector3.Empty;
            //Configuro donde esta la posicion de la camara y hacia donde mira.

            //Camara = new TGC.Group.Camera.TgcFpsCamera(cameraPosition,100,100,Input);
            camaraSpring = new Examples.Camara.TgcSpringThirdPersonCamera();
            camaraSpring.setOrientation(new Vector3(0, FastMath.PI, 0));
            
            camaraSpring.setTargetOffset(character.Position, 50, 50);
            
            Camara = camaraSpring;

            //Cajas| objetivo es juntar una serie de cajas
            var boxTexture = MediaDir + "cajaMadera2.jpg";
            var posYBox = 5;

            BoxClass = new Caja(new Vector3(30,posYBox,30),boxTexture);
            Boxes.Add(BoxClass);

            //Crear SkyBox
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            var texturesPath = MediaDir + "SkyBox1\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "phobos_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "phobos_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "phobos_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "phobos_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "phobos_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "phobos_ft.jpg");
            skyBox.Init();

            //Crear Planta
            //Modelo base 1
            Planta = new TgcSceneLoader().loadSceneFromFile(MediaDir + "Planta\\Planta-TgcScene.xml").Meshes[0];

            //Planta 1
            Planta.Position = new Vector3(60,0,45);
            Planta.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            Planta.Enabled = true;
            Plantas.Add(Planta);

            //Planta 2
            Planta = Planta.clone("Planta2");
            Planta.Position = new Vector3(30, 0, 45);
            Planta.rotateY(30);
            Planta.Enabled = true;
            Plantas.Add(Planta);

            //Planta3
            Planta = Planta.clone("Planta3");
            Planta.Position = new Vector3(5, 0, 45);
            Planta.rotateY(16);
            Planta.Enabled = true;
            Plantas.Add(Planta);

            //Path horizontal
            pathHorizontal = new Horizontal(new Vector3(100,0,0), MediaDir + "grass.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            
            acumTime = 0;
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la l�gica de computo del modelo, as� como tambi�n verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();
    
            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            //Calcular proxima posicion de personaje segun Input
            var moveForward = 0f;
            float rotate = 0;
            bool moving = false;
            bool rotating = false;
            
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
                moving = true;
            }

            //Move backwards
            if (Input.keyDown(Key.S))
            {
                moveForward = velocity;
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

            //Jump
            if (Input.keyPressed(Key.Space) && jump == 0)
            {
                jumping = true;
                moving = true;
            }

            if (jumping /*&& moving*/)
            {
                jump += acumTime * 0.00002f;
            }

            if (jump > 30 || character.Position.Y > 30)
            {
                jumping = false;
            }

            if (character.Position.Y > 0 && !jumping)
            {
                moving = true;
                jump -= 30 * ElapsedTime;
            }

            if (rotating)
            {
                character.playAnimation("Caminando", true);
                var rotAngle = rotate * ElapsedTime;
                character.rotateY(rotAngle);
            }

            //Vector de movimiento
            var movementVector = Vector3.Empty;

            if (moving || rotating || jumping) 
            {
                character.playAnimation("Caminando", true);
                //Colision mocha TODO: arregla esto hermano, solo permite caminar dentro de una parcela
                /*if(Path.BoundingBox.PMin.X < character.Position.X || Path.BoundingBox.PMin.Z > character.Position.Z)*/ movementVector = new Vector3(FastMath.Sin(character.Rotation.Y) * moveForward * 0.1f,jump,FastMath.Cos(character.Rotation.Y) * moveForward * 0.1f);
            }
            else
            {
                character.playAnimation("Parado", true);
            }

            character.move(movementVector);
            character.UpdateMeshTransform();              

            camaraSpring.Target = character.Position;
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqu� todo el c�digo referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones seg�n nuestra conveniencia.
            PreRender();

            acumTime += ElapsedTime;
            //Dibuja un texto por pantalla
            DrawText.drawText("Con la tecla F se dibuja el bounding box.", 0, 20, Color.OrangeRed);
            DrawText.drawText("Con clic izquierdo subimos la camara [Actual]: " + TgcParserUtils.printVector3(Camara.Position), 0, 30, Color.OrangeRed);
            DrawText.drawText("Tiempo Acumulado: " + acumTime, 0, 40, Color.OrangeRed);
            DrawText.drawText("Ubicacion Personaje: " + character.Position, 0, 50, Color.OrangeRed);
            //Renderizo cielo
            skyBox.render();

            //Renderizo camino
            Path.render();
            PathB.render();
            
            //Clase Caja en funcionamiento
            BoxClass.animateBox(ElapsedTime, acumTime);
            BoxClass.takeBox(character.BoundingBox);
            BoxClass.render();

            character.animateAndRender(ElapsedTime);
            //character.BoundingBox.render();

            //Cuando tenemos modelos mesh podemos utilizar un m�todo que hace la matriz de transformaci�n est�ndar.
            //Es �til cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jer�rquicas o complicadas.
            //Mesh.UpdateMeshTransform();
            //Render del mesh
            //Mesh.render();

            foreach (var planta in Plantas)
            {
                planta.render();
            }

            pathHorizontal.render();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecuci�n del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gr�ficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            character.dispose();
            Path.dispose();
            PathB.dispose();
            skyBox.dispose();
            //Dispose de las plantas
            foreach (var planta in Plantas)
            {
                planta.dispose();
            }
            BoxClass.dispose();
        }
    }
}
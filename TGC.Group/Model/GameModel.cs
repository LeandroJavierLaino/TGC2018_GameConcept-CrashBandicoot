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

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    /// 

    delegate bool del(Caja box);

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
        private TgcThirdPersonCamera camaraSpring;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        //Path a partir de un plano
        private TgcPlane Path { get; set; }
        private TgcPlane PathB { get; set; }
        private List<Parcela> FullLevel = new List<Parcela>();

        //Player
        private TgcSkeletalMesh character;
        private TgcBoundingSphere characterSphere;
        private TgcBoundingAxisAlignBox characterBox;
        private bool jumping = false;
        private int boxesTaked = 0;

        //Caja
        private Caja BoxClass;

        //Cajas a Juntar
        private List<Caja> Boxes = new List<Caja>();

        //Parametros varios
        private float velocity = 1.2f;
        private float rotationVelocity = 2f;
        private float acumTime;

        //Skybox
        private TgcSkyBox skyBox { get; set; }

        //Ambiente
        private TgcSimpleTerrain terreno;

        //Shader
        private Microsoft.DirectX.Direct3D.Effect Shader { get; set; }
        private List<TgcMesh> meshToShade = new List<TgcMesh>();

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
            
            //Textura de la carperta Media. Game.Default es un archivo de configuracion (Game.settings) util para poner cosas.
            //Pueden abrir el Game.settings que se ubica dentro de nuestro proyecto para configurar.
            var pathTexturaCaja = MediaDir + Game.Default.TexturaCaja;

            //Cargamos una textura, tener en cuenta que cargar una textura significa crear una copia en memoria.
            //Es importante cargar texturas en Init, si se hace en el render loop podemos tener grandes problemas si instanciamos muchas.
            var texture = TgcTexture.createTexture(pathTexturaCaja);

            //Internamente el framework construye la matriz de view con estos dos vectores.
            //Luego en nuestro juego tendremos que crear una cámara que cambie la matriz de view con variables como movimientos o animaciones de escenas.

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
            //Lo que en realidad necesitamos gráficamente es una matriz de View.
            //El framework maneja una cámara estática, pero debe ser inicializada.
            //Posición de la camara.
            var cameraPosition = new Vector3(0, 0, 125);
            //Quiero que la camara mire hacia el origen (0,0,0).
            var lookAt = Vector3.Empty;
            //Configuro donde esta la posicion de la camara y hacia donde mira.

            //Camara = new TGC.Group.Camera.TgcFpsCamera(cameraPosition,100,100,Input);
            camaraSpring = new TgcThirdPersonCamera(character.Position,30,90);
            //camaraSpring.SetCamera(new Vector3(character.Position.X, character.Position.Y + 30, character.Position.Z), new Vector3(character.Position.X, character.Position.Y+30, character.Position.Z));
            //camaraSpring.setOffset(30);
            //camaraSpring.setTargetOffset(character.Position, 65, -40);
            Camara = camaraSpring;

            //Cajas| objetivo es juntar una serie de cajas
            var boxTexture = MediaDir + "cajaMadera2.jpg";
            var posYBox = 5;

            BoxClass = new Caja(new Vector3(30,posYBox,30),boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(20, posYBox, 80), boxTexture);
            Boxes.Add(BoxClass);

            BoxClass = new Caja(new Vector3(60, posYBox, 70), boxTexture);
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

            //Empezamos los caminos???
            Horizontal pathHorizontal;
            Vertical pathVertical;
            SuperiorLeft pathSuperiorLeft;
            SuperiorRight pathSuperiorRight;
            InferiorLeft pathInferiorLeft;
            InferiorRight pathInferiorRight;
            Pit pathPit;
            Inicio pathInicio;

            //Shaders? yaaaaay
            Shader = TGC.Core.Shaders.TgcShaders.loadEffect(ShadersDir + "TgcMeshShader.fx");
            //Shader = TGC.Core.Shaders.TgcShaders.loadEffect(ShadersDir + "TgcMeshPointLightShader.fx");

            //Paths horizontales
            pathHorizontal = new Horizontal(new Vector3(50,0,50), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathHorizontal);

            //Paths verticales
            pathVertical = new Vertical(new Vector3(0, 0, 0), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new Vector3(150, 0, 50), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new Vector3(150, 0, 150), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathVertical);

            pathVertical = new Vertical(new Vector3(0, 20, 100), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathVertical);

            //Paths superior izquierdo
            pathSuperiorLeft = new SuperiorLeft(new Vector3(0, 0, 50), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathSuperiorLeft);

            //Paths superior derecho
            pathSuperiorRight = new SuperiorRight(new Vector3(100, 0, 50), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathSuperiorRight);

            //Paths inferior izquierdo
            pathInferiorLeft = new InferiorLeft(new Vector3(100, 0, 0), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathInferiorLeft);

            //Paths inferior derecho
            pathInferiorRight = new InferiorRight(new Vector3(150, 0, 0), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg", MediaDir + "Planta\\Planta-TgcScene.xml");
            FullLevel.Add(pathInferiorRight);

            //Path inicio
            pathInicio = new Inicio(new Vector3(0, 0, -50), MediaDir + "azgrss.jpg", MediaDir + "azwallAmoss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathInicio);

            //Paths que son fozas
            pathPit = new Pit(new Vector3(150, 0, 100), MediaDir + "azgrss.jpg", MediaDir + "azwallAd2moss.jpg", MediaDir + "az_pole01.jpg", MediaDir + "AzStatB.jpg");
            FullLevel.Add(pathPit);

            foreach(var path in FullLevel)
            {
                foreach(var wall in path.walls)
                {
                    meshToShade.Add(wall);
                } 
            }

            terreno = new TgcSimpleTerrain();
            terreno.loadHeightmap(MediaDir + "valle.jpg", 100, 4f, new Vector3 (0,-100,0));
            terreno.loadTexture(MediaDir + "azgrssBig.jpg");

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
                jump += acumTime * 0.00006f;
            }

            if (jump > 30 || character.Position.Y > 40)
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
                camaraSpring.rotateY(rotAngle);
                Camara = camaraSpring;
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

            //Acumuolamos tiempo para distintas tareas
            acumTime += ElapsedTime;

            character.move(movementVector);
            character.UpdateMeshTransform();
            
            camaraSpring.Target = character.Position;
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();

            //Aplicar a cada mesh el shader actual
            foreach (var mesh in meshToShade)
            {
                mesh.Effect = Shader;
                //El Technique depende del tipo RenderType del mesh
                mesh.Technique = TgcShaders.Instance.getTgcMeshTechnique(mesh.RenderType);
            }

            foreach(var box in Boxes)
            {
                box.applyEffect(Shader);
            }

            //Dibuja un texto por pantalla
            DrawText.drawText("Con la tecla F se dibuja el bounding box.", 0, 20, Color.OrangeRed);
            DrawText.drawText("Con clic izquierdo subimos la camara [Actual]: " + TgcParserUtils.printVector3(Camara.Position), 0, 30, Color.OrangeRed);
            DrawText.drawText("Tiempo Acumulado: " + acumTime, 0, 40, Color.OrangeRed);
            DrawText.drawText("Ubicacion Personaje: \n" + character.Position, 0, 50, Color.OrangeRed);
            DrawText.drawText("Cajas obtenidas: " + boxesTaked, 0, 110, Color.OrangeRed);

            //Renderizo cielo
            skyBox.render();

            //Cajas renderizadas

            foreach (var box in Boxes)
            {
                if (box.isColliding(character.BoundingBox) && box.boxQuantity == 0) boxesTaked += 1;
                box.takeBox(character.BoundingBox);
                box.render();
            }
            
            foreach(var wall in meshToShade)
            {
                wall.Effect.SetValue("color", ColorValue.FromColor(Color.PeachPuff));
                wall.Effect.SetValue("time",ElapsedTime*3000);
                //wall.Effect.SetValue("lightPosition", new Vector4(500,500,500,1));
                //wall.Effect.SetValue("lightIntensity", 3000);
                //wall.Effect.SetValue("lightAttenuation", 50);

                //Cargar variables de shader de Material. El Material en realidad deberia ser propio de cada mesh. Pero en este ejemplo se simplifica con uno comun para todos
                //wall.Effect.SetValue("materialEmissiveColor", ColorValue.FromColor((Color.PeachPuff)));
                //wall.Effect.SetValue("materialAmbientColor", ColorValue.FromColor((Color.White)));
                //wall.Effect.SetValue("materialDiffuseColor", ColorValue.FromColor((Color.White)));
                //wall.Effect.SetValue("materialSpecularColor", ColorValue.FromColor((Color.White)));
                //wall.Effect.SetValue("materialSpecularExp", 299.9f);
            }

            //renderizo y animo el personaje
            character.animateAndRender(ElapsedTime);

            //Renderizo el camino
            foreach (var path in FullLevel)
            {
                path.render();
            }
            
            //terreno.Effect = Shader;
            //terreno.Technique = "BOX_DIFFUSE_MAP";
            //terreno.Effect.SetValue("color", ColorValue.FromColor(Color.PeachPuff));
            //terreno.Effect.SetValue("time", ElapsedTime);
            terreno.render();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            character.dispose();
            skyBox.dispose();
            BoxClass.dispose();
            foreach(var path in FullLevel)
            {
                path.dispose();
            }
        }
    }
}
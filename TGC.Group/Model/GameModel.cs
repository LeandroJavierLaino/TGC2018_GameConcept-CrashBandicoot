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
using TGC.Core.Textures;
using TGC.Core.Utils;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
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
        
        //Player
        private TgcSkeletalMesh character;
        private TgcBoundingSphere characterSphere;

        private List<TgcPlane> FullCouse = new List<TgcPlane>();

        private TgcBox Box;
        private float OriginalPosYBox;

        private float velocity = 0.2f;
        private float rotationVelocity = 10;

        private float acumTime;
        private float dir;

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
            character.Position = new Vector3(0, 0, 0);
            character.UpdateMeshTransform();
            //BoundingSphere que va a usar el personaje
            character.AutoUpdateBoundingBox = false;
            characterSphere = new TgcBoundingSphere(character.BoundingBox.calculateBoxCenter(), character.BoundingBox.calculateBoxRadius());

            //Suelen utilizarse objetos que manejan el comportamiento de la camara.
            //Lo que en realidad necesitamos gráficamente es una matriz de View.
            //El framework maneja una cámara estática, pero debe ser inicializada.
            //Posición de la camara.
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
            Box = new TgcBox();
            Box.setTexture(TgcTexture.createTexture(boxTexture));
            Box.Size = new Vector3(3, 3, 3);
            Box.AutoTransformEnable = true;
            OriginalPosYBox = 5;
            Box.Position = new Vector3(15, OriginalPosYBox, 15);
            Box.updateValues();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            Box.Enabled = true;

            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            //Calcular proxima posicion de personaje segun Input
            var moveForward = 0f;
            float rotate = 0;
            var moving = false;
            var rotating = false;
            bool jumping = false;
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
            if (Input.keyPressed(Key.Space) && jump == 0 && !jumping)
            {
                jump = 30;
                moving = true;
                jumping = true;
            }

            if (character.Position.Y > 0)
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

            if (moving || rotating) 
            {
                character.playAnimation("Caminando", true);
                movementVector = new Vector3(FastMath.Sin(character.Rotation.Y) * moveForward * 0.1f,jump,FastMath.Cos(character.Rotation.Y) * moveForward * 0.1f);
            }
            else
            {

                character.playAnimation("Parado", true);

            }

            if (FastMath.Floor(character.Position.Y) == 0) jumping = false;

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

            //Dibuja un texto por pantalla
            DrawText.drawText("Con la tecla F se dibuja el bounding box.", 0, 20, Color.OrangeRed);
            DrawText.drawText(
                "Con clic izquierdo subimos la camara [Actual]: " + TgcParserUtils.printVector3(Camara.Position), 0, 30,
                Color.OrangeRed);

            Path.render();

            //movimiento de 1 caja
            acumTime += ElapsedTime;
            var speed = 20 * ElapsedTime;
            if (acumTime > 5f)
            {
                acumTime = 0;
                dir *= -1;
            }

            //Box.rotateY( - ElapsedTime/2);
            //Box.updateValues();

            Box.Position = new Vector3(Box.Position.X, OriginalPosYBox + dir * speed, Box.Position.Z);
            Box.updateValues();

            Box.render();
            

            character.animateAndRender(ElapsedTime);

            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            //Mesh.UpdateMeshTransform();
            //Render del mesh
            //Mesh.render();

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
            Path.dispose();
        }
    }
}
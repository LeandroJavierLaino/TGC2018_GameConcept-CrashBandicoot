using BulletSharp;
using BulletSharp.Math;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BulletPhysics;
using TGC.Core.Input;
using TGC.Core.Mathematica;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class PhysicModel
    {
        //Configuracion de la Simulacion Fisica
        private DiscreteDynamicsWorld dynamicsWorld;

        private CollisionDispatcher dispatcher;
        private DefaultCollisionConfiguration collisionConfiguration;
        private SequentialImpulseConstraintSolver constraintSolver;
        private BroadphaseInterface overlappingPairCache;

        //Capsula del Personaje
        private RigidBody capsuleRigidBody;
        private TGCVector3 director;

        //Meshes del escenario
        private List<TgcMesh> meshes = new List<TgcMesh>();

        void Init()
        {
            #region Configuracion Basica de World

            //Creamos el mundo fisico por defecto.
            collisionConfiguration = new DefaultCollisionConfiguration();
            dispatcher = new CollisionDispatcher(collisionConfiguration);
            GImpactCollisionAlgorithm.RegisterAlgorithm(dispatcher);
            constraintSolver = new SequentialImpulseConstraintSolver();
            overlappingPairCache = new DbvtBroadphase();
            TGCVector3 gravity = new TGCVector3(0, -100f, 0);
            dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, overlappingPairCache, constraintSolver, collisionConfiguration)
            {
                Gravity = gravity.ToBsVector
            };

            #endregion Configuracion Basica de World

            #region Capsula

            //Cuerpo rigido de una capsula basica
            capsuleRigidBody = BulletRigidBodyConstructor.CreateCapsule(10, 50, new TGCVector3(200, 500, 200), 10, false);

            //Valores que podemos modificar a partir del RigidBody base
            capsuleRigidBody.SetDamping(0.1f, 0f);
            capsuleRigidBody.Restitution = 0.1f;
            capsuleRigidBody.Friction = 1;

            //Agregamos el RidigBody al World
            dynamicsWorld.AddRigidBody(capsuleRigidBody);

            director = new TGCVector3(0, 0, 1);
            #endregion Capsula
        }

        void Update(TgcD3dInput input, TGCMatrix matrixPersonaje)
        {
            dynamicsWorld.StepSimulation(1 / 60f, 100);
            var strength = 10.30f;
            var angle = 5;

            #region Comoportamiento

            if (input.keyDown(Key.W))
            {
                //Activa el comportamiento de la simulacion fisica para la capsula
                capsuleRigidBody.ActivationState = ActivationState.ActiveTag;
                capsuleRigidBody.AngularVelocity = TGCVector3.Empty.ToBsVector;
                capsuleRigidBody.ApplyCentralImpulse(-strength * director.ToBsVector);
            }

            if (input.keyDown(Key.S))
            {
                //Activa el comportamiento de la simulacion fisica para la capsula
                capsuleRigidBody.ActivationState = ActivationState.ActiveTag;
                capsuleRigidBody.AngularVelocity = TGCVector3.Empty.ToBsVector;
                capsuleRigidBody.ApplyCentralImpulse(strength * director.ToBsVector);
            }

            if (input.keyDown(Key.A))
            {
                director.TransformCoordinate(TGCMatrix.RotationY(-angle * 0.01f));
                matrixPersonaje = TGCMatrix.Translation(TGCVector3.Empty) * TGCMatrix.RotationY(-angle * 0.01f) * new TGCMatrix(capsuleRigidBody.InterpolationWorldTransform);
                capsuleRigidBody.WorldTransform = matrixPersonaje.ToBsMatrix;
            }

            if (input.keyDown(Key.D))
            {
                director.TransformCoordinate(TGCMatrix.RotationY(angle * 0.01f));
                matrixPersonaje = TGCMatrix.Translation(TGCVector3.Empty) * TGCMatrix.RotationY(angle * 0.01f) * new TGCMatrix(capsuleRigidBody.InterpolationWorldTransform);
                capsuleRigidBody.WorldTransform = matrixPersonaje.ToBsMatrix;
            }

            if (input.keyPressed(Key.Space))
            {
                //Activa el comportamiento de la simulacion fisica para la capsula
                capsuleRigidBody.ActivationState = ActivationState.ActiveTag;
                capsuleRigidBody.ApplyCentralImpulse(new TGCVector3(0, 80 * strength, 0).ToBsVector);
            }

            #endregion Comoportamiento
        }

        Matrix GetWorldInterpolationTransform()
        {
            return capsuleRigidBody.InterpolationWorldTransform;
        }

        void Dispose()
        {
            //Se hace dispose del modelo fisico.
            dynamicsWorld.Dispose();
            dispatcher.Dispose();
            collisionConfiguration.Dispose();
            constraintSolver.Dispose();
            overlappingPairCache.Dispose();
            capsuleRigidBody.Dispose();
        }
    }
}

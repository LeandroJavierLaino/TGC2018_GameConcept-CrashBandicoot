﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

namespace TGC.Group.Model.Parcelas
{
    public class Horizontal : Parcela
    {

        public Horizontal(Vector3 position, string grassTexture, string plantModel)
        {
            //Se define el terrno de la parcela
            floor = new TgcPlane(position, new Vector3(50, 0, 50), TgcPlane.Orientations.XZplane, TgcTexture.createTexture(grassTexture), 4, 4);

            //Variable temporal que contiene el modelo
            var basePlant = new TgcSceneLoader().loadSceneFromFile(plantModel).Meshes[0];
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z);
            basePlant.Scale = new Vector3(0.5f, 0.5f, 0.5f);
            var random = new Random();
            var ran = random.Next(0, 10);
            basePlant.rotateY(ran);
            basePlant.Enabled = true;

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo1");
            basePlant.Position = new Vector3(position.X + 35, position.Y, position.Z);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo2");
            basePlant.Position = new Vector3(position.X + 25, position.Y, position.Z);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo3");
            basePlant.Position = new Vector3(position.X + 15, position.Y, position.Z);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaabajo4");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba1");
            basePlant.Position = new Vector3(position.X + 45, position.Y, position.Z + 50);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba2");
            basePlant.Position = new Vector3(position.X + 35, position.Y, position.Z + 50);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba3");
            basePlant.Position = new Vector3(position.X + 25, position.Y, position.Z + 50);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba4");
            basePlant.Position = new Vector3(position.X + 15, position.Y, position.Z + 50);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);

            basePlant = basePlant.clone("plantaarriba5");
            basePlant.Position = new Vector3(position.X + 5, position.Y, position.Z + 50);
            ran = random.Next(0, 10);
            basePlant.rotateY(ran);

            plants.Add(basePlant);
        }
    }
}
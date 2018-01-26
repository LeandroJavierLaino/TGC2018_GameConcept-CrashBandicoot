using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class Parcela
    {
        //Piso por el momento es simplemente un plano
        protected TgcPlane floor;
        
        //Algo de vegetacion
        protected List<TgcMesh> plants = new List<TgcMesh>();

        //Paredes
        public List<TgcMesh> walls = new List<TgcMesh>();

        //Columnas
        protected List<TgcMesh> columns = new List<TgcMesh>();

        //El tope de las columnas
        protected List<TgcMesh> columnsTops = new List<TgcMesh>();

        //TODO: Agregar List con todos los meshes facilitaria la aplicacion de shaders

        //Posicion a partir de la cual se ubica todo
        protected Vector3 position;

        public void render()
        {
            floor.render();

            foreach(var plant in plants)
            {
                plant.render();
            }

            foreach(var wall in walls)
            {
                wall.render();
            }

            foreach (var column in columns)
            {
                column.render();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.render();
            }
        }

        public void dispose()
        {
            floor.dispose();

            foreach (var plant in plants)
            {
                plant.dispose();
            }

            foreach (var column in columns)
            {
                column.dispose();
            }

            foreach (var columnTop in columnsTops)
            {
                columnTop.dispose();
            }
        }
    }
}

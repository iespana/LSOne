using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class DiagramHost : UserControl
    {
        private List<Shape> shapes = new List<Shape>();
        private List<Connector> connectors = new List<Connector>();

        public DiagramHost()
        {
            InitializeComponent();
        }

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
            Controls.Add(shape);
            AddShapeEventHandlers(shape);
            //Invalidate();
        }

        private void AddShapeEventHandlers(Shape shape)
        {
            shape.Move += new EventHandler(shape_Move);
            shape.Selected += new EventHandler<EventArgs>(shape_Selected);
        }

        private void RemoveShapeEventHandlers(Shape shape)
        {
            shape.Move -= new EventHandler(shape_Move);
            shape.Selected -= new EventHandler<EventArgs>(shape_Selected);
        }

        private void shape_Move(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void shape_Selected(object sender, EventArgs e)
        {
            DeselectAllButOne((Shape)sender);
        }



        public void AddConnector(Shape shape1, Shape shape2)
        {
            Connector connector = new Connector();
            connector.End1 = shape1;
            connector.End2 = shape2;
            connectors.Add(connector);
        }

        private void DiagramHost_MouseClick(object sender, MouseEventArgs e)
        {
            DeselectAllButOne(null);
        }

        private void DeselectAllButOne(Shape excludeShape)
        {
            foreach (Shape shape in shapes)
            {
                if (shape != excludeShape)
                {
                    shape.ShapeState = ShapeState.Normal;
                }
            }
        }
        private void DiagramHost_Paint(object sender, PaintEventArgs e)
        {
            foreach (Connector connector in connectors)
            {
                connector.Draw(e);
            }

        }

        //private void DiagramHost_Paint(object sender, PaintEventArgs e)
        //{
        //    foreach (Shape shape in shapes)
        //    {
        //        shape.Draw(e.Graphics, ShapeState.Normal);
        //    }
        //}




        public void Clear()
        {
            foreach (Shape shape in shapes)
            {
                this.Controls.Remove(shape);
                RemoveShapeEventHandlers(shape);
            }
            shapes.Clear();
            connectors.Clear();
            Invalidate();
        }


        public IQueryable<Shape> Shapes
        {
            get { return this.shapes.AsQueryable(); }
        }
    }
}

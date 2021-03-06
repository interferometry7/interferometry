﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

namespace Interferometry.forms
{
    public partial class VisualisationWindow : Window
    {
        public VisualisationWindow(Interferometry.math_classes.ZArrayDescriptor array)
        {
            InitializeComponent();

            // prepare points
            Point3D[,] points = new Point3D[array.width, array.height];
            for (int i = 0; i < array.width; ++i)
                for (int j = 0; j < array.height; ++j)
                    points[i, j] = new Point3D(i, array.array[i][j], j);

            // build model
            Model3DGroup surface = new Model3DGroup();
            for (int i = 0; i < array.width - 1; ++i)
            {
                for (int j = 0; j < array.height - 1; ++j)
                {
                    surface.Children.Add(createTriangle(points[i, j], points[i + 1, j], points[i, j + 1]));
                    surface.Children.Add(createTriangle(points[i, j + 1], points[i + 1, j], points[i, j]));

                    surface.Children.Add(createTriangle(points[i, j + 1], points[i + 1, j], points[i + 1, j + 1]));
                    surface.Children.Add(createTriangle(points[i + 1, j + 1], points[i + 1, j], points[i, j + 1]));
                }
                System.Console.WriteLine("buidling on row: " + i + "/" + array.width);
            }
            System.Console.WriteLine("model building done");

            ModelVisual3D model = new ModelVisual3D();
            model.Content = surface;
            this.mainViewport.Children.Add(model);
        }

        private double lastX, lastY;
        private double length = 1;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            double old_length = length;
            length += length * 0.005 * e.Delta;
            if (length < 0 || length > 1e5)
                length = old_length;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point p = e.GetPosition(this);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera cam = (PerspectiveCamera)this.mainViewport.Camera;
                double rotY = (lastX - p.X) / this.Width * 3.14;
                double rotR = (lastY - p.Y) / this.Height * 3.14;

                double x = cam.LookDirection.X * Math.Cos(rotY) + cam.LookDirection.Z * Math.Sin(rotY);
                double y = cam.LookDirection.Y;
                double z = -cam.LookDirection.X * Math.Sin(rotY) + cam.LookDirection.Z * Math.Cos(rotY);
                cam.LookDirection = new Vector3D(x, y, z);

                /*
                Vector3D axl = Vector3D.CrossProduct(cam.LookDirection, new Vector3D(0, 1, 0));
                axl.Normalize();
                double cosa = Math.Cos(rotR);
                double sina = Math.Sin(rotR);
                double cos1 = 1 - Math.Cos(rotR);
                Matrix3D customRot = new Matrix3D();
                customRot.M11 = axl.X * axl.X + cosa;
                customRot.M12 = axl.X * axl.Y * cos1 - axl.Z * sina;
                customRot.M13 = axl.X * axl.Z * cos1 + axl.Y * sina;
                customRot.M21 = axl.X * axl.Y * cos1 + axl.Z * sina;
                customRot.M22 = axl.Y * axl.Y * cos1 + cosa;
                customRot.M23 = axl.Y * axl.Z * cos1 - axl.X * sina;
                customRot.M31 = axl.X * axl.Z * cos1 - axl.Y * sina;
                customRot.M32 = axl.X * axl.Z * cos1 + axl.X * sina;
                customRot.M33 = axl.Z * axl.Z * cos1 + cosa;
                cam.LookDirection = Vector3D.Multiply(cam.LookDirection, customRot);
                */
            }

            lastX = p.X;
            lastY = p.Y;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            PerspectiveCamera cam = (PerspectiveCamera)this.mainViewport.Camera;

            Vector3D posVect, normal;
            Vector3D oy = new Vector3D(0, 1, 0);

            switch (e.Key)
            {
                case Key.W:
                    posVect = Vector3D.Multiply(length / cam.LookDirection.Length, cam.LookDirection);
                    cam.Position = Vector3D.Add(posVect, cam.Position);
                    break;

                case Key.S:
                    posVect = Vector3D.Multiply(-length / cam.LookDirection.Length, cam.LookDirection);
                    cam.Position = Vector3D.Add(posVect, cam.Position);
                    break;

                case Key.A:
                    if (Vector3D.AngleBetween(cam.LookDirection, oy) < 1.57)
                        normal = Vector3D.CrossProduct(cam.LookDirection, oy);
                    else
                        normal = Vector3D.CrossProduct(oy, cam.LookDirection);
                    normal = Vector3D.Multiply(length / normal.Length, normal);
                    cam.Position = Vector3D.Add(normal, cam.Position);
                    break;

                case Key.D:
                    if (Vector3D.AngleBetween(cam.LookDirection, oy) < 1.57)
                        normal = Vector3D.CrossProduct(oy, cam.LookDirection);
                    else
                        normal = Vector3D.CrossProduct(cam.LookDirection, oy);
                    normal = Vector3D.Multiply(length / normal.Length, normal);
                    cam.Position = Vector3D.Add(normal, cam.Position);
                    break;

                case Key.Space:
                    if (Vector3D.AngleBetween(cam.LookDirection, oy) < 1.57)
                        normal = Vector3D.CrossProduct(cam.LookDirection, oy);
                    else
                        normal = Vector3D.CrossProduct(oy, cam.LookDirection);
                    normal = Vector3D.CrossProduct(cam.LookDirection, normal);
                    normal = Vector3D.Multiply(length / normal.Length, normal);
                    cam.Position = Vector3D.Add(normal, cam.Position);
                    break;

                case Key.C:
                    if (Vector3D.AngleBetween(cam.LookDirection, oy) < 1.57)
                        normal = Vector3D.CrossProduct(oy, cam.LookDirection);
                    else
                        normal = Vector3D.CrossProduct(cam.LookDirection, oy);
                    normal = Vector3D.CrossProduct(cam.LookDirection, normal);
                    normal = Vector3D.Multiply(length / normal.Length, normal);
                    cam.Position = Vector3D.Add(normal, cam.Position);
                    break;
            }
        }

        private GeometryModel3D createTriangle(Point3D a, Point3D b, Point3D c)
        {
            // add points to mesh
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(a);
            mesh.Positions.Add(b);
            mesh.Positions.Add(c);

            // define triangle
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // calc normal
            Vector3D normal = Vector3D.CrossProduct(new Vector3D(b.X - a.X, b.Y - a.Y, b.Z - a.Z),
                                                    new Vector3D(c.X - a.X, c.Y - a.Y, c.Z - a.Z));
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);

            // calc color
            byte r = invertIfNegative(normal.X / normal.Length * 255);
            byte g = invertIfNegative(normal.Y / normal.Length * 255);
            byte bl = invertIfNegative(normal.Z / normal.Length * 255);
            Material material = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(r, g, bl)));

            return new GeometryModel3D(mesh, material);
        }

        private byte invertIfNegative(double value)
        {
            if (value < 0)
                return (byte)(255 - value); // tuneable
            return (byte)value;
        }

    }
}

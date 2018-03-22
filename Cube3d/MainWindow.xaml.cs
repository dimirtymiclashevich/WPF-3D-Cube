using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Cube3d
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateScene();
        }

        /// <summary>
        /// Creates a scene and start animation
        /// </summary>
        private void CreateScene()
        {
            // create cube
            GeometryModel3D cube = new GeometryModel3D();
            MeshGeometry3D cubeMesh = GetCube();
            cube.Geometry = cubeMesh;
            cube.Material = new DiffuseMaterial(new SolidColorBrush(Colors.GreenYellow));

            // create directional light
            DirectionalLight light = new DirectionalLight();
            light.Color = Colors.White;
            light.Direction = new Vector3D(-1, -1, -1);

            // create camera
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.FarPlaneDistance = 20;
            camera.NearPlaneDistance = 1;
            camera.FieldOfView = 45;
            camera.Position = new Point3D(2, 2, 3);
            camera.LookDirection = new Vector3D(-2, -2, -3);
            camera.UpDirection = new Vector3D(0, 1, 0);

            // collect cube and light in one model
            Model3DGroup modelGroup = new Model3DGroup();
            modelGroup.Children.Add(cube);
            modelGroup.Children.Add(light);
            ModelVisual3D modelsVisual = new ModelVisual3D();
            modelsVisual.Content = modelGroup;

            // create scene
            Viewport3D viewport = new Viewport3D();
            viewport.Camera = camera;
            viewport.Children.Add(modelsVisual);
            canvas.Children.Add(viewport);
            viewport.Height = 600;
            viewport.Width = 600;
            Canvas.SetTop(viewport, 0);
            Canvas.SetLeft(viewport, 0);
            this.Width = viewport.Width;
            this.Height = viewport.Height;

            // create and start animation
            AxisAngleRotation3D axis = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
            RotateTransform3D rotate = new RotateTransform3D(axis);
            cube.Transform = rotate;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360;
            animation.Duration = new Duration(TimeSpan.FromSeconds(20.0));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            NameScope.SetNameScope(canvas, new NameScope());
            canvas.RegisterName("cubeaxis", axis);
            Storyboard.SetTargetName(animation, "cubeaxis");
            Storyboard.SetTargetProperty(animation, new PropertyPath(AxisAngleRotation3D.AngleProperty));
            Storyboard RotCube = new Storyboard();
            RotCube.Children.Add(animation);
            RotCube.Begin(canvas);
        }

        /// <summary>
        /// Create and returns a cubic mesh
        /// </summary>
        MeshGeometry3D GetCube()
        {
            var cube = new MeshGeometry3D();
            var corners = new Point3DCollection
            {
                new Point3D(0.5, 0.5, 0.5),
                new Point3D(-0.5, 0.5, 0.5),
                new Point3D(-0.5, -0.5, 0.5),
                new Point3D(0.5, -0.5, 0.5),
                new Point3D(0.5, 0.5, -0.5),
                new Point3D(-0.5, 0.5, -0.5),
                new Point3D(-0.5, -0.5, -0.5),
                new Point3D(0.5, -0.5, -0.5)
            };
            cube.Positions = corners;

            Int32[] indices ={
                //Front
                0,1,2,
                0,2,3,
                //Back
                4,7,6,
                4,6,5,
                //Right
                4,0,3,
                4,3,7,
                //Left
                1,5,6,
                1,6,2,
                //Top
                1,0,4,
                1,4,5,
                //Bottom
                2,6,7,
                2,7,3 };

            Int32Collection Triangles = new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }
            cube.TriangleIndices = Triangles;
            return cube;
        }
    }
}

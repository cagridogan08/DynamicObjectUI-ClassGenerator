using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragAndDropWithDependency.Controls
{
    /// <summary>
    /// Interaction logic for Canvas1.xaml
    /// </summary>
    public partial class Canvas1 : UserControl
    {


        public bool IsChildTestVisible
        {
            get { return (bool)GetValue(IsChildTestVisibleProperty); }
            set { SetValue(IsChildTestVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChildTestVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildTestVisibleProperty =
            DependencyProperty.Register("IsChildTestVisible", typeof(bool), typeof(Canvas1), new PropertyMetadata(true));


        public Canvas1()
        {
            InitializeComponent();
        }
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildTestVisible = false;
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable,(Rectangle)sender), DragDropEffects.Move);
                IsChildTestVisible = true;
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if(data is UIElement element)
            {
                Point dropPosition = e.GetPosition(mainCanvas);
                Canvas.SetLeft(element, dropPosition.X);
                Canvas.SetTop(element, dropPosition.Y);
                if (!mainCanvas.Children.Contains(element))
                {
                    mainCanvas.Children.Add(element);
                    //element.MouseMove += new(Rectangle_MouseMove);
                }
                
            }
        }

        private void mainCanvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(mainCanvas);
            Canvas.SetLeft(redRectangle, dropPosition.X);
            Canvas.SetTop(redRectangle, dropPosition.Y);
        }
    }
}

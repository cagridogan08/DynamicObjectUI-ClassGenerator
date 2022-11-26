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
    /// Interaction logic for Canvas2.xaml
    /// </summary>
    public partial class Canvas2 : UserControl
    {
        public Canvas2()
        {
            InitializeComponent();
        }
        public bool IsChildTestVisible
        {
            get { return (bool)GetValue(IsChildTestVisibleProperty); }
            set { SetValue(IsChildTestVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChildTestVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsChildTestVisibleProperty =
            DependencyProperty.Register("IsChildTestVisible", typeof(bool), typeof(Canvas2), new PropertyMetadata(true));
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsChildTestVisible = false;
                DragDrop.DoDragDrop(blueRectangle, new DataObject(DataFormats.Serializable, blueRectangle), DragDropEffects.Move);
                IsChildTestVisible = true;
            }
        }

        private void mainCanvas_DragLeave(object sender, DragEventArgs e)
        {
            if(e.OriginalSource == mainCanvas)
            {
                object data = e.Data.GetData(DataFormats.Serializable);
                if (data is UIElement element)
                {
                    mainCanvas.Children.Remove(element);
                }
            }
            
        }

        private void mainCanvas_DragOver(object sender, DragEventArgs e)
        {

        }
        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
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

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(mainCanvas);
            Canvas.SetLeft(blueRectangle, dropPosition.X);
            Canvas.SetTop(blueRectangle, dropPosition.Y);
        }
    }
}

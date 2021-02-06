using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Snake2
{
    class SnakePart 
    {
        public Rectangle part;
        public Point partPosition;
        public SnakePart(Point place)
        {
            this.partPosition = place;
            part = new Rectangle();
            part.Height = 10;
            part.Width = 10;
            part.Stroke = System.Windows.Media.Brushes.Black;
            part.Fill = System.Windows.Media.Brushes.SkyBlue;
        }
    }
}

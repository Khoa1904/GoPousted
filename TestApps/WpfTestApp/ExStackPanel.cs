using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfTestApp
{
    public class ExStackPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            Size childSize = new Size(availableSize.Width, double.PositiveInfinity);

            foreach (UIElement elem in InternalChildren)
                elem.Measure(childSize);

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size childSize;

            double top = 0.0;
            for (int i = 0; i < InternalChildren.Count; i++)
            {
                childSize = new Size(finalSize.Width, InternalChildren[i].DesiredSize.Height);
                Rect r = new Rect(new Point(0.0, top), childSize);
                InternalChildren[i].Arrange(r);
                top += childSize.Height * 0.5;
            }

            return finalSize;
        }

        private bool _locked = false;
        public bool Locked
        {
            get
            {
                return _locked;
            }
            set
            {
                _locked = value;
                this.Children[0].Visibility = Locked ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}

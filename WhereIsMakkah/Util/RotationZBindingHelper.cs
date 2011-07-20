using System;
using System.Windows;
using System.Windows.Media;

namespace WhereIsMakkah.Util
{
    public class RotationZBindingHelper
    {
        #region RotationZ (Attached DependencyProperty)

        public static readonly DependencyProperty RotationZProperty =
                DependencyProperty.RegisterAttached("RotationZ",
                typeof(double),
                typeof(RotationZBindingHelper),
                new PropertyMetadata(0.0, new PropertyChangedCallback(OnRotationZChanged)));

        public static void SetRotationZ(DependencyObject o, double value)
        {
            o.SetValue(RotationZProperty, value);
        }

        public static double GetRotationZ(DependencyObject o)
        {
            return (double)o.GetValue(RotationZProperty);
        }

        private static void OnRotationZChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if (element != null)
            {
                PlaneProjection projection = element.Projection as PlaneProjection;
                if (projection != null)
                {
                    projection.RotationZ = (double)e.NewValue;
                }
            }
        }

        #endregion
    }
}

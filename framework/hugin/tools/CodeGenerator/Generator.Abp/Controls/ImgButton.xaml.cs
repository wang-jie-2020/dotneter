using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AbpGenerator.Controls
{
    public partial class ImgButton : Button
    {
        static ImgButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImgButton), new FrameworkPropertyMetadata(typeof(ImgButton)));
        }

        public ImageSource Image
        {
            get => GetValue(ImageProperty) as ImageSource;
            set => SetValue(ImageProperty, value);
        }

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public ImgButton()
        {
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImgButton), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImgButton), new PropertyMetadata(double.NaN));
    }
}

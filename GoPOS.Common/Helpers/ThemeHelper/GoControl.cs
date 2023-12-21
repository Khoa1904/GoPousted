using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GoPOS.Common.Views;

public class GoControl : Control { }

public class GoRadioButton : RadioButton { }

public class GoDatePicker : DatePicker { }

public class GoButton : Button
{
    public static readonly DependencyProperty P1 = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(GoButton),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty P2 = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(GoButton),
        new FrameworkPropertyMetadata(new CornerRadius(0, 0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty P3 = DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(GoButton),
        new FrameworkPropertyMetadata((double)20, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty P4 = DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(GoButton),
        new FrameworkPropertyMetadata((double)20, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(nameof(Selected), typeof(bool), typeof(GoButton),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Icon
    {
        get => (string)GetValue(P1);
        set => SetValue(P1, value);
    }

    public GoButton()
    {
        this.Focusable = false;
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(P2);
        set => SetValue(P2, value);
    }

    public double IconHeight
    {
        get => (double)GetValue(P3);
        set => SetValue(P3, value);
    }

    public double IconWidth
    {
        get => (double)GetValue(P4);
        set => SetValue(P4, value);
    }

    public bool Selected
    {
        get => (bool)GetValue(SelectedProperty);
        set
        {
            SetValue(SelectedProperty, value);

            if (value)
            {
                var selBG = new BitmapImage(new Uri(@"pack://application:,,,/GoPOS.Resources;component/resource/images/btn_main_right_on.png", UriKind.RelativeOrAbsolute));
                this.Background = new ImageBrush(selBG);
                ((TextBlock)this.Content).Foreground = new SolidColorBrush(Colors.White);
            }
            else
            {
                this.Background = NormalBG;
                ((TextBlock)this.Content).Foreground = NormalFG;
            }
        }
    }

    public System.Windows.Media.Brush NormalBG { get; set; }
    public System.Windows.Media.Brush NormalFG { get; set; }

}

public class GoNumberAlphaTextBox : TextBox, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);
        var ca = CaretIndex;
        Text = LeaveOnlyNumberAlpha(Text);
        CaretIndex = ca;
    }

    private static string LeaveOnlyNumberAlpha(string inString)
    {
        return inString.ToCharArray().Where(c => !Regex.IsMatch(c.ToString(), "^[0-9a-zA-Z]*$")).Aggregate(inString, (current, c) => current.Replace(c.ToString(), ""));
    }
}

public class GoNumberTextBox : TextBox
{
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);
        var ca = CaretIndex;
        Text = LeaveOnlyNumbers(Text);
        CaretIndex = ca;
    }

    private static string LeaveOnlyNumbers(string inString)
    {
        return inString.ToCharArray().Where(c => !Regex.IsMatch(c.ToString(), "^[0-9]*$")).Aggregate(inString, (current, c) => current.Replace(c.ToString(), ""));
    }
}
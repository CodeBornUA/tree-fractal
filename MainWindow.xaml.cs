using System.Runtime.InteropServices;
using System.Threading;

namespace TreeFractal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new FractalViewModel()
            {
                Canvas = canvas
            };
        }
    }
}
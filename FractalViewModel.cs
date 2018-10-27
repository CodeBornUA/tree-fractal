using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TreeFractal.Annotations;

namespace TreeFractal
{
    public class FractalViewModel : INotifyPropertyChanged
    {
        protected internal const string Axiom = "F";
        private double _angle;
        private string _pattern;

        public FractalViewModel()
        {
            Draw = new RelayCommand(o =>
            {
                Canvas.Children.Clear();
                var executor = new PatternInterpreter(Canvas, new Point(Canvas.ActualWidth / 2, Canvas.ActualHeight))
                {
                    AngleDiff = Angle,
                    MaxRecursionLevel = Iterations,
                    SegmentLength = SegmentSize
                };

                executor.Draw(Axiom, new LPattern()
                {
                    From = _pattern.Split(new[] { "->" }, StringSplitOptions.None).First().Single(),
                    To = _pattern.Split(new[] { "->" }, StringSplitOptions.None).Last()
                });
            });
            Set22 = new RelayCommand(o =>
            {
                Pattern = "F->F[+FF][-FF]F[-F][+F]F";
                Angle = 35;
            });
            Set24 = new RelayCommand(o =>
            {
                Pattern = "F->FF+[+F-F-F]-[-F+F+F]";
                Angle = 22.5;
            });

            Set22.Execute(null);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double Angle
        {
            get => _angle;
            set
            {
                if (value.Equals(_angle)) return;
                _angle = value;
                OnPropertyChanged();
            }
        }

        public string Pattern
        {
            get => _pattern;
            set
            {
                if (value == _pattern) return;
                _pattern = value;
                OnPropertyChanged();
            }
        }

        public int Iterations { get; set; } = 4;

        public int SegmentSize { get; set; } = 5;

        public Canvas Canvas { private get; set; }

        public RelayCommand Draw { get; set; }
        public RelayCommand Set22 { get; set; }
        public RelayCommand Set24 { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}

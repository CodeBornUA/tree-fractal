using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreeFractal
{
    public class PatternInterpreter
    {
        private Random _random = new Random();
        private readonly Canvas _canvas;
        private TurtleContext _context;
        private readonly Stack<TurtleContext> _stack = new Stack<TurtleContext>();
        private int _currentRecursionLevel;

        public int MaxRecursionLevel { get; set; }

        public double SegmentLength { get; set; }
        public double AngleDiff { get; set; }

        public PatternInterpreter(Canvas canvas, Point initialPosition)
        {
            _canvas = canvas;
            _context = new TurtleContext()
            {
                X = initialPosition.X,
                Y = initialPosition.Y,
                Angle = 90f
            };
        }

        public void Draw(string axiom, LPattern pattern)
        {
            foreach (var c in axiom)
            {
                Draw(c, pattern);
            }
        }

        private void Draw(char command, LPattern pattern)
        {
            if (pattern.From == command && _currentRecursionLevel < MaxRecursionLevel)
            {
                _currentRecursionLevel++;
                foreach (var c in pattern.To)
                {
                    Draw(c, pattern);
                }
                _currentRecursionLevel--;
            }
            else
            {
                switch (command)
                {
                    case 'F':
                        Forward();
                        break;
                    case '-':
                        TurnLeft();
                        break;
                    case '+':
                        TurnRight();
                        break;
                    case '[':
                        SaveContext();
                        break;
                    case ']':
                        LoadContext();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

        }

        private void LoadContext()
        {
            _context = _stack.Pop();
        }

        private void SaveContext()
        {
            _stack.Push(_context);
        }

        private void TurnLeft()
        {
            _context.Angle += AngleDiff;
        }

        private void TurnRight()
        {
            _context.Angle -= AngleDiff;
        }

        private void Forward()
        {
            _canvas.Children.Add(new Line()
            {
                X1 = _context.X,
                Y1 = _context.Y,
                X2 = _context.X += Math.Cos(Math.PI / 180 * Rnd(_context.Angle, 10.0)) * Rnd(SegmentLength, 0.1, false),
                Y2 = _context.Y -= Math.Sin(Math.PI / 180 * Rnd(_context.Angle, 10.0)) * Rnd(SegmentLength, 0.1, false),
                Stroke = Brushes.Black
            });
        }

        private double Rnd(double input, double range, bool twoDir = true)
        {
            return input + range * (_random.NextDouble() * (twoDir ? 2 : 1) - 1);
        }
    }

    public class LPattern
    {
        public char From { get; set; }
        public string To { get; set; }
    }

    internal struct TurtleContext
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }
    }
}

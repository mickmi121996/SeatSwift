﻿using System;
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
using System.Windows.Shapes;
using AppGestion.ViewModels.Windows;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace AppGestion.Views.Windows
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            InitializeMap();
            AddMarker();
            VMAbout vMAbout = new VMAbout();
            this.DataContext = vMAbout;
        }


        #region Map

        #region Code for the map
        private void InitializeMap()
        {
            MyMap.MapProvider = GMapProviders.GoogleMap;
            MyMap.MinZoom = 15;
            MyMap.MaxZoom = 15;
            MyMap.Zoom = 15;
            MyMap.Cursor = null;
            MyMap.ForceCursor = false;
            MyMap.Position = new PointLatLng(48.0435, -70.0643);
        }

        private void AddMarker()
        {
            // Coordonnées du marqueur
            PointLatLng point = new PointLatLng(48.0435, -70.0681);

            // Création du marqueur
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = CreateMarkerWithText("1910 Route 170, Sagard"),
                Offset = new Point(-10.5, -35),
                ZIndex = int.MaxValue
            };

            // Ajout du marqueur à la carte
            MyMap.Markers.Add(marker);
        }

        #endregion


        #region Code cancer pour le marker

        private UIElement CreateMarkerWithText(string address)
        {
            // Création de la figure du marqueur (triangle)
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(0, 0),
                IsClosed = true,
                Segments = new PathSegmentCollection
                {
                    new LineSegment(new Point(25, 0), true),
                    new LineSegment(new Point(12.5, 35), true)
                }
            };
            PathGeometry pathGeometry = new PathGeometry { Figures = { pathFigure } };

            Path path = new Path
            {
                Width = 25,
                Height = 35,
                Data = pathGeometry,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            // Création du texte pour l'adresse
            TextBlock textBlock = new TextBlock
            {
                Text = address,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                Padding = new Thickness(2)
            };

            // Création de la grille qui contiendra le texte et le marqueur
            Grid grid = new Grid { Width = 200, Height = 50, };

            // Ajout du texte et du marqueur à la grille
            grid.Children.Add(textBlock);
            grid.Children.Add(path);

            return grid;
        }

        #endregion

        #endregion


        #region Button Close | Restore | Minimize 

        /// <summary>
        /// Close the application
        /// </summary>
        private void AlertColor_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Drag the window
        /// </summary>
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion
    }
}
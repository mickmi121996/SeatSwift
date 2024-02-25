using AppGestion.ViewModels.Pages;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace AppGestion.Views.Pages
{
    /// <summary>
    /// Interaction logic for ManageTheater.xaml
    /// </summary>
    public class Section
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string Name { get; set; }

    }

    public class Salle
    {
        public int TotalRows { get; set; }
        public int TotalColumns { get; set; }
        public List<Section> Sections { get; set; } = new List<Section>();
    }

    public class Configuration
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string[,] Salles { get; set; }
    }
    public partial class ManageTheater : Page
    {

        #region properties

        private int totalRows;

        private int totalColumns;

        private string[,] gridColors;

        private Dictionary<Canvas, Point> elementPositions = new Dictionary<Canvas, Point>();

        #endregion

        #region Constructor
        public ManageTheater()
        {
            InitializeComponent();
            totalColumns = 0;
            totalRows = 0;
            VMManageTheater vMManageTheater = new VMManageTheater();
            this.DataContext = vMManageTheater;
        }
        #endregion


        #region Event

        #region Creation

        private void CreateGrid_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxGridRows.Text, out int gridRows) && int.TryParse(textBoxGridColumns.Text, out int gridColumns) && gridRows > 0 && gridColumns > 0)
            {
                gridColors = new string[gridRows, gridColumns];
                totalRows = gridRows;
                totalColumns = gridColumns;
                DrawGrid(gridRows, gridColumns);
            }
            else
            {
                MessageBox.Show("Veuillez entrer des nombres valides pour les lignes et les colonnes de la grille.");
            }
        }

        private void CreateSection_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxRows.Text, out int rows) && int.TryParse(textBoxColumns.Text, out int columns) &&
                int.TryParse(textBoxGridRows.Text, out int gridRows) && int.TryParse(textBoxGridColumns.Text, out int gridColumns) &&
                rows > 0 && columns > 0 && gridRows > 0 && gridColumns > 0)
            {
                string name = comboBoxName.SelectedItem is ComboBoxItem cbi ? cbi.Content.ToString() : "";
                double borderWidth = 5;

                double cellWidth = (canvasSalle.ActualWidth / gridColumns) - borderWidth;
                double cellHeight = (canvasSalle.ActualHeight / gridRows) - borderWidth;

                double totalSectionWidth = columns * (cellWidth + borderWidth);
                double totalSectionHeight = rows * (cellHeight + borderWidth);

                if (totalSectionWidth > canvasSalle.ActualWidth || totalSectionHeight > canvasSalle.ActualHeight)
                {
                    MessageBox.Show("La section dépasse les dimensions de la salle.");
                    return;
                }

                Canvas sectionCanvas = new Canvas();
                double spacing = borderWidth;

                sectionCanvas.Width = columns * (cellWidth + spacing) - spacing;
                sectionCanvas.Height = rows * (cellHeight + spacing) - spacing;
                sectionCanvas.Background = Brushes.Transparent;

                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        var seat = new Rectangle
                        {
                            Width = cellWidth,
                            Height = cellHeight,
                            Fill = GetFillBrushBasedOnName(name),
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            Name = name
                        };

                        seat.MouseRightButtonDown += Seat_MouseRightButtonDown;

                        double leftPosition = column * (cellWidth + spacing);
                        double topPosition = row * (cellHeight + spacing);
                        Canvas.SetLeft(seat, leftPosition);
                        Canvas.SetTop(seat, topPosition);
                        sectionCanvas.Children.Add(seat);
                    }
                }

                Canvas.SetLeft(sectionCanvas, 0);
                Canvas.SetTop(sectionCanvas, 0);
                canvasSalle.Children.Add(sectionCanvas);

                sectionCanvas.MouseDown += SectionCanvas_MouseDown;
            }
            else
            {
                MessageBox.Show("Veuillez entrer des nombres valides pour les lignes, colonnes, et un nom.");
            }
        }

        private void CreateRow_Click(object sender, RoutedEventArgs e)
        {
            string sectionName = SectioncomboBoxRow.SelectedItem is ComboBoxItem cbi ? cbi.Content.ToString() : "";
            if (int.TryParse(textBoxSeats.Text, out int seats) && !string.IsNullOrEmpty(sectionName) && seats > 0)
            {
                double borderWidth = 5;
                double cellWidth = (canvasSalle.ActualWidth / int.Parse(textBoxGridColumns.Text)) - borderWidth;
                double cellHeight = (canvasSalle.ActualHeight / int.Parse(textBoxGridRows.Text)) - borderWidth;

                double totalRowWidth = seats * (cellWidth + borderWidth) - borderWidth;
                if (totalRowWidth > canvasSalle.ActualWidth)
                {
                    MessageBox.Show("La rangée dépasse la largeur de la salle.");
                    return;
                }

                double topPosition = canvasSalle.Children.OfType<Canvas>()
                    .Where(c => c.Tag?.ToString() == sectionName)
                    .Select(c => Canvas.GetTop(c) + c.Height + borderWidth)
                    .DefaultIfEmpty(borderWidth)
                    .Max();

                Canvas rowCanvas = new Canvas
                {
                    Width = totalRowWidth,
                    Height = cellHeight,
                    Background = Brushes.Transparent,
                    Tag = sectionName
                };

                for (int seatNumber = 0; seatNumber < seats; seatNumber++)
                {
                    var seat = new Rectangle
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Fill = GetFillBrushBasedOnName(sectionName),
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Name = sectionName
                    };

                    seat.MouseRightButtonDown += Seat_MouseRightButtonDown;
                    Canvas.SetLeft(seat, seatNumber * (cellWidth + borderWidth));
                    Canvas.SetTop(seat, 0);
                    rowCanvas.Children.Add(seat);
                }

                Canvas.SetLeft(rowCanvas, borderWidth);
                Canvas.SetTop(rowCanvas, topPosition);
                canvasSalle.Children.Add(rowCanvas);

                rowCanvas.MouseDown += SectionCanvas_MouseDown;
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nombre valide de sièges et sélectionner un nom de section.");
            }
        }

        #endregion


        #region DragAndDrop

        private void Rect_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect != null)
            {
                DragDrop.DoDragDrop(rect, rect, DragDropEffects.Move);
            }
        }

        private void CanvasSalle_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void CanvasSalle_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Canvas)) is Canvas sectionCanvas)
            {
                var position = e.GetPosition(canvasSalle);

                if (int.TryParse(textBoxGridRows.Text, out int gridRows) && int.TryParse(textBoxGridColumns.Text, out int gridColumns) && gridRows > 0 && gridColumns > 0)
                {
                    double borderWidth = 5;

                    double cellWidth = (canvasSalle.ActualWidth / gridColumns);
                    double cellHeight = (canvasSalle.ActualHeight / gridRows);

                    double alignedX = Math.Round(position.X / cellWidth) * cellWidth;
                    double alignedY = Math.Round(position.Y / cellHeight) * cellHeight;

                    alignedX = Math.Min(alignedX, canvasSalle.ActualWidth - sectionCanvas.Width - borderWidth);
                    alignedY = Math.Min(alignedY, canvasSalle.ActualHeight - sectionCanvas.Height - borderWidth);

                    Canvas.SetLeft(sectionCanvas, alignedX);
                    Canvas.SetTop(sectionCanvas, alignedY);

                    elementPositions[sectionCanvas] = new Point(alignedX, alignedY);

                    if (!canvasSalle.Children.Contains(sectionCanvas))
                    {
                        canvasSalle.Children.Add(sectionCanvas);
                    }
                }
                else
                {
                    MessageBox.Show("Les dimensions de la grille ne sont pas valides.");
                }
            }
        }

        private void SectionCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas sectionCanvas = sender as Canvas;
            if (sectionCanvas != null)
            {
                DataObject dataObj = new DataObject(typeof(Canvas), sectionCanvas);
                DragDrop.DoDragDrop(sectionCanvas, dataObj, DragDropEffects.Move);
            }
        }

        #endregion


        #region Mouse Click

        private void Seat_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var seat = sender as Rectangle;
            if (seat != null)
            {
                if (seat.Fill == Brushes.Gray)
                {
                    seat.Fill = GetFillBrushBasedOnName(seat.Name);
                }
                else
                {
                    seat.Fill = Brushes.Gray;
                }

                e.Handled = true;
            }
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            canvasSalle.Children.Clear();
        }

        #endregion

        #endregion


        #region Draw

        private void DrawGrid(int rows, int columns)
        {
            double borderWidth = 5;
            double cellWidth = (canvasSalle.ActualWidth - borderWidth) / columns;
            double cellHeight = (canvasSalle.ActualHeight - borderWidth) / rows;

            canvasSalle.Children.Clear();

            for (int col = 0; col < columns; col++)
            {
                var line = new Line
                {
                    X1 = cellWidth * (col + 1),
                    Y1 = 0,
                    X2 = cellWidth * (col + 1),
                    Y2 = canvasSalle.ActualHeight,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = borderWidth
                };
                if (col != columns - 1)
                {
                    canvasSalle.Children.Add(line);
                }
            }

            for (int row = 0; row < rows; row++)
            {
                var line = new Line
                {
                    X1 = 0,
                    Y1 = cellHeight * (row + 1),
                    X2 = canvasSalle.ActualWidth,
                    Y2 = cellHeight * (row + 1),
                    Stroke = Brushes.LightGray,
                    StrokeThickness = borderWidth
                };
                if (row != rows - 1)
                {
                    canvasSalle.Children.Add(line);
                }
            }
        }

        #endregion


        #region Gestion de couleur

        private string GetColorName(Color color)
        {
            if (color == Colors.Green) return "BALCON";
            if (color == Colors.Blue) return "PARTERRE";
            if (color == Colors.Yellow) return "LOGE";
            if (color == Colors.Purple) return "MOBILITÉ RÉDUITE";
            if (color == Colors.Gray) return "HORS SERVICE";
            return "Unknown";
        }

        private Color GetColorFromName(string colorName)
        {
            switch (colorName.ToUpper())
            {
                case "BALCON":
                    return Colors.Green;
                case "PARTERRE":
                    return Colors.Blue;
                case "LOGE":
                    return Colors.Yellow;
                case "MOBILITÉ RÉDUITE":
                    return Colors.Purple;
                case "HORS SERVICE":
                    return Colors.Gray;
                default:
                    return Colors.Transparent;
            }
        }

        private Brush GetFillBrushBasedOnName(string name)
        {
            switch (name.ToUpper())
            {
                case "BALCON":
                    return Brushes.Green;
                case "PARTERRE":
                    return Brushes.Blue;
                case "LOGE":
                    return Brushes.Yellow;
                case "MOBILITÉ RÉDUITE":
                    return Brushes.Purple;
                default:
                    return Brushes.Gray;
            }
        }

        #endregion


        #region Save config

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = new Configuration
            {
                Name = textBoxNameConfig.Text,
                Rows = totalRows,
                Columns = totalColumns,
                Salles = ConvertCanvasToGridState()
            };

            string jsonConfig = JsonConvert.SerializeObject(config, Formatting.Indented);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = $"{config.Name}.json"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, jsonConfig);
                MessageBox.Show($"Configuration sauvegardée dans : {saveFileDialog.FileName}");
            }
        }

        private string[,] ConvertCanvasToGridState()
        {
            string[,] gridColors = new string[totalRows, totalColumns];

            for (int i = 0; i < totalRows; i++)
            {
                for (int j = 0; j < totalColumns; j++)
                {
                    gridColors[i, j] = "HALL";
                }
            }

            double cellWidth = canvasSalle.ActualWidth / totalColumns;
            double cellHeight = canvasSalle.ActualHeight / totalRows;

            foreach (var kvp in elementPositions)
            {
                Canvas sectionCanvas = kvp.Key;
                Point position = kvp.Value;

                int sectionCol = (int)(position.X / cellWidth);
                int sectionRow = (int)(position.Y / cellHeight);

                foreach (UIElement item in sectionCanvas.Children)
                {
                    if (item is Rectangle seat)
                    {
                        int seatCol = sectionCol + (int)(Canvas.GetLeft(seat) / cellWidth);
                        int seatRow = sectionRow + (int)(Canvas.GetTop(seat) / cellHeight);

                        if (seat.Fill is SolidColorBrush brush)
                        {
                            string colorName = GetColorName(brush.Color);
                            gridColors[seatRow, seatCol] = colorName;
                        }
                        else
                        {
                            gridColors[seatRow, seatCol] = "Unknown";
                        }
                    }
                }
            }

            return gridColors;
        }

        #endregion

        private void LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FileName = "Configuration.json"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string jsonConfig = File.ReadAllText(openFileDialog.FileName);
                Configuration config = JsonConvert.DeserializeObject<Configuration>(jsonConfig);

                if (config != null)
                {
                    canvasSalle.Children.Clear();

                    totalRows = config.Rows;
                    totalColumns = config.Columns;

                    DrawGrid(totalRows, totalColumns);

                    ConvertGridStateToCanvas(config.Salles);
                }
            }
        }

        private void ConvertGridStateToCanvas(string[,] gridColors)
        {
            canvasSalle.Children.Clear();

            int totalRows = gridColors.GetLength(0);
            int totalColumns = gridColors.GetLength(1);

            double cellWidth = canvasSalle.ActualWidth / totalColumns;
            double cellHeight = canvasSalle.ActualHeight / totalRows;

            for (int row = 0; row < totalRows; row++)
            {
                for (int column = 0; column < totalColumns; column++)
                {
                    string colorName = gridColors[row, column];
                    if (colorName != "HALL")
                    {
                        Color color = GetColorFromName(colorName);
                        DrawSeat(row, column, color, cellWidth, cellHeight);
                    }
                }
            }
        }

        private void DrawSeat(int row, int column, Color color, double cellWidth, double cellHeight)
        {
            var seat = new Rectangle
            {
                Width = cellWidth,
                Height = cellHeight,
                Fill = new SolidColorBrush(color),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(seat, column * cellWidth);
            Canvas.SetTop(seat, row * cellHeight);
            canvasSalle.Children.Add(seat);
        }
    }
}

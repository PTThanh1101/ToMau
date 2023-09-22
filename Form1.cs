using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ToMau
{
    public partial class Form1 : Form
    {
        private Dictionary<char, Color> vertexColors = new Dictionary<char, Color>();
        private Dictionary<char, Point> vertexPositions = new Dictionary<char, Point>();

        public Form1()
        {
            InitializeComponent();
            InitializeGraph();
            Paint += Form1_Paint;
            ColorButton.Click += ColorButton_Click_1;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (char vertex in vertexPositions.Keys)
            {
                Point position = vertexPositions[vertex];
                Rectangle rect = new Rectangle(position.X, position.Y, 30, 30);
                Color color = vertexColors[vertex];
                using (Brush brush = new SolidBrush(color))
                {
                    g.FillEllipse(brush, rect);
                }
                g.DrawEllipse(Pens.Black, rect);
                g.DrawString(vertex.ToString(), new Font("Arial", 12), Brushes.Black, position.X + 10, position.Y + 10);
            }

            ConnectAllVertices(g);
        }

        private void InitializeGraph()
        {
            vertexColors['A'] = Color.Black; // Màu ban đầu là đen cho tất cả các đỉnh
            vertexColors['B'] = Color.Black;
            vertexColors['C'] = Color.Black;
            vertexColors['D'] = Color.Black;
            vertexColors['E'] = Color.Black;
            vertexColors['F'] = Color.Black;
            vertexColors['G'] = Color.Black;
            vertexColors['Z'] = Color.Black;

            // Khởi tạo vị trí các đỉnh
            vertexPositions['A'] = new Point(12, 219);
            vertexPositions['B'] = new Point(141, 101);
            vertexPositions['C'] = new Point(141, 219);
            vertexPositions['D'] = new Point(141, 337);
            vertexPositions['E'] = new Point(302, 101);
            vertexPositions['F'] = new Point(302, 219);
            vertexPositions['G'] = new Point(302, 337);
            vertexPositions['Z'] = new Point(460, 219);
        }

        private void ConnectAllVertices(Graphics g)
        {
            // Khai báo các đỉnh kề của mỗi đỉnh
            Dictionary<char, List<char>> adjacentVertices = new Dictionary<char, List<char>>
            {
                { 'A', new List<char> { 'B', 'C', 'D' } },
                { 'B', new List<char> { 'A', 'C', 'E' } },
                { 'C', new List<char> { 'A', 'B', 'D', 'E', 'F' } },
                { 'D', new List<char> { 'A', 'C', 'F', 'G' } },
                { 'E', new List<char> { 'B', 'C', 'F', 'Z' } },
                { 'F', new List<char> { 'C', 'D', 'E', 'G', 'Z' } },
                { 'G', new List<char> { 'D', 'F', 'Z' } },
                { 'Z', new List<char> { 'E', 'F', 'G' } }
            };

            // Vẽ các đoạn đường nối giữa các đỉnh
            foreach (char vertex in vertexPositions.Keys)
            {
                Point startPoint = vertexPositions[vertex];

                foreach (char adjacentVertex in adjacentVertices[vertex])
                {
                    Point endPoint = vertexPositions[adjacentVertex];
                    g.DrawLine(Pens.Black, startPoint.X + 15, startPoint.Y + 15, endPoint.X + 15, endPoint.Y + 15);
                }
            }
        }

        public Color GetAvailableColor(char vertex)
        {
            HashSet<Color> neighborColors = new HashSet<Color>();
            foreach (char neighbor in vertexColors.Keys)
            {
                if (AreVerticesConnected(vertex, neighbor))
                {
                    neighborColors.Add(vertexColors[neighbor]);
                }
            }

            List<Color> availableColors = new List<Color>
    {
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Yellow,
        Color.Orange,
        Color.Purple
    };

            // Loại bỏ các màu đã được sử dụng bởi các đỉnh kề
            foreach (Color usedColor in neighborColors)
            {
                availableColors.Remove(usedColor);
            }

            // Nếu không còn màu nào trống, trả về màu đen
            if (availableColors.Count == 0)
            {
                return Color.Black;
            }

            // Chọn màu trống đầu tiên trong danh sách
            return availableColors[0];
        }


        private bool AreVerticesConnected(char vertex1, char vertex2)
        {
            // Viết mã để kiểm tra xem hai đỉnh có nối với nhau không, dựa trên yêu cầu của đồ thị

            if ((vertex1 == 'A' && (vertex2 == 'B' || vertex2 == 'C' || vertex2 == 'D')) ||
                (vertex1 == 'B' && (vertex2 == 'A' || vertex2 == 'C' || vertex2 == 'E')) ||
                (vertex1 == 'C' && (vertex2 == 'A' || vertex2 == 'B' || vertex2 == 'D' || vertex2 == 'E' || vertex2 == 'F')) ||
                (vertex1 == 'D' && (vertex2 == 'A' || vertex2 == 'C' || vertex2 == 'F' || vertex2 == 'G')) ||
                (vertex1 == 'E' && (vertex2 == 'B' || vertex2 == 'C' || vertex2 == 'F' || vertex2 == 'Z')) ||
                (vertex1 == 'F' && (vertex2 == 'C' || vertex2 == 'D' || vertex2 == 'E' || vertex2 == 'G' || vertex2 == 'Z')) ||
                (vertex1 == 'G' && (vertex2 == 'D' || vertex2 == 'F' || vertex2 == 'Z')) ||
                (vertex1 == 'Z' && (vertex2 == 'E' || vertex2 == 'F' || vertex2 == 'G')))
            {
                return true;
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ColorButton_Click_1(object sender, EventArgs e)
        {
            List<char> verticesToUpdate = new List<char>();

            foreach (char vertex in vertexColors.Keys)
            {
                Color color = GetAvailableColor(vertex);

                // Thêm các đỉnh cần cập nhật màu vào danh sách tạm thời
                verticesToUpdate.Add(vertex);
            }

            // Duyệt qua danh sách các đỉnh và cập nhật màu sắc
            foreach (char vertex in verticesToUpdate)
            {
                Color color = GetAvailableColor(vertex);
                vertexColors[vertex] = color;
            }

            // Vẽ lại đồ thị sau khi đã tô màu
            this.Invalidate();
        }
    }
}

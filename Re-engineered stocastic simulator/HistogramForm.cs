using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Re_engineered_stocastic_simulator
{
    public partial class HistogramForm : Form
    {
        
        public HistogramForm()
        {
            InitializeComponent();
            this.BackColor = Color.White;
            this.Width = 800;
            this.Height = 800;
        }

        public void DrawHistogram(List<int> scores)
        {
            // Group scores by value and count their occurrences for histogram
            var groupedScores = scores.GroupBy(score => score)
                                      .OrderBy(g => g.Key)
                                      .ToDictionary(g => g.Key, g => g.Count());

            // Calculate the max frequency for scaling purposes
            int maxFrequency = groupedScores.Values.Max();

            // Get the Graphics object for the form
            Graphics graphics = this.CreateGraphics();
            graphics.Clear(Color.White);

            // Set up drawing parameters
            int padding = 50;
            int barWidth = 40;
            int spacing = 10;
            int chartHeight = this.Height - 2 * padding;

            // Define the starting point for drawing bars
            int xPosition = padding;

            // Define a brush for the bars
            Brush barBrush = new SolidBrush(Color.Coral);

            Font font = new Font("Arial", 10);

            // Draw each bar in the histogram
            foreach (var score in groupedScores)
            {
                int frequency = score.Value;

                // Calculate bar height based on frequency
                int barHeight = (int)((frequency / (float)maxFrequency) * chartHeight);

                // Define the rectangle for the bar
                Rectangle bar = new Rectangle(xPosition, this.Height - padding - barHeight, barWidth, barHeight);

                // Draw the bar
                graphics.FillRectangle(barBrush, bar);

                // Draw the score label below each bar
                string label = score.Key.ToString();
                
                SizeF labelSize = graphics.MeasureString(label, font);
                PointF labelPosition = new PointF(xPosition + (barWidth - labelSize.Width) / 2, this.Height - padding + 5);
                graphics.DrawString(label, font, Brushes.Black, labelPosition);

                // Move the xPosition to the right for the next bar
                xPosition += barWidth + spacing;
            }

            // Draw y-axis and x-axis
            Pen axisPen = new Pen(Color.Black, 2);
            graphics.DrawLine(axisPen, padding, this.Height - padding, this.Width - padding, this.Height - padding); // x-axis
            graphics.DrawLine(axisPen, padding, padding, padding, this.Height - padding); // y-axis

            // Label the y-axis with frequency values
            for (int i = 0; i <= maxFrequency; i++)
            {
                int yPosition = this.Height - padding - (int)((i / (float)maxFrequency) * chartHeight);
                graphics.DrawString(i.ToString(), font, Brushes.Black, padding - 30, yPosition - font.Height / 2);
            }
        }
    }
}

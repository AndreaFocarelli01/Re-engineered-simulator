using System.Windows.Forms;

namespace Re_engineered_stocastic_simulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //START button
        private void button1_Click(object sender, EventArgs e)
        {
            //creating the graph interface 
            Graphics graph = pictureBox1.CreateGraphics();
            graph.Clear(Color.White);

            int attackerNum = int.Parse(textBox1.Text);
            int serverNum = int.Parse(textBox2.Text);

            float probabilityNum = float.Parse(textBox3.Text);
            float lambdaNum = float.Parse(textBox4.Text);

            float lineHeight = pictureBox1.Height / (float) serverNum;
            float lineWidth = pictureBox1.Width / (float) serverNum;

            //starting point of the graph
            PointF pointF = new PointF(0.0f, 0.0f);



            List<int> results = new List<int>();

            if (radioButton1.Checked)
            {
                results = graphMaker1(graph, attackerNum, serverNum, lambdaNum, lineHeight, lineWidth);
            }
            if (radioButton2.Checked)
            {
                results = graphMaker2_1(graph, attackerNum, serverNum, lambdaNum, lineHeight, lineWidth);

            }
            if (radioButton6.Checked) 
            { 
                results = graphMaker2_2(graph, attackerNum, serverNum, probabilityNum, lineHeight, lineWidth);
            
            }
            if (radioButton3.Checked)
            {
                results = graphMaker3(graph, attackerNum, serverNum, lambdaNum, lineHeight, lineWidth);

            }
            if (radioButton4.Checked)
            {
                results = graphMaker4(graph, serverNum, attackerNum, probabilityNum, lineHeight, lineWidth);
            }

            //function to calculate the median, mean and variance of the resulted values
            CalculateStatistics(results);



        }

        //CLEAR button
        private void button2_Click(object sender, EventArgs e)
        {
            //reset of the graph
            Graphics graph = pictureBox1.CreateGraphics();
            graph.Clear(Color.White); 

        }

        private void CalculateStatistics(List<int> scores)
        {
            if (scores.Count == 0)
            {
                MessageBox.Show("No data to calculate statistics.");
                return;
            }

            float mean = (float)scores.Average();
            float variance = scores.Select(score => (score - mean) * (score - mean)).Average();
            float median = GetMedian(scores);

            MessageBox.Show($"Mean: {mean}\nVariance: {variance}\nMedian: {median}");
        }

        private float GetMedian(List<int> scores)
        {
            scores.Sort();
            int count = scores.Count;
            if (count % 2 == 0)
            {
                // Even number of scores: average the two middle elements
                return (scores[count / 2 - 1] + scores[count / 2]) / 2.0f;
            }
            else
            {
                // Odd number of scores: return the middle element
                return scores[count / 2];
            }
        }



        //HW1 function
        private List<int> graphMaker1(Graphics graph, int attackers, int servers, float probability, float width, float height)
        {
            List<int> totalScores = new List<int>();

            Random rand = new Random();

            float lineWidth = pictureBox1.Width / (float)servers;
            float lineHeight = pictureBox1.Height / (float)servers;

            for(int i = 0; i < attackers; i++)
            {
                Pen p = new Pen(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));

                int score = 0;

                PointF start = new PointF(0.0f, pictureBox1.Height);


                for (int j = 0; j < servers; j++)
                {

                    if(rand.NextDouble() <= probability)
                    {
                        //sale di uno se vince l'attacco
                        PointF end = new PointF(start.X, start.Y - lineHeight);
                        graph.DrawLine(p, start, end);
                        start = end;
                        score++;
                    }
                    //avanza e basta
                    PointF next = new PointF(start.X + lineWidth, start.Y);
                    graph.DrawLine(p, start, next);
                    start = next;
                }
                totalScores.Add(score);
            }
            return totalScores;
        }
        //HW2_1 function +1 -1
        private List<int> graphMaker2_1(Graphics graph, int attackers, int servers, float probability, float width, float height)
        {
            List<int> totalScores = new List<int>();

            Random rand = new Random();

            float lineWidth = pictureBox1.Width / (float)servers;
            float lineHeight = pictureBox1.Height / (float)servers;

            for (int i = 0; i < attackers; i++)
            {
                Pen p = new Pen(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)));

                int score = 0;

                PointF start = new PointF(0.0f, pictureBox1.Height/2);


                for (int j = 0; j < servers; j++)
                {
                    //sale di uno se vince l'attacco
                    if (rand.NextDouble() <= probability)
                    {          
                        PointF end = new PointF(start.X, start.Y - lineHeight);
                        graph.DrawLine(p, start, end);
                        start = end;
                        score++;
                    }
                    //scende se perde 
                    else
                    {
                        
                        PointF end = new PointF(start.X, start.Y + lineHeight); 
                        graph.DrawLine(p, start, end);
                        start = end;
                        score--;  // Decrease the score
                    }

                    //avanza e basta
                    PointF next = new PointF(start.X + lineWidth, start.Y);
                    graph.DrawLine(p, start, next);
                    start = next;
                }
                totalScores.Add(score);
            }
            return totalScores;
        }
        //HW2_2
        private List<int> graphMaker2_2(Graphics graph, int servers, int hackers, float probability, float lineWidth, float lineHeight)
        {
            Random rnd = new Random();
            List<int> totalScores = new List<int>();  // Lista per contenere gli score di tutti gli hacker

            // Itera su ogni hacker
            for (int i = 0; i < hackers; i++)
            {
                // Inizializza score per l'hacker corrente
                int jump = 0;
                int attempts = 0;

                // Punto di partenza in basso a sinistra
                PointF start = new PointF(0.0f, pictureBox1.Height / 2);


                Pen p = new Pen(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));

                for (int j = 0; j < servers; j++)
                {
                    attempts++;


                    bool penetrationSuccess = rnd.NextDouble() <= probability;


                    if (penetrationSuccess)
                    {
                        jump++;
                    }

                    float stepSize = lineHeight * (float)jump / attempts; // Movimento proporzionale

                    // Determina la direzione del passo
                    PointF end;
                    if (penetrationSuccess)
                    {
                        end = new PointF(start.X, start.Y - stepSize); // Salita
                    }
                    else
                    {
                        end = new PointF(start.X, start.Y + stepSize); // Discesa
                    }

                    graph.DrawLine(p, start, end);
                    start = end;

                    PointF next = new PointF(start.X + lineWidth, start.Y);
                    graph.DrawLine(p, start, next);
                    start = next;
                }


                totalScores.Add(jump);
            }

            return totalScores;
        }


        ////HW3 function
        private List<int> graphMaker3(Graphics graph, int hackers,int servers, float lambda, float width, float height)
        {
            List<int> totalscores = new List<int>();

            Pen line = new Pen(Color.Black);
            PointF endGraph1 = new PointF(pictureBox1.Width * 0.7f, pictureBox1.Height);
            PointF endGraph2 = new PointF(pictureBox1.Width * 0.7f, 0.0f);

            graph.DrawLine(line, endGraph1, endGraph2);

            Random rnd = new Random();

            for (int i = 0; i < hackers; i++)
            {
                Pen p = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

                int score = 0;

                PointF position = new PointF(0.0f, pictureBox1.Height);

                for (int j = 0; j < servers; j++)
                {
                    if (rnd.NextDouble() < lambda / j)
                    {
                        PointF end = new PointF(position.X, position.Y - height);
                        graph.DrawLine(p, position, end);
                        position = end;
                        score++;
                    }

                    PointF next = new PointF(position.X + width, position.Y);

                    graph.DrawLine(p, position, next);

                    position = next;

                }
                totalscores.Add(score);
            }

            DrawHistogram(graph, hackers, servers, totalscores, height);
            return totalscores;
        }
        private void DrawHistogram(Graphics graph, int hackers, int servers, List<int> scores, float height)
        {
            var groupedScores = scores.GroupBy(i => i).OrderBy(group => group.Key);
            float half = height / 2.0f;
            int maxCount = groupedScores.Max(g => g.Count());

            float scoreHeight = (pictureBox1.Width * 0.25f) / maxCount;
            foreach (var grp in groupedScores)
            {
                SolidBrush blueBrush = new SolidBrush(Color.Coral);
                float yPosition = pictureBox1.Height - height * grp.Key - half;
                RectangleF rect = new RectangleF(pictureBox1.Width * 0.7f, yPosition, scoreHeight * grp.Count(), height);
                graph.FillRectangle(blueBrush, rect);
            }
        }


        ////HW4 function
        private List<int> graphMaker4(Graphics graph, int servers, int hackers, float prob, float width, float height)
        {
            List<int> totalscores = new List<int>();

            float lineWidth = width / servers;
            float stepHeight = (float)Math.Sqrt(1f / servers);  //sqared step

            Random rnd = new Random();


            for (int i = 0; i < hackers; i++)
            {
                Pen p = new Pen(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));
                PointF point = new PointF(0, pictureBox1.Height / 2);
                int score = 0;

                for (int j = 0; j < servers; j++)
                {

                    if (rnd.NextDouble() < prob)
                    {
                        PointF up = new PointF(point.X, point.Y - stepHeight * height);
                        graph.DrawLine(p, point, up);
                        point = up;
                        PointF upRight = new PointF(point.X + width, point.Y);
                        graph.DrawLine(p, point, upRight);
                        point = upRight;
                        score++;
                    }
                    else
                    {
                        PointF down = new PointF(point.X, point.Y + stepHeight * height);
                        graph.DrawLine(p, point, down);
                        point = down;
                        PointF downRight = new PointF(point.X + width, point.Y);
                        graph.DrawLine(p, point, downRight);
                        point = downRight;
                    }

                }
                totalscores.Add(score);

            }
            return totalscores;
        }


        ////HW6 function
        //private List<int> graphMaker6();

        
    }
}

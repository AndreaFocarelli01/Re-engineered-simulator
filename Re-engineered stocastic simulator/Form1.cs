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



            List<int> results = graphMaker1(graph, attackerNum, serverNum, probabilityNum, lineWidth, lineHeight); 

            //function to calculate the median, mean and variance of the resulted values

        }

        //CLEAR button
        private void button2_Click(object sender, EventArgs e)
        {
            //reset of the graph
            Graphics graph = pictureBox1.CreateGraphics();
            graph.Clear(Color.White); 

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
        //HW2 function
        //private List<int> graphMaker2();
        ////HW3 function
        //private List<int> graphMaker3();
        ////HW4 function
        //private List<int> graphMaker4();
        ////HW6 function
        //private List<int> graphMaker6();

        ////histogram function
    }
}

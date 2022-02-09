using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RoutePlanner
{
    public partial class FormMain : Form
    {
        public CityMap Graph { get; set; } = new CityMap();
        public Route Route { get; set; } = new Route();

        public FormMain()
        {
            InitializeComponent();
        }

        private void CityDot_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string nameEng = button.Name.Remove(0, 6);
            string nameUkr = Graph.Cities.EngToUkr(nameEng);

            bool selected;

            if (CompareImages((Bitmap)button.BackgroundImage, Properties.Resources.DotBlack))
            {
                button.BackgroundImage = Properties.Resources.DotRed;
                selected = true;
            }
            else
            {
                button.BackgroundImage = Properties.Resources.DotBlack;
                selected = false;
            }

            if (selected)
            {
                comboBoxStartingCity.Items.Add(nameUkr);
            }
            else
            {
                comboBoxStartingCity.Items.Remove(nameUkr);
            }
            CheckStartingCity();
        }

        private void CheckStartingCity()
        {
            if (comboBoxStartingCity.SelectedIndex == -1 ||
                comboBoxStartingCity.Items.Count < 2)
            {
                buttonCalculate.Enabled = false;
            }
            else
            {
                buttonCalculate.Enabled = true;
            }
        }

        private bool CompareImages(Bitmap image1, Bitmap image2)
        {
            byte[] image1Bytes;
            byte[] image2Bytes;

            using (var mstream = new MemoryStream())
            {
                image1.Save(mstream, image1.RawFormat);
                image1Bytes = mstream.ToArray();
            }

            using (var mstream = new MemoryStream())
            {
                image2.Save(mstream, image2.RawFormat);
                image2Bytes = mstream.ToArray();
            }

            var image164 = Convert.ToBase64String(image1Bytes);
            var image264 = Convert.ToBase64String(image2Bytes);

            return string.Equals(image164, image264);
        }

        private void ComboBoxStartingCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckStartingCity();
        }

        private void ButtonCalculate_Click(object sender, EventArgs e)
        {
            if (comboBoxStartingCity.Items.Count < 2)
            {
                MessageBox.Show(Properties.Resources.ErrorCitiesAmount, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string startPoint = comboBoxStartingCity.Text;

            List<string> destinationPoints = new List<string>();
            foreach (string city in comboBoxStartingCity.Items)
                if (city != startPoint)
                    destinationPoints.Add(city);

            listBoxResults.Enabled = true;

            Route = Graph.BuildRoute(startPoint, destinationPoints);

            listBoxResults.Items.Clear();
            foreach (Chain chain in Route)
                listBoxResults.Items.Add(chain.ToString());
        }
    }
}

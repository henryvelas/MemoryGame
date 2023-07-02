using System.Reflection;

namespace MemoryGame
{

    public partial class Form1 : Form
    {
        List<Icon> _icons = new List<Icon>();
        Random _random = new Random();
        Panel firstSelection, secondSelection, firstCovertSelection, secondCovertSelection;
        Dictionary<string, int> assignedPanels = new Dictionary<string, int>();
        public Form1()
        {
            InitializeComponent();
            LoadImagesFromFiles();
            PopulateIconsToTable();
            ShowCardsInit(true);
        }

        private void ShowCardsInit(bool showCards)
        {
            pn1.Visible = !showCards;
            pn2.Visible = !showCards;
            pn3.Visible = !showCards;
            pn4.Visible = !showCards;
            pn5.Visible = !showCards;
            pn6.Visible = !showCards;
            pn7.Visible = !showCards;
            pn8.Visible = !showCards;
            pn9.Visible = !showCards;
            pn10.Visible = !showCards;
            pn11.Visible = !showCards;
            pn12.Visible = !showCards;
            pn13.Visible = !showCards;
            pn14.Visible = !showCards;
            pn15.Visible = !showCards;
            pn16.Visible = !showCards;

            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            ShowCardsInit(false);
            timer1.Dispose();
        }

        private void LoadImagesFromFiles()
        {
            var files = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            int id = 1;

            foreach (var picture in files)
            {
                if (!picture.EndsWith(".png"))
                {
                    continue;
                }

                var icon = new Icon
                {
                    Id = id,
                    Name = picture.Replace("MemoryGame.Resources.", "").Replace(".png", ""),
                    Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(picture)),
                };

                _icons.Add(icon);
                _icons.Add(icon);
                id++;
            }
        }

        private void PopulateIconsToTable()
        {
            Panel panel;
            int radomNumber;

            foreach (var item in this.Controls)
            {
                if (item is Panel && !((Panel)item).Name.Contains("pn"))
                {
                    panel = (Panel)item;
                }
                else
                    continue;

                radomNumber = _random.Next(0, _icons.Count);

                panel.BackgroundImage = _icons[radomNumber].Image;

                assignedPanels.Add(panel.Name, _icons[radomNumber].Id);

                _icons.RemoveAt(radomNumber);
            }
        }

        private void pn_Click(object sender, EventArgs e)
        {
            if (firstSelection != null && secondSelection != null)
                return;

            Panel clickedPanel = (Panel)sender;

            if (clickedPanel == null)
                return;

            if (!clickedPanel.Visible)
                return;

            clickedPanel.Visible = false;

            if (firstSelection == null)
            {
                firstSelection = GetIconPanell(clickedPanel);
                firstCovertSelection = clickedPanel;
                return;
            }

            if (secondSelection == null)
            {
                secondSelection = GetIconPanell(clickedPanel);
                secondCovertSelection = clickedPanel;
            }

            if (firstSelection != null && secondSelection != null && CheckForMatch())
            {
                CleanSelection(true);
            }
            else
            {
                ResetUnMarched();
            }

        }

        private Panel GetIconPanell(Panel coverPanel)
        {
            Panel iconPanel = null;

            foreach (var item in this.Controls)
            {
                if (item is Panel && !((Panel)item).Name.Contains("pn") && ((Panel)item).Tag == coverPanel.Tag)
                {
                    iconPanel = (Panel)item;
                }
            }
            return iconPanel;
        }

        private bool CheckForMatch()
        {
            return assignedPanels[firstSelection.Name] == assignedPanels[secondSelection.Name];
        }

        private void CleanSelection(bool match)
        {
            if (!match)
            {
                firstCovertSelection.Visible = true;
                secondCovertSelection.Visible = true;
            }

            firstCovertSelection = null;
            secondCovertSelection = null;
            firstSelection = null;
            secondSelection = null;

        }

        private void ResetUnMarched()
        {
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            CleanSelection(false);
            timer2.Stop();
        }
    }
}
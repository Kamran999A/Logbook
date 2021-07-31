using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using System.IO;

namespace WindowsForms_LogBook
{
    public partial class Form1 : Form
    {
        public readonly List<Student> Students = new List<Student>();
        private string _expTxtBxTextHelper = default;
        private bool _isEditIconPressed = false;
        private readonly int _currentY = default;
        private readonly List<UC1> UserControls = new List<UC1>();

        public readonly List<string> _imagePaths = new List<string>() {  "../../Images/AhmedoKamran.jpg", "../../Images/NeandertalRuslan.jpg",
        "../../Images/ChildRafael.jpg", "../../Images/AlpachinoHuseyn.jpg", "../../Images/CSharp.png"};
        public void NullComp()
        {
            for (int i = 0; i < 5; i++)
            {
                Students[i].ExaminationWorkGrade = null;
                Students[i].CommentFromTrainer = null;
                Students[i].ClassWorkGrade = null;
                Students[i].CrystalCount = 0;
                UserControls[i].DeletePctBx_Clicked();

            }
            int _currentY = 225;

            for (int i = 0; i < 5; i++)
            {
                UserControls.Add(new UC1());

                UserControls[i].CrystalLbl = CrystalLbl;

                UserControls[i].PictureBoxPath = _imagePaths[i];
                UserControls[i].FullName = Students[i].FullName;
                UserControls[i].Number = i + 1;

                _currentY += 110;
                UserControls[i].Location = new Point(35, _currentY);

                UserControls[i].Student = Students[i];
                Controls.Add(UserControls[i]);
            }

           
        }
      
        public Form1()
        {
            InitializeComponent();

            CrystalToolTip.SetToolTip(CrystalSymbolPctBx, "Awards which Students have for special works.");
            EditToolTip.SetToolTip(EditPctBx, "Add which experience or experiences will Students see today.");
            MarkAllToolTip.SetToolTip(MarkAllRdBtn, "Everybody here");

            if (File.Exists("Database/Students.json") && File.Exists("Database/CrystalCount.json"))
            {
                string helper = default;
                JsonFileHelper.JSONDeSerialization(ref Students, "Students.json");
                JsonFileHelper.JSONDeSerialization(ref helper, "CrystalCount.json");
                CrystalLbl.Text = helper;
            }
            else
            {
                Students.Add(new Student("Kamran Aliyev", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Students.Add(new Student("Ruslan Mustafayev", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Students.Add(new Student("Rafael Xalilzade", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Students.Add(new Student("Huseyn Rustamli", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
                Students.Add(new Student("Csharp Nedi", DateTime.Now, AttentionStates.None, "-", "-", 0, ""));
            }

            if (File.Exists("Database/Experience.json"))
            {
                string helper = default;
                JsonFileHelper.JSONDeSerialization(ref helper, "Experience.json");
                ExperienceTxtBx.Text = helper;
            }

            _currentY = guna2ShadowPanel1.Location.Y;

            for (int i = 0; i < 5; i++)
            {
                UserControls.Add(new UC1());

                UserControls[i].CrystalLbl = CrystalLbl;

                UserControls[i].PictureBoxPath = _imagePaths[i];
                UserControls[i].FullName = Students[i].FullName;
                UserControls[i].Number = i + 1;

                _currentY += 110;
                UserControls[i].Location = new Point(35, _currentY);

                UserControls[i].Student = Students[i];
                Controls.Add(UserControls[i]);
            }
        }


        private void SetVisibilityWhenWritingExperience()
        {
            ExperienceTxtBx.Visible = true;
            SaveBtn.Visible = true;
            IgnoreBtn.Visible = true;
        }

        private void SetInvisibilityWhenFinishExperience()
        {
            ExperienceTxtBx.Visible = false;
            SaveBtn.Visible = false;
            IgnoreBtn.Visible = false;
        }
        private void EditPctBx_Click(object sender, MouseEventArgs e)
        {
            if (_isEditIconPressed == false)
            {
                _isEditIconPressed = true;
                _expTxtBxTextHelper = ExperienceTxtBx.Text;
                SetVisibilityWhenWritingExperience();
            }
            else
            {
                _isEditIconPressed = false;
                SetInvisibilityWhenFinishExperience();
                ExperienceTxtBx.Text = _expTxtBxTextHelper;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            JsonFileHelper.JSONSerialization(Students, "Students.json");
            JsonFileHelper.JSONSerialization(ExperienceTxtBx.Text, "Experience.json");
            JsonFileHelper.JSONSerialization(CrystalLbl.Text, "CrystalCount.json");
            SetInvisibilityWhenFinishExperience();


            _isEditIconPressed = false;

        }

        private void IgnoreBtn_Click(object sender, EventArgs e)
        {
            SetInvisibilityWhenFinishExperience();
            ExperienceTxtBx.Text = _expTxtBxTextHelper;
            _isEditIconPressed = false;

        }


        private void MarkAllRdBtn_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < UserControls.Count; i++)
            {
                UserControls[i].MarkAllRdBtn_CheckedChanged(sender, e);
            }
        }

        private void ExitBtn_MouseHover(object sender, EventArgs e)
        {
            ExitBtn.BorderThickness = 0;
            ExitBtn.FillColor = Color.Red;
        }

        private void ExitBtn_MouseLeave(object sender, EventArgs e)
        {
            ExitBtn.BorderThickness = 1;
            ExitBtn.FillColor = ColorTranslator.FromHtml("#fffff");
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
            NullComp();
            WriteFileWhenApplicationEnded();
        }


        private void WriteFileWhenApplicationEnded()
        {
            for (int i = 0; i < UserControls.Count; i++)
            {
                if (UserControls[i].HereRdBtn.Checked)
                    Students[i].State = AttentionStates.IsHere;
                else if (UserControls[i].LateRdBtn.Checked)
                    Students[i].State = AttentionStates.IsLate;
                else if (UserControls[i].NotHereRdBtn.Checked)
                    Students[i].State = AttentionStates.IsNotHere;
                else
                    Students[i].State = AttentionStates.None;

                if (UserControls[i].ExamWorkGradeCmbBx.Text == "-")
                {
                    Students[i].ExaminationWorkGrade = "Didn't get any grade";
                }
                else
                    Students[i].ExaminationWorkGrade = UserControls[i].ExamWorkGradeCmbBx.Text;


                if (UserControls[i].UFCWorkGradeCmbBx.Text == "-")
                {
                    Students[i].ClassWorkGrade = "Didn't get any grade";
                }
                else
                    Students[i].ClassWorkGrade = UserControls[i].UFCWorkGradeCmbBx.Text;

                Students[i].CrystalCount = UserControls[i].CrystalCount;

                Students[i].CommentFromTrainer = UserControls[i].CommentTxtBx.Text;

            }

            if (!string.IsNullOrWhiteSpace(ExperienceTxtBx.Text))
                JsonFileHelper.JSONSerialization(ExperienceTxtBx.Text, "Experience.json");

            JsonFileHelper.JSONSerialization(Students, "Students.json");
            JsonFileHelper.JSONSerialization(CrystalLbl.Text, "CrystalCount.json");
        }

        private void ExaminationWorkLbl_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void MissionBtn_Click(object sender, EventArgs e)
        {

        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CrystalSymbolPctBx_Click(object sender, EventArgs e)
        {

        }

        private void CrystalLbl_Click(object sender, EventArgs e)
        {

        }

        private void CommentLbl_Click(object sender, EventArgs e)
        {

        }

        private void MarkLbl_Click(object sender, EventArgs e)
        {

        }

        private void TimeLbl_Click(object sender, EventArgs e)
        {

        }

        private void NameSurnameLbl_Click(object sender, EventArgs e)
        {

        }

        private void CrystalToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void DeleteToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void CommentToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void EditToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void IsHereToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void IsLateToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void IsNotHereToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void MarkAllToolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void EditPctBx_Click(object sender, EventArgs e)
        {

        }

        private void Resetbtn_Click(object sender, EventArgs e)
        {
            NullComp();
        }

      

    }
}
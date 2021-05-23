using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace LawFirmView
{
    public partial class FormMail : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly MailLogic logic;
        private bool hasNext = true;
        private readonly int mailsOnPage = 100;
        private int currentPage = 0;
        public FormMail(MailLogic mailLogic)
        {
            InitializeComponent();
            logic = mailLogic;
        }

        private void FormMail_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var list = logic.Read(new MessageInfoBindingModel { ToSkip = currentPage * mailsOnPage, ToTake = mailsOnPage + 1 });
            hasNext = !(list.Count() <= mailsOnPage);
            if (hasNext)
            {
                buttonNext.Enabled = true;
            }
            else
            {
                buttonNext.Enabled = false;
            }
            if (currentPage==0)
            {
                buttonPrev.Enabled = false;
            }
            else
            {
                buttonPrev.Enabled = true;
            }
            if (list != null)
            {
                dataGridView.DataSource = list.Take(mailsOnPage).ToList();
                dataGridView.Columns[0].Visible = false;
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if ((currentPage - 1) >= 0)
            {
                currentPage--;
                buttonNext.Enabled = true;
                if (currentPage == 0)
                {
                    buttonPrev.Enabled = false;
                }
                label1.Text = "Страница " + (currentPage + 1).ToString();
                LoadData();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (hasNext)
            {
                currentPage++;
                label1.Text = "Страница "+(currentPage + 1).ToString();
                buttonPrev.Enabled = true;
                LoadData();
            }
        }
    }
}

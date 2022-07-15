using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCleaner
{
    public partial class Summary : Form
    {
        Dictionary<string,int> logs = new Dictionary<string,int>();
        Random random = new Random();
        public Summary(Dictionary<string,int> logs)
        {
            InitializeComponent();
            this.logs = logs;
        }

        private void Summary_Load(object sender, EventArgs e)
        {
            int listCount = 0;
            foreach (var item in logs)
            {
                listView1.Items.Add(item.Key);
                int val = item.Value == 0 ? random.Next(1,50) : item.Value;
                listView1.Items[listCount++].SubItems.Add(val.ToString());
            }
        }
    }
}

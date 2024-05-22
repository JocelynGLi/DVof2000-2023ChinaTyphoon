using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LJG
{
    public partial class Form2 : Form
    {
        public static bool isShowLabel = true;
        Form1 f = new Form1();
        public Form2()
        {
            InitializeComponent();
            f.Show();
            f.Dock= DockStyle.Fill;
            panel3.Controls.Add(f);
  
        }

        private void bAddMap_Click(object sender, EventArgs e)
        {
            f.bReadShapefile_Click(null, null);
        }

        private void bMapAT_Click(object sender, EventArgs e)
        {
            dgvValues.Columns.Clear();
            dgvValues.Rows.Clear();
            var layer = f.layer;
            dgvValues.Columns.Add("Index", "Index");

            for (int i = 0; i < layer.Fields.Count; i++)
            {
                dgvValues.Columns.Add(layer.Fields[i].name,
                    layer.Fields[i].name + "(" + i + ")");
            }
            //一行一行，或者说一条记录一条记录的添加属性值
            for (int i = 0; i < layer.FeatureCount(); i++)
            {
                dgvValues.Rows.Add();
                dgvValues.Rows[i].Cells[0].Value = i;

                for (int j = 0; j < layer.Fields.Count; j++)
                {
                    dgvValues.Rows[i].Cells[j + 1].Value = layer.GetFeature(i).GetAttribute(j);
                }
            }
        }

        private void bTyphoonAT_Click(object sender, EventArgs e)
        {
            try
            {
                dgvValues.Columns.Clear();
                dgvValues.Rows.Clear();
                var layer = f.layerTyphoon;
                dgvValues.Columns.Add("Index", "Index");

                for (int i = 0; i < layer.Fields.Count; i++)
                {
                    dgvValues.Columns.Add(layer.Fields[i].name,
                        layer.Fields[i].name + "(" + i + ")");
                }
                //一行一行，或者说一条记录一条记录的添加属性值
                for (int i = 0; i < layer.FeatureCount(); i++)
                {
                    dgvValues.Rows.Add();
                    dgvValues.Rows[i].Cells[0].Value = i;

                    for (int j = 0; j < layer.Fields.Count; j++)
                    {
                        dgvValues.Rows[i].Cells[j + 1].Value = layer.GetFeature(i).GetAttribute(j);
                    }
                }
            }
            catch (Exception ee)
            {

                
            }
           

        }

        private void cbTyphoonCircle_CheckedChanged(object sender, EventArgs e)
        {
            Form1.isCheckTyphoon = !Form1.isCheckTyphoon;
            f.UpdateMap();
        }

        private void cbMapColor_CheckedChanged(object sender, EventArgs e)
        {
            Form1.isCheckMap = !Form1.isCheckMap;
            f.UpdateMap();
        }

        private void cbMapLabel_CheckedChanged(object sender, EventArgs e)
        {
          isShowLabel = cbMapLabel.Checked;
           f. UpdateMap();
        }

        private void ExploreButton_Click(object sender, EventArgs e)
        {
            f.ExploreButton_Click(sender, null);
        }

        private void bFullExtent_Click(object sender, EventArgs e)
        {
            f.bFullExtent_Click(null,null);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           
        }
        string tempstr = "";
        private void tbYear_TextChanged(object sender, EventArgs e)
        {
            cbTyphoonId.Items.Clear();

            HashSet<string> files = new HashSet<string>();
            Directory.GetFiles("data/台风").ToList().ForEach (file => { files.Add(Path.GetFileNameWithoutExtension(file)); }) ;
            foreach (string file in files)
            {
                if(file.Substring(0,4)==tbYear.Text)
                {
                    cbTyphoonId.Items.Add(file.Substring(4, 2));
                }
              
            }
             
        }

        private void bLoadTyphoon_Click(object sender, EventArgs e)
        {
            f.bReadShapefile_Click(tbYear.Text+cbTyphoonId.Text, null);
        }


    }
}

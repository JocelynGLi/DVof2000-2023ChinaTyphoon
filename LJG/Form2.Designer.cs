namespace LJG
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvValues = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbMapLabel = new System.Windows.Forms.CheckBox();
            this.bTyphoonAT = new System.Windows.Forms.Button();
            this.cbTyphoonId = new System.Windows.Forms.ComboBox();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMapColor = new System.Windows.Forms.CheckBox();
            this.cbTyphoonCircle = new System.Windows.Forms.CheckBox();
            this.bMapAT = new System.Windows.Forms.Button();
            this.bLoadTyphoon = new System.Windows.Forms.Button();
            this.bAddMap = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bViewAll = new System.Windows.Forms.Button();
            this.bMoveUp = new System.Windows.Forms.Button();
            this.bZoomOut = new System.Windows.Forms.Button();
            this.bMoveDown = new System.Windows.Forms.Button();
            this.bZoomIn = new System.Windows.Forms.Button();
            this.bMoveLeft = new System.Windows.Forms.Button();
            this.bMoveRight = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(498, 900);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel4.Controls.Add(this.dgvValues);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(2, 2);
            this.panel4.Margin = new System.Windows.Forms.Padding(6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(492, 894);
            this.panel4.TabIndex = 0;
            // 
            // dgvValues
            // 
            this.dgvValues.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvValues.Location = new System.Drawing.Point(0, 600);
            this.dgvValues.Margin = new System.Windows.Forms.Padding(6);
            this.dgvValues.Name = "dgvValues";
            this.dgvValues.RowHeadersWidth = 82;
            this.dgvValues.RowTemplate.Height = 23;
            this.dgvValues.Size = new System.Drawing.Size(492, 294);
            this.dgvValues.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(179)))), ((int)(((byte)(206)))));
            this.panel5.Controls.Add(this.cbMapLabel);
            this.panel5.Controls.Add(this.bTyphoonAT);
            this.panel5.Controls.Add(this.cbTyphoonId);
            this.panel5.Controls.Add(this.tbYear);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.cbMapColor);
            this.panel5.Controls.Add(this.cbTyphoonCircle);
            this.panel5.Controls.Add(this.bMapAT);
            this.panel5.Controls.Add(this.bLoadTyphoon);
            this.panel5.Controls.Add(this.bAddMap);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(6);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(492, 600);
            this.panel5.TabIndex = 0;
            // 
            // cbMapLabel
            // 
            this.cbMapLabel.AutoSize = true;
            this.cbMapLabel.Checked = true;
            this.cbMapLabel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMapLabel.Location = new System.Drawing.Point(266, 56);
            this.cbMapLabel.Margin = new System.Windows.Forms.Padding(6);
            this.cbMapLabel.Name = "cbMapLabel";
            this.cbMapLabel.Size = new System.Drawing.Size(90, 28);
            this.cbMapLabel.TabIndex = 6;
            this.cbMapLabel.Text = "标注";
            this.cbMapLabel.UseVisualStyleBackColor = true;
            this.cbMapLabel.CheckedChanged += new System.EventHandler(this.cbMapLabel_CheckedChanged);
            // 
            // bTyphoonAT
            // 
            this.bTyphoonAT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.bTyphoonAT.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bTyphoonAT.Font = new System.Drawing.Font("宋体", 8F);
            this.bTyphoonAT.ForeColor = System.Drawing.Color.Black;
            this.bTyphoonAT.Location = new System.Drawing.Point(58, 386);
            this.bTyphoonAT.Margin = new System.Windows.Forms.Padding(6);
            this.bTyphoonAT.Name = "bTyphoonAT";
            this.bTyphoonAT.Size = new System.Drawing.Size(186, 50);
            this.bTyphoonAT.TabIndex = 5;
            this.bTyphoonAT.Text = "打开台风属性表";
            this.bTyphoonAT.UseVisualStyleBackColor = false;
            this.bTyphoonAT.Click += new System.EventHandler(this.bTyphoonAT_Click);
            // 
            // cbTyphoonId
            // 
            this.cbTyphoonId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTyphoonId.FormattingEnabled = true;
            this.cbTyphoonId.Location = new System.Drawing.Point(172, 180);
            this.cbTyphoonId.Margin = new System.Windows.Forms.Padding(6);
            this.cbTyphoonId.Name = "cbTyphoonId";
            this.cbTyphoonId.Size = new System.Drawing.Size(176, 32);
            this.cbTyphoonId.TabIndex = 4;
            // 
            // tbYear
            // 
            this.tbYear.Location = new System.Drawing.Point(172, 118);
            this.tbYear.Margin = new System.Windows.Forms.Padding(6);
            this.tbYear.Name = "tbYear";
            this.tbYear.Size = new System.Drawing.Size(176, 35);
            this.tbYear.TabIndex = 3;
            this.tbYear.TextChanged += new System.EventHandler(this.tbYear_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 186);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "选择台风";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入年份";
            // 
            // cbMapColor
            // 
            this.cbMapColor.AutoSize = true;
            this.cbMapColor.Location = new System.Drawing.Point(58, 528);
            this.cbMapColor.Margin = new System.Windows.Forms.Padding(6);
            this.cbMapColor.Name = "cbMapColor";
            this.cbMapColor.Size = new System.Drawing.Size(186, 28);
            this.cbMapColor.TabIndex = 0;
            this.cbMapColor.Text = "地图分级设色";
            this.cbMapColor.UseVisualStyleBackColor = true;
            this.cbMapColor.CheckedChanged += new System.EventHandler(this.cbMapColor_CheckedChanged);
            // 
            // cbTyphoonCircle
            // 
            this.cbTyphoonCircle.AutoSize = true;
            this.cbTyphoonCircle.Location = new System.Drawing.Point(58, 464);
            this.cbTyphoonCircle.Margin = new System.Windows.Forms.Padding(6);
            this.cbTyphoonCircle.Name = "cbTyphoonCircle";
            this.cbTyphoonCircle.Size = new System.Drawing.Size(186, 28);
            this.cbTyphoonCircle.TabIndex = 0;
            this.cbTyphoonCircle.Text = "显示台风风圈";
            this.cbTyphoonCircle.UseVisualStyleBackColor = true;
            this.cbTyphoonCircle.CheckedChanged += new System.EventHandler(this.cbTyphoonCircle_CheckedChanged);
            // 
            // bMapAT
            // 
            this.bMapAT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.bMapAT.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bMapAT.Font = new System.Drawing.Font("宋体", 8F);
            this.bMapAT.ForeColor = System.Drawing.Color.Black;
            this.bMapAT.Location = new System.Drawing.Point(58, 312);
            this.bMapAT.Margin = new System.Windows.Forms.Padding(6);
            this.bMapAT.Name = "bMapAT";
            this.bMapAT.Size = new System.Drawing.Size(186, 50);
            this.bMapAT.TabIndex = 0;
            this.bMapAT.Text = "打开地图属性表";
            this.bMapAT.UseVisualStyleBackColor = false;
            this.bMapAT.Click += new System.EventHandler(this.bMapAT_Click);
            // 
            // bLoadTyphoon
            // 
            this.bLoadTyphoon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.bLoadTyphoon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bLoadTyphoon.Font = new System.Drawing.Font("宋体", 8F);
            this.bLoadTyphoon.ForeColor = System.Drawing.Color.Black;
            this.bLoadTyphoon.Location = new System.Drawing.Point(58, 238);
            this.bLoadTyphoon.Margin = new System.Windows.Forms.Padding(6);
            this.bLoadTyphoon.Name = "bLoadTyphoon";
            this.bLoadTyphoon.Size = new System.Drawing.Size(186, 50);
            this.bLoadTyphoon.TabIndex = 0;
            this.bLoadTyphoon.Text = "加载台风路径";
            this.bLoadTyphoon.UseVisualStyleBackColor = false;
            this.bLoadTyphoon.Click += new System.EventHandler(this.bLoadTyphoon_Click);
            // 
            // bAddMap
            // 
            this.bAddMap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
            this.bAddMap.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bAddMap.Font = new System.Drawing.Font("宋体", 8F);
            this.bAddMap.ForeColor = System.Drawing.Color.Black;
            this.bAddMap.Location = new System.Drawing.Point(58, 42);
            this.bAddMap.Margin = new System.Windows.Forms.Padding(6);
            this.bAddMap.Name = "bAddMap";
            this.bAddMap.Size = new System.Drawing.Size(160, 50);
            this.bAddMap.TabIndex = 0;
            this.bAddMap.Text = "加载中国地图";
            this.bAddMap.UseVisualStyleBackColor = false;
            this.bAddMap.Click += new System.EventHandler(this.bAddMap_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(498, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1102, 80);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(228)))), ((int)(((byte)(241)))));
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.06534F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.34301F));
            this.tableLayoutPanel1.Controls.Add(this.bViewAll, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.bMoveUp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bZoomOut, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.bMoveDown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.bZoomIn, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.bMoveLeft, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.bMoveRight, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1102, 80);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bViewAll
            // 
            this.bViewAll.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bViewAll.BackColor = System.Drawing.SystemColors.Control;
            this.bViewAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bViewAll.Font = new System.Drawing.Font("宋体", 8F);
            this.bViewAll.ForeColor = System.Drawing.Color.Black;
            this.bViewAll.Location = new System.Drawing.Point(828, 15);
            this.bViewAll.Margin = new System.Windows.Forms.Padding(6);
            this.bViewAll.Name = "bViewAll";
            this.bViewAll.Size = new System.Drawing.Size(142, 50);
            this.bViewAll.TabIndex = 0;
            this.bViewAll.Text = "显示全图";
            this.bViewAll.UseVisualStyleBackColor = false;
            this.bViewAll.Click += new System.EventHandler(this.bFullExtent_Click);
            // 
            // bMoveUp
            // 
            this.bMoveUp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bMoveUp.BackColor = System.Drawing.SystemColors.Control;
            this.bMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bMoveUp.Font = new System.Drawing.Font("宋体", 8F);
            this.bMoveUp.ForeColor = System.Drawing.Color.Black;
            this.bMoveUp.Location = new System.Drawing.Point(31, 15);
            this.bMoveUp.Margin = new System.Windows.Forms.Padding(6);
            this.bMoveUp.Name = "bMoveUp";
            this.bMoveUp.Size = new System.Drawing.Size(100, 50);
            this.bMoveUp.TabIndex = 0;
            this.bMoveUp.Text = "上移";
            this.bMoveUp.UseVisualStyleBackColor = false;
            this.bMoveUp.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // bZoomOut
            // 
            this.bZoomOut.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bZoomOut.BackColor = System.Drawing.SystemColors.Control;
            this.bZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bZoomOut.Font = new System.Drawing.Font("宋体", 8F);
            this.bZoomOut.ForeColor = System.Drawing.Color.Black;
            this.bZoomOut.Location = new System.Drawing.Point(716, 15);
            this.bZoomOut.Margin = new System.Windows.Forms.Padding(6);
            this.bZoomOut.Name = "bZoomOut";
            this.bZoomOut.Size = new System.Drawing.Size(100, 50);
            this.bZoomOut.TabIndex = 0;
            this.bZoomOut.Text = "缩小";
            this.bZoomOut.UseVisualStyleBackColor = false;
            this.bZoomOut.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // bMoveDown
            // 
            this.bMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bMoveDown.BackColor = System.Drawing.SystemColors.Control;
            this.bMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bMoveDown.Font = new System.Drawing.Font("宋体", 8F);
            this.bMoveDown.ForeColor = System.Drawing.Color.Black;
            this.bMoveDown.Location = new System.Drawing.Point(168, 15);
            this.bMoveDown.Margin = new System.Windows.Forms.Padding(6);
            this.bMoveDown.Name = "bMoveDown";
            this.bMoveDown.Size = new System.Drawing.Size(100, 50);
            this.bMoveDown.TabIndex = 0;
            this.bMoveDown.Text = "下移";
            this.bMoveDown.UseVisualStyleBackColor = false;
            this.bMoveDown.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // bZoomIn
            // 
            this.bZoomIn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bZoomIn.BackColor = System.Drawing.SystemColors.Control;
            this.bZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bZoomIn.Font = new System.Drawing.Font("宋体", 8F);
            this.bZoomIn.ForeColor = System.Drawing.Color.Black;
            this.bZoomIn.Location = new System.Drawing.Point(579, 15);
            this.bZoomIn.Margin = new System.Windows.Forms.Padding(6);
            this.bZoomIn.Name = "bZoomIn";
            this.bZoomIn.Size = new System.Drawing.Size(100, 50);
            this.bZoomIn.TabIndex = 0;
            this.bZoomIn.Text = "放大";
            this.bZoomIn.UseVisualStyleBackColor = false;
            this.bZoomIn.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // bMoveLeft
            // 
            this.bMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bMoveLeft.BackColor = System.Drawing.SystemColors.Control;
            this.bMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bMoveLeft.Font = new System.Drawing.Font("宋体", 8F);
            this.bMoveLeft.ForeColor = System.Drawing.Color.Black;
            this.bMoveLeft.Location = new System.Drawing.Point(305, 15);
            this.bMoveLeft.Margin = new System.Windows.Forms.Padding(6);
            this.bMoveLeft.Name = "bMoveLeft";
            this.bMoveLeft.Size = new System.Drawing.Size(100, 50);
            this.bMoveLeft.TabIndex = 0;
            this.bMoveLeft.Text = "左移";
            this.bMoveLeft.UseVisualStyleBackColor = false;
            this.bMoveLeft.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // bMoveRight
            // 
            this.bMoveRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.bMoveRight.BackColor = System.Drawing.SystemColors.Control;
            this.bMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bMoveRight.Font = new System.Drawing.Font("宋体", 8F);
            this.bMoveRight.ForeColor = System.Drawing.Color.Black;
            this.bMoveRight.Location = new System.Drawing.Point(442, 15);
            this.bMoveRight.Margin = new System.Windows.Forms.Padding(6);
            this.bMoveRight.Name = "bMoveRight";
            this.bMoveRight.Size = new System.Drawing.Size(100, 50);
            this.bMoveRight.TabIndex = 0;
            this.bMoveRight.Text = "右移";
            this.bMoveRight.UseVisualStyleBackColor = false;
            this.bMoveRight.Click += new System.EventHandler(this.ExploreButton_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(498, 80);
            this.panel3.Margin = new System.Windows.Forms.Padding(6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1102, 820);
            this.panel3.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "鼠标位置：";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form2";
            this.Text = "中国2000年-2023年台风可视化";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValues)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button bAddMap;
        private System.Windows.Forms.CheckBox cbMapLabel;
        private System.Windows.Forms.Button bTyphoonAT;
        private System.Windows.Forms.ComboBox cbTyphoonId;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbMapColor;
        private System.Windows.Forms.CheckBox cbTyphoonCircle;
        private System.Windows.Forms.Button bMapAT;
        private System.Windows.Forms.DataGridView dgvValues;
        private System.Windows.Forms.Button bZoomIn;
        private System.Windows.Forms.Button bMoveRight;
        private System.Windows.Forms.Button bMoveLeft;
        private System.Windows.Forms.Button bMoveDown;
        private System.Windows.Forms.Button bMoveUp;
        private System.Windows.Forms.Button bZoomOut;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button bViewAll;
        private System.Windows.Forms.Button bLoadTyphoon;
        private System.Windows.Forms.Label label3;
    }
}
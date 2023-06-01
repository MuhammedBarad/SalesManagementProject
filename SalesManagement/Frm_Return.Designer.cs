namespace SalesManagement
{
    partial class Frm_Return
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Return));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnBuy = new System.Windows.Forms.RadioButton();
            this.rbtnSales = new System.Windows.Forms.RadioButton();
            this.dtpReturnDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvSearch = new System.Windows.Forms.DataGridView();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtpaied = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReminder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReturnAll = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudQty = new System.Windows.Forms.NumericUpDown();
            this.btnReturnitemOnly = new DevExpress.XtraEditors.SimpleButton();
            this.rbtnReturnQtyOnly = new System.Windows.Forms.RadioButton();
            this.rbtnReturnitem = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnBuy);
            this.groupBox1.Controls.Add(this.rbtnSales);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 70);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            // 
            // rbtnBuy
            // 
            this.rbtnBuy.AutoSize = true;
            this.rbtnBuy.ForeColor = System.Drawing.Color.Blue;
            this.rbtnBuy.Location = new System.Drawing.Point(177, 24);
            this.rbtnBuy.Name = "rbtnBuy";
            this.rbtnBuy.Size = new System.Drawing.Size(135, 27);
            this.rbtnBuy.TabIndex = 1;
            this.rbtnBuy.TabStop = true;
            this.rbtnBuy.Text = "Satıcıya iade";
            this.rbtnBuy.UseVisualStyleBackColor = true;
            this.rbtnBuy.CheckedChanged += new System.EventHandler(this.rbtnBuy_CheckedChanged);
            // 
            // rbtnSales
            // 
            this.rbtnSales.AutoSize = true;
            this.rbtnSales.ForeColor = System.Drawing.Color.Blue;
            this.rbtnSales.Location = new System.Drawing.Point(9, 24);
            this.rbtnSales.Name = "rbtnSales";
            this.rbtnSales.Size = new System.Drawing.Size(165, 27);
            this.rbtnSales.TabIndex = 0;
            this.rbtnSales.TabStop = true;
            this.rbtnSales.Text = "Müşteriden iade";
            this.rbtnSales.UseVisualStyleBackColor = true;
            this.rbtnSales.CheckedChanged += new System.EventHandler(this.rbtnSales_CheckedChanged);
            // 
            // dtpReturnDate
            // 
            this.dtpReturnDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReturnDate.Location = new System.Drawing.Point(465, 33);
            this.dtpReturnDate.Name = "dtpReturnDate";
            this.dtpReturnDate.Size = new System.Drawing.Size(135, 29);
            this.dtpReturnDate.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(352, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "İade Tarihi";
            // 
            // txtOrderID
            // 
            this.txtOrderID.Location = new System.Drawing.Point(825, 34);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(239, 29);
            this.txtOrderID.TabIndex = 35;
            this.txtOrderID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtOrderID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOrderID_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(606, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 24);
            this.label2.TabIndex = 36;
            this.label2.Text = "Fatura numarası ile ara";
            // 
            // dgvSearch
            // 
            this.dgvSearch.AllowUserToAddRows = false;
            this.dgvSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSearch.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSearch.BackgroundColor = System.Drawing.Color.White;
            this.dgvSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearch.Location = new System.Drawing.Point(12, 91);
            this.dgvSearch.Name = "dgvSearch";
            this.dgvSearch.ReadOnly = true;
            this.dgvSearch.RowHeadersVisible = false;
            this.dgvSearch.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.dgvSearch.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            this.dgvSearch.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.dgvSearch.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Blue;
            this.dgvSearch.RowTemplate.Height = 24;
            this.dgvSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearch.Size = new System.Drawing.Size(1052, 451);
            this.dgvSearch.TabIndex = 37;
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(148, 563);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(320, 29);
            this.txtTotal.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(6, 564);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 24);
            this.label3.TabIndex = 39;
            this.label3.Text = "Fatura Tutarı:";
            // 
            // txtpaied
            // 
            this.txtpaied.Location = new System.Drawing.Point(148, 598);
            this.txtpaied.Name = "txtpaied";
            this.txtpaied.ReadOnly = true;
            this.txtpaied.Size = new System.Drawing.Size(320, 29);
            this.txtpaied.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(6, 599);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 24);
            this.label4.TabIndex = 41;
            this.label4.Text = "Ödenen Miktar:";
            // 
            // txtReminder
            // 
            this.txtReminder.Location = new System.Drawing.Point(148, 633);
            this.txtReminder.Name = "txtReminder";
            this.txtReminder.ReadOnly = true;
            this.txtReminder.Size = new System.Drawing.Size(320, 29);
            this.txtReminder.TabIndex = 42;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(6, 634);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 24);
            this.label5.TabIndex = 43;
            this.label5.Text = "Kalan Miktar:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblName.ForeColor = System.Drawing.Color.Blue;
            this.lblName.Location = new System.Drawing.Point(7, 21);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(116, 24);
            this.lblName.TabIndex = 44;
            this.lblName.Text = "Müşteri Adı:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(147, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(321, 29);
            this.txtName.TabIndex = 45;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.lblName);
            this.groupBox2.Location = new System.Drawing.Point(0, 668);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(766, 59);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            // 
            // btnReturnAll
            // 
            this.btnReturnAll.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnReturnAll.Appearance.Options.UseFont = true;
            this.btnReturnAll.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnReturnAll.ImageOptions.Image")));
            this.btnReturnAll.Location = new System.Drawing.Point(683, 684);
            this.btnReturnAll.Name = "btnReturnAll";
            this.btnReturnAll.Size = new System.Drawing.Size(244, 39);
            this.btnReturnAll.TabIndex = 7;
            this.btnReturnAll.Text = "Fatura tümüyle iade";
            this.btnReturnAll.Click += new System.EventHandler(this.btnReturnAll_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudQty);
            this.groupBox3.Controls.Add(this.btnReturnitemOnly);
            this.groupBox3.Controls.Add(this.rbtnReturnQtyOnly);
            this.groupBox3.Controls.Add(this.rbtnReturnitem);
            this.groupBox3.Location = new System.Drawing.Point(474, 569);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(568, 89);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            // 
            // nudQty
            // 
            this.nudQty.DecimalPlaces = 2;
            this.nudQty.Location = new System.Drawing.Point(245, 60);
            this.nudQty.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudQty.Name = "nudQty";
            this.nudQty.Size = new System.Drawing.Size(208, 29);
            this.nudQty.TabIndex = 47;
            this.nudQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnReturnitemOnly
            // 
            this.btnReturnitemOnly.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnReturnitemOnly.Appearance.Options.UseFont = true;
            this.btnReturnitemOnly.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnReturnitemOnly.ImageOptions.Image")));
            this.btnReturnitemOnly.Location = new System.Drawing.Point(245, 15);
            this.btnReturnitemOnly.Name = "btnReturnitemOnly";
            this.btnReturnitemOnly.Size = new System.Drawing.Size(208, 39);
            this.btnReturnitemOnly.TabIndex = 46;
            this.btnReturnitemOnly.Text = "belirlen ürün iade";
            this.btnReturnitemOnly.Click += new System.EventHandler(this.btnReturnitemOnly_Click);
            // 
            // rbtnReturnQtyOnly
            // 
            this.rbtnReturnQtyOnly.AutoSize = true;
            this.rbtnReturnQtyOnly.ForeColor = System.Drawing.Color.Blue;
            this.rbtnReturnQtyOnly.Location = new System.Drawing.Point(6, 52);
            this.rbtnReturnQtyOnly.Name = "rbtnReturnQtyOnly";
            this.rbtnReturnQtyOnly.Size = new System.Drawing.Size(233, 27);
            this.rbtnReturnQtyOnly.TabIndex = 3;
            this.rbtnReturnQtyOnly.TabStop = true;
            this.rbtnReturnQtyOnly.Text = "Belirtilen bir miktarı iade";
            this.rbtnReturnQtyOnly.UseVisualStyleBackColor = true;
            // 
            // rbtnReturnitem
            // 
            this.rbtnReturnitem.AutoSize = true;
            this.rbtnReturnitem.ForeColor = System.Drawing.Color.Blue;
            this.rbtnReturnitem.Location = new System.Drawing.Point(6, 15);
            this.rbtnReturnitem.Name = "rbtnReturnitem";
            this.rbtnReturnitem.Size = new System.Drawing.Size(171, 27);
            this.rbtnReturnitem.TabIndex = 2;
            this.rbtnReturnitem.TabStop = true;
            this.rbtnReturnitem.Text = "Tam miktarı iade";
            this.rbtnReturnitem.UseVisualStyleBackColor = true;
            // 
            // Frm_Return
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1075, 726);
            this.Controls.Add(this.btnReturnAll);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txtReminder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtpaied);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOrderID);
            this.Controls.Add(this.dtpReturnDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Frm_Return";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İade Yöntemi";
            this.Load += new System.EventHandler(this.Frm_Return_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnBuy;
        private System.Windows.Forms.RadioButton rbtnSales;
        private System.Windows.Forms.DateTimePicker dtpReturnDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrderID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvSearch;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtpaied;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReminder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnReturnAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.SimpleButton btnReturnitemOnly;
        private System.Windows.Forms.RadioButton rbtnReturnQtyOnly;
        private System.Windows.Forms.RadioButton rbtnReturnitem;
        private System.Windows.Forms.NumericUpDown nudQty;
    }
}
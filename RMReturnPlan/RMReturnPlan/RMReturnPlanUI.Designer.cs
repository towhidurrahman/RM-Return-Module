namespace RMTransferPlan
{
    partial class RMTransferPlanUI
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
            this.groupBoxHeader = new System.Windows.Forms.GroupBox();
            this.cmbProductLine = new System.Windows.Forms.ComboBox();
            this.cmbProductGroup = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.dateTimePickerHead = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewShow = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPost = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxFooter = new System.Windows.Forms.GroupBox();
            this.groupBoxHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShow)).BeginInit();
            this.groupBoxFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxHeader
            // 
            this.groupBoxHeader.Controls.Add(this.cmbProductLine);
            this.groupBoxHeader.Controls.Add(this.cmbProductGroup);
            this.groupBoxHeader.Controls.Add(this.btnGo);
            this.groupBoxHeader.Controls.Add(this.dateTimePickerHead);
            this.groupBoxHeader.Controls.Add(this.label3);
            this.groupBoxHeader.Controls.Add(this.label2);
            this.groupBoxHeader.Controls.Add(this.label1);
            this.groupBoxHeader.Location = new System.Drawing.Point(12, 7);
            this.groupBoxHeader.Name = "groupBoxHeader";
            this.groupBoxHeader.Size = new System.Drawing.Size(989, 80);
            this.groupBoxHeader.TabIndex = 0;
            this.groupBoxHeader.TabStop = false;
            this.groupBoxHeader.Text = "Date Wise Raw Material Transfer";
            // 
            // cmbProductLine
            // 
            this.cmbProductLine.FormattingEnabled = true;
            this.cmbProductLine.Location = new System.Drawing.Point(106, 48);
            this.cmbProductLine.Name = "cmbProductLine";
            this.cmbProductLine.Size = new System.Drawing.Size(121, 21);
            this.cmbProductLine.TabIndex = 21;
            // 
            // cmbProductGroup
            // 
            this.cmbProductGroup.FormattingEnabled = true;
            this.cmbProductGroup.Location = new System.Drawing.Point(485, 17);
            this.cmbProductGroup.Name = "cmbProductGroup";
            this.cmbProductGroup.Size = new System.Drawing.Size(121, 21);
            this.cmbProductGroup.TabIndex = 20;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(559, 48);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(44, 23);
            this.btnGo.TabIndex = 19;
            this.btnGo.Text = ">>";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // dateTimePickerHead
            // 
            this.dateTimePickerHead.CustomFormat = "MM/dd/yyyy";
            this.dateTimePickerHead.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerHead.Location = new System.Drawing.Point(106, 19);
            this.dateTimePickerHead.Name = "dateTimePickerHead";
            this.dateTimePickerHead.Size = new System.Drawing.Size(125, 20);
            this.dateTimePickerHead.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(385, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Production Group:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Product Line:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Production Date:";
            // 
            // dataGridViewShow
            // 
            this.dataGridViewShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShow.Location = new System.Drawing.Point(14, 96);
            this.dataGridViewShow.Name = "dataGridViewShow";
            this.dataGridViewShow.Size = new System.Drawing.Size(992, 242);
            this.dataGridViewShow.TabIndex = 1;
            this.dataGridViewShow.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewShow_EditingControlShowing);
            this.dataGridViewShow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Column1_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 18);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(66, 28);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(110, 18);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(66, 28);
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(223, 19);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(66, 28);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "&Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPost
            // 
            this.btnPost.Location = new System.Drawing.Point(340, 19);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(66, 28);
            this.btnPost.TabIndex = 5;
            this.btnPost.Text = "&Post";
            this.btnPost.UseVisualStyleBackColor = true;
            this.btnPost.Click += new System.EventHandler(this.btnPost_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(918, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 28);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBoxFooter
            // 
            this.groupBoxFooter.Controls.Add(this.btnSave);
            this.groupBoxFooter.Controls.Add(this.btnClose);
            this.groupBoxFooter.Controls.Add(this.btnEdit);
            this.groupBoxFooter.Controls.Add(this.btnPost);
            this.groupBoxFooter.Controls.Add(this.btnPrint);
            this.groupBoxFooter.Location = new System.Drawing.Point(8, 344);
            this.groupBoxFooter.Name = "groupBoxFooter";
            this.groupBoxFooter.Size = new System.Drawing.Size(990, 52);
            this.groupBoxFooter.TabIndex = 7;
            this.groupBoxFooter.TabStop = false;
            // 
            // RMTransferPlanUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 405);
            this.Controls.Add(this.groupBoxFooter);
            this.Controls.Add(this.dataGridViewShow);
            this.Controls.Add(this.groupBoxHeader);
            this.Name = "RMTransferPlanUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RM Transfer to Production";
            this.Load += new System.EventHandler(this.RMTransferPlanUI_Load);
            this.groupBoxHeader.ResumeLayout(false);
            this.groupBoxHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShow)).EndInit();
            this.groupBoxFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxHeader;
        private System.Windows.Forms.Button btnGo;
        public System.Windows.Forms.DateTimePicker dateTimePickerHead;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView dataGridViewShow;
        private System.Windows.Forms.ComboBox cmbProductLine;
        private System.Windows.Forms.ComboBox cmbProductGroup;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBoxFooter;
    }
}


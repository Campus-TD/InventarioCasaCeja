﻿namespace InventarioCasaCeja
{
    partial class HistEntradasSalidas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exitButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.BSelRegistro = new System.Windows.Forms.Button();
            this.BelimHistorial = new System.Windows.Forms.Button();
            this.clientinfo = new System.Windows.Forms.TableLayoutPanel();
            this.tablaEntradasySalidas = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.BoxTipo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.clientinfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaEntradasySalidas)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.exitButton);
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1460, 821);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HISTORIAL DE ENTRADAS Y SALIDAS";
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.exitButton.Location = new System.Drawing.Point(1185, 20);
            this.exitButton.Margin = new System.Windows.Forms.Padding(4);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(259, 57);
            this.exitButton.TabIndex = 5;
            this.exitButton.Text = "SALIR (Esc)";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.clientinfo, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 85);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1443, 728);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.95652F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.04348F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 528F));
            this.tableLayoutPanel2.Controls.Add(this.BSelRegistro, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.BelimHistorial, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 645);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1435, 71);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // BSelRegistro
            // 
            this.BSelRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BSelRegistro.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.BSelRegistro.Location = new System.Drawing.Point(4, 4);
            this.BSelRegistro.Margin = new System.Windows.Forms.Padding(4);
            this.BSelRegistro.Name = "BSelRegistro";
            this.BSelRegistro.Size = new System.Drawing.Size(665, 63);
            this.BSelRegistro.TabIndex = 7;
            this.BSelRegistro.Text = "SEL. REGISTRO (ENTER)";
            this.BSelRegistro.UseVisualStyleBackColor = true;
            this.BSelRegistro.Click += new System.EventHandler(this.BSelRegistro_Click);
            // 
            // BelimHistorial
            // 
            this.BelimHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BelimHistorial.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.BelimHistorial.Location = new System.Drawing.Point(677, 4);
            this.BelimHistorial.Margin = new System.Windows.Forms.Padding(4);
            this.BelimHistorial.Name = "BelimHistorial";
            this.BelimHistorial.Size = new System.Drawing.Size(754, 63);
            this.BelimHistorial.TabIndex = 9;
            this.BelimHistorial.Text = "ELIM. HISTORIAL (F5)";
            this.BelimHistorial.UseVisualStyleBackColor = true;
            this.BelimHistorial.Click += new System.EventHandler(this.BelimHistorial_Click);
            // 
            // clientinfo
            // 
            this.clientinfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientinfo.ColumnCount = 1;
            this.clientinfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.clientinfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.clientinfo.Controls.Add(this.tablaEntradasySalidas, 0, 0);
            this.clientinfo.Location = new System.Drawing.Point(4, 4);
            this.clientinfo.Margin = new System.Windows.Forms.Padding(4);
            this.clientinfo.Name = "clientinfo";
            this.clientinfo.RowCount = 1;
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66616F));
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66616F));
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66616F));
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66616F));
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66767F));
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.clientinfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66767F));
            this.clientinfo.Size = new System.Drawing.Size(1435, 633);
            this.clientinfo.TabIndex = 0;
            // 
            // tablaEntradasySalidas
            // 
            this.tablaEntradasySalidas.AllowUserToAddRows = false;
            this.tablaEntradasySalidas.AllowUserToDeleteRows = false;
            this.tablaEntradasySalidas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tablaEntradasySalidas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tablaEntradasySalidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tablaEntradasySalidas.DefaultCellStyle = dataGridViewCellStyle2;
            this.tablaEntradasySalidas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaEntradasySalidas.Location = new System.Drawing.Point(3, 3);
            this.tablaEntradasySalidas.Name = "tablaEntradasySalidas";
            this.tablaEntradasySalidas.ReadOnly = true;
            this.tablaEntradasySalidas.RowHeadersVisible = false;
            this.tablaEntradasySalidas.RowHeadersWidth = 51;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.tablaEntradasySalidas.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.tablaEntradasySalidas.RowTemplate.Height = 24;
            this.tablaEntradasySalidas.RowTemplate.ReadOnly = true;
            this.tablaEntradasySalidas.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tablaEntradasySalidas.Size = new System.Drawing.Size(1429, 652);
            this.tablaEntradasySalidas.TabIndex = 0;
            this.tablaEntradasySalidas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tablaEntradasySalidas_KeyDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.01623F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.98377F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 376F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.BoxTipo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(596, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 37.17949F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62.82051F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(598, 90);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(220, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(269, 33);
            this.label2.TabIndex = 7;
            this.label2.Text = "TIPO (F1)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BoxTipo
            // 
            this.BoxTipo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BoxTipo.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.BoxTipo.FormattingEnabled = true;
            this.BoxTipo.Location = new System.Drawing.Point(220, 36);
            this.BoxTipo.Name = "BoxTipo";
            this.BoxTipo.Size = new System.Drawing.Size(269, 49);
            this.BoxTipo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 57);
            this.label1.TabIndex = 0;
            this.label1.Text = "FILTRAR POR";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HistEntradasSalidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 850);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "HistEntradasSalidas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HistEntradasSalidas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.clientinfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablaEntradasySalidas)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button BSelRegistro;
        private System.Windows.Forms.Button BelimHistorial;
        private System.Windows.Forms.TableLayoutPanel clientinfo;
        private System.Windows.Forms.DataGridView tablaEntradasySalidas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BoxTipo;
        private System.Windows.Forms.Label label1;
    }
}
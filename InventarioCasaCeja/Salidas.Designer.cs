
namespace InventarioCasaCeja
{
    partial class Salidas
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tabla = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtcodigo = new System.Windows.Forms.TextBox();
            this.finish = new System.Windows.Forms.Button();
            this.quitarProdButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.exit_button = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtfolio = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtfecha = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.boxsucursales = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSucOrig = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabla)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(3, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1071, 426);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "AGREGAR PRODUCTOS";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tabla, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 38);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1059, 381);
            this.tableLayoutPanel3.TabIndex = 20;
            // 
            // tabla
            // 
            this.tabla.AllowUserToAddRows = false;
            this.tabla.AllowUserToDeleteRows = false;
            this.tabla.AllowUserToOrderColumns = true;
            this.tabla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabla.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.tabla.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tabla.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabla.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabla.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.tabla.Location = new System.Drawing.Point(3, 100);
            this.tabla.MultiSelect = false;
            this.tabla.Name = "tabla";
            this.tabla.RowHeadersVisible = false;
            this.tabla.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.tabla.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tabla.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tabla.Size = new System.Drawing.Size(1053, 278);
            this.tabla.StandardTab = true;
            this.tabla.TabIndex = 5;
            this.tabla.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabla_CellDoubleClick);
            this.tabla.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabla_CellEndEdit);
            this.tabla.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.tabla_EditingControlShowing);
            this.tabla.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tabla_KeyDown);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.981F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.019F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 461F));
            this.tableLayoutPanel2.Controls.Add(this.txtcodigo, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.finish, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.quitarProdButton, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.63504F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.36496F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1053, 91);
            this.tableLayoutPanel2.TabIndex = 21;
            // 
            // txtcodigo
            // 
            this.txtcodigo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtcodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtcodigo.Location = new System.Drawing.Point(3, 35);
            this.txtcodigo.MaxLength = 255;
            this.txtcodigo.Name = "txtcodigo";
            this.txtcodigo.Size = new System.Drawing.Size(278, 50);
            this.txtcodigo.TabIndex = 0;
            this.txtcodigo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtcodigo_KeyDown);
            // 
            // finish
            // 
            this.finish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finish.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.finish.Location = new System.Drawing.Point(287, 35);
            this.finish.Name = "finish";
            this.finish.Size = new System.Drawing.Size(301, 50);
            this.finish.TabIndex = 4;
            this.finish.Text = "COMPLETAR SALIDA (F6)";
            this.finish.UseVisualStyleBackColor = true;
            this.finish.Click += new System.EventHandler(this.finish_Click);
            // 
            // quitarProdButton
            // 
            this.quitarProdButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.quitarProdButton.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.quitarProdButton.Location = new System.Drawing.Point(594, 35);
            this.quitarProdButton.Name = "quitarProdButton";
            this.quitarProdButton.Size = new System.Drawing.Size(456, 50);
            this.quitarProdButton.TabIndex = 16;
            this.quitarProdButton.Text = "QUITAR PRODUCTO (SUPR)";
            this.quitarProdButton.UseVisualStyleBackColor = true;
            this.quitarProdButton.Click += new System.EventHandler(this.quitarProdButton_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(280, 28);
            this.tableLayoutPanel5.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(165, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 28);
            this.label2.TabIndex = 17;
            this.label2.Text = "CATALOGO (F2)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 28);
            this.label4.TabIndex = 10;
            this.label4.Text = "CÓD. BARRAS  (F1)";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.exit_button);
            this.groupBox1.Controls.Add(this.tableLayoutPanel4);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1071, 139);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NUEVA SALIDA";
            // 
            // exit_button
            // 
            this.exit_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exit_button.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            this.exit_button.Location = new System.Drawing.Point(866, -6);
            this.exit_button.Margin = new System.Windows.Forms.Padding(2);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(194, 46);
            this.exit_button.TabIndex = 22;
            this.exit_button.Text = "SALIR (ESC)";
            this.exit_button.UseVisualStyleBackColor = true;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.38281F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.61719F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 387F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 364F));
            this.tableLayoutPanel4.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtfolio, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtfecha, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.boxsucursales, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtSucOrig, 3, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 38);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1059, 94);
            this.tableLayoutPanel4.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(209, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 30);
            this.label3.TabIndex = 24;
            this.label3.Text = "FECHA";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtfolio
            // 
            this.txtfolio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtfolio.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtfolio.Enabled = false;
            this.txtfolio.Location = new System.Drawing.Point(6, 36);
            this.txtfolio.MaxLength = 255;
            this.txtfolio.Name = "txtfolio";
            this.txtfolio.Size = new System.Drawing.Size(197, 50);
            this.txtfolio.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(6, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(180, 30);
            this.label5.TabIndex = 10;
            this.label5.Text = "FOLIO DE SALIDA";
            // 
            // txtfecha
            // 
            this.txtfecha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtfecha.BackColor = System.Drawing.SystemColors.Window;
            this.txtfecha.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtfecha.Enabled = false;
            this.txtfecha.Location = new System.Drawing.Point(209, 36);
            this.txtfecha.Name = "txtfecha";
            this.txtfecha.ShortcutsEnabled = false;
            this.txtfecha.Size = new System.Drawing.Size(92, 50);
            this.txtfecha.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(307, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 30);
            this.label6.TabIndex = 12;
            this.label6.Text = "SUCURSAL DESTINO (F5)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // boxsucursales
            // 
            this.boxsucursales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxsucursales.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.boxsucursales.FormattingEnabled = true;
            this.boxsucursales.Location = new System.Drawing.Point(307, 36);
            this.boxsucursales.Name = "boxsucursales";
            this.boxsucursales.Size = new System.Drawing.Size(381, 53);
            this.boxsucursales.TabIndex = 25;
            this.boxsucursales.KeyDown += new System.Windows.Forms.KeyEventHandler(this.boxsucursales_keydown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(694, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 30);
            this.label1.TabIndex = 26;
            this.label1.Text = "SUCURSAL DE ORIGEN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSucOrig
            // 
            this.txtSucOrig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSucOrig.BackColor = System.Drawing.SystemColors.Window;
            this.txtSucOrig.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSucOrig.Enabled = false;
            this.txtSucOrig.Location = new System.Drawing.Point(694, 36);
            this.txtSucOrig.Name = "txtSucOrig";
            this.txtSucOrig.ShortcutsEnabled = false;
            this.txtSucOrig.Size = new System.Drawing.Size(359, 50);
            this.txtSucOrig.TabIndex = 27;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1077, 574);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // Salidas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 598);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Salidas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Salidas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Salidas_Load);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabla)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView tabla;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtcodigo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button finish;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtfolio;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox boxsucursales;
        private System.Windows.Forms.TextBox txtfecha;
        private System.Windows.Forms.TextBox txtSucOrig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button quitarProdButton;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
    }
}
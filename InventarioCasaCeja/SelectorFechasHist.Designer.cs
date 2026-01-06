using System.Windows.Forms;
using System;

namespace InventarioCasaCeja
{
    partial class SelectorFechasHist
    {
        private System.ComponentModel.IContainer components = null;
        private DateTimePicker dateTimePickerInicio;
        private DateTimePicker dateTimePickerFin;
        private Button btnAceptar;
        private Button btnCancelar;
        private Label labelTitulo;
        private Label labelInicio;
        private Label labelFin;
        private Label labelError;
        private GroupBox groupBoxFechas;
        private GroupBox groupBoxAcciones;
        private Button btnHoy;
        private Button btnUltimos7Dias;
        private Button btnUltimoMes;
        private Button btnTodoElAño;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dateTimePickerInicio = new DateTimePicker();
            this.dateTimePickerFin = new DateTimePicker();
            this.btnAceptar = new Button();
            this.btnCancelar = new Button();
            this.labelTitulo = new Label();
            this.labelInicio = new Label();
            this.labelFin = new Label();
            this.labelError = new Label();
            this.groupBoxFechas = new GroupBox();
            this.groupBoxAcciones = new GroupBox();
            this.btnHoy = new Button();
            this.btnUltimos7Dias = new Button();
            this.btnUltimoMes = new Button();
            this.btnTodoElAño = new Button();
            this.groupBoxFechas.SuspendLayout();
            this.groupBoxAcciones.SuspendLayout();
            this.SuspendLayout();

            // 
            // FormSelectorFechas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Seleccionar Rango de Fechas - Casa Ceja";
            this.BackColor = System.Drawing.Color.White;

            // 
            // labelTitulo
            // 
            this.labelTitulo.Location = new System.Drawing.Point(12, 15);
            this.labelTitulo.Size = new System.Drawing.Size(456, 35);
            this.labelTitulo.Text = "📅 SELECCIONAR RANGO DE FECHAS PARA REPORTE";
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTitulo.ForeColor = System.Drawing.Color.DarkBlue;

            // 
            // groupBoxFechas
            // 
            this.groupBoxFechas.Location = new System.Drawing.Point(20, 60);
            this.groupBoxFechas.Size = new System.Drawing.Size(440, 120);
            this.groupBoxFechas.Text = "Rango de Fechas";
            this.groupBoxFechas.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxFechas.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.groupBoxFechas.Controls.Add(this.labelInicio);
            this.groupBoxFechas.Controls.Add(this.dateTimePickerInicio);
            this.groupBoxFechas.Controls.Add(this.labelFin);
            this.groupBoxFechas.Controls.Add(this.dateTimePickerFin);

            // 
            // labelInicio
            // 
            this.labelInicio.Location = new System.Drawing.Point(20, 30);
            this.labelInicio.Size = new System.Drawing.Size(120, 25);
            this.labelInicio.Text = "🗓️ Fecha Inicio:";
            this.labelInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelInicio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // dateTimePickerInicio
            // 
            this.dateTimePickerInicio.Location = new System.Drawing.Point(150, 30);
            this.dateTimePickerInicio.Size = new System.Drawing.Size(260, 25);
            this.dateTimePickerInicio.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dateTimePickerInicio.Format = DateTimePickerFormat.Short;
            this.dateTimePickerInicio.MinDate = new DateTime(2020, 1, 1);
            this.dateTimePickerInicio.MaxDate = DateTime.Now.Date;

            // 
            // labelFin
            // 
            this.labelFin.Location = new System.Drawing.Point(20, 70);
            this.labelFin.Size = new System.Drawing.Size(120, 25);
            this.labelFin.Text = "📅 Fecha Fin:";
            this.labelFin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelFin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // 
            // dateTimePickerFin
            // 
            this.dateTimePickerFin.Location = new System.Drawing.Point(150, 70);
            this.dateTimePickerFin.Size = new System.Drawing.Size(260, 25);
            this.dateTimePickerFin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dateTimePickerFin.Format = DateTimePickerFormat.Short;
            this.dateTimePickerFin.MinDate = new DateTime(2020, 1, 1);
            this.dateTimePickerFin.MaxDate = DateTime.Now.Date;

            // 
            // groupBoxAcciones
            // 
            this.groupBoxAcciones.Location = new System.Drawing.Point(20, 190);
            this.groupBoxAcciones.Size = new System.Drawing.Size(440, 80);
            this.groupBoxAcciones.Text = "Rangos Rápidos";
            this.groupBoxAcciones.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxAcciones.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.groupBoxAcciones.Controls.Add(this.btnHoy);
            this.groupBoxAcciones.Controls.Add(this.btnUltimos7Dias);
            this.groupBoxAcciones.Controls.Add(this.btnUltimoMes);
            this.groupBoxAcciones.Controls.Add(this.btnTodoElAño);

            // 
            // btnHoy
            // 
            this.btnHoy.Location = new System.Drawing.Point(15, 25);
            this.btnHoy.Size = new System.Drawing.Size(90, 35);
            this.btnHoy.Text = "Hoy (F1)";
            this.btnHoy.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnHoy.UseVisualStyleBackColor = true;
            this.btnHoy.Click += new System.EventHandler(this.btnHoy_Click);

            // 
            // btnUltimos7Dias
            // 
            this.btnUltimos7Dias.Location = new System.Drawing.Point(115, 25);
            this.btnUltimos7Dias.Size = new System.Drawing.Size(90, 35);
            this.btnUltimos7Dias.Text = "7 Días (F2)";
            this.btnUltimos7Dias.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnUltimos7Dias.UseVisualStyleBackColor = true;
            this.btnUltimos7Dias.Click += new System.EventHandler(this.btnUltimos7Dias_Click);

            // 
            // btnUltimoMes
            // 
            this.btnUltimoMes.Location = new System.Drawing.Point(215, 25);
            this.btnUltimoMes.Size = new System.Drawing.Size(90, 35);
            this.btnUltimoMes.Text = "1 Mes (F3)";
            this.btnUltimoMes.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnUltimoMes.UseVisualStyleBackColor = true;
            this.btnUltimoMes.Click += new System.EventHandler(this.btnUltimoMes_Click);

            // 
            // btnTodoElAño
            // 
            this.btnTodoElAño.Location = new System.Drawing.Point(315, 25);
            this.btnTodoElAño.Size = new System.Drawing.Size(90, 35);
            this.btnTodoElAño.Text = "Año (F4)";
            this.btnTodoElAño.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnTodoElAño.UseVisualStyleBackColor = true;
            this.btnTodoElAño.Click += new System.EventHandler(this.btnTodoElAño_Click);

            // 
            // labelError
            // 
            this.labelError.Location = new System.Drawing.Point(20, 280);
            this.labelError.Size = new System.Drawing.Size(440, 20);
            this.labelError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelError.Visible = false;

            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(280, 310);
            this.btnAceptar.Size = new System.Drawing.Size(90, 30);
            this.btnAceptar.Text = "✅ Aceptar";
            this.btnAceptar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAceptar.BackColor = System.Drawing.Color.LightGreen;
            this.btnAceptar.UseVisualStyleBackColor = false;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);

            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(380, 310);
            this.btnCancelar.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar.Text = "❌ Cancelar";
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCancelar.BackColor = System.Drawing.Color.LightCoral;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // Agregar controles al formulario
            this.Controls.Add(this.labelTitulo);
            this.Controls.Add(this.groupBoxFechas);
            this.Controls.Add(this.groupBoxAcciones);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);

            this.groupBoxFechas.ResumeLayout(false);
            this.groupBoxAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
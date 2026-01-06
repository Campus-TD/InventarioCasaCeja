using System;
using System.Windows.Forms;

namespace InventarioCasaCeja
{
    public partial class SelectorFechasHist : Form
    {
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public bool FechasSeleccionadas { get; private set; }

        public SelectorFechasHist()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            this.FechasSeleccionadas = false;

            // Configurar límites ANTES de establecer valores
            dateTimePickerInicio.MinDate = new DateTime(2020, 1, 1); // Fecha mínima razonable
            dateTimePickerInicio.MaxDate = DateTime.Now.Date;
            dateTimePickerFin.MinDate = new DateTime(2020, 1, 1);
            dateTimePickerFin.MaxDate = DateTime.Now.Date;

            // Establecer fechas por defecto (último mes) DESPUÉS de configurar límites
            dateTimePickerInicio.Value = DateTime.Now.AddMonths(-1).Date;
            dateTimePickerFin.Value = DateTime.Now.Date;

            // Eventos para validación
            dateTimePickerInicio.ValueChanged += ValidarFechas;
            dateTimePickerFin.ValueChanged += ValidarFechas;
        }

        private void ValidarFechas(object sender, EventArgs e)
        {
            if (dateTimePickerInicio.Value > dateTimePickerFin.Value)
            {
                labelError.Text = "⚠️ La fecha de inicio no puede ser mayor a la fecha final";
                labelError.Visible = true;
                btnAceptar.Enabled = false;
            }
            else
            {
                labelError.Visible = false;
                btnAceptar.Enabled = true;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dateTimePickerInicio.Value.Date <= dateTimePickerFin.Value.Date)
                {
                    FechaInicio = dateTimePickerInicio.Value.Date;
                    FechaFin = dateTimePickerFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir todo el día final
                    FechasSeleccionadas = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("La fecha de inicio debe ser menor o igual a la fecha final.",
                                  "Fechas inválidas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar las fechas: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FechasSeleccionadas = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUltimoMes_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now.AddMonths(-1).Date;
                DateTime fechaFin = DateTime.Now.Date;

                // Validar que las fechas estén dentro del rango permitido
                if (fechaInicio >= dateTimePickerInicio.MinDate && fechaInicio <= dateTimePickerInicio.MaxDate)
                {
                    dateTimePickerInicio.Value = fechaInicio;
                }
                else
                {
                    dateTimePickerInicio.Value = dateTimePickerInicio.MinDate;
                }

                if (fechaFin >= dateTimePickerFin.MinDate && fechaFin <= dateTimePickerFin.MaxDate)
                {
                    dateTimePickerFin.Value = fechaFin;
                }
                else
                {
                    dateTimePickerFin.Value = dateTimePickerFin.MaxDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al establecer el rango del último mes: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUltimos7Dias_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now.AddDays(-7).Date;
                DateTime fechaFin = DateTime.Now.Date;

                // Validar que las fechas estén dentro del rango permitido
                if (fechaInicio >= dateTimePickerInicio.MinDate && fechaInicio <= dateTimePickerInicio.MaxDate)
                {
                    dateTimePickerInicio.Value = fechaInicio;
                }
                else
                {
                    dateTimePickerInicio.Value = dateTimePickerInicio.MinDate;
                }

                dateTimePickerFin.Value = fechaFin;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al establecer el rango de 7 días: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHoy_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime hoy = DateTime.Now.Date;
                dateTimePickerInicio.Value = hoy;
                dateTimePickerFin.Value = hoy;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al establecer la fecha de hoy: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTodoElAño_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime inicioAño = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime finAño = DateTime.Now.Date;

                // Validar que las fechas estén dentro del rango permitido
                if (inicioAño >= dateTimePickerInicio.MinDate && inicioAño <= dateTimePickerInicio.MaxDate)
                {
                    dateTimePickerInicio.Value = inicioAño;
                }
                else
                {
                    dateTimePickerInicio.Value = dateTimePickerInicio.MinDate;
                }

                dateTimePickerFin.Value = finAño;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al establecer el rango del año: {ex.Message}",
                              "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None)
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        btnAceptar.PerformClick();
                        return true;
                    case Keys.Escape:
                        btnCancelar.PerformClick();
                        return true;
                    case Keys.F1:
                        btnHoy.PerformClick();
                        return true;
                    case Keys.F2:
                        btnUltimos7Dias.PerformClick();
                        return true;
                    case Keys.F3:
                        btnUltimoMes.PerformClick();
                        return true;
                    case Keys.F4:
                        btnTodoElAño.PerformClick();
                        return true;
                }
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
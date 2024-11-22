using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static ControlDeInventario.ControlDeInventario.ControlInventario;

namespace ControlDeInventario.ControlDeInventario
{
    public partial class ControlInventario : Form
    {
        // Declaración de la clase para el control de inventario
        public class Producto
        {
            public int IdProducto { get; private set; }
            public string NombreProducto { get; }
            public int Cantidad { get; }
            public double Precio { get; }
            public string Categoria { get; } // Agregar propiedad Categoria

            public Producto(int id, string nombre, int cantidad, double precio, string categoria)
            {
                IdProducto = id;
                NombreProducto = nombre;
                Cantidad = cantidad;
                Precio = precio;
                Categoria = categoria;
            }
        }

        // Creación de la lista de objetos de inventario
        List<Producto> Productos = new List<Producto>() { };

        // Inicializar todos los controles de objetos
        public ControlInventario()
        {
            InitializeComponent();
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void ControlInventario_Load(object sender, EventArgs e) { }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void AgregarProducto()
        {
            try
            {
                // Validar que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                    string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                    string.IsNullOrWhiteSpace(txtCantidad.Text) ||
                    cmbCategoria.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parsear datos
                string nuevoNombre = txtNombre.Text;
                int nuevoCódigo = int.Parse(txtCodigo.Text);
                string nuevaMarca = txtMarca.Text;
                string nuevaCategoria = cmbCategoria.SelectedItem.ToString();
                decimal nuevoPrecio = decimal.Parse(txtPrecio.Text);
                int nuevaCantidad = int.Parse(txtCantidad.Text);

                Productos.Add(new Producto(nuevoCódigo, nuevoNombre, nuevaCantidad, (double)nuevoPrecio, nuevaCategoria));
                ActualizarDataGridView();
                LimpiarCampos();
                CuentaProductosCategoria();
                CargarDatosEnGrafico();
            }
            catch (FormatException)
            {
                MessageBox.Show("Por favor, asegúrese de ingresar valores numéricos en los campos 'Código', 'Precio' y 'Cantidad'.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarDataGridView()
        {
            dgvInventario.DataSource = null;
            dgvInventario.DataSource = Productos;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int i = dgvInventario.CurrentCell.RowIndex;
                Productos.RemoveAt(i);
                ActualizarDataGridView();
                ActualizarMensajeEstado();
                CuentaProductosCategoria();
                CargarDatosEnGrafico();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Seleccione una fila válida para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void agregarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AgregarProducto();
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuEliminarProduco_Click(object sender, EventArgs e)
        {
            if (dgvInventario.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int i = dgvInventario.CurrentCell.RowIndex;
                Productos.RemoveAt(i);
                ActualizarDataGridView();
                CuentaProductosCategoria();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Seleccione una fila válida para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtMarca.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbTalla.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
        }

        private void CuentaProductosCategoria()
        {
            if (!Productos.Any())
            {
                cmbCategoria.Text = "No hay productos en el inventario.";
                return;
            }

            var productosPorCategoria = Productos
                  .GroupBy(p => p.Categoria)
                  .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                  .ToList();

            string mensaje = "";
            foreach (var categoria in productosPorCategoria)
            {
                mensaje += $"{categoria.Categoria}: {categoria.Cantidad}" + Environment.NewLine;
            }

            cmbCategoria.Text = mensaje;
        }

        private void CargarDatosEnGrafico()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Cantidad de Productos por Categoría");
            chart1.ChartAreas[0].AxisX.Title = "Categorías";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Title = "Cantidad";

            Series serie = new Series("ProductosPorCategoria")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            var productosPorCategoria = Productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                .ToList();

            foreach (var categoria in productosPorCategoria)
            {
                serie.Points.AddXY(categoria.Categoria, categoria.Cantidad);
            }

            chart1.Series.Add(serie);
            chart1.Invalidate();
        }

        private void ActualizarMensajeEstado()
        {
            int totalCantidad = Productos.Sum(p => p.Cantidad);
            toolStripStatusLabel1.Text = $"Número de productos en inventario: {totalCantidad}";
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void dgvInventario_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void archivoToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void tlsEliminar_Click(object sender, EventArgs e) { }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e) { }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }
    }
}


/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static ControlDeInventario.ControlDeInventario.ControlInventario;

namespace ControlDeInventario.ControlDeInventario
{
    public partial class ControlInventario : Form
    {

        //Declaración de la clase para el control de inventario
        public class Producto
        {
            public int IdProducto { get; private set; }
            public string NombreProducto { get; }
            public int Cantidad { get; }
            public double Precio { get; }
            public string Categoria { get; } // Agregar propiedad Categoria

            //Constructor para inicializar los campos de producto, cantidad y precio
            public Producto(int id, string nombre, int cantidad, double precio, string categoria)
            {
                IdProducto = id;
                NombreProducto = nombre;
                Cantidad = cantidad;
                Precio = precio;
                Categoria = categoria; // Inicializar propiedad Categoria
            }
        }

        //creación de la lista de objetos de inventario

        List<Producto> Productos = new List<Producto>() { };

        //Inicializar todos los controles de objetos

        public ControlInventario()
        {
            InitializeComponent();
            CargarDatosEnGrafico();
            ActualizarMensajeEstado();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ActualizarMensajeEstado();
            CuentaProductosCategoria();

        }

        private void ControlInventario_Load(object sender, EventArgs e) { }

        //Agregar datos con botón
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
        }

        private void AgregarProducto()
        {
            string nuevoNombre = txtNombre.Text;
            int nuevoCódigo = int.Parse(txtCodigo.Text);
            string nuevaMarca = txtMarca.Text;
            string nuevaCategoria = cmbCategoria.SelectedItem.ToString();
            decimal nuevoPrecio = decimal.Parse(txtPrecio.Text);
            int nuevaCantidad = int.Parse(txtCantidad.Text);

            Productos.Add(new Producto(nuevoCódigo, nuevoNombre, nuevaCantidad, (double)nuevoPrecio, nuevaCategoria));
            ActualizarDataGridView();
            LimpiarCampos();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();
        }

        //Actualizar datos en DataGridView

        private void ActualizarDataGridView()
        {
            dgvInventario.DataSource = null;
            dgvInventario.DataSource = Productos;
        }

        //Eliminar datos de con botón

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int i = dgvInventario.CurrentCell.RowIndex;
            Productos.RemoveAt(i);
            ActualizarDataGridView();
            ActualizarMensajeEstado();
            CuentaProductosCategoria();
            CargarDatosEnGrafico();
        }

        //AgregarDatos con menú

        private void agregarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AgregarProducto();
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Eliminar datos con menú
        private void mnuEliminarProduco_Click(object sender, EventArgs e)
        {
            int i = dgvInventario.CurrentCell.RowIndex;
            Productos.RemoveAt(i);
            ActualizarDataGridView();
            CuentaProductosCategoria();
        }

        //Limpiar contenido de los controles

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtMarca.Clear();
            cmbCategoria.SelectedIndex = -1;
            cmbTalla.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
        }

        //Imprimir datos en la pestaña 1/caja de texto

        private void CuentaProductosCategoria()
        {
            var productosPorCategoria = Productos
                  .GroupBy(p => p.Categoria)
                  .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                  .ToList();

            string mensaje = "";
            foreach (var categoria in productosPorCategoria)
            {
                mensaje += $"{categoria.Categoria}: {categoria.Cantidad}" + Environment.NewLine;
            }

            txtProductosPorCategoria.Text = mensaje;
        }

        //Crear gráfico en la pestaña 2

        private void CargarDatosEnGrafico()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.Titles.Add("Cantidad de Productos por Categoría");
            chart1.ChartAreas[0].AxisX.Title = "Categorías";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Title = "Cantidad";
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisY.Interval = 1;

            Series serie = new Series("ProductosPorCategoria")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            var productosPorCategoria = Productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                .ToList();

            foreach (var categoria in productosPorCategoria)
            {
                serie.Points.AddXY(categoria.Categoria, categoria.Cantidad);
            }

            chart1.Series.Clear();
            chart1.Series.Add(serie);
            chart1.Invalidate();
        }

        //Imprimir mensaje en la barra de estado

        private void ActualizarMensajeEstado()
        {
            int totalCantidad = 0;
            foreach (var producto in Productos)
            {
                totalCantidad += producto.Cantidad;
            }

            toolStripStatusLabel1.Text = $"Número de productos en inventario: {totalCantidad}";
        }
        private void label1_Click(object sender, EventArgs e)
        {}
        private void dgvInventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {}

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        { }
        private void label2_Click(object sender, EventArgs e)
        { }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { }
        private void tlsEliminar_Click(object sender, EventArgs e)
        { }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        { }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }
    }
}
*/
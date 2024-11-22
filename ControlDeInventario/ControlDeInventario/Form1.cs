using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlDeInventario;


namespace ControlDeInventario
    {
        public partial class FrmInicioSesion : Form
        {
            // Declaración de la clase para inicio de sesión
            public class InicioSesion
            {
                public int IdUsuario { get; private set; }
                public string Contra { get; }

                // Constructor para inicializar los campos de usuario y contraseña
                public InicioSesion(int id, string contra)
                {
                    IdUsuario = id;
                    Contra = contra;
                }
            }

            // Lista de usuarios registrados
            private List<InicioSesion> Usuarios = new List<InicioSesion>();

            // Inicializar los controles de objetos
            public FrmInicioSesion()
            {
                InitializeComponent();
            }

            private void btnIngresar_Click(object sender, EventArgs e)
            {
                string usuario = txtId.Text;
                string contra = txtContra.Text;

                // Validar campos vacíos
                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contra))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar si ID es un número válido
                if (!int.TryParse(usuario, out int idUsuario))
                {
                    MessageBox.Show("El ID del usuario debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar si el usuario existe
                if (Usuarios.Any(u => u.IdUsuario == idUsuario && u.Contra == contra))
                {
                    MessageBox.Show($"Bienvenido, {usuario}!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Aquí puedes redirigir a la siguiente pantalla o funcionalidad
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            private void btnCancelar_Click(object sender, EventArgs e)
            {
                txtId.Clear();
                txtContra.Clear();
                txtId.Focus();
            }

            private void btnRegistrar_Click(object sender, EventArgs e)
            {
                string nuevoId = txtId.Text;
                string nuevaContra = txtContra.Text;

                // Validar campos vacíos
                if (string.IsNullOrEmpty(nuevoId) || string.IsNullOrEmpty(nuevaContra))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar si ID es un número válido
                if (!int.TryParse(nuevoId, out int idUsuario))
                {
                    MessageBox.Show("El ID del usuario debe ser un número válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validar si el usuario ya existe
                if (Usuarios.Any(u => u.IdUsuario == idUsuario))
                {
                    MessageBox.Show($"El usuario '{nuevoId}' ya existe. Intente con otro ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Registrar nuevo usuario
                try
                {
                    Usuarios.Add(new InicioSesion(idUsuario, nuevaContra)); // Mejor usar contraseñas cifradas aquí
                    MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Limpiar campos
                txtId.Clear();
                txtContra.Clear();
            }

            private void btnVolver_Click(object sender, EventArgs e)
            {
                // Limpiar campos
                txtId.Clear();
                txtContra.Clear();
            }
        }
    }



/*public partial class FrmInicioSesion : Form
{
        // Declaración de la clase para inicio de sesión
        public class InicioSesion
{
    public int IdUsuario { get; private set; }
    public string Contra { get; }

    // Constructor para inicializar los campos de usuario y contraseña
    public InicioSesion(int id, string contra)
    {
        IdUsuario = id;
        Contra = contra;
    }
}

// Lista de usuarios registrados
private List<InicioSesion> Usuarios = new List<InicioSesion>();

// Inicializar los controles de objetos
public FrmInicioSesion()
{
    InitializeComponent();

}

private void btnIngresar_Click(object sender, EventArgs e)
{
    string usuario = txtId.Text;
    string contra = txtContra.Text;

    // Validar campos vacíos
    if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contra))
    {
        MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    // Validar si el usuario existe
    if (Usuarios.Any(u => u.IdUsuario.ToString() == usuario && u.Contra == contra))
    {
        MessageBox.Show($"Bienvenido, {usuario}!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        // Aquí puedes redirigir a la siguiente pantalla o funcionalidad
    }
    else
    {
        MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void btnCancelar_Click(object sender, EventArgs e)
{
    txtId.Clear();
    txtContra.Clear();
    txtId.Focus();
}

private void btnRegistrar_Click(object sender, EventArgs e)
{
    string nuevoId = txtId.Text;
    string nuevaContra = txtContra.Text;

    // Validar campos vacíos
    if (string.IsNullOrEmpty(nuevoId) || string.IsNullOrEmpty(nuevaContra))
    {
        MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    // Validar si el usuario ya existe
    if (Usuarios.Any(u => u.IdUsuario.ToString() == nuevoId))
    {
        MessageBox.Show($"El usuario '{nuevoId}' ya existe. Intente con otro ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    // Registrar nuevo usuario
    Usuarios.Add(new InicioSesion(int.Parse(nuevoId), nuevaContra));
    MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

    // Limpiar campos y volver a inicio de sesión
    txtId.Clear();
    txtContra.Clear();
    gpbIniciarSesion.Visible = true;
}

private void btnVolver_Click(object sender, EventArgs e)
{
    // Limpiar campos
    txtId.Clear();
    txtContra.Clear();
    gpbIniciarSesion.Visible = true;
}
    }*/
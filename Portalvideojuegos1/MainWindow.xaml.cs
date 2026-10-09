using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Portalvideojuegos1
{
    public partial class LoginWindow : Window
    {
        // Almacén dinámico de credenciales en memoria (Usuario, Contraseña)
        private static List<string[]> usuariosValidos = new List<string[]>()
        {
            new string[] { "admin", "1234" },
            new string[] { "uriel", "password" },
            new string[] { "user", "user123" }
        };

        private int intentosFallidos = 0;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Password.Trim();

            // TC04: Campos en blanco
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Introduzca datos en todos los campos.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TC05 / TC07: Verificación de bloqueos por 3 intentos fallidos
            if (intentosFallidos >= 3)
            {
                MessageBox.Show("Se ha superado el número de intentos. El usuario se encuentra bloqueado temporalmente.", "Usuario Bloqueado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool usuarioEncontrado = false;
            bool loginExitoso = false;

            foreach (var cred in usuariosValidos)
            {
                if (cred[0] == usuario)
                {
                    usuarioEncontrado = true;
                    if (cred[1] == password)
                    {
                        loginExitoso = true;
                    }
                    break;
                }
            }

            if (loginExitoso)
            {
                MessageBox.Show("¡Inicio de sesión exitoso!", "Bienvenido", MessageBoxButton.OK, MessageBoxImage.Information);
                intentosFallidos = 0;

                // Abrir la ventana principal (el portal de videojuegos)
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else if (usuarioEncontrado)
            {
                // TC02: Contraseña incorrecta
                intentosFallidos++;
                int restantes = 3 - intentosFallidos;
                if (restantes > 0)
                {
                    MessageBox.Show($"La contraseña introducida no es correcta. Intentos restantes: {restantes}", "Error de Autenticación", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show("La contraseña introducida no es correcta. Se bloquea temporalmente el usuario por seguridad.", "Cuenta Bloqueada", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // TC03: Usuario no existe con opción directa de registro
                MessageBoxResult resultado = MessageBox.Show(
                    "El usuario introducido no existe. ¿Desea registrarse con estas credenciales?",
                    "Usuario no encontrado",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    usuariosValidos.Add(new string[] { usuario, password });
                    MessageBox.Show("¡Usuario registrado con éxito! Ya puede iniciar sesión.", "Registro Completado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, escriba un usuario y una contraseña para registrarse.", "Campos vacíos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Comprobar si ya existe
            foreach (var cred in usuariosValidos)
            {
                if (cred[0] == usuario)
                {
                    MessageBox.Show("El nombre de usuario ya existe. Elija otro.", "Error de Registro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Añadir nuevo usuario
            usuariosValidos.Add(new string[] { usuario, password });
            MessageBox.Show("¡Usuario registrado correctamente! Ya puede iniciar sesión.", "Registro Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            // TC06: Salir de la aplicación
            Application.Current.Shutdown();
        }
    }
}
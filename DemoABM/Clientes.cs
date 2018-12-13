using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoABM
{
    
    public partial class Clientes : Form
    {
        DemoBDEntities objCliente;
        Boolean bandera;
        public Clientes()
        {
            InitializeComponent();
            cargaGrid();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtDNI.Text);
                objCliente = new DemoBDEntities();
                Cliente buscacli = (from a in objCliente.Clientes
                                    where a.IdDNI == id
                                    select a).SingleOrDefault();
                if( bandera==false)
                {
                    buscacli.Nombres = txtNombre.Text;
                    buscacli.Apellidos = txtApellido.Text;
                    buscacli.FecNacimiento = dtFecNac.Value.Date;
                    buscacli.RutaImagen = txtRutaImg.Text;
                    objCliente.SaveChanges();
                    MessageBox.Show("Registro Actualizado");
                    DesHabilitar();
                    cargaGrid();
                    limpiar();
                   

                }
                
                //validar que ingrese numero en dni

                else if (buscacli == null && bandera == true)
                {

                    //validar que nombre y apellido no sean nulos
                    Cliente newCli = new Cliente()
                    {
                        IdDNI = int.Parse(txtDNI.Text),
                        Nombres = txtNombre.Text,
                        Apellidos = txtApellido.Text,
                        FecNacimiento = dtFecNac.Value.Date,
                        RutaImagen = txtRutaImg.Text
                    };
                    //agregamos a la base de datos
                    objCliente.Clientes.Add(newCli);
                    objCliente.SaveChanges();
                    MessageBox.Show("Registro guardado correctamente");
                    limpiar();
                    cargaGrid();
                    DesHabilitar();
                }
                else
                {
                    MessageBox.Show("El DNI ingresado ya existe, por favor verifiquelo");
                    txtDNI.Focus();
                }
                     
            }
            catch (Exception error)
            {
                MessageBox.Show("Se produjo un error " + error);
            }
            
        }
        public void limpiar()
        {
            txtDNI.Text = ""; txtApellido.Text = ""; txtDNI.Text = ""; txtNombre.Text = ""; txtRutaImg.Text = "";
            pbImagen.Image = null;
        }
        public void Habilitar()
        {
            txtApellido.Enabled = true;  txtNombre.Enabled = true; txtRutaImg.Enabled = true;
            dtFecNac.Enabled = true;
            btnGuardar.Enabled = true; btnCancelar.Enabled = true;
            btnNuevo.Enabled = false;


        }
        public void DesHabilitar()
        {
            txtApellido.Enabled = false;  txtNombre.Enabled = false; txtRutaImg.Enabled = false; txtDNI.Enabled = true;
            dtFecNac.Enabled = false;
            btnGuardar.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
            btnNuevo.Enabled = true;
        }
        public void cargaGrid()
        {
            try
            {
                objCliente = new DemoBDEntities();
                var datos = from a in objCliente.Clientes
                            select new
                            {
                                Nombre = a.Nombres,
                                Apellido  = a.Apellidos,
                                Fecha_Nacimiento= a.FecNacimiento,
                            };
                dgVista.DataSource = datos.ToList();
                dgVista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }
            catch (Exception error)
            {
                MessageBox.Show("Se produjo un error " + error);
            }
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog buscaFoto = new OpenFileDialog();
                buscaFoto.Title = "Buscar Imagen";
                buscaFoto.Filter= "Archivos JPG( *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                if (buscaFoto.ShowDialog() == DialogResult.OK)
                {
                    string rutimg = buscaFoto.FileName;
                    txtRutaImg.Text = rutimg;
                    pbImagen.Image = Image.FromFile(rutimg);
                }
                else MessageBox.Show("No se selecciono ninguana Imagen");
            }
            catch (Exception error)
            {
                MessageBox.Show("Se produjo un error " + error);
            }
        }
        private void btneliminar_Click(object sender, EventArgs e)
        {
            //label4.Text = dtFecNac.Value.ToShortDateString();
            try
            {
                if(txtDNI.Text != string.Empty)
                {
                    int id = int.Parse(txtDNI.Text);
                    objCliente = new DemoBDEntities();
                    Cliente bajacli = (from a in objCliente.Clientes
                                        where a.IdDNI == id
                                        select a).SingleOrDefault();
                    objCliente.Clientes.Remove(bajacli);
                    objCliente.SaveChanges();
                    MessageBox.Show("Registro eliminado");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Se produjo un error " + error);
            }            
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDNI.Text != string.Empty)
                {
                    int id = int.Parse(txtDNI.Text);
                    objCliente = new DemoBDEntities();
                    Cliente buscacli = (from a in objCliente.Clientes
                                        where a.IdDNI == id
                                        select a).SingleOrDefault();
                    if (buscacli !=null)
                    {
                        txtNombre.Text = buscacli.Nombres;
                        txtApellido.Text = buscacli.Apellidos;
                        txtRutaImg.Text = buscacli.RutaImagen;
                        dtFecNac.Value = buscacli.FecNacimiento.Value;
                        pbImagen.Image = Image.FromFile(buscacli.RutaImagen);
                        btnModificar.Enabled = true; btnEliminar.Enabled = true;
                        txtNombre.Focus();
                    }
                    else MessageBox.Show("No se encontró Registro");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Se produjo un error " + error);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar();
            limpiar();
            txtDNI.Enabled = true;
            txtDNI.Focus();
            bandera = true;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            txtDNI.Enabled = false;
            Habilitar();
            txtNombre.Focus();
            bandera = false;
            btnModificar.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DesHabilitar();
            limpiar();
            cargaGrid();
            
        }

    }
}

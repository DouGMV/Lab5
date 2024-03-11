using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabRepaso1
{
    public partial class Form1 : Form
    {
        //Cargar Empledos y Asistencia
        List<Empleado> empleados = new List<Empleado>();
        List<Asistencia> asistencias = new List<Asistencia>();
        List<Resultado> resultados = new List<Resultado>();

        public Form1()
        {
            InitializeComponent();
        }

        public void CargarEmpleados()
        {
            //Leer el archivo y cargarlo a la lista
            string fileName = "Empleados.txt";

            //Abrimos el archivo, en este caso lo abrimos para lectura
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            //Un ciclo para leer el archivo hasta el final del archivo
            while (reader.Peek() > -1)
            {
                //Leer los datos de un empleado
                Empleado empleado = new Empleado();
                empleado.NoEmpleado = Convert.ToInt16(reader.ReadLine());
                empleado.Nombre = reader.ReadLine();
                empleado.SueldoHora = Convert.ToDecimal(reader.ReadLine());

                //Guardar el empleado a la lista de empleados
                empleados.Add(empleado);
            }
            //Cerrar el archivo, esta linea es importante porque sino despues de correr varias veces el programa daría error de que el archivo quedó abierto muchas veces. Entonces es necesario cerrarlo despues de terminar de leerlo.
            reader.Close();
        }

        public void MostrarEmpleados()
        {
            //Mostrar la lista de empleados en el DataGridView
            dataGridViewEmpleados.DataSource = null;
            dataGridViewEmpleados.DataSource = empleados;
            dataGridViewEmpleados.Refresh();
        }

        public void CargarAsistencia()
        {
            //Leer el archivo y cargarlo a la lista
            string fileName = "Asistencia.txt";

            //Abrimos el archivo, en este caso lo abrimos para lectura
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);

            //Un ciclo para leer el archivo hasta el final del archivo
            while (reader.Peek() > -1)
            {
                //Leer la asistencia de un empleado
                Asistencia asistencia = new Asistencia();
                asistencia.NoEmpleado = Convert.ToInt16(reader.ReadLine());
                asistencia.HorasMes = Convert.ToInt16(reader.ReadLine());
                asistencia.Mes = Convert.ToInt16(reader.ReadLine());

                //Guardar la asistencia a la lista de asistencia
                asistencias.Add(asistencia);
            }
            //Cerrar el archivo, esta linea es importante porque sino despues de correr varias veces el programa daría error de que el archivo quedó abierto muchas veces. Entonces es necesario cerrarlo despues de terminar de leerlo.
            reader.Close();
        }

        public void MostrarAsistencia()
        {
            //Mostrar la lista de asistencia en el DataGridView
            dataGridViewAsistencia.DataSource = null;
            dataGridViewAsistencia.DataSource = asistencias;
            dataGridViewAsistencia.Refresh();
        }

        private void buttonLeerDatos_Click(object sender, EventArgs e)
        {
            MostrarEmpleados();
            MostrarAsistencia();
        }

        public void CargarResultados()
        {
            //Con este ciclo foreach se extraen los numeros de empleados para operar
            foreach (Empleado empleado in empleados)
            {
                int noEmpleado = empleado.NoEmpleado;

                foreach (Asistencia asistencia in asistencias)
                {
                    if (empleado.NoEmpleado == asistencia.NoEmpleado)
                    {
                        Resultado resultado = new Resultado();
                        resultado.NombreEmpleado = empleado.Nombre;
                        resultado.Mes = asistencia.Mes;
                        resultado.SueldoMensual = empleado.SueldoHora * asistencia.HorasMes;

                        resultados.Add(resultado);
                    }
                }
            }
        }

        public void MostrarResultado()
        {
            //Mostrar la lista de resultado en el DataGridView
            dataGridViewResultado.DataSource = null;
            dataGridViewResultado.DataSource = resultados;
            dataGridViewResultado.Refresh();
        }

        private void buttonCalcular_Click(object sender, EventArgs e)
        {
            CargarResultados();
            MostrarResultado();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
            CargarAsistencia();
            comboBoxEmpleados.DisplayMember = "Nombre";
            comboBoxEmpleados.ValueMember = "noEmpleado";
            comboBoxEmpleados.DataSource = empleados;

        }

        private void comboBoxEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            int noEmpleado = Convert.ToInt16(comboBoxEmpleados.SelectedValue);

            //for(int i = 0; i < empleados.Count; i++)
            //{
            //    if(noEmpleado == empleados[i].NoEmpleado)
            //    {
            //        //Desplegar datos
            //        label1.Text = empleados[i].Nombre;
            //    }
            //}

            Empleado empleadoEncontrado = empleados.Find(c => c.NoEmpleado == noEmpleado);
            Asistencia asistenciaEncontrada = asistencias.Find(c => c.NoEmpleado == noEmpleado);

            decimal sueldo = empleadoEncontrado.SueldoHora * asistenciaEncontrada.HorasMes;

            label1.Text = empleadoEncontrado.Nombre;
            label2.Text = sueldo.ToString();
        }
    }
}

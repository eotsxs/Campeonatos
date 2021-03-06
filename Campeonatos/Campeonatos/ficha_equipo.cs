﻿// AUTOR: EDWIN OTTONIEL RODRIGUEZ TAYLOR
// 0901-07-1527
// FORM QUE CREA FICHA DE EQUIPOS


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLiteConnect;
using System.Collections;

namespace Campeonatos
{
    public partial class ficha_equipo : Form
    {
        DBConnect db = new DBConnect(Properties.Settings.Default.ruta);

        static int cont = 0;
        static int senal = 0;
        int id_equipo;
        System.Collections.ArrayList arr = new ArrayList();
        

        public ficha_equipo(int equipo, string nombre, string encargado, string telefono)
        {
            InitializeComponent();
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.quitar_btn, "Quitar Jugador de Lista");
            ToolTip1.SetToolTip(this.agregar_btn, "Agregar Jugador a Lista");
            ToolTip1.SetToolTip(this.nuevo_jugador_btn, "Llamar Modulo de Jugadores");
            
            
            label5.Text = nombre;
            label6.Text = encargado;
            label7.Text = telefono;

            id_equipo = equipo;
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string query = "select idjugador as 'ID', nombre as 'Nombre', telefono as 'Telefono', direccion as 'Direccion' from jugador ";
            //grid_bd_jugadores.DataSource = db.consultarGrid(query).DataSource;

            string query = "select j.idjugador as 'id', j.nombre as 'Nombre', j.telefono as 'Telefono', j.direccion as 'Direccion' from jugador j, equipo e, ficha_jugador f ";
            query += "where f.idjugador=j.idjugador and e.idequipo=f.idequipo and e.idequipo = "+id_equipo;
            //grid_lista_jugadores.DataSource = db.consultarGrid(query).DataSource;
            System.Collections.ArrayList array = db.consultar(query);
            grid_lista_jugadores.RowCount = array.Count;
            grid_lista_jugadores.ColumnCount = 4;
            int cont = 0;
            foreach (Dictionary<string, string> d in array)
            {
                grid_lista_jugadores.Rows[cont].Cells[0].Value = d["id"];
                grid_lista_jugadores.Rows[cont].Cells[1].Value = d["Nombre"];
                grid_lista_jugadores.Rows[cont].Cells[2].Value = d["Telefono"];
                grid_lista_jugadores.Rows[cont].Cells[3].Value = d["Direccion"];
                cont++;
                arr.Add(d["id"]);
            }
            grid_lista_jugadores.Columns[0].Visible = false;

           // query = "select idjugador, nombre as 'Nombre', telefono as 'Telefono', direccion as 'Direccion' from jugador";
            /*
            for (int i = 0; i < grid_lista_jugadores.RowCount; i++)
            {
                query += " idjugador <> " + grid_lista_jugadores.Rows[i].Cells[0].Value;
                if (i < (grid_lista_jugadores.RowCount - 1))
                {
                    query += " and ";
                }
            }
            */
           // grid_bd_jugadores.DataSource = db.consultarGrid(query).DataSource;


            /*
            BLOQUE CON EL QUE SE LLAMA A LA CLASE Y SE LLENA EL DATAGRIDVIEW
 */
            query = "select idcampeonato from equipo where idequipo=" + id_equipo;
            array = db.consultar(query);
            int id_torneo = 0;
            foreach (Dictionary<string, string> d in array)
            {
                id_torneo = Convert.ToInt32(d["idcampeonato"]);
            }
            

            Seleccion_jugadores sele = new Seleccion_jugadores(id_torneo, id_equipo);
            grid_bd_jugadores.DataSource = sele.consultar().DataSource;
            grid_bd_jugadores.Columns[0].Visible = false;
            /*
             *          HASTA ACÁ
             */


        }

        private void button_nuevo_jugador_Click(object sender, EventArgs e)
        {
            //jugadorescs f = new jugadorescs();
            //f.Show();
            jugadorescs f = new jugadorescs();
            f.ShowDialog();

            string query = "select j.idjugador as 'id', j.nombre as 'Nombre', j.telefono as 'Telefono', j.direccion as 'Direccion' from jugador j, equipo e, ficha_jugador f ";
            query += "where f.idjugador=j.idjugador and e.idequipo=f.idequipo and e.idequipo = " + id_equipo;

        }

        private void button_agregar_Click(object sender, EventArgs e)
        {
            
            int rowIndex = grid_bd_jugadores.CurrentCell.RowIndex;

            
            if (grid_lista_jugadores.RowCount > 0)
            {
                for (int i = 0; i < grid_lista_jugadores.RowCount; i++)
                {
                    int a = Convert.ToInt32(grid_bd_jugadores.Rows[rowIndex].Cells[0].Value);
                    int b = Convert.ToInt32(grid_lista_jugadores.Rows[i].Cells[0].Value);

                    if (a == b)
                    {
                        MessageBox.Show("El Jugador ya esta agregado");
                        senal = 1;
                    }
                }
            }

            if (senal == 0)
            {


                //int rowIndex = grid_bd_jugadores.CurrentCell.RowIndex;
                cont = grid_lista_jugadores.RowCount;
                grid_lista_jugadores.RowCount += 1;
                

                grid_lista_jugadores.Rows[cont].Cells[0].Value = grid_bd_jugadores.Rows[rowIndex].Cells[0].Value;
                grid_lista_jugadores.Rows[cont].Cells[1].Value = grid_bd_jugadores.Rows[rowIndex].Cells[1].Value;
                grid_lista_jugadores.Rows[cont].Cells[2].Value = grid_bd_jugadores.Rows[rowIndex].Cells[2].Value;
                grid_lista_jugadores.Rows[cont].Cells[3].Value = grid_bd_jugadores.Rows[rowIndex].Cells[3].Value;

                cont++;

               /* string query = "select idjugador from ficha_jugador where idjugador = " + grid_lista_jugadores.Rows[cont].Cells[0].Value + " and idequipo = " + id_equipo;
                // MessageBox.Show(query + " " + grid_lista_jugadores.RowCount);
                System.Collections.ArrayList a = db.consultar(query);
                */
               // if (a.Count == 0)
                //{
                    int cont3 = grid_bd_jugadores.CurrentCell.RowIndex;
                    string tabla = "ficha_jugador";
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("idjugador", grid_bd_jugadores.Rows[cont3].Cells[0].Value.ToString());
                    dict.Add("idequipo", id_equipo.ToString());
                    db.insertar(tabla, dict);
                    //grid_bd_jugadores.Rows.RemoveAt(rowIndex);
                //}


            }
            else
            {
                senal = 0;
            }

        }


        private void grid_bd_jugadores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_lista_jugadores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_quitar_Click(object sender, EventArgs e)
        {
            MessageBox.Show(grid_lista_jugadores.CurrentCell.RowIndex.ToString());
            if (grid_lista_jugadores.RowCount > 0)
            {
               
                    int rowIndex = grid_lista_jugadores.CurrentCell.RowIndex;

                    if (rowIndex == null)
                    {

                   
                }
                else
                {
                    grid_lista_jugadores.Rows.RemoveAt(rowIndex);
                    grid_lista_jugadores.Rows.Add(1);
                    cont = cont - 1;
                    // grid_lista_jugadores.RowCount -= 1;

                    int row = grid_lista_jugadores.CurrentCell.RowIndex;
                    string j = grid_lista_jugadores.Rows[row].Cells[0].Value.ToString();

                    string condicion = "idjugador = " + j + " and idequipo = " + id_equipo;
                    db.eliminar("ficha_jugador", condicion);

                    grid_lista_jugadores.RowCount -= 1;

                }
            }

                    
        }

        private void button_buscar_jugador_Click(object sender, EventArgs e)
        {
            string searchValue = buscar_buscar.Text;
            int rowIndex2 = -1;

            grid_bd_jugadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in grid_bd_jugadores.Rows)
                {
                    if (row.Cells[row.Index].Value.ToString().Equals(searchValue))
                    {
                        rowIndex2 = row.Index;
                        grid_bd_jugadores.Rows[row.Index].Selected = true;
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        
          // string searchedText = text_buscar.Text;
          // var matchedRows = this.grid_bd_jugadores.Rows.Cast<DataGridViewRow>().Select(r => r.Cells["ID"].Value).Where(v => v != null && v.ToString() == searchedText).ToList();


            

        }

        private void button_crear_equipo_Click(object sender, EventArgs e)
        {

            this.Close();

        }
    }
} 
        
    


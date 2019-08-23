using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Perso
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Equipment> equipmentList= new List<Equipment>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(txtId.Text.ToString());
            Equipment usedEquipment = null;
            foreach (Equipment equipment in equipmentList)
            {
                if (equipment.ankamaId == id)
                {
                    usedEquipment = equipment;
                    break;
                }

            }
            if (usedEquipment != null)
            {
                string lignes = usedEquipment.name + "\r\n";
                foreach (Statistic ligne in usedEquipment.statistics)
                {
                    lignes += ligne.name + "\r\n" +
                        "min = " + ligne.min.ToString() + "\r\n" +
                        "value = " + ligne.value.ToString() + "\r\n" +
                        "max = " + ligne.max.ToString() + "\r\n";
                }
                lblValue.Text = lignes;
            }

            else
                lblValue.Text = "null";
        }
        public MainWindow()
        {
            InitializeComponent();
            extraction2();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text);
        }

        public void extraction2()
        {
            JsonSerializer serializer = new JsonSerializer();
            string dirpath = @"C:\Users\Thur\Desktop\j\Perso\allequipments.json";
            using (FileStream s = File.Open(dirpath, FileMode.Open))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                    equipmentList = serializer.Deserialize<List<Equipment>>(reader);
                

        }
        }
    }
}

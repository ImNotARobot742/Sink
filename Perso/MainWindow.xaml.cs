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
using Newtonsoft.Json.Linq;

namespace Perso
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO handle file path to ressources and remove from bin debug
        //TODO create query maker to get from api and not files 
        //TODO Create appropriate classes
        // TODO dispaly -- Query sent, message received, creating obj, ready ++ error messages



        public List<Equipment> equipmentList= new List<Equipment>();

        #region HMI

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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text);
        }



        #endregion

        public MainWindow()
        {
            InitializeComponent();
            string EquipJsonString = File.ReadAllText(@"allequipments.json");
            EquipmentCreator(EquipJsonString);
            string WeapjsonString = File.ReadAllText(@"allweapons.json");
            EquipmentCreator(WeapjsonString);
            
        }

        #region JSONImporter

        void EquipmentCreator(string jsonString)
        {

            JArray results = JArray.Parse(jsonString);
            foreach (var result in results)
            {
                Equipment e = new Equipment();
                e.ankamaId = (int)result["ankamaId"];
                e.name = (string)result["name"];
                if (e.ankamaId == 18691)
                { }
                try
                {
                    JArray statistics = (JArray)result["statistics"];
                    foreach (var stat in statistics)
                    {
                        Statistic ligne = new Statistic();

                        JObject lignes = (JObject)stat;
                        foreach (JProperty o in lignes.Children())
                        {
                            ligne.name = o.Name;
                            JToken min = null;
                            JToken max = null;
                            try
                            {
                                min = o.Value["min"];
                                max = o.Value["max"];
                            }
                            catch (Exception exception)
                            {

                            }

                            if (min != null)
                                ligne.min = (int)min;
                            if (max != null)
                                ligne.max = (int)max;
                        }

                        e.statistics.Add(ligne);
                    }
                }
                catch (Exception exception)
                {
                }
                equipmentList.Add(e);
            }
        }

        #endregion
    }
}
